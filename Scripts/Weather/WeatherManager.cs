using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using UI;
using System.Collections;
using Utility;
using ApplicationManagers;
using GameManagers;
using CustomSkins;

namespace Weather
{
    class WeatherManager : MonoBehaviour
    {
        // consts
        static WeatherManager _instance;
        const float LerpDelay = 0.05f;
        const float SyncDelay = 5f;
        HashSet<WeatherEffect> LowEffects = new HashSet<WeatherEffect> { WeatherEffect.Daylight, WeatherEffect.AmbientLight, 
            WeatherEffect.Flashlight, WeatherEffect.Skybox };
        static Dictionary<string, Material> SkyboxMaterials = new Dictionary<string, Material>();
        static Dictionary<string, Dictionary<string, Material>> SkyboxBlendedMaterials = new Dictionary<string, Dictionary<string, Material>>();
        static Shader _blendedShader;

        // instance
        List<WeatherScheduleRunner> _scheduleRunners = new List<WeatherScheduleRunner>();
        Dictionary<WeatherEffect, BaseWeatherEffect> _effects = new Dictionary<WeatherEffect, BaseWeatherEffect>();
        public WeatherSet _currentWeather = new WeatherSet();
        public WeatherSet _targetWeather = new WeatherSet();
        public WeatherSet _startWeather = new WeatherSet();
        public Dictionary<int, float> _targetWeatherStartTimes = new Dictionary<int, float>();
        public Dictionary<int, float> _targetWeatherEndTimes = new Dictionary<int, float>();
        List<WeatherEffect> _needApply = new List<WeatherEffect>();
        public float _currentTime;
        public bool _needSync;
        public Dictionary<WeatherScheduleRunner, float> _currentScheduleWait = new Dictionary<WeatherScheduleRunner, float>();
        float _currentLerpWait;
        float _currentSyncWait;
        bool _finishedLoading;

        // cache
        Light _mainLight;
        Skybox _skybox;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
        }

        public static void FinishLoadAssets()
        {
            LoadSkyboxes();
            ThunderWeatherEffect.FinishLoadAssets();
            _instance.StartCoroutine(_instance.RestartWeather());
        }

        private static void LoadSkyboxes()
        {
            _blendedShader = AssetBundleManager.InstantiateAsset<Shader>("SkyboxBlendShader");
            string[] skyboxNames = RCextensions.EnumToStringArray<WeatherSkybox>();
            string[] parts = RCextensions.EnumToStringArray<SkyboxCustomSkinPartId>();
            foreach (string skyboxName in skyboxNames)
                SkyboxMaterials.Add(skyboxName, AssetBundleManager.InstantiateAsset<Material>(skyboxName.ToString() + "Skybox"));
            foreach (string skybox1 in skyboxNames)
            {
                SkyboxBlendedMaterials.Add(skybox1, new Dictionary<string, Material>());
                foreach (string skybox2 in skyboxNames)
                {
                    Material blend = CreateBlendedSkybox(_blendedShader, parts, skybox1, skybox2);
                    SkyboxBlendedMaterials[skybox1].Add(skybox2, blend);
                }
            }
        }

        public static void TakeFlashlight(Transform parent)
        {
            if (_instance._effects.ContainsKey(WeatherEffect.Flashlight) && _instance._effects[WeatherEffect.Flashlight] != null)
                _instance._effects[WeatherEffect.Flashlight].SetParent(parent);
        }

        private static Material CreateBlendedSkybox(Shader shader, string[] parts, string skybox1, string skybox2)
        {
            Material blend = new Material(shader);
            foreach (string part in parts)
            {
                string texName = "_" + part + "Tex";
                blend.SetTexture(texName, SkyboxMaterials[skybox1].GetTexture(texName));
                blend.SetTexture(texName + "2", SkyboxMaterials[skybox2].GetTexture(texName));
            }
            SetSkyboxBlend(blend, 0f);
            return blend;
        }

        private static void SetSkyboxBlend(Material skybox, float blend)
        {
            skybox.SetFloat("_Blend", blend);
        }

        private void Cache()
        {
            _mainLight = GameObject.Find("mainLight").GetComponent<Light>();
            _skybox = Camera.main.GetComponent<Skybox>();
        }

        private void ResetSkyboxColors()
        {
            foreach (string skybox1 in SkyboxBlendedMaterials.Keys)
            {
                foreach (string skybox2 in SkyboxBlendedMaterials[skybox1].Keys)
                {
                    SkyboxBlendedMaterials[skybox1][skybox2].SetColor("_Tint", new Color(0.5f, 0.5f, 0.5f));
                }
            }
        }

        private IEnumerator RestartWeather()
        {
            while (Camera.main == null)
                yield return null;
            Cache();
            ResetSkyboxColors();
            _scheduleRunners.Clear();
            _effects.Clear();
            _currentWeather.SetDefault();
            _startWeather.SetDefault();
            _targetWeather.SetDefault();
            _targetWeatherStartTimes.Clear();
            _targetWeatherEndTimes.Clear();
            _needApply.Clear();
            _currentTime = 0f;
            _currentScheduleWait.Clear();
            CreateEffects();
            if (Application.loadedLevel == 0 && SettingsManager.GraphicsSettings.AnimatedIntro.Value)
                SetMainMenuWeather();
            ApplyCurrentWeather(firstStart: true, applyAll: true);
            bool weatherOff = SettingsManager.GraphicsSettings.WeatherEffects.Value == (int)WeatherEffectLevel.Off;
            if (Application.loadedLevel != 0 && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || PhotonNetwork.isMasterClient))
            {
                if (!weatherOff)
                {
                    _currentWeather.Copy(SettingsManager.WeatherSettings.WeatherSets.GetSelectedSet());
                    CreateScheduleRunners(_currentWeather.Schedule.Value);
                    _currentWeather.Schedule.SetDefault();
                }
                if (_currentWeather.UseSchedule.Value)
                {
                    foreach (WeatherScheduleRunner runner in _scheduleRunners)
                    {
                        runner.ProcessSchedule();
                        runner.ConsumeSchedule();
                    }
                }
                SyncWeather();
                _currentSyncWait = SyncDelay;
                _needSync = false;
            }
            _currentLerpWait = LerpDelay;
            _finishedLoading = true;
        }

        private void SetMainMenuWeather()
        {
            _currentWeather.Rain.Value = 0.45f;
            _currentWeather.Thunder.Value = 0.1f;
            _currentWeather.Skybox.Value = "Storm";
            _currentWeather.FogDensity.Value = 0.01f;
            _currentWeather.Daylight.Value = new Color(0.1f, 0.1f, 0.1f);
            _currentWeather.AmbientLight.Value = new Color(0.1f, 0.1f, 0.1f);
        }

        private void CreateScheduleRunners(string schedule)
        {
            WeatherScheduleRunner runner = new WeatherScheduleRunner(this);
            WeatherSchedule mainSchedule = new WeatherSchedule(schedule);
            foreach (WeatherEvent ev in mainSchedule.Events)
            {
                if (ev.Action == WeatherAction.BeginSchedule)
                {
                    runner = new WeatherScheduleRunner(this);
                    _scheduleRunners.Add(runner);
                    _currentScheduleWait.Add(runner, 0f);
                }
                runner.Schedule.Events.Add(ev);
            }
        }

        private void CreateEffects()
        {
            _effects.Add(WeatherEffect.Rain, AssetBundleManager.InstantiateAsset<GameObject>("RainEffect").AddComponent<RainWeatherEffect>());
            _effects.Add(WeatherEffect.Snow, AssetBundleManager.InstantiateAsset<GameObject>("SnowEffect").AddComponent<SnowWeatherEffect>());
            _effects.Add(WeatherEffect.Wind, AssetBundleManager.InstantiateAsset<GameObject>("WindEffect").AddComponent<WindWeatherEffect>());
            _effects.Add(WeatherEffect.Thunder, AssetBundleManager.InstantiateAsset<GameObject>("ThunderEffect").AddComponent<ThunderWeatherEffect>());
            Transform cameraTransform = Camera.main.transform;
            foreach (BaseWeatherEffect effect in _effects.Values)
            {
                effect.Setup(cameraTransform);
                effect.Randomize();
                effect.Disable(fadeOut: false);
            }
            CreateFlashlight();
        }

        private void CreateFlashlight()
        {
            _effects.Add(WeatherEffect.Flashlight, AssetBundleManager.InstantiateAsset<GameObject>("FlashlightEffect").AddComponent<FlashlightWeatherEffect>());
            _effects[WeatherEffect.Flashlight].Setup(null);
            _effects[WeatherEffect.Flashlight].Disable(fadeOut: false);
            if (IN_GAME_MAIN_CAMERA.Instance != null)
                TakeFlashlight(IN_GAME_MAIN_CAMERA.Instance.transform);
        }

        private void FixedUpdate()
        {
            if (!_finishedLoading)
                return;
            _currentTime += Time.fixedDeltaTime;
            if (_targetWeatherStartTimes.Count > 0)
            {
                _currentLerpWait -= Time.fixedDeltaTime;
                if (_currentLerpWait <= 0f)
                {
                    LerpCurrentWeatherToTarget();
                    ApplyCurrentWeather(firstStart: false, applyAll: false);
                    _currentLerpWait = LerpDelay;
                }
            }
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || PhotonNetwork.isMasterClient)
            {
                if (_currentWeather.UseSchedule.Value)
                {
                    foreach (WeatherScheduleRunner key in new List<WeatherScheduleRunner>(_currentScheduleWait.Keys))
                    {
                        _currentScheduleWait[key] -= Time.fixedDeltaTime;
                        if (_currentScheduleWait[key] <= 0f)
                            key.ConsumeSchedule();
                    }
                    _currentSyncWait -= Time.fixedDeltaTime;
                    if (_currentSyncWait <= 0f && _needSync)
                    {
                        LerpCurrentWeatherToTarget();
                        SyncWeather();
                        _needSync = false;
                        _currentSyncWait = SyncDelay;
                    }
                }
            }
        }

        private void SyncWeather()
        {
            ApplyCurrentWeather(firstStart: false, applyAll: true);
            if (PhotonNetwork.isMasterClient && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
            {
                CustomRPCManager.PhotonView.RPC("SetWeatherRPC", PhotonTargets.Others, new object[]{ _currentWeather.SerializeToJsonString(), 
                    _startWeather.SerializeToJsonString(), _targetWeather.SerializeToJsonString(), _targetWeatherStartTimes, _targetWeatherEndTimes, _currentTime });
            }
        }

        private void OnLevelWasLoaded(int level)
        {
            WindWeatherEffect.WindEnabled = false;
            foreach (List<LightningParticle> list in ThunderWeatherEffect.LightningPool)
            {
                foreach (LightningParticle particle in list)
                    particle.Disable();
            }
            if (Application.loadedLevelName != "characterCreation" && (Application.loadedLevelName != "SnapShot"))
            {
                _finishedLoading = false;
                StartCoroutine(RestartWeather());
            }
        }

        private void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            if (PhotonNetwork.isMasterClient)
            {
                CustomRPCManager.PhotonView.RPC("SetWeatherRPC", player, new object[]{ _currentWeather.SerializeToJsonString(), _startWeather.SerializeToJsonString(), 
                    _targetWeather.SerializeToJsonString(), _targetWeatherStartTimes, _targetWeatherEndTimes, _currentTime });
            }
        }

        private void LerpCurrentWeatherToTarget()
        {
            List<int> remove = new List<int>();
            foreach (KeyValuePair<int, float> entry in _targetWeatherEndTimes)
            {
                float lerp;
                if (entry.Value <= _currentTime)
                {
                    remove.Add(entry.Key);
                    lerp = 1f;
                }
                else
                {
                    float startTime = _targetWeatherStartTimes[entry.Key];
                    float endTime = entry.Value;
                    lerp = (_currentTime - startTime) / Mathf.Max(endTime - startTime, 1f);
                    lerp = Mathf.Clamp(lerp, 0f, 1f);
                }
                string effectName = ((WeatherEffect)entry.Key).ToString();
                BaseSetting startSetting = (BaseSetting)_startWeather.Settings[effectName];
                BaseSetting currentSetting = (BaseSetting)_currentWeather.Settings[effectName];
                BaseSetting targetSetting = (BaseSetting)_targetWeather.Settings[effectName];
                switch ((WeatherEffect)entry.Key)
                {
                    case WeatherEffect.Daylight:
                    case WeatherEffect.AmbientLight:
                    case WeatherEffect.FogColor:
                    case WeatherEffect.Flashlight:
                    case WeatherEffect.SkyboxColor:
                        ((ColorSetting)currentSetting).Value = Color.Lerp(((ColorSetting)startSetting).Value, ((ColorSetting)targetSetting).Value, lerp);
                        break;
                    case WeatherEffect.FogDensity:
                    case WeatherEffect.Rain:
                    case WeatherEffect.Thunder:
                    case WeatherEffect.Snow:
                    case WeatherEffect.Wind:
                        ((FloatSetting)currentSetting).Value = Mathf.Lerp(((FloatSetting)startSetting).Value, ((FloatSetting)targetSetting).Value, lerp);
                        break;
                    case WeatherEffect.Skybox:
                        Material mat = GetBlendedSkybox(_currentWeather.Skybox.Value, _targetWeather.Skybox.Value);
                        if (mat != null)
                        {
                            if (lerp >= 1f)
                                ((StringSetting)currentSetting).Value = ((StringSetting)targetSetting).Value;
                            SetSkyboxBlend(mat, lerp);
                        }
                        break;
                }
                _needApply.Add((WeatherEffect)entry.Key);
            }
            foreach (int r in remove)
            {
                _targetWeatherStartTimes.Remove(r);
                _targetWeatherEndTimes.Remove(r);
            }
        }

        private void ApplyCurrentWeather(bool firstStart, bool applyAll)
        {
            if (applyAll)
                _needApply = RCextensions.EnumToList<WeatherEffect>();
            WeatherEffectLevel level = (WeatherEffectLevel)SettingsManager.GraphicsSettings.WeatherEffects.Value;
            foreach (WeatherEffect effect in _needApply)
            {
                if (!firstStart && level == WeatherEffectLevel.Low && !LowEffects.Contains(effect))
                    continue;
                switch (effect)
                {
                    case WeatherEffect.Daylight:
                        _mainLight.color = _currentWeather.Daylight.Value;
                        break;
                    case WeatherEffect.AmbientLight:
                        RenderSettings.ambientLight = _currentWeather.AmbientLight.Value;
                        break;
                    case WeatherEffect.FogColor:
                        RenderSettings.fogColor = _currentWeather.FogColor.Value;
                        break;
                    case WeatherEffect.FogDensity:
                        if (_currentWeather.FogDensity.Value > 0f)
                        {
                            RenderSettings.fog = true;
                            RenderSettings.fogMode = FogMode.Exponential;
                            RenderSettings.fogDensity = _currentWeather.FogDensity.Value * 0.05f;
                        }
                        else
                        {
                            RenderSettings.fog = false;
                        }
                        break;
                    case WeatherEffect.Flashlight:
                        ((FlashlightWeatherEffect)_effects[WeatherEffect.Flashlight]).SetColor(_currentWeather.Flashlight.Value);
                        if (_currentWeather.Flashlight.Value.a > 0f && _currentWeather.Flashlight.Value != Color.black)
                        {
                            if (!_effects[WeatherEffect.Flashlight].gameObject.activeSelf)
                                _effects[WeatherEffect.Flashlight].Enable();
                        }
                        else
                            _effects[WeatherEffect.Flashlight].Disable();
                        break;
                    case WeatherEffect.Skybox:
                        StartCoroutine(WaitAndApplySkybox());
                        break;
                    case WeatherEffect.SkyboxColor:
                        Material mat = GetBlendedSkybox(_currentWeather.Skybox.Value, _targetWeather.Skybox.Value);
                        if (mat != null)
                        {
                            mat.SetColor("_Tint", _currentWeather.SkyboxColor.Value);
                        }
                        break;
                    case WeatherEffect.Rain:
                    case WeatherEffect.Snow:
                    case WeatherEffect.Wind:
                    case WeatherEffect.Thunder:
                        float value = ((FloatSetting)_currentWeather.Settings[effect.ToString()]).Value;
                        _effects[effect].SetLevel(value);
                        if (value > 0f)
                        {
                            if (!_effects[effect].gameObject.activeSelf)
                            {
                                _effects[effect].Randomize();
                                _effects[effect].Enable();
                            }
                        }
                        else
                            _effects[effect].Disable(fadeOut: true);
                        break;
                }
            }
            _needApply.Clear();
        }

        private IEnumerator WaitAndApplySkybox()
        {
            yield return new WaitForEndOfFrame();
            Material mat = GetBlendedSkybox(_currentWeather.Skybox.Value, _targetWeather.Skybox.Value);
            if (mat != null && _skybox.material != mat && SkyboxCustomSkinLoader.SkyboxMaterial == null)
            {
                mat.SetColor("_Tint", _currentWeather.SkyboxColor.Value);
                _skybox.material = mat;
                if (IN_GAME_MAIN_CAMERA.Instance != null)
                    IN_GAME_MAIN_CAMERA.Instance.UpdateSnapshotSkybox();
            }
        }

        private Material GetBlendedSkybox(string skybox1, string skybox2)
        {
            if (SkyboxBlendedMaterials.ContainsKey(skybox1))
            {
                if (SkyboxBlendedMaterials[skybox1].ContainsKey(skybox2))
                    return SkyboxBlendedMaterials[skybox1][skybox2];
            }
            return null;
        }

        public static void OnSetWeatherRPC(string currentWeatherJson, string startWeatherJson, string targetWeatherJson, Dictionary<int, float> targetWeatherStartTimes, 
            Dictionary<int, float> targetWeatherEndTimes, float currentTime, PhotonMessageInfo info)
        {
            if (info != null && info.sender != PhotonNetwork.masterClient)
                return;
            if (SettingsManager.GraphicsSettings.WeatherEffects.Value == (int)WeatherEffectLevel.Off)
                return;
            _instance.StartCoroutine(_instance.WaitAndFinishOnSetWeather(currentWeatherJson, startWeatherJson, targetWeatherJson, targetWeatherStartTimes,
                targetWeatherEndTimes, currentTime));
        }

        private IEnumerator WaitAndFinishOnSetWeather(string currentWeatherJson, string startWeatherJson, string targetWeatherJson, Dictionary<int, float> targetWeatherStartTimes,
            Dictionary<int, float> targetWeatherEndTimes, float currentTime)
        {
            while (!_finishedLoading)
                yield return null;
            _currentWeather.DeserializeFromJsonString(currentWeatherJson);
            _startWeather.DeserializeFromJsonString(startWeatherJson);
            _targetWeather.DeserializeFromJsonString(targetWeatherJson);
            _targetWeatherStartTimes = targetWeatherStartTimes;
            _targetWeatherEndTimes = targetWeatherEndTimes;
            _currentTime = currentTime;
            LerpCurrentWeatherToTarget();
            ApplyCurrentWeather(firstStart: false, applyAll: true);
        }
    }
}
