using UnityEngine;
using ApplicationManagers;
using Settings;
using UnityEngine.UI;
using UI;
using System.Collections;
using System;
using Constants;
using Weather;

class IN_GAME_MAIN_CAMERA : MonoBehaviour
{
    public RotationAxes axes;
    public AudioSource bgmusic;
    public static float cameraDistance = 0.6f;
    public static CAMERA_TYPE cameraMode;
    public static int character = 1;
    private float closestDistance;
    private int currentPeekPlayerIndex;
    private float decay;
    public static int difficulty;
    private float distance = 10f;
    private float distanceMulti;
    private float distanceOffsetMulti;
    private float duration;
    private float flashDuration;
    private bool flip;
    public static GAMEMODE gamemode;
    public bool gameOver;
    public static GAMETYPE gametype = GAMETYPE.STOP;
    private bool hasSnapShot;
    private Transform head;
    private float height = 5f;
    private float heightDamping = 2f;
    private float heightMulti;
    public static bool isCheating;
    public static bool isTyping;
    public float justHit;
    public int lastScore;
    public static int level;
    private bool lockAngle;
    private Vector3 lockCameraPosition;
    private GameObject locker;
    private GameObject lockTarget;
    public GameObject main_object;
    public float maximumX = 360f;
    public float maximumY = 60f;
    public float minimumX = -360f;
    public float minimumY = -60f;
    public static bool needSetHUD;
    private float R;
    private float rotationY;
    public int score;
    public static string singleCharacter;
    public Material skyBoxDAWN;
    public Material skyBoxDAY;
    public Material skyBoxNIGHT;
    public GameObject snapShotCamera;
    public RenderTexture snapshotRT;
    public bool spectatorMode;
    public static STEREO_3D_TYPE stereoType;
    private Transform target;
    public Texture texture;
    public float timer;
    public static bool triggerAutoLock;
    public static bool usingTitan;
    private Vector3 verticalHeightOffset = Vector3.zero;
    private float verticalRotationOffset;
    private float xSpeed = -3f;
    private float ySpeed = -0.8f;
    public static IN_GAME_MAIN_CAMERA Instance;
    private Transform _transform;
    private UILabel _bottomRightText;
    private static float _lastRestartTime = 0f;

    private void Awake()
    {
        Cache();
        Instance = this;
        isTyping = false;
        GameMenu.TogglePause(false);
        base.name = "MainCamera";
        ApplyGraphicsSettings();
        this.CreateMinimap();
        WeatherManager.TakeFlashlight(transform);
    }

    public static void ApplyGraphicsSettings()
    {
        Camera cam = Camera.main;
        GraphicsSettings settings = SettingsManager.GraphicsSettings;
        if (settings != null && cam != null)
        {
            cam.farClipPlane = settings.RenderDistance.Value;
            if (!FengGameManagerMKII.level.StartsWith("Custom"))
                cam.GetComponent<TiltShift>().enabled = settings.BlurEnabled.Value;
            else
                cam.GetComponent<TiltShift>().enabled = false;
        }
    }

    private void Cache()
    {
        _transform = transform;
    }

    private void camareMovement()
    {
        Camera cam = camera;
        this.distanceOffsetMulti = (cameraDistance * (200f - cam.fieldOfView)) / 150f;
        _transform.position = (this.head == null) ? this.main_object.transform.position : this.head.position;
        _transform.position += (Vector3)(Vector3.up * this.heightMulti);
        _transform.position -= (Vector3)((Vector3.up * (0.6f - cameraDistance)) * 2f);
        float sensitivity = SettingsManager.GeneralSettings.MouseSpeed.Value;
        int invertY = SettingsManager.GeneralSettings.InvertMouse.Value ? -1 : 1;
        if (GameMenu.InMenu())
            sensitivity = 0f;
        if (cameraMode == CAMERA_TYPE.WOW)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                float angle = (Input.GetAxis("Mouse X") * 10f) * sensitivity;
                float num2 = ((-Input.GetAxis("Mouse Y") * 10f) * sensitivity) * invertY;
                _transform.RotateAround(_transform.position, Vector3.up, angle);
                _transform.RotateAround(_transform.position, _transform.right, num2);
            }
            _transform.position -= (Vector3)(((_transform.transform.forward * this.distance) * this.distanceMulti) * this.distanceOffsetMulti);
        }
        else if (cameraMode == CAMERA_TYPE.ORIGINAL)
        {
            float num3 = 0f;
            if (Input.mousePosition.x < (Screen.width * 0.4f))
            {
                num3 = (-((((Screen.width * 0.4f) - Input.mousePosition.x) / ((float)Screen.width)) * 0.4f) * this.getSensitivityMultiWithDeltaTime(sensitivity)) * 150f;
                _transform.RotateAround(_transform.position, Vector3.up, num3);
            }
            else if (Input.mousePosition.x > (Screen.width * 0.6f))
            {
                num3 = ((((Input.mousePosition.x - (Screen.width * 0.6f)) / ((float)Screen.width)) * 0.4f) * this.getSensitivityMultiWithDeltaTime(sensitivity)) * 150f;
                _transform.RotateAround(_transform.position, Vector3.up, num3);
            }
            float x = ((140f * ((Screen.height * 0.6f) - Input.mousePosition.y)) / ((float)Screen.height)) * 0.5f;
            _transform.rotation = Quaternion.Euler(x, _transform.rotation.eulerAngles.y, _transform.rotation.eulerAngles.z);
            _transform.position -= (Vector3)(((_transform.forward * this.distance) * this.distanceMulti) * this.distanceOffsetMulti);
        }
        else if (cameraMode == CAMERA_TYPE.TPS)
        {
            float num5 = (Input.GetAxis("Mouse X") * 10f) * sensitivity;
            float num6 = ((-Input.GetAxis("Mouse Y") * 10f) * sensitivity) * invertY;
            _transform.RotateAround(_transform.position, Vector3.up, num5);
            float num7 = _transform.rotation.eulerAngles.x % 360f;
            float num8 = num7 + num6;
            if (((num6 <= 0f) || (((num7 >= 260f) || (num8 <= 260f)) && ((num7 >= 80f) || (num8 <= 80f)))) && ((num6 >= 0f) || (((num7 <= 280f) || (num8 >= 280f)) && ((num7 <= 100f) || (num8 >= 100f)))))
            {
                _transform.RotateAround(_transform.position, _transform.right, num6);
            }
            _transform.position -= (Vector3)(((_transform.forward * this.distance) * this.distanceMulti) * this.distanceOffsetMulti);
        }
        if (cameraDistance < 0.65f)
        {
            _transform.position += (Vector3) (_transform.right * Mathf.Max((float) ((0.6f - cameraDistance) * 2f), (float) 0.65f));
        }
    }

    public void CameraMovementLive(HERO hero)
    {
        float magnitude = hero.rigidbody.velocity.magnitude;
        if (magnitude > 10f)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Mathf.Min((float) 100f, (float) (magnitude + 40f)), 0.1f);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 50f, 0.1f);
        }
        float num2 = (hero.CameraMultiplier * (200f - Camera.main.fieldOfView)) / 150f;
        base.transform.position = (Vector3) ((this.head.transform.position + (Vector3.up * this.heightMulti)) - ((Vector3.up * (0.6f - cameraDistance)) * 2f));
        Transform transform = base.transform;
        transform.position -= (Vector3) (((base.transform.forward * this.distance) * this.distanceMulti) * num2);
        if (hero.CameraMultiplier < 0.65f)
        {
            Transform transform2 = base.transform;
            transform2.position += (Vector3) (base.transform.right * Mathf.Max((float) ((0.6f - hero.CameraMultiplier) * 2f), (float) 0.65f));
        }
        base.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, hero.GetComponent<SmoothSyncMovement>().correctCameraRot, Time.deltaTime * 5f);
    }

    private void CreateMinimap()
    {
        LevelInfo info = LevelInfo.getInfo(FengGameManagerMKII.level);
        if (info != null)
        {
            Minimap minimap = base.gameObject.AddComponent<Minimap>();
            if (Minimap.instance.myCam == null)
            {
                Minimap.instance.myCam = new GameObject().AddComponent<Camera>();
                Minimap.instance.myCam.nearClipPlane = 0.3f;
                Minimap.instance.myCam.farClipPlane = 1000f;
                Minimap.instance.myCam.enabled = false;
            }
            if (!SettingsManager.GeneralSettings.MinimapEnabled.Value || SettingsManager.LegacyGameSettings.GlobalMinimapDisable.Value)
            {
                minimap.SetEnabled(false);
                Minimap.instance.myCam.gameObject.SetActive(false);
            }
            else
            {
                Minimap.instance.myCam.gameObject.SetActive(true);
                minimap.CreateMinimap(Minimap.instance.myCam, 0x200, 0.3f, info.minimapPreset);
            }
        }
    }

    public void createSnapShotRT2()
    {
        if (this.snapshotRT != null)
        {
            this.snapshotRT.Release();
        }
        if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
        {
            snapShotCamera.SetActive(true);
            this.snapshotRT = new RenderTexture((int)(Screen.width * 0.4f), (int)(Screen.height * 0.4f), 0x18);
            this.snapShotCamera.GetComponent<Camera>().targetTexture = this.snapshotRT;
        }
        else
            snapShotCamera.SetActive(false);
    }

    private GameObject findNearestTitan()
    {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag("titan");
        GameObject obj2 = null;
        float num2 = this.closestDistance = float.PositiveInfinity;
        Vector3 position = this.main_object.transform.position;
        foreach (GameObject obj3 in objArray)
        {
            Vector3 vector2 = obj3.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position - position;
            float magnitude = vector2.magnitude;
            if ((magnitude < num2) && ((obj3.GetComponent<TITAN>() == null) || !obj3.GetComponent<TITAN>().hasDie))
            {
                obj2 = obj3;
                num2 = magnitude;
                this.closestDistance = num2;
            }
        }
        return obj2;
    }

    public void flashBlind()
    {
        GameObject.Find("flash").GetComponent<UISprite>().alpha = 1f;
        this.flashDuration = 2f;
    }

    private float getSensitivityMultiWithDeltaTime(float sensitivity)
    {
        return ((sensitivity * Time.deltaTime) * 62f);
    }

    private void reset()
    {
        if (gametype == GAMETYPE.SINGLE)
        {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().restartGameSingle2();
        }
    }

    private Texture2D RTImage2(Camera cam)
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D textured = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, false);
        int num = (int) (cam.targetTexture.width * 0.04f);
        int destX = (int) (cam.targetTexture.width * 0.02f);
        try
        {
            textured.SetPixel(0, 0, Color.white);
            textured.ReadPixels(new Rect((float) num, (float) num, (float) (cam.targetTexture.width - num), (float) (cam.targetTexture.height - num)), destX, destX);
            RenderTexture.active = active;
        }
        catch
        {
            textured = new Texture2D(1, 1);
            textured.SetPixel(0, 0, Color.white);
            return textured;
        }
        return textured;
    }

    public void UpdateSnapshotSkybox()
    {
        this.snapShotCamera.gameObject.GetComponent<Skybox>().material = base.gameObject.GetComponent<Skybox>().material;
    }

    private void UpdateBottomRightText()
    {
        if (_bottomRightText == null)
        {
            GameObject obj = GameObject.Find("LabelInfoBottomRight");
            if (obj != null)
                _bottomRightText = obj.GetComponent<UILabel>();
        }
        if (_bottomRightText != null)
        {
            _bottomRightText.text = "Pause : " + SettingsManager.InputSettings.General.Pause.ToString() + " ";
            if (SettingsManager.UISettings.ShowInterpolation.Value && main_object != null)
            {
                HERO hero = main_object.GetComponent<HERO>();
                if (hero != null && hero.baseRigidBody.interpolation == RigidbodyInterpolation.Interpolate)
                    _bottomRightText.text = "Interpolation : ON \n" + _bottomRightText.text;
                else
                    _bottomRightText.text = "Interpolation: OFF \n" + _bottomRightText.text;
            }
        }
    }

    public void setHUDposition()
    {
        GameObject.Find("Flare").transform.localPosition = new Vector3((float) (((int) (-Screen.width * 0.5f)) + 14), (float) ((int) (-Screen.height * 0.5f)), 0f);
        GameObject obj2 = GameObject.Find("LabelInfoBottomRight");
        obj2.transform.localPosition = new Vector3((float) ((int) (Screen.width * 0.5f)), (float) ((int) (-Screen.height * 0.5f)), 0f);
        GameObject.Find("LabelInfoTopCenter").transform.localPosition = new Vector3(0f, (float) ((int) (Screen.height * 0.5f)), 0f);
        GameObject.Find("LabelInfoTopRight").transform.localPosition = new Vector3((float) ((int) (Screen.width * 0.5f)), (float) ((int) (Screen.height * 0.5f)), 0f);
        GameObject.Find("LabelNetworkStatus").transform.localPosition = new Vector3((float) ((int) (-Screen.width * 0.5f)), (float) ((int) (Screen.height * 0.5f)), 0f);
        GameObject.Find("LabelInfoTopLeft").transform.localPosition = new Vector3((float) ((int) (-Screen.width * 0.5f)), (float) ((int) ((Screen.height * 0.5f) - 20f)), 0f);
        GameObject.Find("Chatroom").transform.localPosition = new Vector3((float) ((int) (-Screen.width * 0.5f)), (float) ((int) (-Screen.height * 0.5f)), 0f);
        if (GameObject.Find("Chatroom") != null)
        {
            GameObject.Find("Chatroom").GetComponent<InRoomChat>().setPosition();
        }
        if (!usingTitan || (gametype == GAMETYPE.SINGLE))
        {
            GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0f, (float) ((int) ((-Screen.height * 0.5f) + 5f)), 0f);
            GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
            GameObject.Find("stamina_titan").transform.localPosition = new Vector3(0f, 9999f, 0f);
            GameObject.Find("stamina_titan_bottom").transform.localPosition = new Vector3(0f, 9999f, 0f);
        }
        else
        {
            Vector3 vector = new Vector3(0f, 9999f, 0f);
            GameObject.Find("skill_cd_bottom").transform.localPosition = vector;
            GameObject.Find("skill_cd_armin").transform.localPosition = vector;
            GameObject.Find("skill_cd_eren").transform.localPosition = vector;
            GameObject.Find("skill_cd_jean").transform.localPosition = vector;
            GameObject.Find("skill_cd_levi").transform.localPosition = vector;
            GameObject.Find("skill_cd_marco").transform.localPosition = vector;
            GameObject.Find("skill_cd_mikasa").transform.localPosition = vector;
            GameObject.Find("skill_cd_petra").transform.localPosition = vector;
            GameObject.Find("skill_cd_sasha").transform.localPosition = vector;
            GameObject.Find("GasUI").transform.localPosition = vector;
            GameObject.Find("stamina_titan").transform.localPosition = new Vector3(-160f, (float) ((int) ((-Screen.height * 0.5f) + 15f)), 0f);
            GameObject.Find("stamina_titan_bottom").transform.localPosition = new Vector3(-160f, (float) ((int) ((-Screen.height * 0.5f) + 15f)), 0f);
        }
        if ((this.main_object != null) && (this.main_object.GetComponent<HERO>() != null))
        {
            if (gametype == GAMETYPE.SINGLE)
            {
                this.main_object.GetComponent<HERO>().setSkillHUDPosition2();
            }
            else if ((this.main_object.GetPhotonView() != null) && this.main_object.GetPhotonView().isMine)
            {
                this.main_object.GetComponent<HERO>().setSkillHUDPosition2();
            }
        }
        if (stereoType == STEREO_3D_TYPE.SIDE_BY_SIDE)
        {
            base.gameObject.GetComponent<Camera>().aspect = Screen.width / Screen.height;
        }
        this.createSnapShotRT2();
    }

    public GameObject setMainObject(GameObject obj, bool resetRotation = true, bool lockAngle = false)
    {
        this.main_object = obj;
        if (obj == null)
        {
            this.head = null;
            this.distanceMulti = this.heightMulti = 1f;
        }
        else if (this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            this.head = this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
            this.distanceMulti = (this.head != null) ? (Vector3.Distance(this.head.transform.position, this.main_object.transform.position) * 0.2f) : 1f;
            this.heightMulti = (this.head != null) ? (Vector3.Distance(this.head.transform.position, this.main_object.transform.position) * 0.33f) : 1f;
            if (resetRotation)
            {
                base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else if (this.main_object.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            this.head = this.main_object.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
            this.distanceMulti = this.heightMulti = 0.64f;
            if (resetRotation)
            {
                base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else
        {
            this.head = null;
            this.distanceMulti = this.heightMulti = 1f;
            if (resetRotation)
            {
                base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        this.lockAngle = lockAngle;
        return obj;
    }

    public GameObject setMainObjectASTITAN(GameObject obj)
    {
        this.main_object = obj;
        if (this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            this.head = this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
            this.distanceMulti = (this.head != null) ? (Vector3.Distance(this.head.transform.position, this.main_object.transform.position) * 0.4f) : 1f;
            this.heightMulti = (this.head != null) ? (Vector3.Distance(this.head.transform.position, this.main_object.transform.position) * 0.45f) : 1f;
            base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        return obj;
    }

    public void setSpectorMode(bool val)
    {
        this.spectatorMode = val;
        GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = !val;
        GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = !val;
    }

    private void shakeUpdate()
    {
        if (this.duration > 0f)
        {
            this.duration -= Time.deltaTime;
            if (this.flip)
            {
                Transform transform = base.gameObject.transform;
                transform.position += (Vector3) (Vector3.up * this.R);
            }
            else
            {
                Transform transform2 = base.gameObject.transform;
                transform2.position -= (Vector3) (Vector3.up * this.R);
            }
            this.flip = !this.flip;
            this.R *= this.decay;
        }
    }

    private void Start()
    {
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addCamera(this);
        this.locker = GameObject.Find("locker");
        cameraDistance = SettingsManager.GeneralSettings.CameraDistance.Value + 0.3f;
        camera.farClipPlane = SettingsManager.GraphicsSettings.RenderDistance.Value;
        this.createSnapShotRT2();
    }

    public void startShake(float R, float duration, float decay = 0.95f)
    {
        if (this.duration < duration)
        {
            this.R = R;
            this.duration = duration;
            this.decay = decay;
        }
    }

    public void startSnapShot2(Vector3 p, int dmg, GameObject target, float startTime)
    {
        if (!snapShotCamera.activeSelf)
            return;
        if (dmg >= SettingsManager.GeneralSettings.SnapshotsMinimumDamage.Value)
        {
            StartCoroutine(CreateSnapshot(p, dmg, target, startTime));
        }
    }

    private IEnumerator CreateSnapshot(Vector3 position, int damage, GameObject target, float startTime)
    {
        UITexture display = GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>();
        yield return new WaitForSeconds(startTime);
        SetSnapshotPosition(target, position);
        Texture2D snapshot = RTImage2(this.snapShotCamera.GetComponent<Camera>());
        yield return new WaitForSeconds(0.2f);
        snapshot.Apply();
        display.mainTexture = snapshot;
        display.transform.localScale = new Vector3(Screen.width * 0.4f, Screen.height * 0.4f, 1f);
        display.transform.localPosition = new Vector3(-Screen.width * 0.225f, Screen.height * 0.225f, 0f);
        display.transform.rotation = Quaternion.Euler(0f, 0f, 10f);
        if (SettingsManager.GeneralSettings.SnapshotsShowInGame.Value)
            display.enabled = true;
        else
            display.enabled = false;
        yield return new WaitForSeconds(0.2f);
        SnapshotManager.AddSnapshot(snapshot, damage);
        yield return new WaitForSeconds(2f);
        display.enabled = false;
        Destroy(snapshot);
    }

    private void SetSnapshotPosition(GameObject target, Vector3 snapshotPosition)
    {
        Vector3 vector;
        RaycastHit hit;
        this.snapShotCamera.transform.position = (this.head == null) ? this.main_object.transform.position : this.head.transform.position;
        Transform transform = this.snapShotCamera.transform;
        transform.position += (Vector3)(Vector3.up * this.heightMulti);
        Transform transform2 = this.snapShotCamera.transform;
        transform2.position -= (Vector3)(Vector3.up * 1.1f);
        Vector3 worldPosition = vector = this.snapShotCamera.transform.position;
        Vector3 vector3 = (Vector3)((worldPosition + snapshotPosition) * 0.5f);
        this.snapShotCamera.transform.position = vector3;
        worldPosition = vector3;
        this.snapShotCamera.transform.LookAt(snapshotPosition);
        this.snapShotCamera.transform.RotateAround(base.transform.position, Vector3.up, UnityEngine.Random.Range((float)-20f, (float)20f));
        this.snapShotCamera.transform.LookAt(worldPosition);
        this.snapShotCamera.transform.RotateAround(worldPosition, base.transform.right, UnityEngine.Random.Range((float)-20f, (float)20f));
        float num = Vector3.Distance(snapshotPosition, vector);
        if ((target != null) && (target.GetComponent<TITAN>() != null))
        {
            num += (target.transform.localScale.x) * 15f;
        }
        Transform transform3 = this.snapShotCamera.transform;
        transform3.position -= (Vector3)(this.snapShotCamera.transform.forward * UnityEngine.Random.Range((float)(num + 3f), (float)(num + 10f)));
        this.snapShotCamera.transform.LookAt(worldPosition);
        this.snapShotCamera.transform.RotateAround(worldPosition, base.transform.forward, UnityEngine.Random.Range((float)-30f, (float)30f));
        Vector3 end = (this.head == null) ? this.main_object.transform.position : this.head.transform.position;
        Vector3 vector5 = ((this.head == null) ? this.main_object.transform.position : this.head.transform.position) - this.snapShotCamera.transform.position;
        end -= vector5;
        LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask | mask2;
        if (this.head != null)
        {
            if (Physics.Linecast(this.head.transform.position, end, out hit, (int)mask))
            {
                this.snapShotCamera.transform.position = hit.point;
            }
            else if (Physics.Linecast(this.head.transform.position - ((Vector3)((vector5 * this.distanceMulti) * 3f)), end, out hit, (int)mask3))
            {
                this.snapShotCamera.transform.position = hit.point;
            }
        }
        else if (Physics.Linecast(this.main_object.transform.position + Vector3.up, end, out hit, (int)mask3))
        {
            this.snapShotCamera.transform.position = hit.point;
        }
    }

    public void update2()
    {
        UpdateBottomRightText();
        if (this.flashDuration > 0f)
        {
            this.flashDuration -= Time.deltaTime;
            if (this.flashDuration <= 0f)
            {
                this.flashDuration = 0f;
            }
            GameObject.Find("flash").GetComponent<UISprite>().alpha = this.flashDuration * 0.5f;
        }
        if (gametype == GAMETYPE.STOP)
        {
        }
        else
        {
            if ((gametype != GAMETYPE.SINGLE) && this.gameOver)
            {
                if (SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown())
                {
                    if (this.spectatorMode)
                    {
                        this.setSpectorMode(false);
                    }
                    else
                    {
                        this.setSpectorMode(true);
                    }
                }
                if (SettingsManager.InputSettings.General.SpectateNextPlayer.GetKeyDown())
                {
                    this.currentPeekPlayerIndex++;
                    int length = GameObject.FindGameObjectsWithTag("Player").Length;
                    if (this.currentPeekPlayerIndex >= length)
                    {
                        this.currentPeekPlayerIndex = 0;
                    }
                    if (length > 0)
                    {
                        this.setMainObject(GameObject.FindGameObjectsWithTag("Player")[this.currentPeekPlayerIndex], true, false);
                        this.setSpectorMode(false);
                        this.lockAngle = false;
                    }
                }
                if (SettingsManager.InputSettings.General.SpectatePreviousPlayer.GetKeyDown())
                {
                    this.currentPeekPlayerIndex--;
                    int length = GameObject.FindGameObjectsWithTag("Player").Length;
                    if (this.currentPeekPlayerIndex >= length)
                    {
                        this.currentPeekPlayerIndex = 0;
                    }
                    if (this.currentPeekPlayerIndex < 0)
                    {
                        this.currentPeekPlayerIndex = length - 1;
                    }
                    if (length > 0)
                    {
                        this.setMainObject(GameObject.FindGameObjectsWithTag("Player")[this.currentPeekPlayerIndex], true, false);
                        this.setSpectorMode(false);
                        this.lockAngle = false;
                    }
                }
                if (this.spectatorMode)
                {
                    return;
                }
            }
            if (GameMenu.Paused)
            {
                if (this.main_object != null)
                {
                    Vector3 position = base.transform.position;
                    position = (this.head == null) ? this.main_object.transform.position : this.head.transform.position;
                    position += (Vector3)(Vector3.up * this.heightMulti);
                    base.transform.position = Vector3.Lerp(base.transform.position, position - ((Vector3)(base.transform.forward * 5f)), 0.2f);
                }
                return;
            }
            if (SettingsManager.InputSettings.General.Pause.GetKeyDown())
            {
                GameMenu.TogglePause(true);
            }
            if (needSetHUD)
            {
                needSetHUD = false;
                this.setHUDposition();
            }
            if (SettingsManager.InputSettings.General.ToggleFullscreen.GetKeyDown())
            {
                FullscreenHandler.ToggleFullscreen();
                needSetHUD = true;
            }
            if (SettingsManager.InputSettings.General.RestartGame.GetKeyDown())
            {
                float timeDiff = Time.realtimeSinceStartup - _lastRestartTime;
                if (gametype != GAMETYPE.SINGLE && PhotonNetwork.isMasterClient && timeDiff > 2f)
                {
                    _lastRestartTime = Time.realtimeSinceStartup;
                    object[] objArray = new object[] { "<color=#FFCC00>MasterClient has restarted the game!</color>", "" };
                    FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
                    FengGameManagerMKII.instance.restartRC();
                }
            }
            if (SettingsManager.InputSettings.General.RestartGame.GetKeyDown() || SettingsManager.InputSettings.General.ChangeCharacter.GetKeyDown())
            {
                this.reset();
            }
            if (this.main_object != null)
            {
                RaycastHit hit;
                if (SettingsManager.InputSettings.General.ChangeCamera.GetKeyDown())
                {
                    if (cameraMode == CAMERA_TYPE.ORIGINAL)
                    {
                        cameraMode = CAMERA_TYPE.WOW;
                    }
                    else if (cameraMode == CAMERA_TYPE.WOW)
                    {
                        cameraMode = CAMERA_TYPE.TPS;
                    }
                    else if (cameraMode == CAMERA_TYPE.TPS)
                    {
                        cameraMode = CAMERA_TYPE.ORIGINAL;
                    }
                    this.verticalRotationOffset = 0f;
                }
                if (SettingsManager.InputSettings.General.HideUI.GetKeyDown())
                {
                    GameMenu.HideCrosshair = !GameMenu.HideCrosshair;
                }
                if (SettingsManager.InputSettings.Human.FocusTitan.GetKeyDown())
                {
                    triggerAutoLock = !triggerAutoLock;
                    if (triggerAutoLock)
                    {
                        this.lockTarget = this.findNearestTitan();
                        if (this.closestDistance >= 150f)
                        {
                            this.lockTarget = null;
                            triggerAutoLock = false;
                        }
                    }
                }
                if (this.gameOver && (this.main_object != null))
                {
                    if (SettingsManager.InputSettings.General.SpectateToggleLive.GetKeyDown())
                    {
                        SettingsManager.LegacyGeneralSettings.LiveSpectate.Value = !SettingsManager.LegacyGeneralSettings.LiveSpectate.Value;
                    }
                    HERO component = this.main_object.GetComponent<HERO>();
                    if (component != null && SettingsManager.LegacyGeneralSettings.LiveSpectate.Value && component.GetComponent<SmoothSyncMovement>().enabled && component.isPhotonCamera)
                    {
                        this.CameraMovementLive(component);
                    }
                    else if (this.lockAngle)
                    {
                        base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.main_object.transform.rotation, 0.2f);
                        base.transform.position = Vector3.Lerp(base.transform.position, this.main_object.transform.position - ((Vector3) (this.main_object.transform.forward * 5f)), 0.2f);
                    }
                    else
                    {
                        this.camareMovement();
                    }
                }
                else
                {
                    this.camareMovement();
                }
                if (triggerAutoLock && (this.lockTarget != null))
                {
                    float z = base.transform.eulerAngles.z;
                    Transform transform = this.lockTarget.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                    Vector3 vector2 = transform.position - ((this.head == null) ? this.main_object.transform.position : this.head.transform.position);
                    vector2.Normalize();
                    this.lockCameraPosition = (this.head == null) ? this.main_object.transform.position : this.head.transform.position;
                    this.lockCameraPosition -= (Vector3) (((vector2 * this.distance) * this.distanceMulti) * this.distanceOffsetMulti);
                    this.lockCameraPosition += (Vector3) (((Vector3.up * 3f) * this.heightMulti) * this.distanceOffsetMulti);
                    base.transform.position = Vector3.Lerp(base.transform.position, this.lockCameraPosition, Time.deltaTime * 4f);
                    if (this.head != null)
                    {
                        base.transform.LookAt((Vector3) ((this.head.transform.position * 0.8f) + (transform.position * 0.2f)));
                    }
                    else
                    {
                        base.transform.LookAt((Vector3) ((this.main_object.transform.position * 0.8f) + (transform.position * 0.2f)));
                    }
                    base.transform.localEulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, z);
                    Vector2 vector3 = base.camera.WorldToScreenPoint(transform.position - ((Vector3) (transform.forward * this.lockTarget.transform.localScale.x)));
                    this.locker.transform.localPosition = new Vector3(vector3.x - (Screen.width * 0.5f), vector3.y - (Screen.height * 0.5f), 0f);
                    if ((this.lockTarget.GetComponent<TITAN>() != null) && this.lockTarget.GetComponent<TITAN>().hasDie)
                    {
                        this.lockTarget = null;
                    }
                }
                else
                {
                    this.locker.transform.localPosition = new Vector3(0f, (-Screen.height * 0.5f) - 50f, 0f);
                }
                Vector3 end = (this.head == null) ? this.main_object.transform.position : this.head.position;
                Vector3 vector5 = ((this.head == null) ? this.main_object.transform.position : this.head.position) - _transform.position;
                Vector3 normalized = vector5.normalized;
                end -= (Vector3) ((this.distance * normalized) * this.distanceMulti);
                LayerMask mask = ((int) 1) << PhysicsLayer.Ground;
                LayerMask mask2 = ((int) 1) << PhysicsLayer.EnemyBox;
                LayerMask mask3 = mask | mask2;
                if (this.head != null)
                {
                    if (Physics.Linecast(this.head.position, end, out hit, (int) mask))
                    {
                        _transform.position = hit.point;
                    }
                    else if (Physics.Linecast(this.head.position - ((Vector3) ((normalized * this.distanceMulti) * 3f)), end, out hit, (int) mask2))
                    {
                        _transform.position = hit.point;
                    }
                }
                else if (Physics.Linecast(this.main_object.transform.position + Vector3.up, end, out hit, (int) mask3))
                {
                    _transform.position = hit.point;
                }
                this.shakeUpdate();
            }
        }
    }

    public enum RotationAxes
    {
        MouseXAndY,
        MouseX,
        MouseY
    }
}

