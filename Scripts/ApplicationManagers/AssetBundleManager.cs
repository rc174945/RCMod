using UnityEngine;
using System.Collections;
using Utility;
using System.Collections.Generic;

namespace ApplicationManagers
{
    public class AssetBundleManager : MonoBehaviour
    {
        public static AssetBundle MainAssetBundle;
        public static AssetBundleStatus Status = AssetBundleStatus.Loading;
        public static bool CloseFailureBox = false;
        static AssetBundleManager _instance;
        static Dictionary<string, Object> _cache = new Dictionary<string, Object>();

        // consts
        static readonly string RootDataPath = Application.dataPath;
        static readonly string LocalAssetBundlePath = "file:///" + RootDataPath + "/RCAssets.unity3d";
        static readonly string BackupAssetBundleURL = AutoUpdateManager.PlatformUpdateURL + "/RCAssets.unity3d";

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            LoadAssetBundle();
        }

        public static void LoadAssetBundle()
        {
            _instance.StartCoroutine(_instance.LoadAssetBundleCoroutine());
        }

        public static Object LoadAsset(string name, bool cached = false)
        {
            if (cached)
            {
                if (!_cache.ContainsKey(name))
                    _cache.Add(name, MainAssetBundle.Load(name));
                return _cache[name];
            }
            return MainAssetBundle.Load(name);
        }

        public static T InstantiateAsset<T>(string name) where T: Object
        {
            return (T)Instantiate(MainAssetBundle.Load(name));
        }

        public static T InstantiateAsset<T>(string name, Vector3 position, Quaternion rotation) where T: Object
        {
            return (T)Instantiate(MainAssetBundle.Load(name), position, rotation);
        }

        IEnumerator LoadAssetBundleCoroutine()
        {
            Status = AssetBundleStatus.Loading;
            while (AutoUpdateManager.Status == AutoUpdateStatus.Updating || !Caching.ready)
                yield return null;
            // try loading local asset bundle
            using (WWW wwwLocal = new WWW(LocalAssetBundlePath))
            {
                yield return wwwLocal;
                if (wwwLocal.error != null)
                {
                    // try loading server asset bundle
                    Debug.Log("Failed to load local asset bundle, trying backup URL at " + BackupAssetBundleURL + ": " + wwwLocal.error);
                    using (WWW wwwBackup = WWW.LoadFromCacheOrDownload(BackupAssetBundleURL, ApplicationConfig.AssetBundleVersion))
                    {
                        yield return wwwBackup;
                        if (wwwBackup.error != null)
                        {
                            Debug.Log("The backup asset bundle failed too: " + wwwBackup.error);
                            Status = AssetBundleStatus.Failed;
                            yield break;
                        }
                        else
                            OnAssetBundleLoaded(wwwBackup);
                    }
                }
                else
                    OnAssetBundleLoaded(wwwLocal);
            }
        }

        private void OnAssetBundleLoaded(WWW www)
        {
            FengGameManagerMKII.RCassets = www.assetBundle;
            FengGameManagerMKII.isAssetLoaded = true;
            MainAssetBundle = FengGameManagerMKII.RCassets;
            MainApplicationManager.FinishLoadAssets();
            Status = AssetBundleStatus.Ready;
        }
    }

    public enum AssetBundleStatus
    {
        Loading,
        Ready,
        Failed
    }
}