using ExitGames.Client.Photon;
using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;
using Settings;
using ApplicationManagers;
using CustomSkins;
using UI;
using Weather;
using GameManagers;

public class FengGameManagerMKII : Photon.MonoBehaviour
{
    public static bool JustLeftRoom = false;
    public Dictionary<int, CannonValues> allowedToCannon;
    public Dictionary<string, Texture2D> assetCacheTextures;
    public static ExitGames.Client.Photon.Hashtable banHash;
    public static ExitGames.Client.Photon.Hashtable boolVariables;
    public static Dictionary<string, GameObject> CachedPrefabs;
    private ArrayList chatContent;
    public InRoomChat chatRoom;
    public GameObject checkpoint;
    private ArrayList cT;
    public static string currentLevel;
    private float currentSpeed;
    public static bool customLevelLoaded;
    public int cyanKills;
    public int difficulty;
    public float distanceSlider;
    private bool endRacing;
    private ArrayList eT;
    public static ExitGames.Client.Photon.Hashtable floatVariables;
    private ArrayList fT;
    private float gameEndCD;
    private float gameEndTotalCDtime = 9f;
    public bool gameStart;
    private bool gameTimesUp;
    public static ExitGames.Client.Photon.Hashtable globalVariables;
    public List<GameObject> groundList;
    public static bool hasLogged;
    private ArrayList heroes;
    public static ExitGames.Client.Photon.Hashtable heroHash;
    private int highestwave = 1;
    private ArrayList hooks;
    private int humanScore;
    public static List<int> ignoreList;
    public static ExitGames.Client.Photon.Hashtable imatitan;
    public static FengGameManagerMKII instance;
    public static ExitGames.Client.Photon.Hashtable intVariables;
    public static bool isAssetLoaded;
    public bool isFirstLoad;
    private bool isLosing;
    private bool isPlayer1Winning;
    private bool isPlayer2Winning;
    public bool isRecompiling;
    public bool isRestarting;
    public bool isSpawning;
    public bool isUnloading;
    private bool isWinning;
    public bool justSuicide;
    private ArrayList kicklist;
    private ArrayList killInfoGO = new ArrayList();
    public static bool LAN;
    public static string level = string.Empty;
    public List<string[]> levelCache;
    public static ExitGames.Client.Photon.Hashtable[] linkHash;
    private string localRacingResult;
    public static bool logicLoaded;
    public static int loginstate;
    public int magentaKills;
    private IN_GAME_MAIN_CAMERA mainCamera;
    public static bool masterRC;
    public int maxPlayers;
    private float maxSpeed;
    public float mouseSlider;
    private string myLastHero;
    private string myLastRespawnTag = "playerRespawn";
    public float myRespawnTime;
    public string name;
    public static string nameField;
    public bool needChooseSide;
    public static bool noRestart;
    public static string oldScript;
    public static string oldScriptLogic;
    public static string passwordField;
    public float pauseWaitTime;
    public string playerList;
    public List<Vector3> playerSpawnsC;
    public List<Vector3> playerSpawnsM;
    public List<PhotonPlayer> playersRPC;
    public static ExitGames.Client.Photon.Hashtable playerVariables;
    public Dictionary<string, int[]> PreservedPlayerKDR;
    public static string PrivateServerAuthPass;
    public static string privateServerField;
    public static string privateLobbyField;
    public int PVPhumanScore;
    private int PVPhumanScoreMax = 200;
    public int PVPtitanScore;
    private int PVPtitanScoreMax = 200;
    public float qualitySlider;
    public List<GameObject> racingDoors;
    private ArrayList racingResult;
    public Vector3 racingSpawnPoint;
    public bool racingSpawnPointSet;
    public static AssetBundle RCassets;
    public static ExitGames.Client.Photon.Hashtable RCEvents;
    public static ExitGames.Client.Photon.Hashtable RCRegions;
    public static ExitGames.Client.Photon.Hashtable RCRegionTriggers;
    public static ExitGames.Client.Photon.Hashtable RCVariableNames;
    public List<float> restartCount;
    public bool restartingBomb;
    public bool restartingEren;
    public bool restartingHorse;
    public bool restartingMC;
    public bool restartingTitan;
    public float retryTime;
    public float roundTime;
    public Vector2 scroll;
    public Vector2 scroll2;
    public GameObject selectedObj;
    public static object[] settingsOld;
    private int single_kills;
    private int single_maxDamage;
    private int single_totalDamage;
    public List<GameObject> spectateSprites;
    private bool startRacing;
    public static ExitGames.Client.Photon.Hashtable stringVariables;
    private int[] teamScores;
    private int teamWinner;
    public Texture2D textureBackgroundBlack;
    public Texture2D textureBackgroundBlue;
    public int time = 600;
    private float timeElapse;
    private float timeTotalServer;
    private ArrayList titans;
    private int titanScore;
    public List<TitanSpawner> titanSpawners;
    public List<Vector3> titanSpawns;
    public static ExitGames.Client.Photon.Hashtable titanVariables;
    public float transparencySlider;
    public GameObject ui;
    public float updateTime;
    public static string usernameField;
    public int wave = 1;
    public Dictionary<string, Material> customMapMaterials;
    public float LastRoomPropertyCheckTime = 0f;
    private SkyboxCustomSkinLoader _skyboxCustomSkinLoader;
    private ForestCustomSkinLoader _forestCustomSkinLoader;
    private CityCustomSkinLoader _cityCustomSkinLoader;
    private CustomLevelCustomSkinLoader _customLevelCustomSkinLoader;

    public void OnJoinedLobby()
    {
        if (JustLeftRoom)
        {
            PhotonNetwork.Disconnect();
            JustLeftRoom = false;
        }
        else if (UIManager.CurrentMenu != null && UIManager.CurrentMenu.GetComponent<MainMenu>() != null)
            UIManager.CurrentMenu.GetComponent<MainMenu>().ShowMultiplayerRoomListPopup();
    }

    private void Awake()
    {
        _skyboxCustomSkinLoader = gameObject.AddComponent<SkyboxCustomSkinLoader>();
        _forestCustomSkinLoader = gameObject.AddComponent<ForestCustomSkinLoader>();
        _cityCustomSkinLoader = gameObject.AddComponent<CityCustomSkinLoader>();
        _customLevelCustomSkinLoader = gameObject.AddComponent<CustomLevelCustomSkinLoader>();
        gameObject.AddComponent<CustomRPCManager>();
    }

    private string getMaterialHash(string material, string x, string y)
    {
        return material + "," + x + "," + y;
    }

    public void addCamera(IN_GAME_MAIN_CAMERA c)
    {
        this.mainCamera = c;
    }

    public void addCT(COLOSSAL_TITAN titan)
    {
        this.cT.Add(titan);
    }

    public void addET(TITAN_EREN hero)
    {
        this.eT.Add(hero);
    }

    public void addFT(FEMALE_TITAN titan)
    {
        this.fT.Add(titan);
    }

    public void addHero(HERO hero)
    {
        this.heroes.Add(hero);
    }

    public void addHook(Bullet h)
    {
        this.hooks.Add(h);
    }

    public void addTime(float time)
    {
        this.timeTotalServer -= time;
    }

    public void addTitan(TITAN titan)
    {
        this.titans.Add(titan);
    }

    private void cache()
    {
        ClothFactory.ClearClothCache();
        this.chatRoom = GameObject.Find("Chatroom").GetComponent<InRoomChat>();
        this.playersRPC.Clear();
        this.titanSpawners.Clear();
        this.groundList.Clear();
        this.PreservedPlayerKDR = new Dictionary<string, int[]>();
        noRestart = false;
        this.isSpawning = false;
        this.retryTime = 0f;
        logicLoaded = false;
        customLevelLoaded = true;
        this.isUnloading = false;
        this.isRecompiling = false;
        Time.timeScale = 1f;
        Camera.main.farClipPlane = 1500f;
        this.pauseWaitTime = 0f;
        this.spectateSprites = new List<GameObject>();
        this.isRestarting = false;
        if (PhotonNetwork.isMasterClient)
        {
            base.StartCoroutine(this.WaitAndResetRestarts());
        }
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            this.roundTime = 0f;
            if (level.StartsWith("Custom"))
            {
                customLevelLoaded = false;
            }
            if (PhotonNetwork.isMasterClient)
            {
                if (this.isFirstLoad)
                {
                    this.setGameSettings(this.checkGameGUI());
                }
                if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
                {
                    base.StartCoroutine(this.respawnE((float) SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value));
                }
            }
        }
        if (SettingsManager.UISettings.GameFeed.Value)
        {
            this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round Start.");
        }
        this.isFirstLoad = false;
        this.RecompilePlayerList(0.5f);
    }

    [RPC]
    private void Chat(string content, string sender, PhotonMessageInfo info)
    {
        if (sender != string.Empty)
        {
            content = sender + ":" + content;
        }
        content = "<color=#FFC000>[" + Convert.ToString(info.sender.ID) + "]</color> " + content;
        this.chatRoom.addLINE(content);
    }

    [RPC]
    private void ChatPM(string sender, string content, PhotonMessageInfo info)
    {
        content = sender + ":" + content;
        content = "<color=#FFC000>FROM [" + Convert.ToString(info.sender.ID) + "]</color> " + content;
        this.chatRoom.addLINE(content);
    }

    private ExitGames.Client.Photon.Hashtable checkGameGUI()
    {
        int num2;
        PhotonPlayer player;
        int num4;
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        LegacyGameSettings settings = SettingsManager.LegacyGameSettingsUI;
        if (settings.InfectionModeEnabled.Value)
        {
            settings.BombModeEnabled.Value = false;
            settings.TeamMode.Value = 0;
            settings.PointModeEnabled.Value = false;
            settings.BladePVP.Value = 0;
            if (settings.InfectionModeAmount.Value > PhotonNetwork.countOfPlayers)
            {
                settings.InfectionModeAmount.Value = 1;
            }
            hashtable.Add("infection", settings.InfectionModeAmount.Value);
            if (!SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value || 
                SettingsManager.LegacyGameSettings.InfectionModeAmount.Value != settings.InfectionModeAmount.Value)
            {
                imatitan.Clear();
                for (num2 = 0; num2 < PhotonNetwork.playerList.Length; num2++)
                {
                    player = PhotonNetwork.playerList[num2];
                    ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.isTitan, 1);
                    player.SetCustomProperties(propertiesToSet);
                }
                int length = PhotonNetwork.playerList.Length;
                num4 = settings.InfectionModeAmount.Value;
                for (num2 = 0; num2 < PhotonNetwork.playerList.Length; num2++)
                {
                    PhotonPlayer player2 = PhotonNetwork.playerList[num2];
                    if ((length > 0) && (UnityEngine.Random.Range((float)0f, (float)1f) <= (((float)num4) / ((float)length))))
                    {
                        ExitGames.Client.Photon.Hashtable hashtable3 = new ExitGames.Client.Photon.Hashtable();
                        hashtable3.Add(PhotonPlayerProperty.isTitan, 2);
                        player2.SetCustomProperties(hashtable3);
                        imatitan.Add(player2.ID, 2);
                        num4--;
                    }
                    length--;
                }
            }
        }
        if (settings.BombModeEnabled.Value)
        {
            hashtable.Add("bomb", 1);
        }
        if (settings.GlobalMinimapDisable.Value)
        {
            hashtable.Add("globalDisableMinimap", 1);
        }
        if (settings.TeamMode.Value > 0)
        {
            hashtable.Add("team", settings.TeamMode.Value);
            if (SettingsManager.LegacyGameSettings.TeamMode.Value != settings.TeamMode.Value)
            {
                num4 = 1;
                for (num2 = 0; num2 < PhotonNetwork.playerList.Length; num2++)
                {
                    player = PhotonNetwork.playerList[num2];
                    switch (num4)
                    {
                        case 1:
                            base.photonView.RPC("setTeamRPC", player, new object[] { 1 });
                            num4 = 2;
                            break;

                        case 2:
                            base.photonView.RPC("setTeamRPC", player, new object[] { 2 });
                            num4 = 1;
                            break;
                    }
                }
            }
        }
        if (settings.PointModeEnabled.Value)
        {
            hashtable.Add("point", settings.PointModeAmount.Value);
        }
        if (!settings.RockThrowEnabled.Value) // reversed for legacy compatibility
        {
            hashtable.Add("rock", 1);
        }
        if (settings.TitanExplodeEnabled.Value)
        {
            hashtable.Add("explode", settings.TitanExplodeRadius.Value);
        }
        if (settings.TitanHealthMode.Value > 0)
        {
            hashtable.Add("healthMode", settings.TitanHealthMode.Value);
            hashtable.Add("healthLower", settings.TitanHealthMin.Value);
            hashtable.Add("healthUpper", settings.TitanHealthMax.Value);
        }
        if (settings.KickShifters.Value)
        {
            hashtable.Add("eren", 1);
        }
        if (settings.TitanNumberEnabled.Value)
        {
            hashtable.Add("titanc", settings.TitanNumber.Value);
        }
        if (settings.TitanArmorEnabled.Value)
        {
            hashtable.Add("damage", settings.TitanArmor.Value);
        }
        if (settings.TitanSizeEnabled.Value)
        {
            hashtable.Add("sizeMode", 1);
            hashtable.Add("sizeLower", settings.TitanSizeMin.Value);
            hashtable.Add("sizeUpper", settings.TitanSizeMax.Value);
        }
        if (settings.TitanSpawnEnabled.Value)
        {
            if (settings.TitanSpawnNormal.Value + settings.TitanSpawnAberrant.Value + settings.TitanSpawnCrawler.Value + 
                settings.TitanSpawnJumper.Value + settings.TitanSpawnPunk.Value > 100f)
            {
                settings.TitanSpawnNormal.Value = 20f;
                settings.TitanSpawnAberrant.Value = 20f;
                settings.TitanSpawnCrawler.Value = 20f;
                settings.TitanSpawnJumper.Value = 20f;
                settings.TitanSpawnPunk.Value = 20f;
            }
            hashtable.Add("spawnMode", 1);
            hashtable.Add("nRate", settings.TitanSpawnNormal.Value);
            hashtable.Add("aRate", settings.TitanSpawnAberrant.Value);
            hashtable.Add("jRate", settings.TitanSpawnJumper.Value);
            hashtable.Add("cRate", settings.TitanSpawnCrawler.Value);
            hashtable.Add("pRate", settings.TitanSpawnPunk.Value);
        }
        if (settings.AllowHorses.Value)
        {
            hashtable.Add("horse", 1);
        }
        if (settings.TitanPerWavesEnabled.Value)
        {
            hashtable.Add("waveModeOn", 1);
            hashtable.Add("waveModeNum", settings.TitanPerWaves.Value);
        }
        if (settings.FriendlyMode.Value)
        {
            hashtable.Add("friendly", 1);
        }
        if (settings.BladePVP.Value > 0)
        {
            hashtable.Add("pvp", settings.BladePVP.Value);
        }
        if (settings.TitanMaxWavesEnabled.Value)
        {
            hashtable.Add("maxwave", settings.TitanMaxWaves.Value);
        }
        if (settings.EndlessRespawnEnabled.Value)
        {
            hashtable.Add("endless", settings.EndlessRespawnTime.Value);
        }
        if (settings.Motd.Value != string.Empty)
        {
            hashtable.Add("motd", settings.Motd.Value);
        }
        if (!settings.AHSSAirReload.Value) // reversed
        {
            hashtable.Add("ahssReload", 1);
        }
        if (!settings.PunksEveryFive.Value) // reversed
        {
            hashtable.Add("punkWaves", 1);
        }
        if (settings.CannonsFriendlyFire.Value)
        {
            hashtable.Add("deadlycannons", 1);
        }
        if (settings.RacingEndless.Value)
        {
            hashtable.Add("asoracing", 1);
        }
        LegacyGameSettings legacySettings = SettingsManager.LegacyGameSettings;
        legacySettings.PreserveKDR.Value = settings.PreserveKDR.Value;
        legacySettings.TitanSpawnCap.Value = settings.TitanSpawnCap.Value;
        legacySettings.GameType.Value = settings.GameType.Value;
        legacySettings.LevelScript.Value = settings.LevelScript.Value;
        legacySettings.LogicScript.Value = settings.LogicScript.Value;
        return hashtable;
    }

    private bool checkIsTitanAllDie()
    {
        foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        {
            if ((obj2.GetComponent<TITAN>() != null) && !obj2.GetComponent<TITAN>().hasDie)
            {
                return false;
            }
            if (obj2.GetComponent<FEMALE_TITAN>() != null)
            {
                return false;
            }
        }
        return true;
    }

    public void checkPVPpts()
    {
        if (this.PVPtitanScore >= this.PVPtitanScoreMax)
        {
            this.PVPtitanScore = this.PVPtitanScoreMax;
            this.gameLose2();
        }
        else if (this.PVPhumanScore >= this.PVPhumanScoreMax)
        {
            this.PVPhumanScore = this.PVPhumanScoreMax;
            this.gameWin2();
        }
    }

    [RPC]
    private void clearlevel(string[] link, int gametype, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            if (gametype == 0)
            {
                IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.KILL_TITAN;
            }
            else if (gametype == 1)
            {
                IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.SURVIVE_MODE;
            }
            else if (gametype == 2)
            {
                IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.PVP_AHSS;
            }
            else if (gametype == 3)
            {
                IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.RACING;
            }
            else if (gametype == 4)
            {
                IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.None;
            }
            if (info.sender.isMasterClient && (link.Length > 6))
            {
                base.StartCoroutine(this.clearlevelE(link));
            }
        }
    }

    private IEnumerator clearlevelE(string[] skybox)
    {
        if (IsValidSkybox(skybox))
        {
            yield return StartCoroutine(_skyboxCustomSkinLoader.LoadSkinsFromRPC(skybox));
            yield return StartCoroutine(_customLevelCustomSkinLoader.LoadSkinsFromRPC(skybox));
        }
        else
            SkyboxCustomSkinLoader.SkyboxMaterial = null;
        StartCoroutine(reloadSky());
    }

    public void compileScript(string str)
    {
        int num3;
        string[] strArray2 = str.Replace(" ", string.Empty).Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        int num = 0;
        int num2 = 0;
        bool flag = false;
        for (num3 = 0; num3 < strArray2.Length; num3++)
        {
            if (strArray2[num3] == "{")
            {
                num++;
            }
            else if (strArray2[num3] == "}")
            {
                num2++;
            }
            else
            {
                int num4 = 0;
                int num5 = 0;
                int num6 = 0;
                foreach (char ch in strArray2[num3])
                {
                    switch (ch)
                    {
                        case '(':
                            num4++;
                            break;

                        case ')':
                            num5++;
                            break;

                        case '"':
                            num6++;
                            break;
                    }
                }
                if (num4 != num5)
                {
                    int num8 = num3 + 1;
                    this.chatRoom.addLINE("Script Error: Parentheses not equal! (line " + num8.ToString() + ")");
                    flag = true;
                }
                if ((num6 % 2) != 0)
                {
                    this.chatRoom.addLINE("Script Error: Quotations not equal! (line " + ((num3 + 1)).ToString() + ")");
                    flag = true;
                }
            }
        }
        if (num != num2)
        {
            this.chatRoom.addLINE("Script Error: Bracket count not equivalent!");
            flag = true;
        }
        if (!flag)
        {
            try
            {
                int num10;
                num3 = 0;
                while (num3 < strArray2.Length)
                {
                    if (strArray2[num3].StartsWith("On") && (strArray2[num3 + 1] == "{"))
                    {
                        int key = num3;
                        num10 = num3 + 2;
                        int num11 = 0;
                        for (int i = num3 + 2; i < strArray2.Length; i++)
                        {
                            if (strArray2[i] == "{")
                            {
                                num11++;
                            }
                            if (strArray2[i] == "}")
                            {
                                if (num11 > 0)
                                {
                                    num11--;
                                }
                                else
                                {
                                    num10 = i - 1;
                                    i = strArray2.Length;
                                }
                            }
                        }
                        hashtable.Add(key, num10);
                        num3 = num10;
                    }
                    num3++;
                }
                foreach (int num9 in hashtable.Keys)
                {
                    int num14;
                    int num15;
                    string str4;
                    string str5;
                    RegionTrigger trigger;
                    string str3 = strArray2[num9];
                    num10 = (int) hashtable[num9];
                    string[] stringArray = new string[(num10 - num9) + 1];
                    int index = 0;
                    for (num3 = num9; num3 <= num10; num3++)
                    {
                        stringArray[index] = strArray2[num3];
                        index++;
                    }
                    RCEvent event2 = this.parseBlock(stringArray, 0, 0, null);
                    if (str3.StartsWith("OnPlayerEnterRegion"))
                    {
                        num14 = str3.IndexOf('[');
                        num15 = str3.IndexOf(']');
                        str4 = str3.Substring(num14 + 2, (num15 - num14) - 3);
                        num14 = str3.IndexOf('(');
                        num15 = str3.IndexOf(')');
                        str5 = str3.Substring(num14 + 2, (num15 - num14) - 3);
                        if (RCRegionTriggers.ContainsKey(str4))
                        {
                            trigger = (RegionTrigger) RCRegionTriggers[str4];
                            trigger.playerEventEnter = event2;
                            trigger.myName = str4;
                            RCRegionTriggers[str4] = trigger;
                        }
                        else
                        {
                            trigger = new RegionTrigger {
                                playerEventEnter = event2,
                                myName = str4
                            };
                            RCRegionTriggers.Add(str4, trigger);
                        }
                        RCVariableNames.Add("OnPlayerEnterRegion[" + str4 + "]", str5);
                    }
                    else if (str3.StartsWith("OnPlayerLeaveRegion"))
                    {
                        num14 = str3.IndexOf('[');
                        num15 = str3.IndexOf(']');
                        str4 = str3.Substring(num14 + 2, (num15 - num14) - 3);
                        num14 = str3.IndexOf('(');
                        num15 = str3.IndexOf(')');
                        str5 = str3.Substring(num14 + 2, (num15 - num14) - 3);
                        if (RCRegionTriggers.ContainsKey(str4))
                        {
                            trigger = (RegionTrigger) RCRegionTriggers[str4];
                            trigger.playerEventExit = event2;
                            trigger.myName = str4;
                            RCRegionTriggers[str4] = trigger;
                        }
                        else
                        {
                            trigger = new RegionTrigger {
                                playerEventExit = event2,
                                myName = str4
                            };
                            RCRegionTriggers.Add(str4, trigger);
                        }
                        RCVariableNames.Add("OnPlayerExitRegion[" + str4 + "]", str5);
                    }
                    else if (str3.StartsWith("OnTitanEnterRegion"))
                    {
                        num14 = str3.IndexOf('[');
                        num15 = str3.IndexOf(']');
                        str4 = str3.Substring(num14 + 2, (num15 - num14) - 3);
                        num14 = str3.IndexOf('(');
                        num15 = str3.IndexOf(')');
                        str5 = str3.Substring(num14 + 2, (num15 - num14) - 3);
                        if (RCRegionTriggers.ContainsKey(str4))
                        {
                            trigger = (RegionTrigger) RCRegionTriggers[str4];
                            trigger.titanEventEnter = event2;
                            trigger.myName = str4;
                            RCRegionTriggers[str4] = trigger;
                        }
                        else
                        {
                            trigger = new RegionTrigger {
                                titanEventEnter = event2,
                                myName = str4
                            };
                            RCRegionTriggers.Add(str4, trigger);
                        }
                        RCVariableNames.Add("OnTitanEnterRegion[" + str4 + "]", str5);
                    }
                    else if (str3.StartsWith("OnTitanLeaveRegion"))
                    {
                        num14 = str3.IndexOf('[');
                        num15 = str3.IndexOf(']');
                        str4 = str3.Substring(num14 + 2, (num15 - num14) - 3);
                        num14 = str3.IndexOf('(');
                        num15 = str3.IndexOf(')');
                        str5 = str3.Substring(num14 + 2, (num15 - num14) - 3);
                        if (RCRegionTriggers.ContainsKey(str4))
                        {
                            trigger = (RegionTrigger) RCRegionTriggers[str4];
                            trigger.titanEventExit = event2;
                            trigger.myName = str4;
                            RCRegionTriggers[str4] = trigger;
                        }
                        else
                        {
                            trigger = new RegionTrigger {
                                titanEventExit = event2,
                                myName = str4
                            };
                            RCRegionTriggers.Add(str4, trigger);
                        }
                        RCVariableNames.Add("OnTitanExitRegion[" + str4 + "]", str5);
                    }
                    else if (str3.StartsWith("OnFirstLoad()"))
                    {
                        RCEvents.Add("OnFirstLoad", event2);
                    }
                    else if (str3.StartsWith("OnRoundStart()"))
                    {
                        RCEvents.Add("OnRoundStart", event2);
                    }
                    else if (str3.StartsWith("OnUpdate()"))
                    {
                        RCEvents.Add("OnUpdate", event2);
                    }
                    else
                    {
                        string[] strArray4;
                        if (str3.StartsWith("OnTitanDie"))
                        {
                            num14 = str3.IndexOf('(');
                            num15 = str3.LastIndexOf(')');
                            strArray4 = str3.Substring(num14 + 1, (num15 - num14) - 1).Split(new char[] { ',' });
                            strArray4[0] = strArray4[0].Substring(1, strArray4[0].Length - 2);
                            strArray4[1] = strArray4[1].Substring(1, strArray4[1].Length - 2);
                            RCVariableNames.Add("OnTitanDie", strArray4);
                            RCEvents.Add("OnTitanDie", event2);
                        }
                        else if (str3.StartsWith("OnPlayerDieByTitan"))
                        {
                            RCEvents.Add("OnPlayerDieByTitan", event2);
                            num14 = str3.IndexOf('(');
                            num15 = str3.LastIndexOf(')');
                            strArray4 = str3.Substring(num14 + 1, (num15 - num14) - 1).Split(new char[] { ',' });
                            strArray4[0] = strArray4[0].Substring(1, strArray4[0].Length - 2);
                            strArray4[1] = strArray4[1].Substring(1, strArray4[1].Length - 2);
                            RCVariableNames.Add("OnPlayerDieByTitan", strArray4);
                        }
                        else if (str3.StartsWith("OnPlayerDieByPlayer"))
                        {
                            RCEvents.Add("OnPlayerDieByPlayer", event2);
                            num14 = str3.IndexOf('(');
                            num15 = str3.LastIndexOf(')');
                            strArray4 = str3.Substring(num14 + 1, (num15 - num14) - 1).Split(new char[] { ',' });
                            strArray4[0] = strArray4[0].Substring(1, strArray4[0].Length - 2);
                            strArray4[1] = strArray4[1].Substring(1, strArray4[1].Length - 2);
                            RCVariableNames.Add("OnPlayerDieByPlayer", strArray4);
                        }
                        else if (str3.StartsWith("OnChatInput"))
                        {
                            RCEvents.Add("OnChatInput", event2);
                            num14 = str3.IndexOf('(');
                            num15 = str3.LastIndexOf(')');
                            str5 = str3.Substring(num14 + 1, (num15 - num14) - 1);
                            RCVariableNames.Add("OnChatInput", str5.Substring(1, str5.Length - 2));
                        }
                    }
                }
            }
            catch (UnityException exception)
            {
                this.chatRoom.addLINE(exception.Message);
            }
        }
    }

    public int conditionType(string str)
    {
        if (!str.StartsWith("Int"))
        {
            if (str.StartsWith("Bool"))
            {
                return 1;
            }
            if (str.StartsWith("String"))
            {
                return 2;
            }
            if (str.StartsWith("Float"))
            {
                return 3;
            }
            if (str.StartsWith("Titan"))
            {
                return 5;
            }
            if (str.StartsWith("Player"))
            {
                return 4;
            }
        }
        return 0;
    }

    private void core2()
    {
        if (((int) settingsOld[0x40]) >= 100)
        {
            this.coreeditor();
        }
        else
        {
            if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && this.needChooseSide)
            {
                if (SettingsManager.InputSettings.Human.Flare1.GetKeyDown())
                {
                    if (NGUITools.GetActive(this.ui.GetComponent<UIReferArray>().panels[3]))
                    {
                        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[0], true);
                        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[1], false);
                        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[2], false);
                        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[3], false);
                        Camera.main.GetComponent<SpectatorMovement>().disable = false;
                        Camera.main.GetComponent<MouseLook>().disable = false;
                    }
                    else
                    {
                        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[0], false);
                        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[1], false);
                        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[2], false);
                        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[3], true);
                        Camera.main.GetComponent<SpectatorMovement>().disable = true;
                        Camera.main.GetComponent<MouseLook>().disable = true;
                    }
                }
                if (SettingsManager.InputSettings.General.Pause.GetKeyDown() && !GameMenu.Paused)
                {
                    Camera.main.GetComponent<SpectatorMovement>().disable = true;
                    Camera.main.GetComponent<MouseLook>().disable = true;
                    GameMenu.TogglePause(true);
                }
            }
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER))
            {
                int length;
                float num3;
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                {
                    this.coreadd();
                    this.ShowHUDInfoTopLeft(this.playerList);
                    if ((((Camera.main != null) && (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)) && (Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !this.needChooseSide)) && !SettingsManager.LegacyGeneralSettings.SpecMode.Value)
                    {
                        this.ShowHUDInfoCenter("Press [F7D358]" + SettingsManager.InputSettings.General.SpectateNextPlayer.ToString() + "[-] to spectate the next player. \nPress [F7D358]" + SettingsManager.InputSettings.General.SpectatePreviousPlayer.ToString() + "[-] to spectate the previous player.\nPress [F7D358]" + SettingsManager.InputSettings.Human.AttackSpecial.ToString() + "[-] to enter the spectator mode.\n\n\n\n");
                        if (((LevelInfo.getInfo(level).respawnMode == RespawnMode.DEATHMATCH) || (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)) || (((SettingsManager.LegacyGameSettings.BombModeEnabled.Value) || (SettingsManager.LegacyGameSettings.BladePVP.Value > 0)) && (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)))
                        {
                            this.myRespawnTime += Time.deltaTime;
                            int endlessMode = 5;
                            if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
                            {
                                endlessMode = 10;
                            }
                            if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
                            {
                                endlessMode = SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value;
                            }
                            length = endlessMode - ((int) this.myRespawnTime);
                            this.ShowHUDInfoCenterADD("Respawn in " + length.ToString() + "s.");
                            if (this.myRespawnTime > endlessMode)
                            {
                                this.myRespawnTime = 0f;
                                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                                if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
                                {
                                    this.SpawnNonAITitan2(this.myLastHero, "titanRespawn");
                                }
                                else
                                {
                                    base.StartCoroutine(this.WaitAndRespawn1(0.1f, this.myLastRespawnTag));
                                }
                                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                                this.ShowHUDInfoCenter(string.Empty);
                            }
                        }
                    }
                }
                else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
                    {
                        if (!this.isLosing)
                        {
                            this.currentSpeed = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity.magnitude;
                            this.maxSpeed = Mathf.Max(this.maxSpeed, this.currentSpeed);
                            this.ShowHUDInfoTopLeft(string.Concat(new object[] { "Current Speed : ", (int) this.currentSpeed, "\nMax Speed:", this.maxSpeed }));
                        }
                    }
                    else
                    {
                        this.ShowHUDInfoTopLeft(string.Concat(new object[] { "Kills:", this.single_kills, "\nMax Damage:", this.single_maxDamage, "\nTotal Damage:", this.single_totalDamage }));
                    }
                }
                if (this.isLosing && (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING))
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
                        {
                            this.ShowHUDInfoCenter(string.Concat(new object[] { "Survive ", this.wave, " Waves!\n Press ", SettingsManager.InputSettings.General.Restart.ToString(), " to Restart.\n\n\n" }));
                        }
                        else
                        {
                            this.ShowHUDInfoCenter("Humanity Fail!\n Press " + SettingsManager.InputSettings.General.Restart.ToString() + " to Restart.\n\n\n");
                        }
                    }
                    else
                    {
                        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
                        {
                            this.ShowHUDInfoCenter(string.Concat(new object[] { "Survive ", this.wave, " Waves!\nGame Restart in ", (int) this.gameEndCD, "s\n\n" }));
                        }
                        else
                        {
                            this.ShowHUDInfoCenter("Humanity Fail!\nAgain!\nGame Restart in " + ((int) this.gameEndCD) + "s\n\n");
                        }
                        if (this.gameEndCD <= 0f)
                        {
                            this.gameEndCD = 0f;
                            if (PhotonNetwork.isMasterClient)
                            {
                                this.restartRC();
                            }
                            this.ShowHUDInfoCenter(string.Empty);
                        }
                        else
                        {
                            this.gameEndCD -= Time.deltaTime;
                        }
                    }
                }
                if (this.isWinning)
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
                        {
                            num3 = (((int) (this.timeTotalServer * 10f)) * 0.1f) - 5f;
                            this.ShowHUDInfoCenter(num3.ToString() + "s !\n Press " + SettingsManager.InputSettings.General.Restart.ToString() + " to Restart.\n\n\n");
                        }
                        else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
                        {
                            this.ShowHUDInfoCenter("Survive All Waves!\n Press " + SettingsManager.InputSettings.General.Restart.ToString() + " to Restart.\n\n\n");
                        }
                        else
                        {
                            this.ShowHUDInfoCenter("Humanity Win!\n Press " + SettingsManager.InputSettings.General.Restart.ToString() + " to Restart.\n\n\n");
                        }
                    }
                    else
                    {
                        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
                        {
                            this.ShowHUDInfoCenter(string.Concat(new object[] { this.localRacingResult, "\n\nGame Restart in ", (int) this.gameEndCD, "s" }));
                        }
                        else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
                        {
                            this.ShowHUDInfoCenter("Survive All Waves!\nGame Restart in " + ((int) this.gameEndCD) + "s\n\n");
                        }
                        else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
                        {
                            if ((SettingsManager.LegacyGameSettings.BladePVP.Value == 0) && (!SettingsManager.LegacyGameSettings.BombModeEnabled.Value))
                            {
                                this.ShowHUDInfoCenter(string.Concat(new object[] { "Team ", this.teamWinner, " Win!\nGame Restart in ", (int) this.gameEndCD, "s\n\n" }));
                            }
                            else
                            {
                                this.ShowHUDInfoCenter(string.Concat(new object[] { "Round Ended!\nGame Restart in ", (int) this.gameEndCD, "s\n\n" }));
                            }
                        }
                        else
                        {
                            this.ShowHUDInfoCenter("Humanity Win!\nGame Restart in " + ((int) this.gameEndCD) + "s\n\n");
                        }
                        if (this.gameEndCD <= 0f)
                        {
                            this.gameEndCD = 0f;
                            if (PhotonNetwork.isMasterClient)
                            {
                                this.restartRC();
                            }
                            this.ShowHUDInfoCenter(string.Empty);
                        }
                        else
                        {
                            this.gameEndCD -= Time.deltaTime;
                        }
                    }
                }
                this.timeElapse += Time.deltaTime;
                this.roundTime += Time.deltaTime;
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
                    {
                        if (!this.isWinning)
                        {
                            this.timeTotalServer += Time.deltaTime;
                        }
                    }
                    else if (!(this.isLosing || this.isWinning))
                    {
                        this.timeTotalServer += Time.deltaTime;
                    }
                }
                else
                {
                    this.timeTotalServer += Time.deltaTime;
                }
                if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (!this.isWinning)
                        {
                            this.ShowHUDInfoTopCenter("Time : " + (this.timeTotalServer - 5f).ToString("0.00"));
                        }
                        if (this.timeTotalServer < 5f)
                        {
                            this.ShowHUDInfoCenter("RACE START IN " + (5f - this.timeTotalServer).ToString("0.00"));
                        }
                        else if (!this.startRacing)
                        {
                            this.ShowHUDInfoCenter(string.Empty);
                            this.startRacing = true;
                            this.endRacing = false;
                            GameObject.Find("door").SetActive(false);
                        }
                    }
                    else
                    {
                        this.ShowHUDInfoTopCenter("Time : " + ((this.roundTime >= 20f) ? (this.roundTime - 20f).ToString("0.00") : "WAITING"));
                        if (this.roundTime < 20f)
                        {
                            this.ShowHUDInfoCenter("RACE START IN " + (20f - this.roundTime).ToString("0.00") + (!(this.localRacingResult == string.Empty) ? ("\nLast Round\n" + this.localRacingResult) : "\n\n"));
                        }
                        else if (!this.startRacing)
                        {
                            this.ShowHUDInfoCenter(string.Empty);
                            this.startRacing = true;
                            this.endRacing = false;
                            GameObject obj2 = GameObject.Find("door");
                            if (obj2 != null)
                            {
                                obj2.SetActive(false);
                            }
                            if ((this.racingDoors != null) && customLevelLoaded)
                            {
                                foreach (GameObject obj3 in this.racingDoors)
                                {
                                    obj3.SetActive(false);
                                }
                                this.racingDoors = null;
                            }
                        }
                        else if ((this.racingDoors != null) && customLevelLoaded)
                        {
                            foreach (GameObject obj3 in this.racingDoors)
                            {
                                obj3.SetActive(false);
                            }
                            this.racingDoors = null;
                        }
                        if (needChooseSide)
                        {
                            string joinButton = SettingsManager.InputSettings.Human.Flare1.ToString();
                            this.ShowHUDInfoTopCenterADD("\n\nPRESS " + joinButton + " TO ENTER GAME");
                        }
                    }
                    if ((Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !this.needChooseSide) && customLevelLoaded && !SettingsManager.LegacyGeneralSettings.SpecMode.Value)
                    {
                        this.myRespawnTime += Time.deltaTime;
                        if (this.myRespawnTime > 1.5f)
                        {
                            this.myRespawnTime = 0f;
                            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                            if (this.checkpoint != null)
                            {
                                base.StartCoroutine(this.WaitAndRespawn2(0.1f, this.checkpoint));
                            }
                            else
                            {
                                base.StartCoroutine(this.WaitAndRespawn1(0.1f, this.myLastRespawnTag));
                            }
                            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                            this.ShowHUDInfoCenter(string.Empty);
                        }
                    }
                }
                if (this.timeElapse > 1f)
                {
                    this.timeElapse--;
                    string content = string.Empty;
                    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
                    {
                        length = this.time - ((int) this.timeTotalServer);
                        content = content + "Time : " + length.ToString();
                    }
                    else if ((IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN) || (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.None))
                    {
                        content = "Titan Left: ";
                        length = GameObject.FindGameObjectsWithTag("titan").Length;
                        content = content + length.ToString() + "  Time : ";
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            length = (int) this.timeTotalServer;
                            content = content + length.ToString();
                        }
                        else
                        {
                            length = this.time - ((int) this.timeTotalServer);
                            content = content + length.ToString();
                        }
                    }
                    else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
                    {
                        content = "Titan Left: ";
                        object[] objArray = new object[4];
                        objArray[0] = content;
                        length = GameObject.FindGameObjectsWithTag("titan").Length;
                        objArray[1] = length.ToString();
                        objArray[2] = " Wave : ";
                        objArray[3] = this.wave;
                        content = string.Concat(objArray);
                    }
                    else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT)
                    {
                        content = "Time : ";
                        length = this.time - ((int) this.timeTotalServer);
                        content = content + length.ToString() + "\nDefeat the Colossal Titan.\nPrevent abnormal titan from running to the north gate";
                    }
                    else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
                    {
                        string str2 = "| ";
                        for (int i = 0; i < PVPcheckPoint.chkPts.Count; i++)
                        {
                            str2 = str2 + (PVPcheckPoint.chkPts[i] as PVPcheckPoint).getStateString() + " ";
                        }
                        str2 = str2 + "|";
                        length = this.time - ((int) this.timeTotalServer);
                        content = string.Concat(new object[] { this.PVPtitanScoreMax - this.PVPtitanScore, "  ", str2, "  ", this.PVPhumanScoreMax - this.PVPhumanScore, "\n" }) + "Time : " + length.ToString();
                    }
                    if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
                    {
                        content = content + "\n[00FFFF]Cyan:" + Convert.ToString(this.cyanKills) + "       [FF00FF]Magenta:" + Convert.ToString(this.magentaKills) + "[ffffff]";
                    }
                    if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
                        this.ShowHUDInfoTopCenter(content);
                    content = string.Empty;
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
                        {
                            content = "Time : ";
                            length = (int) this.timeTotalServer;
                            content = content + length.ToString();
                        }
                    }
                    else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
                    {
                        content = string.Concat(new object[] { "Humanity ", this.humanScore, " : Titan ", this.titanScore, " " });
                    }
                    else if (((IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN) || (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT)) || (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE))
                    {
                        content = string.Concat(new object[] { "Humanity ", this.humanScore, " : Titan ", this.titanScore, " " });
                    }
                    else if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.CAGE_FIGHT)
                    {
                        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
                        {
                            content = "Time : ";
                            length = this.time - ((int) this.timeTotalServer);
                            content = content + length.ToString();
                        }
                        else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
                        {
                            for (int j = 0; j < this.teamScores.Length; j++)
                            {
                                string str3 = content;
                                content = string.Concat(new object[] { str3, (j == 0) ? string.Empty : " : ", "Team", j + 1, " ", this.teamScores[j], string.Empty });
                            }
                            content = content + "\nTime : " + ((this.time - ((int) this.timeTotalServer))).ToString();
                        }
                    }
                    this.ShowHUDInfoTopRight(content);
                    string str4 = (IN_GAME_MAIN_CAMERA.difficulty >= 0) ? ((IN_GAME_MAIN_CAMERA.difficulty != 0) ? ((IN_GAME_MAIN_CAMERA.difficulty != 1) ? "Abnormal" : "Hard") : "Normal") : "Trainning";
                    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.CAGE_FIGHT)
                    {
                        this.ShowHUDInfoTopRightMAPNAME(string.Concat(new object[] { (int) this.roundTime, "s\n", level, " : ", str4 }));
                    }
                    else
                    {
                        this.ShowHUDInfoTopRightMAPNAME("\n" + level + " : " + str4);
                    }
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                    {
                        char[] separator = new char[] { "`"[0] };
                        string str5 = PhotonNetwork.room.name.Split(separator)[0];
                        if (str5.Length > 20)
                        {
                            str5 = str5.Remove(0x13) + "...";
                        }
                        this.ShowHUDInfoTopRightMAPNAME("\n" + str5 + " [FFC000](" + Convert.ToString(PhotonNetwork.room.playerCount) + "/" + Convert.ToString(PhotonNetwork.room.maxPlayers) + ")");
                        if (this.needChooseSide && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
                        {
                            string joinButton = SettingsManager.InputSettings.Human.Flare1.ToString();
                            
                            this.ShowHUDInfoTopCenterADD("\n\nPRESS " + joinButton + " TO ENTER GAME");
                        }
                    }
                }
                if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && (this.killInfoGO.Count > 0)) && (this.killInfoGO[0] == null))
                {
                    this.killInfoGO.RemoveAt(0);
                }
                if (((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && PhotonNetwork.isMasterClient) && (this.timeTotalServer > this.time))
                {
                    string str11;
                    IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
                    this.gameStart = false;
                    string str6 = string.Empty;
                    string str7 = string.Empty;
                    string str8 = string.Empty;
                    string str9 = string.Empty;
                    string str10 = string.Empty;
                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {
                        if (player != null)
                        {
                            str6 = str6 + player.customProperties[PhotonPlayerProperty.name] + "\n";
                            str7 = str7 + player.customProperties[PhotonPlayerProperty.kills] + "\n";
                            str8 = str8 + player.customProperties[PhotonPlayerProperty.deaths] + "\n";
                            str9 = str9 + player.customProperties[PhotonPlayerProperty.max_dmg] + "\n";
                            str10 = str10 + player.customProperties[PhotonPlayerProperty.total_dmg] + "\n";
                        }
                    }
                    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
                    {
                        str11 = string.Empty;
                        for (int k = 0; k < this.teamScores.Length; k++)
                        {
                            str11 = str11 + ((k == 0) ? string.Concat(new object[] { "Team", k + 1, " ", this.teamScores[k], " " }) : " : ");
                        }
                    }
                    else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
                    {
                        str11 = "Highest Wave : " + this.highestwave;
                    }
                    else
                    {
                        str11 = string.Concat(new object[] { "Humanity ", this.humanScore, " : Titan ", this.titanScore });
                    }
                    object[] parameters = new object[] { str6, str7, str8, str9, str10, str11 };
                    base.photonView.RPC("showResult", PhotonTargets.AllBuffered, parameters);
                }
            }
        }
    }

    private void coreadd()
    {
        if (PhotonNetwork.isMasterClient)
        {
            this.OnUpdate();
            if (customLevelLoaded)
            {
                for (int i = 0; i < this.titanSpawners.Count; i++)
                {
                    TitanSpawner item = this.titanSpawners[i];
                    item.time -= Time.deltaTime;
                    if ((item.time <= 0f) && ((this.titans.Count + this.fT.Count) < Math.Min(SettingsManager.LegacyGameSettings.TitanSpawnCap.Value, 80)))
                    {
                        string name = item.name;
                        if (name == "spawnAnnie")
                        {
                            PhotonNetwork.Instantiate("FEMALE_TITAN", item.location, new Quaternion(0f, 0f, 0f, 1f), 0);
                        }
                        else
                        {
                            GameObject obj2 = PhotonNetwork.Instantiate("TITAN_VER3.1", item.location, new Quaternion(0f, 0f, 0f, 1f), 0);
                            if (name == "spawnAbnormal")
                            {
                                obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, false);
                            }
                            else if (name == "spawnJumper")
                            {
                                obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
                            }
                            else if (name == "spawnCrawler")
                            {
                                obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
                            }
                            else if (name == "spawnPunk")
                            {
                                obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, false);
                            }
                        }
                        if (item.endless)
                        {
                            item.time = item.delay;
                        }
                        else
                        {
                            this.titanSpawners.Remove(item);
                        }
                    }
                }
            }
        }
        if (Time.timeScale <= 0.1f)
        {
            if (this.pauseWaitTime <= 3f)
            {
                this.pauseWaitTime -= Time.deltaTime * 1000000f;
                if (this.pauseWaitTime <= 1f)
                {
                    Camera.main.farClipPlane = 1500f;
                }
                if (this.pauseWaitTime <= 0f)
                {
                    this.pauseWaitTime = 0f;
                    Time.timeScale = 1f;
                }
            }
            this.justRecompileThePlayerList();
        }
    }

    private void coreeditor()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            GUI.FocusControl(null);
        }
        if (this.selectedObj != null)
        {
            float num = 0.2f;
            if (SettingsManager.InputSettings.RCEditor.Slow.GetKey())
            {
                num = 0.04f;
            }
            else if (SettingsManager.InputSettings.RCEditor.Fast.GetKey())
            {
                num = 0.6f;
            }
            if (SettingsManager.InputSettings.General.Forward.GetKey())
            {
                Transform transform = this.selectedObj.transform;
                transform.position += (Vector3) (num * new Vector3(Camera.mainCamera.transform.forward.x, 0f, Camera.mainCamera.transform.forward.z));
            }
            else if (SettingsManager.InputSettings.General.Back.GetKey())
            {
                Transform transform9 = this.selectedObj.transform;
                transform9.position -= (Vector3) (num * new Vector3(Camera.mainCamera.transform.forward.x, 0f, Camera.mainCamera.transform.forward.z));
            }
            if (SettingsManager.InputSettings.General.Left.GetKey())
            {
                Transform transform10 = this.selectedObj.transform;
                transform10.position -= (Vector3) (num * new Vector3(Camera.mainCamera.transform.right.x, 0f, Camera.mainCamera.transform.right.z));
            }
            else if (SettingsManager.InputSettings.General.Right.GetKey())
            {
                Transform transform11 = this.selectedObj.transform;
                transform11.position += (Vector3) (num * new Vector3(Camera.mainCamera.transform.right.x, 0f, Camera.mainCamera.transform.right.z));
            }
            if (SettingsManager.InputSettings.RCEditor.Down.GetKey())
            {
                Transform transform12 = this.selectedObj.transform;
                transform12.position -= (Vector3) (Vector3.up * num);
            }
            else if (SettingsManager.InputSettings.RCEditor.Up.GetKey())
            {
                Transform transform13 = this.selectedObj.transform;
                transform13.position += (Vector3) (Vector3.up * num);
            }
            if (!this.selectedObj.name.StartsWith("misc,region"))
            {
                if (SettingsManager.InputSettings.RCEditor.RotateRight.GetKey())
                {
                    this.selectedObj.transform.Rotate((Vector3) (Vector3.up * num));
                }
                else if (SettingsManager.InputSettings.RCEditor.RotateLeft.GetKey())
                {
                    this.selectedObj.transform.Rotate((Vector3) (Vector3.down * num));
                }
                if (SettingsManager.InputSettings.RCEditor.RotateCCW.GetKey())
                {
                    this.selectedObj.transform.Rotate((Vector3) (Vector3.forward * num));
                }
                else if (SettingsManager.InputSettings.RCEditor.RotateCW.GetKey())
                {
                    this.selectedObj.transform.Rotate((Vector3) (Vector3.back * num));
                }
                if (SettingsManager.InputSettings.RCEditor.RotateBack.GetKey())
                {
                    this.selectedObj.transform.Rotate((Vector3) (Vector3.left * num));
                }
                else if (SettingsManager.InputSettings.RCEditor.RotateForward.GetKey())
                {
                    this.selectedObj.transform.Rotate((Vector3) (Vector3.right * num));
                }
            }
            if (SettingsManager.InputSettings.RCEditor.Place.GetKeyDown())
            {
                linkHash[3].Add(this.selectedObj.GetInstanceID(), this.selectedObj.name + "," + Convert.ToString(this.selectedObj.transform.position.x) + "," + Convert.ToString(this.selectedObj.transform.position.y) + "," + Convert.ToString(this.selectedObj.transform.position.z) + "," + Convert.ToString(this.selectedObj.transform.rotation.x) + "," + Convert.ToString(this.selectedObj.transform.rotation.y) + "," + Convert.ToString(this.selectedObj.transform.rotation.z) + "," + Convert.ToString(this.selectedObj.transform.rotation.w));
                this.selectedObj = null;
                Camera.main.GetComponent<MouseLook>().enabled = true;
            }
            if (SettingsManager.InputSettings.RCEditor.Delete.GetKeyDown())
            {
                UnityEngine.Object.Destroy(this.selectedObj);
                this.selectedObj = null;
                Camera.main.GetComponent<MouseLook>().enabled = true;
                linkHash[3].Remove(this.selectedObj.GetInstanceID());
            }
        }
        else
        {
            if (Camera.main.GetComponent<MouseLook>().enabled)
            {
                float num2 = 100f;
                if (SettingsManager.InputSettings.RCEditor.Slow.GetKey())
                {
                    num2 = 20f;
                }
                else if (SettingsManager.InputSettings.RCEditor.Fast.GetKey())
                {
                    num2 = 400f;
                }
                Transform transform7 = Camera.main.transform;
                if (SettingsManager.InputSettings.General.Forward.GetKey())
                {
                    transform7.position += (Vector3) ((transform7.forward * num2) * Time.deltaTime);
                }
                else if (SettingsManager.InputSettings.General.Back.GetKey())
                {
                    transform7.position -= (Vector3) ((transform7.forward * num2) * Time.deltaTime);
                }
                if (SettingsManager.InputSettings.General.Left.GetKey())
                {
                    transform7.position -= (Vector3) ((transform7.right * num2) * Time.deltaTime);
                }
                else if (SettingsManager.InputSettings.General.Right.GetKey())
                {
                    transform7.position += (Vector3) ((transform7.right * num2) * Time.deltaTime);
                }
                if (SettingsManager.InputSettings.RCEditor.Up.GetKey())
                {
                    transform7.position += (Vector3) ((transform7.up * num2) * Time.deltaTime);
                }
                else if (SettingsManager.InputSettings.RCEditor.Down.GetKey())
                {
                    transform7.position -= (Vector3) ((transform7.up * num2) * Time.deltaTime);
                }
            }
            if (SettingsManager.InputSettings.RCEditor.Cursor.GetKeyDown())
            {
                if (Camera.main.GetComponent<MouseLook>().enabled)
                {
                    Camera.main.GetComponent<MouseLook>().enabled = false;
                }
                else
                {
                    Camera.main.GetComponent<MouseLook>().enabled = true;
                }
            }
            if (((Input.GetKeyDown(KeyCode.Mouse0) && !Screen.lockCursor) && (GUIUtility.hotControl == 0)) && (((Input.mousePosition.x > 300f) && (Input.mousePosition.x < (Screen.width - 300f))) || ((Screen.height - Input.mousePosition.y) > 600f)))
            {
                RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
                {
                    Transform transform8 = hitInfo.transform;
                    if ((((transform8.gameObject.name.StartsWith("custom") || transform8.gameObject.name.StartsWith("base")) || (transform8.gameObject.name.StartsWith("racing") || transform8.gameObject.name.StartsWith("photon"))) || transform8.gameObject.name.StartsWith("spawnpoint")) || transform8.gameObject.name.StartsWith("misc"))
                    {
                        this.selectedObj = transform8.gameObject;
                        Camera.main.GetComponent<MouseLook>().enabled = false;
                        Screen.lockCursor = true;
                        linkHash[3].Remove(this.selectedObj.GetInstanceID());
                    }
                    else if (((transform8.parent.gameObject.name.StartsWith("custom") || transform8.parent.gameObject.name.StartsWith("base")) || transform8.parent.gameObject.name.StartsWith("racing")) || transform8.parent.gameObject.name.StartsWith("photon"))
                    {
                        this.selectedObj = transform8.parent.gameObject;
                        Camera.main.GetComponent<MouseLook>().enabled = false;
                        Screen.lockCursor = true;
                        linkHash[3].Remove(this.selectedObj.GetInstanceID());
                    }
                }
            }
        }
    }

    private IEnumerator customlevelcache()
    {
        for (int i = 0; i < this.levelCache.Count; i++)
        {
            this.customlevelclientE(this.levelCache[i], false);
            yield return new WaitForEndOfFrame();
        }
    }

    private void customlevelclientE(string[] content, bool renewHash)
    {
        int num;
        string[] strArray;
        bool flag = false;
        bool flag2 = false;
        if (content[content.Length - 1].StartsWith("a"))
        {
            flag = true;
            this.customMapMaterials.Clear();
        }
        else if (content[content.Length - 1].StartsWith("z"))
        {
            flag2 = true;
            customLevelLoaded = true;
            this.spawnPlayerCustomMap();
            Minimap.TryRecaptureInstance();
            this.unloadAssets();
            Camera.main.GetComponent<TiltShift>().enabled = false;
        }
        if (renewHash)
        {
            if (flag)
            {
                currentLevel = string.Empty;
                this.levelCache.Clear();
                this.titanSpawns.Clear();
                this.playerSpawnsC.Clear();
                this.playerSpawnsM.Clear();
                for (num = 0; num < content.Length; num++)
                {
                    strArray = content[num].Split(new char[] { ',' });
                    if (strArray[0] == "titan")
                    {
                        this.titanSpawns.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
                    }
                    else if (strArray[0] == "playerC")
                    {
                        this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
                    }
                    else if (strArray[0] == "playerM")
                    {
                        this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
                    }
                }
                this.spawnPlayerCustomMap();
            }
            currentLevel = currentLevel + content[content.Length - 1];
            this.levelCache.Add(content);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.currentLevel, currentLevel);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
        if (!flag && !flag2)
        {
            for (num = 0; num < content.Length; num++)
            {
                float num2;
                GameObject obj2;
                float num3;
                float num5;
                float num6;
                float num7;
                Color color;
                Mesh mesh;
                Color[] colorArray;
                int num8;
                strArray = content[num].Split(new char[] { ',' });
                if (strArray[0].StartsWith("custom"))
                {
                    num2 = 1f;
                    obj2 = null;
                    obj2 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])));
                    if (strArray[2] != "default")
                    {
                        if (strArray[2].StartsWith("transparent"))
                        {
                            if (float.TryParse(strArray[2].Substring(11), out num3))
                            {
                                num2 = num3;
                            }
                            foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                            {
                                renderer.material = (Material)RCassets.Load("transparent");
                                if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                {
                                    string matHash = this.getMaterialHash(strArray[2], strArray[10], strArray[11]);
                                    if (this.customMapMaterials.ContainsKey(matHash))
                                        renderer.material = this.customMapMaterials[matHash];
                                    else
                                    {
                                        renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                        this.customMapMaterials.Add(matHash, renderer.material);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                            {
                                renderer.material = (Material)RCassets.Load(strArray[2]);
                                if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                {
                                    string matHash = this.getMaterialHash(strArray[2], strArray[10], strArray[11]);
                                    if (this.customMapMaterials.ContainsKey(matHash))
                                        renderer.material = this.customMapMaterials[matHash];
                                    else
                                    {
                                        renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                        this.customMapMaterials.Add(matHash, renderer.material);
                                    }
                                }
                            }
                        }
                    }
                    num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[3]);
                    num5 -= 0.001f;
                    num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[4]);
                    num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[5]);
                    obj2.transform.localScale = new Vector3(num5, num6, num7);
                    if (strArray[6] != "0")
                    {
                        color = new Color(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), num2);
                        foreach (MeshFilter filter in obj2.GetComponentsInChildren<MeshFilter>())
                        {
                            mesh = filter.mesh;
                            colorArray = new Color[mesh.vertexCount];
                            num8 = 0;
                            while (num8 < mesh.vertexCount)
                            {
                                colorArray[num8] = color;
                                num8++;
                            }
                            mesh.colors = colorArray;
                        }
                    }
                }
                else if (strArray[0].StartsWith("base"))
                {
                    if (strArray.Length < 15)
                    {
                        UnityEngine.Object.Instantiate(Resources.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])));
                    }
                    else
                    {
                        num2 = 1f;
                        obj2 = null;
                        obj2 = (GameObject) UnityEngine.Object.Instantiate((GameObject) Resources.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])));
                        if (strArray[2] != "default")
                        {
                            if (strArray[2].StartsWith("transparent"))
                            {
                                if (float.TryParse(strArray[2].Substring(11), out num3))
                                {
                                    num2 = num3;
                                }
                                foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                                {
                                    renderer.material = (Material) RCassets.Load("transparent");
                                    if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                    {
                                        string matHash = this.getMaterialHash(strArray[2], strArray[10], strArray[11]);
                                        if (this.customMapMaterials.ContainsKey(matHash))
                                            renderer.material = this.customMapMaterials[matHash];
                                        else
                                        {
                                            renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                            this.customMapMaterials.Add(matHash, renderer.material);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                                {
                                    if (!renderer.name.Contains("Particle System") || !obj2.name.Contains("aot_supply"))
                                    {
                                        renderer.material = (Material) RCassets.Load(strArray[2]);
                                        if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                        {
                                            string matHash = this.getMaterialHash(strArray[2], strArray[10], strArray[11]);
                                            if (this.customMapMaterials.ContainsKey(matHash))
                                                renderer.material = this.customMapMaterials[matHash];
                                            else
                                            {
                                                renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                                this.customMapMaterials.Add(matHash, renderer.material);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[3]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[4]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[5]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        if (strArray[6] != "0")
                        {
                            color = new Color(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), num2);
                            foreach (MeshFilter filter in obj2.GetComponentsInChildren<MeshFilter>())
                            {
                                mesh = filter.mesh;
                                colorArray = new Color[mesh.vertexCount];
                                for (num8 = 0; num8 < mesh.vertexCount; num8++)
                                {
                                    colorArray[num8] = color;
                                }
                                mesh.colors = colorArray;
                            }
                        }
                    }
                }
                else if (strArray[0].StartsWith("misc"))
                {
                    if (strArray[1].StartsWith("barrier"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                    }
                    else if (strArray[1].StartsWith("racingStart"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        if (this.racingDoors != null)
                        {
                            this.racingDoors.Add(obj2);
                        }
                    }
                    else if (strArray[1].StartsWith("racingEnd"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.AddComponent<LevelTriggerRacingEnd>();
                    }
                    else if (strArray[1].StartsWith("region") && PhotonNetwork.isMasterClient)
                    {
                        Vector3 loc = new Vector3(Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]));
                        RCRegion region = new RCRegion(loc, Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4]), Convert.ToSingle(strArray[5]));
                        string key = strArray[2];
                        if (RCRegionTriggers.ContainsKey(key))
                        {
                            GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load("region"));
                            obj3.transform.position = loc;
                            obj3.AddComponent<RegionTrigger>();
                            obj3.GetComponent<RegionTrigger>().CopyTrigger((RegionTrigger) RCRegionTriggers[key]);
                            num5 = obj3.transform.localScale.x * Convert.ToSingle(strArray[3]);
                            num5 -= 0.001f;
                            num6 = obj3.transform.localScale.y * Convert.ToSingle(strArray[4]);
                            num7 = obj3.transform.localScale.z * Convert.ToSingle(strArray[5]);
                            obj3.transform.localScale = new Vector3(num5, num6, num7);
                            region.myBox = obj3;
                        }
                        RCRegions.Add(key, region);
                    }
                }
                else if (strArray[0].StartsWith("racing"))
                {
                    if (strArray[1].StartsWith("start"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        if (this.racingDoors != null)
                        {
                            this.racingDoors.Add(obj2);
                        }
                    }
                    else if (strArray[1].StartsWith("end"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.GetComponentInChildren<Collider>().gameObject.AddComponent<LevelTriggerRacingEnd>();
                    }
                    else if (strArray[1].StartsWith("kill"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingKillTrigger>();
                    }
                    else if (strArray[1].StartsWith("checkpoint"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingCheckpointTrigger>();
                    }
                }
                else if (strArray[0].StartsWith("map"))
                {
                    if (strArray[1].StartsWith("disablebounds"))
                    {
                        UnityEngine.Object.Destroy(GameObject.Find("gameobjectOutSide"));
                        UnityEngine.Object.Instantiate(RCassets.Load("outside"));
                    }
                }
                else if (PhotonNetwork.isMasterClient && strArray[0].StartsWith("photon"))
                {
                    if (strArray[1].StartsWith("Cannon"))
                    {
                        if (strArray.Length > 15)
                        {
                            GameObject go = PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop", new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])), 0);
                            go.GetComponent<CannonPropRegion>().settings = content[num];
                            go.GetPhotonView().RPC("SetSize", PhotonTargets.AllBuffered, new object[] { content[num] });
                        }
                        else
                        {
                            PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop", new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0).GetComponent<CannonPropRegion>().settings = content[num];
                        }
                    }
                    else
                    {
                        TitanSpawner item = new TitanSpawner();
                        num5 = 30f;
                        if (float.TryParse(strArray[2], out num3))
                        {
                            num5 = Mathf.Max(Convert.ToSingle(strArray[2]), 1f);
                        }
                        item.time = num5;
                        item.delay = num5;
                        item.name = strArray[1];
                        if (strArray[3] == "1")
                        {
                            item.endless = true;
                        }
                        else
                        {
                            item.endless = false;
                        }
                        item.location = new Vector3(Convert.ToSingle(strArray[4]), Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]));
                        this.titanSpawners.Add(item);
                    }
                }
            }
        }
    }

    private IEnumerator customlevelE(List<PhotonPlayer> players)
    {
        string[] strArray;
        if (!(currentLevel == string.Empty))
        {
            for (int i = 0; i < this.levelCache.Count; i++)
            {
                foreach (PhotonPlayer player in players)
                {
                    if (((player.customProperties[PhotonPlayerProperty.currentLevel] != null) && (currentLevel != string.Empty)) && (RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.currentLevel]) == currentLevel))
                    {
                        if (i == 0)
                        {
                            strArray = new string[] { "loadcached" };
                            this.photonView.RPC("customlevelRPC", player, new object[] { strArray });
                        }
                    }
                    else
                    {
                        this.photonView.RPC("customlevelRPC", player, new object[] { this.levelCache[i] });
                    }
                }
                if (i > 0)
                {
                    yield return new WaitForSeconds(0.75f);
                }
                else
                {
                    yield return new WaitForSeconds(0.25f);
                }
            }
            yield break;
        }
        strArray = new string[] { "loadempty" };
        foreach (PhotonPlayer player in players)
        {
            this.photonView.RPC("customlevelRPC", player, new object[] { strArray });
        }
        customLevelLoaded = true;
    }

    [RPC]
    private void customlevelRPC(string[] content, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            if ((content.Length == 1) && (content[0] == "loadcached"))
            {
                base.StartCoroutine(this.customlevelcache());
            }
            else if ((content.Length == 1) && (content[0] == "loadempty"))
            {
                currentLevel = string.Empty;
                this.levelCache.Clear();
                this.titanSpawns.Clear();
                this.playerSpawnsC.Clear();
                this.playerSpawnsM.Clear();
                this.customMapMaterials.Clear();
                ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                propertiesToSet.Add(PhotonPlayerProperty.currentLevel, currentLevel);
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                customLevelLoaded = true;
                this.spawnPlayerCustomMap();
            }
            else
            {
                this.customlevelclientE(content, true);
            }
        }
    }

    public void debugChat(string str)
    {
        this.chatRoom.addLINE(str);
    }

    public void DestroyAllExistingCloths()
    {
        Cloth[] clothArray = UnityEngine.Object.FindObjectsOfType<Cloth>();
        if (clothArray.Length > 0)
        {
            for (int i = 0; i < clothArray.Length; i++)
            {
                ClothFactory.DisposeObject(clothArray[i].gameObject);
            }
        }
    }

    private void endGameInfectionRC()
    {
        int num;
        imatitan.Clear();
        for (num = 0; num < PhotonNetwork.playerList.Length; num++)
        {
            PhotonPlayer player = PhotonNetwork.playerList[num];
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.isTitan, 1);
            player.SetCustomProperties(propertiesToSet);
        }
        int length = PhotonNetwork.playerList.Length;
        int infectionMode = SettingsManager.LegacyGameSettings.InfectionModeAmount.Value;
        for (num = 0; num < PhotonNetwork.playerList.Length; num++)
        {
            PhotonPlayer player2 = PhotonNetwork.playerList[num];
            if ((length > 0) && (UnityEngine.Random.Range((float) 0f, (float) 1f) <= (((float) infectionMode) / ((float) length))))
            {
                ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
                hashtable2.Add(PhotonPlayerProperty.isTitan, 2);
                player2.SetCustomProperties(hashtable2);
                imatitan.Add(player2.ID, 2);
                infectionMode--;
            }
            length--;
        }
        this.gameEndCD = 0f;
        this.restartGame2(false);
    }

    private void endGameRC()
    {
        if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
        {
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                PhotonPlayer player = PhotonNetwork.playerList[i];
                ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                propertiesToSet.Add(PhotonPlayerProperty.kills, 0);
                propertiesToSet.Add(PhotonPlayerProperty.deaths, 0);
                propertiesToSet.Add(PhotonPlayerProperty.max_dmg, 0);
                propertiesToSet.Add(PhotonPlayerProperty.total_dmg, 0);
                player.SetCustomProperties(propertiesToSet);
            }
        }
        this.gameEndCD = 0f;
        this.restartGame2(false);
    }

    public void EnterSpecMode(bool enter)
    {
        if (enter)
        {
            this.spectateSprites = new List<GameObject>();
            foreach (GameObject obj2 in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if ((obj2.GetComponent<UISprite>() != null) && obj2.activeInHierarchy)
                {
                    string name = obj2.name;
                    if (((name.Contains("blade") || name.Contains("bullet")) || (name.Contains("gas") || name.Contains("flare"))) || name.Contains("skill_cd"))
                    {
                        if (!this.spectateSprites.Contains(obj2))
                        {
                            this.spectateSprites.Add(obj2);
                        }
                        obj2.SetActive(false);
                    }
                }
            }
            string[] strArray2 = new string[] { "Flare", "LabelInfoBottomRight" };
            foreach (string str2 in strArray2)
            {
                GameObject item = GameObject.Find(str2);
                if (item != null)
                {
                    if (!this.spectateSprites.Contains(item))
                    {
                        this.spectateSprites.Add(item);
                    }
                    item.SetActive(false);
                }
            }
            foreach (HERO hero in instance.getPlayers())
            {
                if (hero.photonView.isMine)
                {
                    PhotonNetwork.Destroy(hero.photonView);
                }
            }
            if ((RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2) && !RCextensions.returnBoolFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.dead]))
            {
                foreach (TITAN titan in instance.getTitans())
                {
                    if (titan.photonView.isMine)
                    {
                        PhotonNetwork.Destroy(titan.photonView);
                    }
                }
            }
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], false);
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], false);
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], false);
            instance.needChooseSide = false;
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
            GameObject obj4 = GameObject.FindGameObjectWithTag("Player");
            if ((obj4 != null) && (obj4.GetComponent<HERO>() != null))
            {
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(obj4, true, false);
            }
            else
            {
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
            }
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            base.StartCoroutine(this.reloadSky(true));
        }
        else
        {
            if (GameObject.Find("cross1") != null)
            {
                GameObject.Find("cross1").transform.localPosition = (Vector3) (Vector3.up * 5000f);
            }
            if (this.spectateSprites != null)
            {
                foreach (GameObject obj2 in this.spectateSprites)
                {
                    if (obj2 != null)
                    {
                        obj2.SetActive(true);
                    }
                }
            }
            this.spectateSprites = new List<GameObject>();
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], false);
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], false);
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], false);
            instance.needChooseSide = true;
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        }
    }

    public void gameLose2()
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !PhotonNetwork.isMasterClient)
            return;
        if (!this.isWinning && !this.isLosing)
        {
            this.isLosing = true;
            this.titanScore++;
            this.gameEndCD = this.gameEndTotalCDtime;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                object[] parameters = new object[] { this.titanScore };
                base.photonView.RPC("netGameLose", PhotonTargets.Others, parameters);
            }
            if (SettingsManager.UISettings.GameFeed.Value)
            {
                this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game lose).");
            }
        }
    }

    public void gameWin2()
    {
        if (!this.isLosing && !this.isWinning)
        {
            this.isWinning = true;
            this.humanScore++;
            if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
            {
                if (SettingsManager.LegacyGameSettings.RacingEndless.Value)
                {
                    this.gameEndCD = 1000f;
                }
                else
                {
                    this.gameEndCD = 20f;
                }
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                {
                    object[] parameters = new object[] { 0 };
                    base.photonView.RPC("netGameWin", PhotonTargets.Others, parameters);
                }
            }
            else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
            {
                this.gameEndCD = this.gameEndTotalCDtime;
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                {
                    object[] objArray3 = new object[] { this.teamWinner };
                    base.photonView.RPC("netGameWin", PhotonTargets.Others, objArray3);
                }
                this.teamScores[this.teamWinner - 1]++;
            }
            else
            {
                this.gameEndCD = this.gameEndTotalCDtime;
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                {
                    object[] objArray4 = new object[] { this.humanScore };
                    base.photonView.RPC("netGameWin", PhotonTargets.Others, objArray4);
                }
            }
            if (SettingsManager.UISettings.GameFeed.Value)
            {
                this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
            }
        }
    }

    public ArrayList getPlayers()
    {
        return this.heroes;
    }

    public ArrayList getErens()
    {
        return this.eT;
    }

    [RPC]
    private void getRacingResult(string player, float time, PhotonMessageInfo info)
    {
        if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
        {
            if (info != null)
                kickPlayerRCIfMC(info.sender, true, "racing exploit");
            return;
        }
        RacingResult result = new RacingResult {
            name = player,
            time = time
        };
        this.racingResult.Add(result);
        this.refreshRacingResult2();
    }

    public ArrayList getTitans()
    {
        return this.titans;
    }

    private string hairtype(int lol)
    {
        if (lol < 0)
        {
            return "Random";
        }
        return ("Male " + lol);
    }

    [RPC]
    private void ignorePlayer(int ID, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            PhotonPlayer player = PhotonPlayer.Find(ID);
            if ((player != null) && !ignoreList.Contains(ID))
            {
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i] == player)
                    {
                        ignoreList.Add(ID);
                        RaiseEventOptions options = new RaiseEventOptions {
                            TargetActors = new int[] { ID }
                        };
                        PhotonNetwork.RaiseEvent(0xfe, null, true, options);
                    }
                }
            }
        }
        this.RecompilePlayerList(0.1f);
    }

    [RPC]
    private void ignorePlayerArray(int[] IDS, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            for (int i = 0; i < IDS.Length; i++)
            {
                int iD = IDS[i];
                PhotonPlayer player = PhotonPlayer.Find(iD);
                if ((player != null) && !ignoreList.Contains(iD))
                {
                    for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
                    {
                        if (PhotonNetwork.playerList[j] == player)
                        {
                            ignoreList.Add(iD);
                            RaiseEventOptions options = new RaiseEventOptions {
                                TargetActors = new int[] { iD }
                            };
                            PhotonNetwork.RaiseEvent(0xfe, null, true, options);
                        }
                    }
                }
            }
        }
        this.RecompilePlayerList(0.1f);
    }

    public static GameObject InstantiateCustomAsset(string key)
    {
        key = key.Substring(8);
        return (GameObject) RCassets.Load(key);
    }

    public bool isPlayerAllDead()
    {
        int num = 0;
        int num2 = 0;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if (((int) player.customProperties[PhotonPlayerProperty.isTitan]) == 1)
            {
                num++;
                if ((bool) player.customProperties[PhotonPlayerProperty.dead])
                {
                    num2++;
                }
            }
        }
        return (num == num2);
    }

    public bool isPlayerAllDead2()
    {
        int num = 0;
        int num2 = 0;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if (RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.isTitan]) == 1)
            {
                num++;
                if (RCextensions.returnBoolFromObject(player.customProperties[PhotonPlayerProperty.dead]))
                {
                    num2++;
                }
            }
        }
        return (num == num2);
    }

    public bool isTeamAllDead(int team)
    {
        int num = 0;
        int num2 = 0;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if ((((int) player.customProperties[PhotonPlayerProperty.isTitan]) == 1) && (((int) player.customProperties[PhotonPlayerProperty.team]) == team))
            {
                num++;
                if ((bool) player.customProperties[PhotonPlayerProperty.dead])
                {
                    num2++;
                }
            }
        }
        return (num == num2);
    }

    public bool isTeamAllDead2(int team)
    {
        int num = 0;
        int num2 = 0;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if (((player.customProperties[PhotonPlayerProperty.isTitan] != null) && (player.customProperties[PhotonPlayerProperty.team] != null)) && ((RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.isTitan]) == 1) && (RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.team]) == team)))
            {
                num++;
                if (RCextensions.returnBoolFromObject(player.customProperties[PhotonPlayerProperty.dead]))
                {
                    num2++;
                }
            }
        }
        return (num == num2);
    }

    public void justRecompileThePlayerList()
    {
        int num15;
        string str3;
        int num16;
        int num17;
        int num18;
        int num19;
        string str = string.Empty;
        if (SettingsManager.LegacyGameSettings.TeamMode.Value != 0)
        {
            int num10;
            string str2;
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            Dictionary<int, PhotonPlayer> dictionary = new Dictionary<int, PhotonPlayer>();
            Dictionary<int, PhotonPlayer> dictionary2 = new Dictionary<int, PhotonPlayer>();
            Dictionary<int, PhotonPlayer> dictionary3 = new Dictionary<int, PhotonPlayer>();
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if ((player.customProperties[PhotonPlayerProperty.dead] != null) && !ignoreList.Contains(player.ID))
                {
                    num10 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.RCteam]);
                    switch (num10)
                    {
                        case 0:
                            dictionary3.Add(player.ID, player);
                            break;

                        case 1:
                            dictionary.Add(player.ID, player);
                            num += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.kills]);
                            num3 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.deaths]);
                            num5 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.max_dmg]);
                            num7 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.total_dmg]);
                            break;

                        case 2:
                            dictionary2.Add(player.ID, player);
                            num2 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.kills]);
                            num4 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.deaths]);
                            num6 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.max_dmg]);
                            num8 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.total_dmg]);
                            break;
                    }
                }
            }
            this.cyanKills = num;
            this.magentaKills = num2;
            if (PhotonNetwork.isMasterClient)
            {
                if (SettingsManager.LegacyGameSettings.TeamMode.Value == 2)
                {
                    foreach (PhotonPlayer player2 in PhotonNetwork.playerList)
                    {
                        int num11 = 0;
                        if (dictionary.Count > (dictionary2.Count + 1))
                        {
                            num11 = 2;
                            if (dictionary.ContainsKey(player2.ID))
                            {
                                dictionary.Remove(player2.ID);
                            }
                            if (!dictionary2.ContainsKey(player2.ID))
                            {
                                dictionary2.Add(player2.ID, player2);
                            }
                        }
                        else if (dictionary2.Count > (dictionary.Count + 1))
                        {
                            num11 = 1;
                            if (!dictionary.ContainsKey(player2.ID))
                            {
                                dictionary.Add(player2.ID, player2);
                            }
                            if (dictionary2.ContainsKey(player2.ID))
                            {
                                dictionary2.Remove(player2.ID);
                            }
                        }
                        if (num11 > 0)
                        {
                            base.photonView.RPC("setTeamRPC", player2, new object[] { num11 });
                        }
                    }
                }
                else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 3)
                {
                    foreach (PhotonPlayer player3 in PhotonNetwork.playerList)
                    {
                        int num12 = 0;
                        num10 = RCextensions.returnIntFromObject(player3.customProperties[PhotonPlayerProperty.RCteam]);
                        if (num10 > 0)
                        {
                            switch (num10)
                            {
                                case 1:
                                {
                                    int num13 = 0;
                                    num13 = RCextensions.returnIntFromObject(player3.customProperties[PhotonPlayerProperty.kills]);
                                    if (((num2 + num13) + 7) < (num - num13))
                                    {
                                        num12 = 2;
                                        num2 += num13;
                                        num -= num13;
                                    }
                                    break;
                                }
                                case 2:
                                {
                                    int num14 = 0;
                                    num14 = RCextensions.returnIntFromObject(player3.customProperties[PhotonPlayerProperty.kills]);
                                    if (((num + num14) + 7) < (num2 - num14))
                                    {
                                        num12 = 1;
                                        num += num14;
                                        num2 -= num14;
                                    }
                                    break;
                                }
                            }
                            if (num12 > 0)
                            {
                                base.photonView.RPC("setTeamRPC", player3, new object[] { num12 });
                            }
                        }
                    }
                }
            }
            str = string.Concat(new object[] { str, "[00FFFF]TEAM CYAN", "[ffffff]:", this.cyanKills, "/", num3, "/", num5, "/", num7, "\n" });
            foreach (PhotonPlayer player4 in dictionary.Values)
            {
                num10 = RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.RCteam]);
                if ((player4.customProperties[PhotonPlayerProperty.dead] != null) && (num10 == 1))
                {
                    if (ignoreList.Contains(player4.ID))
                    {
                        str = str + "[FF0000][X] ";
                    }
                    if (player4.isLocal)
                    {
                        str = str + "[00CC00]";
                    }
                    else
                    {
                        str = str + "[FFCC00]";
                    }
                    str = str + "[" + Convert.ToString(player4.ID) + "] ";
                    if (player4.isMasterClient)
                    {
                        str = str + "[ffffff][M] ";
                    }
                    if (RCextensions.returnBoolFromObject(player4.customProperties[PhotonPlayerProperty.dead]))
                    {
                        str = str + "[" + ColorSet.color_red + "] *dead* ";
                    }
                    if (RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.isTitan]) < 2)
                    {
                        num15 = RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.team]);
                        if (num15 < 2)
                        {
                            str = str + "[" + ColorSet.color_human + "] H ";
                        }
                        else if (num15 == 2)
                        {
                            str = str + "[" + ColorSet.color_human_1 + "] A ";
                        }
                    }
                    else if (RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.isTitan]) == 2)
                    {
                        str = str + "[" + ColorSet.color_titan_player + "] <T> ";
                    }
                    str2 = str;
                    str3 = string.Empty;
                    str3 = RCextensions.returnStringFromObject(player4.customProperties[PhotonPlayerProperty.name]);
                    num16 = 0;
                    num16 = RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.kills]);
                    num17 = 0;
                    num17 = RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.deaths]);
                    num18 = 0;
                    num18 = RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.max_dmg]);
                    num19 = 0;
                    num19 = RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.total_dmg]);
                    str = string.Concat(new object[] { str2, string.Empty, str3, "[ffffff]:", num16, "/", num17, "/", num18, "/", num19 });
                    if (RCextensions.returnBoolFromObject(player4.customProperties[PhotonPlayerProperty.dead]))
                    {
                        str = str + "[-]";
                    }
                    str = str + "\n";
                }
            }
            str = string.Concat(new object[] { str, " \n", "[FF00FF]TEAM MAGENTA", "[ffffff]:", this.magentaKills, "/", num4, "/", num6, "/", num8, "\n" });
            foreach (PhotonPlayer player5 in dictionary2.Values)
            {
                num10 = RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.RCteam]);
                if ((player5.customProperties[PhotonPlayerProperty.dead] != null) && (num10 == 2))
                {
                    if (ignoreList.Contains(player5.ID))
                    {
                        str = str + "[FF0000][X] ";
                    }
                    if (player5.isLocal)
                    {
                        str = str + "[00CC00]";
                    }
                    else
                    {
                        str = str + "[FFCC00]";
                    }
                    str = str + "[" + Convert.ToString(player5.ID) + "] ";
                    if (player5.isMasterClient)
                    {
                        str = str + "[ffffff][M] ";
                    }
                    if (RCextensions.returnBoolFromObject(player5.customProperties[PhotonPlayerProperty.dead]))
                    {
                        str = str + "[" + ColorSet.color_red + "] *dead* ";
                    }
                    if (RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.isTitan]) < 2)
                    {
                        num15 = RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.team]);
                        if (num15 < 2)
                        {
                            str = str + "[" + ColorSet.color_human + "] H ";
                        }
                        else if (num15 == 2)
                        {
                            str = str + "[" + ColorSet.color_human_1 + "] A ";
                        }
                    }
                    else if (RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.isTitan]) == 2)
                    {
                        str = str + "[" + ColorSet.color_titan_player + "] <T> ";
                    }
                    str2 = str;
                    str3 = string.Empty;
                    str3 = RCextensions.returnStringFromObject(player5.customProperties[PhotonPlayerProperty.name]);
                    num16 = 0;
                    num16 = RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.kills]);
                    num17 = 0;
                    num17 = RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.deaths]);
                    num18 = 0;
                    num18 = RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.max_dmg]);
                    num19 = 0;
                    num19 = RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.total_dmg]);
                    str = string.Concat(new object[] { str2, string.Empty, str3, "[ffffff]:", num16, "/", num17, "/", num18, "/", num19 });
                    if (RCextensions.returnBoolFromObject(player5.customProperties[PhotonPlayerProperty.dead]))
                    {
                        str = str + "[-]";
                    }
                    str = str + "\n";
                }
            }
            str = string.Concat(new object[] { str, " \n", "[00FF00]INDIVIDUAL\n" });
            foreach (PhotonPlayer player6 in dictionary3.Values)
            {
                num10 = RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.RCteam]);
                if ((player6.customProperties[PhotonPlayerProperty.dead] != null) && (num10 == 0))
                {
                    if (ignoreList.Contains(player6.ID))
                    {
                        str = str + "[FF0000][X] ";
                    }
                    if (player6.isLocal)
                    {
                        str = str + "[00CC00]";
                    }
                    else
                    {
                        str = str + "[FFCC00]";
                    }
                    str = str + "[" + Convert.ToString(player6.ID) + "] ";
                    if (player6.isMasterClient)
                    {
                        str = str + "[ffffff][M] ";
                    }
                    if (RCextensions.returnBoolFromObject(player6.customProperties[PhotonPlayerProperty.dead]))
                    {
                        str = str + "[" + ColorSet.color_red + "] *dead* ";
                    }
                    if (RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.isTitan]) < 2)
                    {
                        num15 = RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.team]);
                        if (num15 < 2)
                        {
                            str = str + "[" + ColorSet.color_human + "] H ";
                        }
                        else if (num15 == 2)
                        {
                            str = str + "[" + ColorSet.color_human_1 + "] A ";
                        }
                    }
                    else if (RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.isTitan]) == 2)
                    {
                        str = str + "[" + ColorSet.color_titan_player + "] <T> ";
                    }
                    str2 = str;
                    str3 = string.Empty;
                    str3 = RCextensions.returnStringFromObject(player6.customProperties[PhotonPlayerProperty.name]);
                    num16 = 0;
                    num16 = RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.kills]);
                    num17 = 0;
                    num17 = RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.deaths]);
                    num18 = 0;
                    num18 = RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.max_dmg]);
                    num19 = 0;
                    num19 = RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.total_dmg]);
                    str = string.Concat(new object[] { str2, string.Empty, str3, "[ffffff]:", num16, "/", num17, "/", num18, "/", num19 });
                    if (RCextensions.returnBoolFromObject(player6.customProperties[PhotonPlayerProperty.dead]))
                    {
                        str = str + "[-]";
                    }
                    str = str + "\n";
                }
            }
        }
        else
        {
            foreach (PhotonPlayer player7 in PhotonNetwork.playerList)
            {
                if (player7.customProperties[PhotonPlayerProperty.dead] != null)
                {
                    if (ignoreList.Contains(player7.ID))
                    {
                        str = str + "[FF0000][X] ";
                    }
                    if (player7.isLocal)
                    {
                        str = str + "[00CC00]";
                    }
                    else
                    {
                        str = str + "[FFCC00]";
                    }
                    str = str + "[" + Convert.ToString(player7.ID) + "] ";
                    if (player7.isMasterClient)
                    {
                        str = str + "[ffffff][M] ";
                    }
                    if (RCextensions.returnBoolFromObject(player7.customProperties[PhotonPlayerProperty.dead]))
                    {
                        str = str + "[" + ColorSet.color_red + "] *dead* ";
                    }
                    if (RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.isTitan]) < 2)
                    {
                        num15 = RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.team]);
                        if (num15 < 2)
                        {
                            str = str + "[" + ColorSet.color_human + "] H ";
                        }
                        else if (num15 == 2)
                        {
                            str = str + "[" + ColorSet.color_human_1 + "] A ";
                        }
                    }
                    else if (RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.isTitan]) == 2)
                    {
                        str = str + "[" + ColorSet.color_titan_player + "] <T> ";
                    }
                    string str4 = str;
                    str3 = string.Empty;
                    str3 = RCextensions.returnStringFromObject(player7.customProperties[PhotonPlayerProperty.name]);
                    num16 = 0;
                    num16 = RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.kills]);
                    num17 = 0;
                    num17 = RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.deaths]);
                    num18 = 0;
                    num18 = RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.max_dmg]);
                    num19 = 0;
                    num19 = RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.total_dmg]);
                    str = string.Concat(new object[] { str4, string.Empty, str3, "[ffffff]:", num16, "/", num17, "/", num18, "/", num19 });
                    if (RCextensions.returnBoolFromObject(player7.customProperties[PhotonPlayerProperty.dead]))
                    {
                        str = str + "[-]";
                    }
                    str = str + "\n";
                }
            }
        }
        this.playerList = str;
        if (PhotonNetwork.isMasterClient && ((!this.isWinning && !this.isLosing) && (this.roundTime >= 5f)))
        {
            int num21;
            if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
            {
                int num20 = 0;
                for (num21 = 0; num21 < PhotonNetwork.playerList.Length; num21++)
                {
                    PhotonPlayer targetPlayer = PhotonNetwork.playerList[num21];
                    if ((!ignoreList.Contains(targetPlayer.ID) && (targetPlayer.customProperties[PhotonPlayerProperty.dead] != null)) && (targetPlayer.customProperties[PhotonPlayerProperty.isTitan] != null))
                    {
                        if (RCextensions.returnIntFromObject(targetPlayer.customProperties[PhotonPlayerProperty.isTitan]) == 1)
                        {
                            if (RCextensions.returnBoolFromObject(targetPlayer.customProperties[PhotonPlayerProperty.dead]) && (RCextensions.returnIntFromObject(targetPlayer.customProperties[PhotonPlayerProperty.deaths]) > 0))
                            {
                                if (!imatitan.ContainsKey(targetPlayer.ID))
                                {
                                    imatitan.Add(targetPlayer.ID, 2);
                                }
                                ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                                propertiesToSet.Add(PhotonPlayerProperty.isTitan, 2);
                                targetPlayer.SetCustomProperties(propertiesToSet);
                                base.photonView.RPC("spawnTitanRPC", targetPlayer, new object[0]);
                            }
                            else if (imatitan.ContainsKey(targetPlayer.ID))
                            {
                                for (int i = 0; i < this.heroes.Count; i++)
                                {
                                    HERO hero = (HERO) this.heroes[i];
                                    if (hero.photonView.owner == targetPlayer)
                                    {
                                        hero.markDie();
                                        hero.photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "no switching in infection" });
                                    }
                                }
                            }
                        }
                        else if (!((RCextensions.returnIntFromObject(targetPlayer.customProperties[PhotonPlayerProperty.isTitan]) != 2) || RCextensions.returnBoolFromObject(targetPlayer.customProperties[PhotonPlayerProperty.dead])))
                        {
                            num20++;
                        }
                    }
                }
                if ((num20 <= 0) && (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.KILL_TITAN))
                {
                    this.gameWin2();
                }
            }
            else if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
            {
                if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
                {
                    if (this.cyanKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
                    {
                        object[] parameters = new object[] { "<color=#00FFFF>Team Cyan wins! </color>", string.Empty };
                        base.photonView.RPC("Chat", PhotonTargets.All, parameters);
                        this.gameWin2();
                    }
                    else if (this.magentaKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
                    {
                        object[] objArray2 = new object[] { "<color=#FF00FF>Team Magenta wins! </color>", string.Empty };
                        base.photonView.RPC("Chat", PhotonTargets.All, objArray2);
                        this.gameWin2();
                    }
                }
                else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 0)
                {
                    for (num21 = 0; num21 < PhotonNetwork.playerList.Length; num21++)
                    {
                        PhotonPlayer player9 = PhotonNetwork.playerList[num21];
                        if (RCextensions.returnIntFromObject(player9.customProperties[PhotonPlayerProperty.kills]) >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
                        {
                            object[] objArray4 = new object[] { "<color=#FFCC00>" + RCextensions.returnStringFromObject(player9.customProperties[PhotonPlayerProperty.name]).hexColor() + " wins!</color>", string.Empty };
                            base.photonView.RPC("Chat", PhotonTargets.All, objArray4);
                            this.gameWin2();
                        }
                    }
                }
            }
            else if ((!SettingsManager.LegacyGameSettings.PointModeEnabled.Value) && ((SettingsManager.LegacyGameSettings.BombModeEnabled.Value) || (SettingsManager.LegacyGameSettings.BladePVP.Value > 0)))
            {
                if ((SettingsManager.LegacyGameSettings.TeamMode.Value > 0) && (PhotonNetwork.playerList.Length > 1))
                {
                    int num23 = 0;
                    int num24 = 0;
                    int num25 = 0;
                    int num26 = 0;
                    for (num21 = 0; num21 < PhotonNetwork.playerList.Length; num21++)
                    {
                        PhotonPlayer player10 = PhotonNetwork.playerList[num21];
                        if ((!ignoreList.Contains(player10.ID) && (player10.customProperties[PhotonPlayerProperty.RCteam] != null)) && (player10.customProperties[PhotonPlayerProperty.dead] != null))
                        {
                            if (RCextensions.returnIntFromObject(player10.customProperties[PhotonPlayerProperty.RCteam]) == 1)
                            {
                                num25++;
                                if (!RCextensions.returnBoolFromObject(player10.customProperties[PhotonPlayerProperty.dead]))
                                {
                                    num23++;
                                }
                            }
                            else if (RCextensions.returnIntFromObject(player10.customProperties[PhotonPlayerProperty.RCteam]) == 2)
                            {
                                num26++;
                                if (!RCextensions.returnBoolFromObject(player10.customProperties[PhotonPlayerProperty.dead]))
                                {
                                    num24++;
                                }
                            }
                        }
                    }
                    if ((num25 > 0) && (num26 > 0))
                    {
                        if (num23 == 0)
                        {
                            object[] objArray5 = new object[] { "<color=#FF00FF>Team Magenta wins! </color>", string.Empty };
                            base.photonView.RPC("Chat", PhotonTargets.All, objArray5);
                            this.gameWin2();
                        }
                        else if (num24 == 0)
                        {
                            object[] objArray6 = new object[] { "<color=#00FFFF>Team Cyan wins! </color>", string.Empty };
                            base.photonView.RPC("Chat", PhotonTargets.All, objArray6);
                            this.gameWin2();
                        }
                    }
                }
                else if ((SettingsManager.LegacyGameSettings.TeamMode.Value == 0) && (PhotonNetwork.playerList.Length > 1))
                {
                    int num27 = 0;
                    string text = "Nobody";
                    PhotonPlayer player11 = PhotonNetwork.playerList[0];
                    for (num21 = 0; num21 < PhotonNetwork.playerList.Length; num21++)
                    {
                        PhotonPlayer player12 = PhotonNetwork.playerList[num21];
                        if (!((player12.customProperties[PhotonPlayerProperty.dead] == null) || RCextensions.returnBoolFromObject(player12.customProperties[PhotonPlayerProperty.dead])))
                        {
                            text = RCextensions.returnStringFromObject(player12.customProperties[PhotonPlayerProperty.name]).hexColor();
                            player11 = player12;
                            num27++;
                        }
                    }
                    if (num27 <= 1)
                    {
                        string str6 = " 5 points added.";
                        if (text == "Nobody")
                        {
                            str6 = string.Empty;
                        }
                        else
                        {
                            for (num21 = 0; num21 < 5; num21++)
                            {
                                this.playerKillInfoUpdate(player11, 0);
                            }
                        }
                        object[] objArray7 = new object[] { "<color=#FFCC00>" + text.hexColor() + " wins." + str6 + "</color>", string.Empty };
                        base.photonView.RPC("Chat", PhotonTargets.All, objArray7);
                        this.gameWin2();
                    }
                }
            }
        }
    }

    private void kickPhotonPlayer(string name)
    {
        UnityEngine.MonoBehaviour.print("KICK " + name + "!!!");
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if ((player.ID.ToString() == name) && !player.isMasterClient)
            {
                PhotonNetwork.CloseConnection(player);
                return;
            }
        }
    }

    private void kickPlayer(string kickPlayer, string kicker)
    {
        KickState state;
        bool flag = false;
        for (int i = 0; i < this.kicklist.Count; i++)
        {
            if (((KickState) this.kicklist[i]).name == kickPlayer)
            {
                state = (KickState) this.kicklist[i];
                state.addKicker(kicker);
                this.tryKick(state);
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            state = new KickState();
            state.init(kickPlayer);
            state.addKicker(kicker);
            this.kicklist.Add(state);
            this.tryKick(state);
        }
    }

    public void kickPlayerRCIfMC(PhotonPlayer player, bool ban, string reason)
    {
        if (PhotonNetwork.isMasterClient)
            kickPlayerRC(player, ban, reason);
    }

    public void kickPlayerRC(PhotonPlayer player, bool ban, string reason)
    {
        string str;
        if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
        {
            str = string.Empty;
            str = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
            ServerCloseConnection(player, ban, str);
        }
        else
        {
            if (PhotonNetwork.isMasterClient && player == PhotonNetwork.player && reason != string.Empty)
            {
                this.chatRoom.addLINE("Attempting to ban myself for:" + reason + ", please report this to the devs.");
                return;
            }
            PhotonNetwork.DestroyPlayerObjects(player);
            PhotonNetwork.CloseConnection(player);
            base.photonView.RPC("ignorePlayer", PhotonTargets.Others, new object[] { player.ID });
            if (!ignoreList.Contains(player.ID))
            {
                ignoreList.Add(player.ID);
                RaiseEventOptions options = new RaiseEventOptions {
                    TargetActors = new int[] { player.ID }
                };
                PhotonNetwork.RaiseEvent(0xfe, null, true, options);
            }
            if (!(!ban || banHash.ContainsKey(player.ID)))
            {
                str = string.Empty;
                str = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
                banHash.Add(player.ID, str);
            }
            if (reason != string.Empty)
            {
                this.chatRoom.addLINE("Player " + player.ID.ToString() + " was autobanned. Reason:" + reason);
            }
            this.RecompilePlayerList(0.1f);
        }
    }

    [RPC]
    private void labelRPC(int setting, PhotonMessageInfo info)
    {
        if (PhotonView.Find(setting) != null)
        {
            PhotonPlayer owner = PhotonView.Find(setting).owner;
            if (owner == info.sender)
            {
                string str = RCextensions.returnStringFromObject(owner.customProperties[PhotonPlayerProperty.guildName]);
                string str2 = RCextensions.returnStringFromObject(owner.customProperties[PhotonPlayerProperty.name]);
                GameObject gameObject = PhotonView.Find(setting).gameObject;
                if (gameObject != null)
                {
                    HERO component = gameObject.GetComponent<HERO>();
                    if (component != null)
                    {
                        if (str != string.Empty)
                        {
                            component.myNetWorkName.GetComponent<UILabel>().text = "[FFFF00]" + str + "\n[FFFFFF]" + str2;
                        }
                        else
                        {
                            component.myNetWorkName.GetComponent<UILabel>().text = str2;
                        }
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (this.gameStart)
        {
            IEnumerator enumerator = this.heroes.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ((HERO) enumerator.Current).lateUpdate2();
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                	disposable.Dispose();
            }
            IEnumerator enumerator2 = this.eT.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    ((TITAN_EREN) enumerator2.Current).lateUpdate();
                }
            }
            finally
            {
                IDisposable disposable2 = enumerator2 as IDisposable;
                if (disposable2 != null)
                	disposable2.Dispose();
            }
            IEnumerator enumerator3 = this.titans.GetEnumerator();
            try
            {
                while (enumerator3.MoveNext())
                {
                    ((TITAN) enumerator3.Current).lateUpdate2();
                }
            }
            finally
            {
                IDisposable disposable3 = enumerator3 as IDisposable;
                if (disposable3 != null)
                	disposable3.Dispose();
            }
            IEnumerator enumerator4 = this.fT.GetEnumerator();
            try
            {
                while (enumerator4.MoveNext())
                {
                    ((FEMALE_TITAN) enumerator4.Current).lateUpdate2();
                }
            }
            finally
            {
                IDisposable disposable4 = enumerator4 as IDisposable;
                if (disposable4 != null)
                	disposable4.Dispose();
            }
            this.core2();
        }
    }

    private void loadconfig()
    {
        object[] objArray = new object[500];
        objArray[0x1f] = 0;
        objArray[0x40] = 0;
        objArray[0x44] = 100;
        objArray[0x45] = "default";
        objArray[70] = "1";
        objArray[0x47] = "1";
        objArray[0x48] = "1";
        objArray[0x49] = 1f;
        objArray[0x4a] = 1f;
        objArray[0x4b] = 1f;
        objArray[0x4c] = 0;
        objArray[0x4d] = string.Empty;
        objArray[0x4e] = 0;
        objArray[0x4f] = "1.0";
        objArray[80] = "1.0";
        objArray[0x51] = 0;
        objArray[0x53] = "30";
        objArray[0x54] = 0;
        objArray[0x5b] = 0;
        objArray[100] = 0;
        objArray[0xb9] = 0;
        objArray[0xba] = 0;
        objArray[0xbb] = 0;
        objArray[0xbc] = 0;
        objArray[190] = 0;
        objArray[0xbf] = string.Empty;
        objArray[230] = 0;
        objArray[0x107] = 0;
        linkHash = new ExitGames.Client.Photon.Hashtable[] { new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable() };
        settingsOld = objArray;
        this.scroll = Vector2.zero;
        this.scroll2 = Vector2.zero;
        this.transparencySlider = 1f;
        SettingsManager.LegacyGeneralSettings.SetDefault();
        MaterialCache.Clear();
    }

    private void loadskin()
    {
        GameObject[] objArray;
        int num;
        GameObject obj2;
        if (((int) settingsOld[0x40]) >= 100)
        {
            string[] strArray2 = new string[] { "Flare", "LabelInfoBottomRight", "LabelNetworkStatus", "skill_cd_bottom", "GasUI" };
            objArray = (GameObject[]) UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
            for (num = 0; num < objArray.Length; num++)
            {
                obj2 = objArray[num];
                if ((obj2.name.Contains("TREE") || obj2.name.Contains("aot_supply")) || obj2.name.Contains("gameobjectOutSide"))
                {
                    UnityEngine.Object.Destroy(obj2);
                }
            }
            GameObject.Find("Cube_001").renderer.material.mainTexture = ((Material) RCassets.Load("grass")).mainTexture;
            UnityEngine.Object.Instantiate(RCassets.Load("spawnPlayer"), new Vector3(-10f, 1f, -10f), new Quaternion(0f, 0f, 0f, 1f));
            for (num = 0; num < strArray2.Length; num++)
            {
                string name = strArray2[num];
                GameObject obj3 = GameObject.Find(name);
                if (obj3 != null)
                {
                    UnityEngine.Object.Destroy(obj3);
                }
            }
            Camera.main.GetComponent<SpectatorMovement>().disable = true;
        }
        else
        {
            GameObject obj4;
            int num2;
            InstantiateTracker.instance.Dispose();
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
            {
                this.updateTime = 1f;
                if (oldScriptLogic != SettingsManager.LegacyGameSettings.LogicScript.Value)
                {
                    intVariables.Clear();
                    boolVariables.Clear();
                    stringVariables.Clear();
                    floatVariables.Clear();
                    globalVariables.Clear();
                    RCEvents.Clear();
                    RCVariableNames.Clear();
                    playerVariables.Clear();
                    titanVariables.Clear();
                    RCRegionTriggers.Clear();
                    oldScriptLogic = SettingsManager.LegacyGameSettings.LogicScript.Value;
                    this.compileScript(SettingsManager.LegacyGameSettings.LogicScript.Value);
                    if (RCEvents.ContainsKey("OnFirstLoad"))
                    {
                        RCEvent event2 = (RCEvent) RCEvents["OnFirstLoad"];
                        event2.checkEvent();
                    }
                }
                if (RCEvents.ContainsKey("OnRoundStart"))
                {
                    ((RCEvent) RCEvents["OnRoundStart"]).checkEvent();
                }
                base.photonView.RPC("setMasterRC", PhotonTargets.All, new object[0]);
            }
            logicLoaded = true;
            this.racingSpawnPoint = new Vector3(0f, 0f, 0f);
            this.racingSpawnPointSet = false;
            this.racingDoors = new List<GameObject>();
            this.allowedToCannon = new Dictionary<int, CannonValues>();
            bool sendSkins = false;
            string[] skyboxUrls = new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
            if (SettingsManager.CustomSkinSettings.Skybox.SkinsEnabled.Value)
            {
                SkyboxCustomSkinSet set = (SkyboxCustomSkinSet)SettingsManager.CustomSkinSettings.Skybox.GetSelectedSet();
                skyboxUrls = new string[] { set.Front.Value, set.Back.Value, set.Left.Value, set.Right.Value, set.Up.Value, set.Down.Value };
                sendSkins = true;
            }
            if ((!level.StartsWith("Custom") && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || PhotonNetwork.isMasterClient)))
            {
                string n = string.Empty;
                string url1 = string.Empty;
                string url2 = string.Empty;
                if (LevelInfo.getInfo(level).mapName.Contains("City") && SettingsManager.CustomSkinSettings.City.SkinsEnabled.Value)
                {
                    CityCustomSkinSet set = (CityCustomSkinSet)SettingsManager.CustomSkinSettings.City.GetSelectedSet();
                    List<string> houses = new List<string>();
                    foreach (StringSetting house in set.Houses.GetItems())
                        houses.Add(house.Value);
                    url1 = string.Join(",", houses.ToArray());
                    for (int i = 0; i < 250; i++)
                    {
                        n = n + Convert.ToString((int) UnityEngine.Random.Range((float) 0f, (float) 8f));
                    }
                    url2 = string.Join(",", new string[] { set.Ground.Value, set.Wall.Value, set.Gate.Value });
                    sendSkins = true;
                }
                else if (LevelInfo.getInfo(level).mapName.Contains("Forest") && SettingsManager.CustomSkinSettings.Forest.SkinsEnabled.Value)
                {
                    ForestCustomSkinSet set = (ForestCustomSkinSet)SettingsManager.CustomSkinSettings.Forest.GetSelectedSet();
                    List<string> trees = new List<string>();
                    foreach (StringSetting tree in set.TreeTrunks.GetItems())
                        trees.Add(tree.Value);
                    url1 = string.Join(",", trees.ToArray());
                    List<string> leafs = new List<string>();
                    foreach (StringSetting leaf in set.TreeLeafs.GetItems())
                        leafs.Add(leaf.Value);
                    leafs.Add(set.Ground.Value);
                    url2 = string.Join(",", leafs.ToArray());
                    for (int i = 0; i < 150; i++)
                    {
                        string str = Convert.ToString((int) UnityEngine.Random.Range((float) 0f, (float) 8f));
                        n = n + str;
                        if (!set.RandomizedPairs.Value)
                        {
                            n = n + str;
                        }
                        else
                        {
                            n = n + Convert.ToString((int) UnityEngine.Random.Range((float) 0f, (float) 8f));
                        }
                    }
                    sendSkins = true;
                }
                if (sendSkins)
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        base.StartCoroutine(this.loadskinE(n, url1, url2, skyboxUrls));
                    }
                    else if (PhotonNetwork.isMasterClient)
                    {
                        base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, new object[] { n, url1, url2, skyboxUrls });
                    }
                }
            }
            else if (level.StartsWith("Custom") && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE))
            {
                GameObject[] objArray3 = GameObject.FindGameObjectsWithTag("playerRespawn");
                for (num = 0; num < objArray3.Length; num++)
                {
                    obj4 = objArray3[num];
                    obj4.transform.position = new Vector3(UnityEngine.Random.Range((float) -5f, (float) 5f), 0f, UnityEngine.Random.Range((float) -5f, (float) 5f));
                }
                objArray = (GameObject[]) UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
                for (num = 0; num < objArray.Length; num++)
                {
                    obj2 = objArray[num];
                    if (obj2.name.Contains("TREE") || obj2.name.Contains("aot_supply"))
                    {
                        UnityEngine.Object.Destroy(obj2);
                    }
                    else if (((obj2.name == "Cube_001") && (obj2.transform.parent.gameObject.tag != "player")) && (obj2.renderer != null))
                    {
                        this.groundList.Add(obj2);
                        obj2.renderer.material.mainTexture = ((Material) RCassets.Load("grass")).mainTexture;
                    }
                }
                if (PhotonNetwork.isMasterClient)
                {
                    string[] urls = new string[7];
                    for (int i = 0; i < 6; i++)
                        urls[i] = skyboxUrls[i];
                    urls[6] = ((CustomLevelCustomSkinSet)SettingsManager.CustomSkinSettings.CustomLevel.GetSelectedSet()).Ground.Value;
                    SettingsManager.LegacyGameSettings.TitanSpawnCap.Value = Math.Min(100, SettingsManager.LegacyGameSettings.TitanSpawnCap.Value);
                    base.photonView.RPC("clearlevel", PhotonTargets.AllBuffered, new object[] { urls, SettingsManager.LegacyGameSettings.GameType.Value });
                    RCRegions.Clear();
                    if (oldScript != SettingsManager.LegacyGameSettings.LevelScript.Value)
                    {
                        ExitGames.Client.Photon.Hashtable hashtable;
                        this.levelCache.Clear();
                        this.titanSpawns.Clear();
                        this.playerSpawnsC.Clear();
                        this.playerSpawnsM.Clear();
                        this.titanSpawners.Clear();
                        currentLevel = string.Empty;
                        if (SettingsManager.LegacyGameSettings.LevelScript.Value == string.Empty)
                        {
                            hashtable = new ExitGames.Client.Photon.Hashtable();
                            hashtable.Add(PhotonPlayerProperty.currentLevel, currentLevel);
                            PhotonNetwork.player.SetCustomProperties(hashtable);
                            oldScript = SettingsManager.LegacyGameSettings.LevelScript.Value;
                        }
                        else
                        {
                            string[] strArray4 = Regex.Replace(SettingsManager.LegacyGameSettings.LevelScript.Value, @"\s+", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Split(new char[] { ';' });
                            for (num = 0; num < (Mathf.FloorToInt((float) ((strArray4.Length - 1) / 100)) + 1); num++)
                            {
                                string[] strArray5;
                                int num7;
                                string[] strArray6;
                                string str6;
                                if (num < Mathf.FloorToInt((float) (strArray4.Length / 100)))
                                {
                                    strArray5 = new string[0x65];
                                    num7 = 0;
                                    num2 = 100 * num;
                                    while (num2 < ((100 * num) + 100))
                                    {
                                        if (strArray4[num2].StartsWith("spawnpoint"))
                                        {
                                            strArray6 = strArray4[num2].Split(new char[] { ',' });
                                            if (strArray6[1] == "titan")
                                            {
                                                this.titanSpawns.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                            }
                                            else if (strArray6[1] == "playerC")
                                            {
                                                this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                            }
                                            else if (strArray6[1] == "playerM")
                                            {
                                                this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                            }
                                        }
                                        strArray5[num7] = strArray4[num2];
                                        num7++;
                                        num2++;
                                    }
                                    str6 = UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                                    strArray5[100] = str6;
                                    currentLevel = currentLevel + str6;
                                    this.levelCache.Add(strArray5);
                                }
                                else
                                {
                                    strArray5 = new string[(strArray4.Length % 100) + 1];
                                    num7 = 0;
                                    for (num2 = 100 * num; num2 < ((100 * num) + (strArray4.Length % 100)); num2++)
                                    {
                                        if (strArray4[num2].StartsWith("spawnpoint"))
                                        {
                                            strArray6 = strArray4[num2].Split(new char[] { ',' });
                                            if (strArray6[1] == "titan")
                                            {
                                                this.titanSpawns.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                            }
                                            else if (strArray6[1] == "playerC")
                                            {
                                                this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                            }
                                            else if (strArray6[1] == "playerM")
                                            {
                                                this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                            }
                                        }
                                        strArray5[num7] = strArray4[num2];
                                        num7++;
                                    }
                                    str6 = UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                                    strArray5[strArray4.Length % 100] = str6;
                                    currentLevel = currentLevel + str6;
                                    this.levelCache.Add(strArray5);
                                }
                            }
                            List<string> list = new List<string>();
                            foreach (Vector3 vector in this.titanSpawns)
                            {
                                list.Add("titan," + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString());
                            }
                            foreach (Vector3 vector in this.playerSpawnsC)
                            {
                                list.Add("playerC," + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString());
                            }
                            foreach (Vector3 vector in this.playerSpawnsM)
                            {
                                list.Add("playerM," + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString());
                            }
                            string item = "a" + UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                            list.Add(item);
                            currentLevel = item + currentLevel;
                            this.levelCache.Insert(0, list.ToArray());
                            string str8 = "z" + UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                            this.levelCache.Add(new string[] { str8 });
                            currentLevel = currentLevel + str8;
                            hashtable = new ExitGames.Client.Photon.Hashtable();
                            hashtable.Add(PhotonPlayerProperty.currentLevel, currentLevel);
                            PhotonNetwork.player.SetCustomProperties(hashtable);
                            oldScript = SettingsManager.LegacyGameSettings.LevelScript.Value;
                        }
                    }
                    for (num = 0; num < PhotonNetwork.playerList.Length; num++)
                    {
                        PhotonPlayer player = PhotonNetwork.playerList[num];
                        if (!player.isMasterClient)
                        {
                            this.playersRPC.Add(player);
                        }
                    }
                    base.StartCoroutine(this.customlevelE(this.playersRPC));
                    base.StartCoroutine(this.customlevelcache());
                }
            }
        }
    }

    private IEnumerator loadskinE(string n, string url, string url2, string[] skybox)
    {
        if (IsValidSkybox(skybox))
            yield return StartCoroutine(_skyboxCustomSkinLoader.LoadSkinsFromRPC(skybox));
        else
            SkyboxCustomSkinLoader.SkyboxMaterial = null;
        if (n != string.Empty)
        {
            if (LevelInfo.getInfo(level).mapName.Contains("Forest"))
            {
                yield return StartCoroutine(_forestCustomSkinLoader.LoadSkinsFromRPC(new object[] { n, url, url2 }));
            }
            else if (LevelInfo.getInfo(level).mapName.Contains("City"))
            {
                yield return StartCoroutine(_cityCustomSkinLoader.LoadSkinsFromRPC(new object[] { n, url, url2 }));
            }
        }
        Minimap.TryRecaptureInstance();
        StartCoroutine(reloadSky());
        yield return null;
    }

    private bool IsValidSkybox(string[] skybox)
    {
        foreach (string str in skybox)
        {
            if (TextureDownloader.ValidTextureURL(str))
                return true;
        }
        return false;
    }

    [RPC]
    private void loadskinRPC(string n, string url1, string url2, string[] skybox, PhotonMessageInfo info)
    {
        if (!info.sender.isMasterClient)
            return;
        if (LevelInfo.getInfo(level).mapName.Contains("Forest"))
        {
            BaseCustomSkinSettings<ForestCustomSkinSet> settings = SettingsManager.CustomSkinSettings.Forest;
            if (settings.SkinsEnabled.Value && (!settings.SkinsLocal.Value || PhotonNetwork.isMasterClient))
                base.StartCoroutine(this.loadskinE(n, url1, url2, skybox));
        }
        else if (LevelInfo.getInfo(level).mapName.Contains("City"))
        {
            BaseCustomSkinSettings<CityCustomSkinSet> settings = SettingsManager.CustomSkinSettings.City;
            if (settings.SkinsEnabled.Value && (!settings.SkinsLocal.Value || PhotonNetwork.isMasterClient))
                base.StartCoroutine(this.loadskinE(n, url1, url2, skybox));
        }
    }

    private IEnumerator loginFeng()
    {
        WWW iteratorVariable1;
        WWWForm form = new WWWForm();
        form.AddField("userid", usernameField);
        form.AddField("password", passwordField);
        if (Application.isWebPlayer)
        {
            iteratorVariable1 = new WWW("http://aotskins.com/version/getinfo.php", form);
        }
        else
        {
            iteratorVariable1 = new WWW("http://fenglee.com/game/aog/require_user_info.php", form);
        }
        yield return iteratorVariable1;
        if (!((iteratorVariable1.error != null) || iteratorVariable1.text.Contains("Error,please sign in again.")))
        {
            char[] separator = new char[] { '|' };
            string[] strArray = iteratorVariable1.text.Split(separator);
            LoginFengKAI.player.name = usernameField;
            LoginFengKAI.player.guildname = strArray[0];
            loginstate = 3;
        }
        else
        {
            loginstate = 2;
        }
    }

    private string mastertexturetype(int lol)
    {
        if (lol == 0)
        {
            return "High";
        }
        if (lol == 1)
        {
            return "Med";
        }
        return "Low";
    }

    public void multiplayerRacingFinsih()
    {
        float time = this.roundTime - 20f;
        if (PhotonNetwork.isMasterClient)
        {
            this.getRacingResult(LoginFengKAI.player.name, time, null);
        }
        else
        {
            object[] parameters = new object[] { LoginFengKAI.player.name, time };
            base.photonView.RPC("getRacingResult", PhotonTargets.MasterClient, parameters);
        }
        this.gameWin2();
    }

    [RPC]
    private void netGameLose(int score, PhotonMessageInfo info)
    {
        this.isLosing = true;
        this.titanScore = score;
        this.gameEndCD = this.gameEndTotalCDtime;
        if (SettingsManager.UISettings.GameFeed.Value)
        {
            this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game lose).");
        }
        if (((info.sender != PhotonNetwork.masterClient) && !info.sender.isLocal) && PhotonNetwork.isMasterClient)
        {
            this.chatRoom.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID.ToString() + "</color>");
        }
    }

    [RPC]
    private void netGameWin(int score, PhotonMessageInfo info)
    {
        this.humanScore = score;
        this.isWinning = true;
        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
        {
            this.teamWinner = score;
            this.teamScores[this.teamWinner - 1]++;
            this.gameEndCD = this.gameEndTotalCDtime;
        }
        else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
        {
            if (SettingsManager.LegacyGameSettings.RacingEndless.Value)
            {
                this.gameEndCD = 1000f;
            }
            else
            {
                this.gameEndCD = 20f;
            }
        }
        else
        {
            this.gameEndCD = this.gameEndTotalCDtime;
        }
        if (SettingsManager.UISettings.GameFeed.Value)
        {
            this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
        }
        if (!((info.sender == PhotonNetwork.masterClient) || info.sender.isLocal))
        {
            this.chatRoom.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID.ToString() + "</color>");
        }
    }

    [RPC]
    private void netRefreshRacingResult(string tmp)
    {
        this.localRacingResult = tmp;
    }

    [RPC]
    public void netShowDamage(int speed)
    {
        GameObject.Find("Stylish").GetComponent<StylishComponent>().Style(speed);
        GameObject target = GameObject.Find("LabelScore");
        if (target != null)
        {
            target.GetComponent<UILabel>().text = speed.ToString();
            target.transform.localScale = Vector3.zero;
            speed = (int) (speed * 0.1f);
            speed = Mathf.Max(40, speed);
            speed = Mathf.Min(150, speed);
            iTween.Stop(target);
            object[] args = new object[] { "x", speed, "y", speed, "z", speed, "easetype", iTween.EaseType.easeOutElastic, "time", 1f };
            iTween.ScaleTo(target, iTween.Hash(args));
            object[] objArray2 = new object[] { "x", 0, "y", 0, "z", 0, "easetype", iTween.EaseType.easeInBounce, "time", 0.5f, "delay", 2f };
            iTween.ScaleTo(target, iTween.Hash(objArray2));
        }
    }

    public void NOTSpawnNonAITitan(string id)
    {
        this.myLastHero = id.ToUpper();
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add("dead", true);
        ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add(PhotonPlayerProperty.isTitan, 2);
        propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        this.ShowHUDInfoCenter("the game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    }

    public void NOTSpawnNonAITitanRC(string id)
    {
        this.myLastHero = id.ToUpper();
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add("dead", true);
        ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add(PhotonPlayerProperty.isTitan, 2);
        propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        this.ShowHUDInfoCenter("Syncing spawn locations...");
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    }

    public void NOTSpawnPlayer(string id)
    {
        this.myLastHero = id.ToUpper();
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add("dead", true);
        ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add(PhotonPlayerProperty.isTitan, 1);
        propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        this.ShowHUDInfoCenter("the game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    }

    public void NOTSpawnPlayerRC(string id)
    {
        this.myLastHero = id.ToUpper();
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add("dead", true);
        ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add(PhotonPlayerProperty.isTitan, 1);
        propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    }

    public void OnConnectedToMaster()
    {
        UnityEngine.MonoBehaviour.print("OnConnectedToMaster");
    }

    public void OnConnectedToPhoton()
    {
        UnityEngine.MonoBehaviour.print("OnConnectedToPhoton");
    }

    public void OnConnectionFail(DisconnectCause cause)
    {
        UnityEngine.MonoBehaviour.print("OnConnectionFail : " + cause.ToString());
        IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
        this.gameStart = false;
        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[0], false);
        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[1], false);
        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[2], false);
        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[3], false);
        NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[4], true);
        GameObject.Find("LabelDisconnectInfo").GetComponent<UILabel>().text = "OnConnectionFail : " + cause.ToString();
    }

    public void OnCreatedRoom()
    {
        this.kicklist = new ArrayList();
        this.racingResult = new ArrayList();
        this.teamScores = new int[2];
        UnityEngine.MonoBehaviour.print("OnCreatedRoom");
    }

    public void OnCustomAuthenticationFailed()
    {
        UnityEngine.MonoBehaviour.print("OnCustomAuthenticationFailed");
    }

    public void OnDisconnectedFromPhoton()
    {
        UnityEngine.MonoBehaviour.print("OnDisconnectedFromPhoton");
    }

    [RPC]
    public void oneTitanDown(string name1, bool onPlayerLeave)
    {
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || PhotonNetwork.isMasterClient)
        {
            if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
            {
                if (name1 != string.Empty)
                {
                    if (name1 == "Titan")
                    {
                        this.PVPhumanScore++;
                    }
                    else if (name1 == "Aberrant")
                    {
                        this.PVPhumanScore += 2;
                    }
                    else if (name1 == "Jumper")
                    {
                        this.PVPhumanScore += 3;
                    }
                    else if (name1 == "Crawler")
                    {
                        this.PVPhumanScore += 4;
                    }
                    else if (name1 == "Female Titan")
                    {
                        this.PVPhumanScore += 10;
                    }
                    else
                    {
                        this.PVPhumanScore += 3;
                    }
                }
                this.checkPVPpts();
                object[] parameters = new object[] { this.PVPhumanScore, this.PVPtitanScore };
                base.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters);
            }
            else if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.CAGE_FIGHT)
            {
                if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN)
                {
                    if (this.checkIsTitanAllDie())
                    {
                        this.gameWin2();
                        Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
                    }
                }
                else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
                {
                    if (this.checkIsTitanAllDie())
                    {
                        this.wave++;
                        if (((LevelInfo.getInfo(level).respawnMode == RespawnMode.NEWROUND) || (level.StartsWith("Custom") && (SettingsManager.LegacyGameSettings.GameType.Value == 1))) && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER))
                        {
                            foreach (PhotonPlayer player in PhotonNetwork.playerList)
                            {
                                if (RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.isTitan]) != 2)
                                {
                                    base.photonView.RPC("respawnHeroInNewRound", player, new object[0]);
                                }
                            }
                        }
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                        {
                            this.sendChatContentInfo("<color=#A8FF24>Wave : " + this.wave + "</color>");
                        }
                        if (this.wave > this.highestwave)
                        {
                            this.highestwave = this.wave;
                        }
                        if (PhotonNetwork.isMasterClient)
                        {
                            this.RequireStatus();
                        }
                        if (((!SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value) && (this.wave > 20)) || ((SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value) && (this.wave > SettingsManager.LegacyGameSettings.TitanMaxWaves.Value)))
                        {
                            this.gameWin2();
                        }
                        else
                        {
                            int abnormal = 90;
                            if (this.difficulty == 1)
                            {
                                abnormal = 70;
                            }
                            if (!LevelInfo.getInfo(level).punk)
                            {
                                this.spawnTitanCustom("titanRespawn", abnormal, this.wave + 2, false);
                            }
                            else if (this.wave == 5)
                            {
                                this.spawnTitanCustom("titanRespawn", abnormal, 1, true);
                            }
                            else if (this.wave == 10)
                            {
                                this.spawnTitanCustom("titanRespawn", abnormal, 2, true);
                            }
                            else if (this.wave == 15)
                            {
                                this.spawnTitanCustom("titanRespawn", abnormal, 3, true);
                            }
                            else if (this.wave == 20)
                            {
                                this.spawnTitanCustom("titanRespawn", abnormal, 4, true);
                            }
                            else
                            {
                                this.spawnTitanCustom("titanRespawn", abnormal, this.wave + 2, false);
                            }
                        }
                    }
                }
                else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
                {
                    if (!onPlayerLeave)
                    {
                        this.humanScore++;
                        int num2 = 90;
                        if (this.difficulty == 1)
                        {
                            num2 = 70;
                        }
                        this.spawnTitanCustom("titanRespawn", num2, 1, false);
                    }
                }
                else if (LevelInfo.getInfo(level).enemyNumber == -1)
                {
                }
            }
        }
    }

    public void OnFailedToConnectToPhoton()
    {
        UnityEngine.MonoBehaviour.print("OnFailedToConnectToPhoton");
    }

    void DrawBackgroundIfLoading()
    {
        if (AssetBundleManager.Status == AssetBundleStatus.Loading || AutoUpdateManager.Status == AutoUpdateStatus.Updating)
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), this.textureBackgroundBlue);
    }

    public void OnGUI()
    {
        float num7;
        float num8;
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.STOP) && (Application.loadedLevelName != "characterCreation"))
        {
            LegacyPopupTemplate popup = new LegacyPopupTemplate(new Color(0f, 0f, 0f, 1f), textureBackgroundBlack, new Color(1f, 1f, 1f, 1f),
                    Screen.width / 2f, Screen.height / 2f, 230f, 140f, 2f);
            DrawBackgroundIfLoading();
            if (AutoUpdateManager.Status == AutoUpdateStatus.Updating)
            {
                popup.DrawPopup("Auto-updating mod...", 130f, 22f);
            }
            else if (AutoUpdateManager.Status == AutoUpdateStatus.NeedRestart && !AutoUpdateManager.CloseFailureBox)
            {
                bool[] buttons = (popup.DrawPopupWithTwoButtons("Mod has been updated and requires a restart.", 190f, 44f, "Restart Now", 90f, "Ignore", 90f, 25f));
                if (buttons[0])
                {
                    if (Application.platform == RuntimePlatform.WindowsPlayer)
                        Process.Start(Application.dataPath.Replace("_Data", ".exe"));
                    else if (Application.platform == RuntimePlatform.OSXPlayer)
                        Process.Start(Application.dataPath + "/MacOS/MacTest");
                    Application.Quit();
                }
                else if (buttons[1])
                    AutoUpdateManager.CloseFailureBox = true;
            }
            else if (AutoUpdateManager.Status == AutoUpdateStatus.LauncherOutdated && !AutoUpdateManager.CloseFailureBox)
            {
                
                if (popup.DrawPopupWithButton("Game launcher is outdated, visit aotrc.weebly.com for a new game version.", 190f, 66f, "Continue", 80f, 25f))
                    AutoUpdateManager.CloseFailureBox = true;
            }
            else if (AutoUpdateManager.Status == AutoUpdateStatus.FailedUpdate && !AutoUpdateManager.CloseFailureBox)
            {
                if (popup.DrawPopupWithButton("Auto-update failed, check internet connection or aotrc.weebly.com for a new game version.", 190f, 66f, "Continue", 80f, 25f))
                    AutoUpdateManager.CloseFailureBox = true;
            }
            else if (AutoUpdateManager.Status == AutoUpdateStatus.MacTranslocated && !AutoUpdateManager.CloseFailureBox)
            {
                if (popup.DrawPopupWithButton("Your game is not in the Applications folder, cannot auto-update and some bugs may occur.", 190f, 66f, "Continue", 80f, 25f))
                    AutoUpdateManager.CloseFailureBox = true;
            }
            else if (AssetBundleManager.Status == AssetBundleStatus.Loading)
            {
                popup.DrawPopup("Downloading asset bundle...", 170f, 22f);
            }
            else if (AssetBundleManager.Status == AssetBundleStatus.Failed && !AssetBundleManager.CloseFailureBox)
            {
                if (popup.DrawPopupWithButton("Failed to load asset bundle, check your internet connection.", 190f, 44f, "Continue", 80f, 25f))
                    AssetBundleManager.CloseFailureBox = true;
            }
        }
        else if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.STOP)
        {
            bool flag2;
            int num13;
            int num18;
            TextEditor editor;
            int num23;
            Event current;
            bool flag4;
            string str4;
            bool flag5;
            Texture2D textured;
            bool flag6;
            int num30;
            bool flag10;
            if (((int) settingsOld[0x40]) >= 100)
            {
                GameObject obj4;
                float num14;
                Color color;
                Mesh mesh;
                Color[] colorArray;
                float num20;
                float num21;
                float num27;
                int num28;
                int num29;
                float num31;
                float num11 = Screen.width - 300f;
                GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
                GUI.DrawTexture(new Rect(7f, 7f, 291f, 586f), this.textureBackgroundBlue);
                GUI.DrawTexture(new Rect(num11 + 2f, 7f, 291f, 586f), this.textureBackgroundBlue);
                flag2 = false;
                bool flag3 = false;
                GUI.Box(new Rect(5f, 5f, 295f, 590f), string.Empty);
                GUI.Box(new Rect(num11, 5f, 295f, 590f), string.Empty);
                if (GUI.Button(new Rect(10f, 10f, 60f, 25f), "Script", "box"))
                {
                    settingsOld[0x44] = 100;
                }
                if (GUI.Button(new Rect(75f, 10f, 80f, 25f), "Full Screen", "box"))
                {
                    FullscreenHandler.ToggleFullscreen();
                }
                if ((((int) settingsOld[0x44]) == 100) || (((int) settingsOld[0x44]) == 0x66))
                {
                    string str2;
                    int num19;
                    GUI.Label(new Rect(115f, 40f, 100f, 20f), "Level Script:", "Label");
                    GUI.Label(new Rect(115f, 115f, 100f, 20f), "Import Data", "Label");
                    GUI.Label(new Rect(12f, 535f, 280f, 60f), "Warning: your current level will be lost if you quit or import data. Make sure to save the level to a text document.", "Label");
                    settingsOld[0x4d] = GUI.TextField(new Rect(10f, 140f, 285f, 350f), (string) settingsOld[0x4d]);
                    if (GUI.Button(new Rect(35f, 500f, 60f, 30f), "Apply"))
                    {
                        foreach (GameObject obj2 in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
                        {
                            if ((((obj2.name.StartsWith("custom") || obj2.name.StartsWith("base")) || (obj2.name.StartsWith("photon") || obj2.name.StartsWith("spawnpoint"))) || obj2.name.StartsWith("misc")) || obj2.name.StartsWith("racing"))
                            {
                                UnityEngine.Object.Destroy(obj2);
                            }
                        }
                        linkHash[3].Clear();
                        settingsOld[0xba] = 0;
                        string[] strArray = Regex.Replace((string) settingsOld[0x4d], @"\s+", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Split(new char[] { ';' });
                        for (num13 = 0; num13 < strArray.Length; num13++)
                        {
                            string[] strArray2 = strArray[num13].Split(new char[] { ',' });
                            if ((((strArray2[0].StartsWith("custom") || strArray2[0].StartsWith("base")) || (strArray2[0].StartsWith("photon") || strArray2[0].StartsWith("spawnpoint"))) || strArray2[0].StartsWith("misc")) || strArray2[0].StartsWith("racing"))
                            {
                                float num15;
                                float num16;
                                float num17;
                                GameObject obj3 = null;
                                if (strArray2[0].StartsWith("custom"))
                                {
                                    obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray2[1]), new Vector3(Convert.ToSingle(strArray2[12]), Convert.ToSingle(strArray2[13]), Convert.ToSingle(strArray2[14])), new Quaternion(Convert.ToSingle(strArray2[15]), Convert.ToSingle(strArray2[0x10]), Convert.ToSingle(strArray2[0x11]), Convert.ToSingle(strArray2[0x12])));
                                }
                                else if (strArray2[0].StartsWith("photon"))
                                {
                                    if (strArray2[1].StartsWith("Cannon"))
                                    {
                                        if (strArray2.Length < 15)
                                        {
                                            obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray2[1] + "Prop"), new Vector3(Convert.ToSingle(strArray2[2]), Convert.ToSingle(strArray2[3]), Convert.ToSingle(strArray2[4])), new Quaternion(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7]), Convert.ToSingle(strArray2[8])));
                                        }
                                        else
                                        {
                                            obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray2[1] + "Prop"), new Vector3(Convert.ToSingle(strArray2[12]), Convert.ToSingle(strArray2[13]), Convert.ToSingle(strArray2[14])), new Quaternion(Convert.ToSingle(strArray2[15]), Convert.ToSingle(strArray2[0x10]), Convert.ToSingle(strArray2[0x11]), Convert.ToSingle(strArray2[0x12])));
                                        }
                                    }
                                    else
                                    {
                                        obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray2[1]), new Vector3(Convert.ToSingle(strArray2[4]), Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6])), new Quaternion(Convert.ToSingle(strArray2[7]), Convert.ToSingle(strArray2[8]), Convert.ToSingle(strArray2[9]), Convert.ToSingle(strArray2[10])));
                                    }
                                }
                                else if (strArray2[0].StartsWith("spawnpoint"))
                                {
                                    obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray2[1]), new Vector3(Convert.ToSingle(strArray2[2]), Convert.ToSingle(strArray2[3]), Convert.ToSingle(strArray2[4])), new Quaternion(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7]), Convert.ToSingle(strArray2[8])));
                                }
                                else if (strArray2[0].StartsWith("base"))
                                {
                                    if (strArray2.Length < 15)
                                    {
                                        obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) Resources.Load(strArray2[1]), new Vector3(Convert.ToSingle(strArray2[2]), Convert.ToSingle(strArray2[3]), Convert.ToSingle(strArray2[4])), new Quaternion(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7]), Convert.ToSingle(strArray2[8])));
                                    }
                                    else
                                    {
                                        obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) Resources.Load(strArray2[1]), new Vector3(Convert.ToSingle(strArray2[12]), Convert.ToSingle(strArray2[13]), Convert.ToSingle(strArray2[14])), new Quaternion(Convert.ToSingle(strArray2[15]), Convert.ToSingle(strArray2[0x10]), Convert.ToSingle(strArray2[0x11]), Convert.ToSingle(strArray2[0x12])));
                                    }
                                }
                                else if (strArray2[0].StartsWith("misc"))
                                {
                                    if (strArray2[1].StartsWith("barrier"))
                                    {
                                        obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load("barrierEditor"), new Vector3(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7])), new Quaternion(Convert.ToSingle(strArray2[8]), Convert.ToSingle(strArray2[9]), Convert.ToSingle(strArray2[10]), Convert.ToSingle(strArray2[11])));
                                    }
                                    else if (strArray2[1].StartsWith("region"))
                                    {
                                        obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load("regionEditor"));
                                        obj3.transform.position = new Vector3(Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7]), Convert.ToSingle(strArray2[8]));
                                        obj4 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
                                        obj4.name = "RegionLabel";
                                        obj4.transform.parent = obj3.transform;
                                        num14 = 1f;
                                        if (Convert.ToSingle(strArray2[4]) > 100f)
                                        {
                                            num14 = 0.8f;
                                        }
                                        else if (Convert.ToSingle(strArray2[4]) > 1000f)
                                        {
                                            num14 = 0.5f;
                                        }
                                        obj4.transform.localPosition = new Vector3(0f, num14, 0f);
                                        obj4.transform.localScale = new Vector3(5f / Convert.ToSingle(strArray2[3]), 5f / Convert.ToSingle(strArray2[4]), 5f / Convert.ToSingle(strArray2[5]));
                                        obj4.GetComponent<UILabel>().text = strArray2[2];
                                        obj3.AddComponent<RCRegionLabel>();
                                        obj3.GetComponent<RCRegionLabel>().myLabel = obj4;
                                    }
                                    else if (strArray2[1].StartsWith("racingStart"))
                                    {
                                        obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load("racingStart"), new Vector3(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7])), new Quaternion(Convert.ToSingle(strArray2[8]), Convert.ToSingle(strArray2[9]), Convert.ToSingle(strArray2[10]), Convert.ToSingle(strArray2[11])));
                                    }
                                    else if (strArray2[1].StartsWith("racingEnd"))
                                    {
                                        obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load("racingEnd"), new Vector3(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7])), new Quaternion(Convert.ToSingle(strArray2[8]), Convert.ToSingle(strArray2[9]), Convert.ToSingle(strArray2[10]), Convert.ToSingle(strArray2[11])));
                                    }
                                }
                                else if (strArray2[0].StartsWith("racing"))
                                {
                                    obj3 = (GameObject) UnityEngine.Object.Instantiate((GameObject) RCassets.Load(strArray2[1]), new Vector3(Convert.ToSingle(strArray2[5]), Convert.ToSingle(strArray2[6]), Convert.ToSingle(strArray2[7])), new Quaternion(Convert.ToSingle(strArray2[8]), Convert.ToSingle(strArray2[9]), Convert.ToSingle(strArray2[10]), Convert.ToSingle(strArray2[11])));
                                }
                                if ((strArray2[2] != "default") && ((strArray2[0].StartsWith("custom") || (strArray2[0].StartsWith("base") && (strArray2.Length > 15))) || (strArray2[0].StartsWith("photon") && (strArray2.Length > 15))))
                                {
                                    foreach (Renderer renderer in obj3.GetComponentsInChildren<Renderer>())
                                    {
                                        if (!(renderer.name.Contains("Particle System") && obj3.name.Contains("aot_supply")))
                                        {
                                            renderer.material = (Material) RCassets.Load(strArray2[2]);
                                            renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray2[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray2[11]));
                                        }
                                    }
                                }
                                if ((strArray2[0].StartsWith("custom") || (strArray2[0].StartsWith("base") && (strArray2.Length > 15))) || (strArray2[0].StartsWith("photon") && (strArray2.Length > 15)))
                                {
                                    num15 = obj3.transform.localScale.x * Convert.ToSingle(strArray2[3]);
                                    num15 -= 0.001f;
                                    num16 = obj3.transform.localScale.y * Convert.ToSingle(strArray2[4]);
                                    num17 = obj3.transform.localScale.z * Convert.ToSingle(strArray2[5]);
                                    obj3.transform.localScale = new Vector3(num15, num16, num17);
                                    if (strArray2[6] != "0")
                                    {
                                        color = new Color(Convert.ToSingle(strArray2[7]), Convert.ToSingle(strArray2[8]), Convert.ToSingle(strArray2[9]), 1f);
                                        foreach (MeshFilter filter in obj3.GetComponentsInChildren<MeshFilter>())
                                        {
                                            mesh = filter.mesh;
                                            colorArray = new Color[mesh.vertexCount];
                                            num18 = 0;
                                            while (num18 < mesh.vertexCount)
                                            {
                                                colorArray[num18] = color;
                                                num18++;
                                            }
                                            mesh.colors = colorArray;
                                        }
                                    }
                                    obj3.name = strArray2[0] + "," + strArray2[1] + "," + strArray2[2] + "," + strArray2[3] + "," + strArray2[4] + "," + strArray2[5] + "," + strArray2[6] + "," + strArray2[7] + "," + strArray2[8] + "," + strArray2[9] + "," + strArray2[10] + "," + strArray2[11];
                                }
                                else if (strArray2[0].StartsWith("misc"))
                                {
                                    if (strArray2[1].StartsWith("barrier") || strArray2[1].StartsWith("racing"))
                                    {
                                        num15 = obj3.transform.localScale.x * Convert.ToSingle(strArray2[2]);
                                        num15 -= 0.001f;
                                        num16 = obj3.transform.localScale.y * Convert.ToSingle(strArray2[3]);
                                        num17 = obj3.transform.localScale.z * Convert.ToSingle(strArray2[4]);
                                        obj3.transform.localScale = new Vector3(num15, num16, num17);
                                        obj3.name = strArray2[0] + "," + strArray2[1] + "," + strArray2[2] + "," + strArray2[3] + "," + strArray2[4];
                                    }
                                    else if (strArray2[1].StartsWith("region"))
                                    {
                                        num15 = obj3.transform.localScale.x * Convert.ToSingle(strArray2[3]);
                                        num15 -= 0.001f;
                                        num16 = obj3.transform.localScale.y * Convert.ToSingle(strArray2[4]);
                                        num17 = obj3.transform.localScale.z * Convert.ToSingle(strArray2[5]);
                                        obj3.transform.localScale = new Vector3(num15, num16, num17);
                                        obj3.name = strArray2[0] + "," + strArray2[1] + "," + strArray2[2] + "," + strArray2[3] + "," + strArray2[4] + "," + strArray2[5];
                                    }
                                }
                                else if (strArray2[0].StartsWith("racing"))
                                {
                                    num15 = obj3.transform.localScale.x * Convert.ToSingle(strArray2[2]);
                                    num15 -= 0.001f;
                                    num16 = obj3.transform.localScale.y * Convert.ToSingle(strArray2[3]);
                                    num17 = obj3.transform.localScale.z * Convert.ToSingle(strArray2[4]);
                                    obj3.transform.localScale = new Vector3(num15, num16, num17);
                                    obj3.name = strArray2[0] + "," + strArray2[1] + "," + strArray2[2] + "," + strArray2[3] + "," + strArray2[4];
                                }
                                else if (!(!strArray2[0].StartsWith("photon") || strArray2[1].StartsWith("Cannon")))
                                {
                                    obj3.name = strArray2[0] + "," + strArray2[1] + "," + strArray2[2] + "," + strArray2[3];
                                }
                                else
                                {
                                    obj3.name = strArray2[0] + "," + strArray2[1];
                                }
                                linkHash[3].Add(obj3.GetInstanceID(), strArray[num13]);
                            }
                            else if (strArray2[0].StartsWith("map") && strArray2[1].StartsWith("disablebounds"))
                            {
                                settingsOld[0xba] = 1;
                                if (!linkHash[3].ContainsKey("mapbounds"))
                                {
                                    linkHash[3].Add("mapbounds", "map,disablebounds");
                                }
                            }
                        }
                        this.unloadAssets();
                        settingsOld[0x4d] = string.Empty;
                    }
                    else if (GUI.Button(new Rect(205f, 500f, 60f, 30f), "Exit"))
                    {
                        IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
                        UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
                        Application.LoadLevel("menu");
                    }
                    else if (GUI.Button(new Rect(15f, 70f, 115f, 30f), "Copy to Clipboard"))
                    {
                        str2 = string.Empty;
                        num19 = 0;
                        foreach (string str3 in linkHash[3].Values)
                        {
                            num19++;
                            str2 = str2 + str3 + ";\n";
                        }
                        editor = new TextEditor {
                            content = new GUIContent(str2)
                        };
                        editor.SelectAll();
                        editor.Copy();
                    }
                    else if (GUI.Button(new Rect(175f, 70f, 115f, 30f), "View Script"))
                    {
                        settingsOld[0x44] = 0x66;
                    }
                    if (((int) settingsOld[0x44]) == 0x66)
                    {
                        str2 = string.Empty;
                        num19 = 0;
                        foreach (string str3 in linkHash[3].Values)
                        {
                            num19++;
                            str2 = str2 + str3 + ";\n";
                        }
                        num20 = (Screen.width / 2) - 110.5f;
                        num21 = (Screen.height / 2) - 250f;
                        GUI.DrawTexture(new Rect(num20 + 2f, num21 + 2f, 217f, 496f), this.textureBackgroundBlue);
                        GUI.Box(new Rect(num20, num21, 221f, 500f), string.Empty);
                        if (GUI.Button(new Rect(num20 + 10f, num21 + 460f, 60f, 30f), "Copy"))
                        {
                            editor = new TextEditor {
                                content = new GUIContent(str2)
                            };
                            editor.SelectAll();
                            editor.Copy();
                        }
                        else if (GUI.Button(new Rect(num20 + 151f, num21 + 460f, 60f, 30f), "Done"))
                        {
                            settingsOld[0x44] = 100;
                        }
                        GUI.TextArea(new Rect(num20 + 5f, num21 + 5f, 211f, 415f), str2);
                        GUI.Label(new Rect(num20 + 10f, num21 + 430f, 150f, 20f), "Object Count: " + Convert.ToString(num19), "Label");
                    }
                }
                if ((((int) settingsOld[0x40]) != 0x69) && (((int) settingsOld[0x40]) != 0x6a))
                {
                    GUI.Label(new Rect(num11 + 13f, 445f, 125f, 20f), "Scale Multipliers:", "Label");
                    GUI.Label(new Rect(num11 + 13f, 470f, 50f, 22f), "Length:", "Label");
                    settingsOld[0x48] = GUI.TextField(new Rect(num11 + 58f, 470f, 40f, 20f), (string) settingsOld[0x48]);
                    GUI.Label(new Rect(num11 + 13f, 495f, 50f, 20f), "Width:", "Label");
                    settingsOld[70] = GUI.TextField(new Rect(num11 + 58f, 495f, 40f, 20f), (string) settingsOld[70]);
                    GUI.Label(new Rect(num11 + 13f, 520f, 50f, 22f), "Height:", "Label");
                    settingsOld[0x47] = GUI.TextField(new Rect(num11 + 58f, 520f, 40f, 20f), (string) settingsOld[0x47]);
                    if (((int) settingsOld[0x40]) <= 0x6a)
                    {
                        GUI.Label(new Rect(num11 + 155f, 554f, 50f, 22f), "Tiling:", "Label");
                        settingsOld[0x4f] = GUI.TextField(new Rect(num11 + 200f, 554f, 40f, 20f), (string) settingsOld[0x4f]);
                        settingsOld[80] = GUI.TextField(new Rect(num11 + 245f, 554f, 40f, 20f), (string) settingsOld[80]);
                        GUI.Label(new Rect(num11 + 219f, 570f, 10f, 22f), "x:", "Label");
                        GUI.Label(new Rect(num11 + 264f, 570f, 10f, 22f), "y:", "Label");
                        GUI.Label(new Rect(num11 + 155f, 445f, 50f, 20f), "Color:", "Label");
                        GUI.Label(new Rect(num11 + 155f, 470f, 10f, 20f), "R:", "Label");
                        GUI.Label(new Rect(num11 + 155f, 495f, 10f, 20f), "G:", "Label");
                        GUI.Label(new Rect(num11 + 155f, 520f, 10f, 20f), "B:", "Label");
                        settingsOld[0x49] = GUI.HorizontalSlider(new Rect(num11 + 170f, 475f, 100f, 20f), (float) settingsOld[0x49], 0f, 1f);
                        settingsOld[0x4a] = GUI.HorizontalSlider(new Rect(num11 + 170f, 500f, 100f, 20f), (float) settingsOld[0x4a], 0f, 1f);
                        settingsOld[0x4b] = GUI.HorizontalSlider(new Rect(num11 + 170f, 525f, 100f, 20f), (float) settingsOld[0x4b], 0f, 1f);
                        GUI.Label(new Rect(num11 + 13f, 554f, 57f, 22f), "Material:", "Label");
                        if (GUI.Button(new Rect(num11 + 66f, 554f, 60f, 20f), (string) settingsOld[0x45]))
                        {
                            settingsOld[0x4e] = 1;
                        }
                        if (((int) settingsOld[0x4e]) == 1)
                        {
                            string[] strArray4 = new string[] { "bark", "bark2", "bark3", "bark4" };
                            string[] strArray5 = new string[] { "wood1", "wood2", "wood3", "wood4" };
                            string[] strArray6 = new string[] { "grass", "grass2", "grass3", "grass4" };
                            string[] strArray7 = new string[] { "brick1", "brick2", "brick3", "brick4" };
                            string[] strArray8 = new string[] { "metal1", "metal2", "metal3", "metal4" };
                            string[] strArray9 = new string[] { "rock1", "rock2", "rock3" };
                            string[] strArray10 = new string[] { "stone1", "stone2", "stone3", "stone4", "stone5", "stone6", "stone7", "stone8", "stone9", "stone10" };
                            string[] strArray11 = new string[] { "earth1", "earth2", "ice1", "lava1", "crystal1", "crystal2", "empty" };
                            string[] strArray12 = new string[0];
                            List<string[]> list2 = new List<string[]> {
                                strArray4,
                                strArray5,
                                strArray6,
                                strArray7,
                                strArray8,
                                strArray9,
                                strArray10,
                                strArray11
                            };
                            string[] strArray13 = new string[] { "bark", "wood", "grass", "brick", "metal", "rock", "stone", "misc", "transparent" };
                            int index = 0x4e;
                            int num25 = 0x45;
                            num20 = (Screen.width / 2) - 110.5f;
                            num21 = (Screen.height / 2) - 220f;
                            int num26 = (int) settingsOld[0xb9];
                            num27 = 10f + (104f * ((list2[num26].Length / 3) + 1));
                            num27 = Math.Max(num27, 280f);
                            GUI.DrawTexture(new Rect(num20 + 2f, num21 + 2f, 208f, 446f), this.textureBackgroundBlue);
                            GUI.Box(new Rect(num20, num21, 212f, 450f), string.Empty);
                            for (num13 = 0; num13 < list2.Count; num13++)
                            {
                                num28 = num13 / 3;
                                num29 = num13 % 3;
                                if (GUI.Button(new Rect((num20 + 5f) + (69f * num29), (num21 + 5f) + (30 * num28), 64f, 25f), strArray13[num13], "box"))
                                {
                                    settingsOld[0xb9] = num13;
                                }
                            }
                            this.scroll2 = GUI.BeginScrollView(new Rect(num20, num21 + 110f, 225f, 290f), this.scroll2, new Rect(num20, num21 + 110f, 212f, num27), true, true);
                            if (num26 != 8)
                            {
                                for (num13 = 0; num13 < list2[num26].Length; num13++)
                                {
                                    num28 = num13 / 3;
                                    num29 = num13 % 3;
                                    GUI.DrawTexture(new Rect((num20 + 5f) + (69f * num29), (num21 + 115f) + (104f * num28), 64f, 64f), this.RCLoadTexture("p" + list2[num26][num13]));
                                    if (GUI.Button(new Rect((num20 + 5f) + (69f * num29), (num21 + 184f) + (104f * num28), 64f, 30f), list2[num26][num13]))
                                    {
                                        settingsOld[num25] = list2[num26][num13];
                                        settingsOld[index] = 0;
                                    }
                                }
                            }
                            GUI.EndScrollView();
                            if (GUI.Button(new Rect(num20 + 24f, num21 + 410f, 70f, 30f), "Default"))
                            {
                                settingsOld[num25] = "default";
                                settingsOld[index] = 0;
                            }
                            else if (GUI.Button(new Rect(num20 + 118f, num21 + 410f, 70f, 30f), "Done"))
                            {
                                settingsOld[index] = 0;
                            }
                        }
                        flag5 = false;
                        if (((int) settingsOld[0x4c]) == 1)
                        {
                            flag5 = true;
                            textured = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                            textured.SetPixel(0, 0, new Color((float) settingsOld[0x49], (float) settingsOld[0x4a], (float) settingsOld[0x4b], 1f));
                            textured.Apply();
                            GUI.DrawTexture(new Rect(num11 + 235f, 445f, 30f, 20f), textured, ScaleMode.StretchToFill);
                            UnityEngine.Object.Destroy(textured);
                        }
                        flag6 = GUI.Toggle(new Rect(num11 + 193f, 445f, 40f, 20f), flag5, "On");
                        if (flag5 != flag6)
                        {
                            if (flag6)
                            {
                                settingsOld[0x4c] = 1;
                            }
                            else
                            {
                                settingsOld[0x4c] = 0;
                            }
                        }
                    }
                }
                if (GUI.Button(new Rect(num11 + 5f, 10f, 60f, 25f), "General", "box"))
                {
                    settingsOld[0x40] = 0x65;
                }
                else if (GUI.Button(new Rect(num11 + 70f, 10f, 70f, 25f), "Geometry", "box"))
                {
                    settingsOld[0x40] = 0x66;
                }
                else if (GUI.Button(new Rect(num11 + 145f, 10f, 65f, 25f), "Buildings", "box"))
                {
                    settingsOld[0x40] = 0x67;
                }
                else if (GUI.Button(new Rect(num11 + 215f, 10f, 50f, 25f), "Nature", "box"))
                {
                    settingsOld[0x40] = 0x68;
                }
                else if (GUI.Button(new Rect(num11 + 5f, 45f, 70f, 25f), "Spawners", "box"))
                {
                    settingsOld[0x40] = 0x69;
                }
                else if (GUI.Button(new Rect(num11 + 80f, 45f, 70f, 25f), "Racing", "box"))
                {
                    settingsOld[0x40] = 0x6c;
                }
                else if (GUI.Button(new Rect(num11 + 155f, 45f, 40f, 25f), "Misc", "box"))
                {
                    settingsOld[0x40] = 0x6b;
                }
                else if (GUI.Button(new Rect(num11 + 200f, 45f, 70f, 25f), "Credits", "box"))
                {
                    settingsOld[0x40] = 0x6a;
                }
                if (((int) settingsOld[0x40]) == 0x65)
                {
                    GameObject obj5;
                    this.scroll = GUI.BeginScrollView(new Rect(num11, 80f, 305f, 350f), this.scroll, new Rect(num11, 80f, 300f, 470f), true, true);
                    GUI.Label(new Rect(num11 + 100f, 80f, 120f, 20f), "General Objects:", "Label");
                    GUI.Label(new Rect(num11 + 108f, 245f, 120f, 20f), "Spawn Points:", "Label");
                    GUI.Label(new Rect(num11 + 7f, 415f, 290f, 60f), "* The above titan spawn points apply only to randomly spawned titans specified by the Random Titan #.", "Label");
                    GUI.Label(new Rect(num11 + 7f, 470f, 290f, 60f), "* If team mode is disabled both cyan and magenta spawn points will be randomly chosen for players.", "Label");
                    GUI.DrawTexture(new Rect(num11 + 27f, 110f, 64f, 64f), this.RCLoadTexture("psupply"));
                    GUI.DrawTexture(new Rect(num11 + 118f, 110f, 64f, 64f), this.RCLoadTexture("pcannonwall"));
                    GUI.DrawTexture(new Rect(num11 + 209f, 110f, 64f, 64f), this.RCLoadTexture("pcannonground"));
                    GUI.DrawTexture(new Rect(num11 + 27f, 275f, 64f, 64f), this.RCLoadTexture("pspawnt"));
                    GUI.DrawTexture(new Rect(num11 + 118f, 275f, 64f, 64f), this.RCLoadTexture("pspawnplayerC"));
                    GUI.DrawTexture(new Rect(num11 + 209f, 275f, 64f, 64f), this.RCLoadTexture("pspawnplayerM"));
                    if (GUI.Button(new Rect(num11 + 27f, 179f, 64f, 60f), "Supply"))
                    {
                        flag2 = true;
                        obj5 = (GameObject) Resources.Load("aot_supply");
                        this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj5);
                        this.selectedObj.name = "base,aot_supply";
                    }
                    else if (GUI.Button(new Rect(num11 + 118f, 179f, 64f, 60f), "Cannon \nWall"))
                    {
                        flag2 = true;
                        obj5 = (GameObject) RCassets.Load("CannonWallProp");
                        this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj5);
                        this.selectedObj.name = "photon,CannonWall";
                    }
                    else if (GUI.Button(new Rect(num11 + 209f, 179f, 64f, 60f), "Cannon\n Ground"))
                    {
                        flag2 = true;
                        obj5 = (GameObject) RCassets.Load("CannonGroundProp");
                        this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj5);
                        this.selectedObj.name = "photon,CannonGround";
                    }
                    else if (GUI.Button(new Rect(num11 + 27f, 344f, 64f, 60f), "Titan"))
                    {
                        flag2 = true;
                        flag3 = true;
                        obj5 = (GameObject) RCassets.Load("titan");
                        this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj5);
                        this.selectedObj.name = "spawnpoint,titan";
                    }
                    else if (GUI.Button(new Rect(num11 + 118f, 344f, 64f, 60f), "Player \nCyan"))
                    {
                        flag2 = true;
                        flag3 = true;
                        obj5 = (GameObject) RCassets.Load("playerC");
                        this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj5);
                        this.selectedObj.name = "spawnpoint,playerC";
                    }
                    else if (GUI.Button(new Rect(num11 + 209f, 344f, 64f, 60f), "Player \nMagenta"))
                    {
                        flag2 = true;
                        flag3 = true;
                        obj5 = (GameObject) RCassets.Load("playerM");
                        this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj5);
                        this.selectedObj.name = "spawnpoint,playerM";
                    }
                    GUI.EndScrollView();
                }
                else
                {
                    GameObject obj6;
                    if (((int) settingsOld[0x40]) == 0x6b)
                    {
                        GUI.DrawTexture(new Rect(num11 + 30f, 90f, 64f, 64f), this.RCLoadTexture("pbarrier"));
                        GUI.DrawTexture(new Rect(num11 + 30f, 199f, 64f, 64f), this.RCLoadTexture("pregion"));
                        GUI.Label(new Rect(num11 + 110f, 243f, 200f, 22f), "Region Name:", "Label");
                        GUI.Label(new Rect(num11 + 110f, 179f, 200f, 22f), "Disable Map Bounds:", "Label");
                        bool flag7 = false;
                        if (((int) settingsOld[0xba]) == 1)
                        {
                            flag7 = true;
                            if (!linkHash[3].ContainsKey("mapbounds"))
                            {
                                linkHash[3].Add("mapbounds", "map,disablebounds");
                            }
                        }
                        else if (linkHash[3].ContainsKey("mapbounds"))
                        {
                            linkHash[3].Remove("mapbounds");
                        }
                        if (GUI.Button(new Rect(num11 + 30f, 159f, 64f, 30f), "Barrier"))
                        {
                            flag2 = true;
                            flag3 = true;
                            obj6 = (GameObject) RCassets.Load("barrierEditor");
                            this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj6);
                            this.selectedObj.name = "misc,barrier";
                        }
                        else if (GUI.Button(new Rect(num11 + 30f, 268f, 64f, 30f), "Region"))
                        {
                            if (((string) settingsOld[0xbf]) == string.Empty)
                            {
                                settingsOld[0xbf] = "Region" + UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                            }
                            flag2 = true;
                            flag3 = true;
                            obj6 = (GameObject) RCassets.Load("regionEditor");
                            this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj6);
                            obj4 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
                            obj4.name = "RegionLabel";
                            if (!float.TryParse((string) settingsOld[0x47], out num31))
                            {
                                settingsOld[0x47] = "1";
                            }
                            if (!float.TryParse((string) settingsOld[70], out num31))
                            {
                                settingsOld[70] = "1";
                            }
                            if (!float.TryParse((string) settingsOld[0x48], out num31))
                            {
                                settingsOld[0x48] = "1";
                            }
                            obj4.transform.parent = this.selectedObj.transform;
                            num14 = 1f;
                            if (Convert.ToSingle((string) settingsOld[0x47]) > 100f)
                            {
                                num14 = 0.8f;
                            }
                            else if (Convert.ToSingle((string) settingsOld[0x47]) > 1000f)
                            {
                                num14 = 0.5f;
                            }
                            obj4.transform.localPosition = new Vector3(0f, num14, 0f);
                            obj4.transform.localScale = new Vector3(5f / Convert.ToSingle((string) settingsOld[70]), 5f / Convert.ToSingle((string) settingsOld[0x47]), 5f / Convert.ToSingle((string) settingsOld[0x48]));
                            obj4.GetComponent<UILabel>().text = (string) settingsOld[0xbf];
                            this.selectedObj.AddComponent<RCRegionLabel>();
                            this.selectedObj.GetComponent<RCRegionLabel>().myLabel = obj4;
                            this.selectedObj.name = "misc,region," + ((string) settingsOld[0xbf]);
                        }
                        settingsOld[0xbf] = GUI.TextField(new Rect(num11 + 200f, 243f, 75f, 20f), (string) settingsOld[0xbf]);
                        bool flag8 = GUI.Toggle(new Rect(num11 + 240f, 179f, 40f, 20f), flag7, "On");
                        if (flag8 != flag7)
                        {
                            if (flag8)
                            {
                                settingsOld[0xba] = 1;
                            }
                            else
                            {
                                settingsOld[0xba] = 0;
                            }
                        }
                    }
                    else if (((int) settingsOld[0x40]) == 0x69)
                    {
                        float num32;
                        GameObject obj7;
                        GUI.Label(new Rect(num11 + 95f, 85f, 130f, 20f), "Custom Spawners:", "Label");
                        GUI.DrawTexture(new Rect(num11 + 7.8f, 110f, 64f, 64f), this.RCLoadTexture("ptitan"));
                        GUI.DrawTexture(new Rect(num11 + 79.6f, 110f, 64f, 64f), this.RCLoadTexture("pabnormal"));
                        GUI.DrawTexture(new Rect(num11 + 151.4f, 110f, 64f, 64f), this.RCLoadTexture("pjumper"));
                        GUI.DrawTexture(new Rect(num11 + 223.2f, 110f, 64f, 64f), this.RCLoadTexture("pcrawler"));
                        GUI.DrawTexture(new Rect(num11 + 7.8f, 224f, 64f, 64f), this.RCLoadTexture("ppunk"));
                        GUI.DrawTexture(new Rect(num11 + 79.6f, 224f, 64f, 64f), this.RCLoadTexture("pannie"));
                        if (GUI.Button(new Rect(num11 + 7.8f, 179f, 64f, 30f), "Titan"))
                        {
                            if (!float.TryParse((string) settingsOld[0x53], out num32))
                            {
                                settingsOld[0x53] = "30";
                            }
                            flag2 = true;
                            flag3 = true;
                            obj7 = (GameObject) RCassets.Load("spawnTitan");
                            this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj7);
                            num30 = (int) settingsOld[0x54];
                            this.selectedObj.name = "photon,spawnTitan," + ((string) settingsOld[0x53]) + "," + num30.ToString();
                        }
                        else if (GUI.Button(new Rect(num11 + 79.6f, 179f, 64f, 30f), "Aberrant"))
                        {
                            if (!float.TryParse((string) settingsOld[0x53], out num32))
                            {
                                settingsOld[0x53] = "30";
                            }
                            flag2 = true;
                            flag3 = true;
                            obj7 = (GameObject) RCassets.Load("spawnAbnormal");
                            this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj7);
                            num30 = (int) settingsOld[0x54];
                            this.selectedObj.name = "photon,spawnAbnormal," + ((string) settingsOld[0x53]) + "," + num30.ToString();
                        }
                        else if (GUI.Button(new Rect(num11 + 151.4f, 179f, 64f, 30f), "Jumper"))
                        {
                            if (!float.TryParse((string) settingsOld[0x53], out num32))
                            {
                                settingsOld[0x53] = "30";
                            }
                            flag2 = true;
                            flag3 = true;
                            obj7 = (GameObject) RCassets.Load("spawnJumper");
                            this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj7);
                            num30 = (int) settingsOld[0x54];
                            this.selectedObj.name = "photon,spawnJumper," + ((string) settingsOld[0x53]) + "," + num30.ToString();
                        }
                        else if (GUI.Button(new Rect(num11 + 223.2f, 179f, 64f, 30f), "Crawler"))
                        {
                            if (!float.TryParse((string) settingsOld[0x53], out num32))
                            {
                                settingsOld[0x53] = "30";
                            }
                            flag2 = true;
                            flag3 = true;
                            obj7 = (GameObject) RCassets.Load("spawnCrawler");
                            this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj7);
                            num30 = (int) settingsOld[0x54];
                            this.selectedObj.name = "photon,spawnCrawler," + ((string) settingsOld[0x53]) + "," + num30.ToString();
                        }
                        else if (GUI.Button(new Rect(num11 + 7.8f, 293f, 64f, 30f), "Punk"))
                        {
                            if (!float.TryParse((string) settingsOld[0x53], out num32))
                            {
                                settingsOld[0x53] = "30";
                            }
                            flag2 = true;
                            flag3 = true;
                            obj7 = (GameObject) RCassets.Load("spawnPunk");
                            this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj7);
                            num30 = (int) settingsOld[0x54];
                            this.selectedObj.name = "photon,spawnPunk," + ((string) settingsOld[0x53]) + "," + num30.ToString();
                        }
                        else if (GUI.Button(new Rect(num11 + 79.6f, 293f, 64f, 30f), "Annie"))
                        {
                            if (!float.TryParse((string) settingsOld[0x53], out num32))
                            {
                                settingsOld[0x53] = "30";
                            }
                            flag2 = true;
                            flag3 = true;
                            obj7 = (GameObject) RCassets.Load("spawnAnnie");
                            this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj7);
                            num30 = (int) settingsOld[0x54];
                            this.selectedObj.name = "photon,spawnAnnie," + ((string) settingsOld[0x53]) + "," + num30.ToString();
                        }
                        GUI.Label(new Rect(num11 + 7f, 379f, 140f, 22f), "Spawn Timer:", "Label");
                        settingsOld[0x53] = GUI.TextField(new Rect(num11 + 100f, 379f, 50f, 20f), (string) settingsOld[0x53]);
                        GUI.Label(new Rect(num11 + 7f, 356f, 140f, 22f), "Endless spawn:", "Label");
                        GUI.Label(new Rect(num11 + 7f, 405f, 290f, 80f), "* The above settingsOld apply only to the next placed spawner. You can have unique spawn times and settingsOld for each individual titan spawner.", "Label");
                        bool flag9 = false;
                        if (((int) settingsOld[0x54]) == 1)
                        {
                            flag9 = true;
                        }
                        flag10 = GUI.Toggle(new Rect(num11 + 100f, 356f, 40f, 20f), flag9, "On");
                        if (flag9 != flag10)
                        {
                            if (flag10)
                            {
                                settingsOld[0x54] = 1;
                            }
                            else
                            {
                                settingsOld[0x54] = 0;
                            }
                        }
                    }
                    else
                    {
                        string[] strArray14;
                        if (((int) settingsOld[0x40]) == 0x66)
                        {
                            strArray14 = new string[] { "cuboid", "plane", "sphere", "cylinder", "capsule", "pyramid", "cone", "prism", "arc90", "arc180", "torus", "tube" };
                            for (num13 = 0; num13 < strArray14.Length; num13++)
                            {
                                num29 = num13 % 4;
                                num28 = num13 / 4;
                                GUI.DrawTexture(new Rect((num11 + 7.8f) + (71.8f * num29), 90f + (114f * num28), 64f, 64f), this.RCLoadTexture("p" + strArray14[num13]));
                                if (GUI.Button(new Rect((num11 + 7.8f) + (71.8f * num29), 159f + (114f * num28), 64f, 30f), strArray14[num13]))
                                {
                                    flag2 = true;
                                    obj6 = (GameObject) RCassets.Load(strArray14[num13]);
                                    this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj6);
                                    this.selectedObj.name = "custom," + strArray14[num13];
                                }
                            }
                        }
                        else
                        {
                            List<string> list4;
                            GameObject obj8;
                            if (((int) settingsOld[0x40]) == 0x67)
                            {
                                list4 = new List<string> { "arch1", "house1" };
                                strArray14 = new string[] { 
                                    "tower1", "tower2", "tower3", "tower4", "tower5", "house1", "house2", "house3", "house4", "house5", "house6", "house7", "house8", "house9", "house10", "house11", 
                                    "house12", "house13", "house14", "pillar1", "pillar2", "village1", "village2", "windmill1", "arch1", "canal1", "castle1", "church1", "cannon1", "statue1", "statue2", "wagon1", 
                                    "elevator1", "bridge1", "dummy1", "spike1", "wall1", "wall2", "wall3", "wall4", "arena1", "arena2", "arena3", "arena4"
                                 };
                                num27 = 110f + (114f * ((strArray14.Length - 1) / 4));
                                this.scroll = GUI.BeginScrollView(new Rect(num11, 90f, 303f, 350f), this.scroll, new Rect(num11, 90f, 300f, num27), true, true);
                                for (num13 = 0; num13 < strArray14.Length; num13++)
                                {
                                    num29 = num13 % 4;
                                    num28 = num13 / 4;
                                    GUI.DrawTexture(new Rect((num11 + 7.8f) + (71.8f * num29), 90f + (114f * num28), 64f, 64f), this.RCLoadTexture("p" + strArray14[num13]));
                                    if (GUI.Button(new Rect((num11 + 7.8f) + (71.8f * num29), 159f + (114f * num28), 64f, 30f), strArray14[num13]))
                                    {
                                        flag2 = true;
                                        obj8 = (GameObject) RCassets.Load(strArray14[num13]);
                                        this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj8);
                                        if (list4.Contains(strArray14[num13]))
                                        {
                                            this.selectedObj.name = "customb," + strArray14[num13];
                                        }
                                        else
                                        {
                                            this.selectedObj.name = "custom," + strArray14[num13];
                                        }
                                    }
                                }
                                GUI.EndScrollView();
                            }
                            else if (((int) settingsOld[0x40]) == 0x68)
                            {
                                list4 = new List<string> { "tree0" };
                                strArray14 = new string[] { 
                                    "leaf0", "leaf1", "leaf2", "field1", "field2", "tree0", "tree1", "tree2", "tree3", "tree4", "tree5", "tree6", "tree7", "log1", "log2", "trunk1", 
                                    "boulder1", "boulder2", "boulder3", "boulder4", "boulder5", "cave1", "cave2"
                                 };
                                num27 = 110f + (114f * ((strArray14.Length - 1) / 4));
                                this.scroll = GUI.BeginScrollView(new Rect(num11, 90f, 303f, 350f), this.scroll, new Rect(num11, 90f, 300f, num27), true, true);
                                for (num13 = 0; num13 < strArray14.Length; num13++)
                                {
                                    num29 = num13 % 4;
                                    num28 = num13 / 4;
                                    GUI.DrawTexture(new Rect((num11 + 7.8f) + (71.8f * num29), 90f + (114f * num28), 64f, 64f), this.RCLoadTexture("p" + strArray14[num13]));
                                    if (GUI.Button(new Rect((num11 + 7.8f) + (71.8f * num29), 159f + (114f * num28), 64f, 30f), strArray14[num13]))
                                    {
                                        flag2 = true;
                                        obj8 = (GameObject) RCassets.Load(strArray14[num13]);
                                        this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj8);
                                        if (list4.Contains(strArray14[num13]))
                                        {
                                            this.selectedObj.name = "customb," + strArray14[num13];
                                        }
                                        else
                                        {
                                            this.selectedObj.name = "custom," + strArray14[num13];
                                        }
                                    }
                                }
                                GUI.EndScrollView();
                            }
                            else if (((int) settingsOld[0x40]) == 0x6c)
                            {
                                string[] strArray15 = new string[] { "Cuboid", "Plane", "Sphere", "Cylinder", "Capsule", "Pyramid", "Cone", "Prism", "Arc90", "Arc180", "Torus", "Tube" };
                                strArray14 = new string[12];
                                for (num13 = 0; num13 < strArray14.Length; num13++)
                                {
                                    strArray14[num13] = "start" + strArray15[num13];
                                }
                                num27 = 110f + (114f * ((strArray14.Length - 1) / 4));
                                num27 *= 4f;
                                num27 += 200f;
                                this.scroll = GUI.BeginScrollView(new Rect(num11, 90f, 303f, 350f), this.scroll, new Rect(num11, 90f, 300f, num27), true, true);
                                GUI.Label(new Rect(num11 + 90f, 90f, 200f, 22f), "Racing Start Barrier");
                                int num33 = 0x7d;
                                for (num13 = 0; num13 < strArray14.Length; num13++)
                                {
                                    num29 = num13 % 4;
                                    num28 = num13 / 4;
                                    GUI.DrawTexture(new Rect((num11 + 7.8f) + (71.8f * num29), num33 + (114f * num28), 64f, 64f), this.RCLoadTexture("p" + strArray14[num13]));
                                    if (GUI.Button(new Rect((num11 + 7.8f) + (71.8f * num29), (num33 + 69f) + (114f * num28), 64f, 30f), strArray15[num13]))
                                    {
                                        flag2 = true;
                                        flag3 = true;
                                        obj8 = (GameObject) RCassets.Load(strArray14[num13]);
                                        this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj8);
                                        this.selectedObj.name = "racing," + strArray14[num13];
                                    }
                                }
                                num33 += (0x72 * (strArray14.Length / 4)) + 10;
                                GUI.Label(new Rect(num11 + 93f, (float) num33, 200f, 22f), "Racing End Trigger");
                                num33 += 0x23;
                                for (num13 = 0; num13 < strArray14.Length; num13++)
                                {
                                    strArray14[num13] = "end" + strArray15[num13];
                                }
                                for (num13 = 0; num13 < strArray14.Length; num13++)
                                {
                                    num29 = num13 % 4;
                                    num28 = num13 / 4;
                                    GUI.DrawTexture(new Rect((num11 + 7.8f) + (71.8f * num29), num33 + (114f * num28), 64f, 64f), this.RCLoadTexture("p" + strArray14[num13]));
                                    if (GUI.Button(new Rect((num11 + 7.8f) + (71.8f * num29), (num33 + 69f) + (114f * num28), 64f, 30f), strArray15[num13]))
                                    {
                                        flag2 = true;
                                        flag3 = true;
                                        obj8 = (GameObject) RCassets.Load(strArray14[num13]);
                                        this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj8);
                                        this.selectedObj.name = "racing," + strArray14[num13];
                                    }
                                }
                                num33 += (0x72 * (strArray14.Length / 4)) + 10;
                                GUI.Label(new Rect(num11 + 113f, (float) num33, 200f, 22f), "Kill Trigger");
                                num33 += 0x23;
                                for (num13 = 0; num13 < strArray14.Length; num13++)
                                {
                                    strArray14[num13] = "kill" + strArray15[num13];
                                }
                                for (num13 = 0; num13 < strArray14.Length; num13++)
                                {
                                    num29 = num13 % 4;
                                    num28 = num13 / 4;
                                    GUI.DrawTexture(new Rect((num11 + 7.8f) + (71.8f * num29), num33 + (114f * num28), 64f, 64f), this.RCLoadTexture("p" + strArray14[num13]));
                                    if (GUI.Button(new Rect((num11 + 7.8f) + (71.8f * num29), (num33 + 69f) + (114f * num28), 64f, 30f), strArray15[num13]))
                                    {
                                        flag2 = true;
                                        flag3 = true;
                                        obj8 = (GameObject) RCassets.Load(strArray14[num13]);
                                        this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj8);
                                        this.selectedObj.name = "racing," + strArray14[num13];
                                    }
                                }
                                num33 += (0x72 * (strArray14.Length / 4)) + 10;
                                GUI.Label(new Rect(num11 + 95f, (float) num33, 200f, 22f), "Checkpoint Trigger");
                                num33 += 0x23;
                                for (num13 = 0; num13 < strArray14.Length; num13++)
                                {
                                    strArray14[num13] = "checkpoint" + strArray15[num13];
                                }
                                for (num13 = 0; num13 < strArray14.Length; num13++)
                                {
                                    num29 = num13 % 4;
                                    num28 = num13 / 4;
                                    GUI.DrawTexture(new Rect((num11 + 7.8f) + (71.8f * num29), num33 + (114f * num28), 64f, 64f), this.RCLoadTexture("p" + strArray14[num13]));
                                    if (GUI.Button(new Rect((num11 + 7.8f) + (71.8f * num29), (num33 + 69f) + (114f * num28), 64f, 30f), strArray15[num13]))
                                    {
                                        flag2 = true;
                                        flag3 = true;
                                        obj8 = (GameObject) RCassets.Load(strArray14[num13]);
                                        this.selectedObj = (GameObject) UnityEngine.Object.Instantiate(obj8);
                                        this.selectedObj.name = "racing," + strArray14[num13];
                                    }
                                }
                                GUI.EndScrollView();
                            }
                            else if (((int) settingsOld[0x40]) == 0x6a)
                            {
                                GUI.Label(new Rect(num11 + 10f, 80f, 200f, 22f), "- Tree 2 designed by Ken P.", "Label");
                                GUI.Label(new Rect(num11 + 10f, 105f, 250f, 22f), "- Tower 2, House 5 designed by Matthew Santos", "Label");
                                GUI.Label(new Rect(num11 + 10f, 130f, 200f, 22f), "- Cannon retextured by Mika", "Label");
                                GUI.Label(new Rect(num11 + 10f, 155f, 200f, 22f), "- Arena 1,2,3 & 4 created by Gun", "Label");
                                GUI.Label(new Rect(num11 + 10f, 180f, 250f, 22f), "- Cannon Wall/Ground textured by Bellfox", "Label");
                                GUI.Label(new Rect(num11 + 10f, 205f, 250f, 120f), "- House 7 - 14, Statue1, Statue2, Wagon1, Wall 1, Wall 2, Wall 3, Wall 4, CannonWall, CannonGround, Tower5, Bridge1, Dummy1, Spike1 created by meecube", "Label");
                            }
                        }
                    }
                }
                if (flag2 && (this.selectedObj != null))
                {
                    float num36;
                    float num37;
                    float num38;
                    float num39;
                    float z;
                    float num41;
                    string name;
                    if (!float.TryParse((string) settingsOld[70], out num31))
                    {
                        settingsOld[70] = "1";
                    }
                    if (!float.TryParse((string) settingsOld[0x47], out num31))
                    {
                        settingsOld[0x47] = "1";
                    }
                    if (!float.TryParse((string) settingsOld[0x48], out num31))
                    {
                        settingsOld[0x48] = "1";
                    }
                    if (!float.TryParse((string) settingsOld[0x4f], out num31))
                    {
                        settingsOld[0x4f] = "1";
                    }
                    if (!float.TryParse((string) settingsOld[80], out num31))
                    {
                        settingsOld[80] = "1";
                    }
                    if (!flag3)
                    {
                        float a = 1f;
                        if (((string) settingsOld[0x45]) != "default")
                        {
                            if (((string) settingsOld[0x45]).StartsWith("transparent"))
                            {
                                float num35;
                                if (float.TryParse(((string) settingsOld[0x45]).Substring(11), out num35))
                                {
                                    a = num35;
                                }
                                foreach (Renderer renderer2 in this.selectedObj.GetComponentsInChildren<Renderer>())
                                {
                                    renderer2.material = (Material) RCassets.Load("transparent");
                                    renderer2.material.mainTextureScale = new Vector2(renderer2.material.mainTextureScale.x * Convert.ToSingle((string) settingsOld[0x4f]), renderer2.material.mainTextureScale.y * Convert.ToSingle((string) settingsOld[80]));
                                }
                            }
                            else
                            {
                                foreach (Renderer renderer2 in this.selectedObj.GetComponentsInChildren<Renderer>())
                                {
                                    if (!(renderer2.name.Contains("Particle System") && this.selectedObj.name.Contains("aot_supply")))
                                    {
                                        renderer2.material = (Material) RCassets.Load((string) settingsOld[0x45]);
                                        renderer2.material.mainTextureScale = new Vector2(renderer2.material.mainTextureScale.x * Convert.ToSingle((string) settingsOld[0x4f]), renderer2.material.mainTextureScale.y * Convert.ToSingle((string) settingsOld[80]));
                                    }
                                }
                            }
                        }
                        num36 = 1f;
                        foreach (MeshFilter filter in this.selectedObj.GetComponentsInChildren<MeshFilter>())
                        {
                            if (this.selectedObj.name.StartsWith("customb"))
                            {
                                if (num36 < filter.mesh.bounds.size.y)
                                {
                                    num36 = filter.mesh.bounds.size.y;
                                }
                            }
                            else if (num36 < filter.mesh.bounds.size.z)
                            {
                                num36 = filter.mesh.bounds.size.z;
                            }
                        }
                        num37 = this.selectedObj.transform.localScale.x * Convert.ToSingle((string) settingsOld[70]);
                        num37 -= 0.001f;
                        num38 = this.selectedObj.transform.localScale.y * Convert.ToSingle((string) settingsOld[0x47]);
                        num39 = this.selectedObj.transform.localScale.z * Convert.ToSingle((string) settingsOld[0x48]);
                        this.selectedObj.transform.localScale = new Vector3(num37, num38, num39);
                        if (((int) settingsOld[0x4c]) == 1)
                        {
                            color = new Color((float) settingsOld[0x49], (float) settingsOld[0x4a], (float) settingsOld[0x4b], a);
                            foreach (MeshFilter filter in this.selectedObj.GetComponentsInChildren<MeshFilter>())
                            {
                                mesh = filter.mesh;
                                colorArray = new Color[mesh.vertexCount];
                                num18 = 0;
                                while (num18 < mesh.vertexCount)
                                {
                                    colorArray[num18] = color;
                                    num18++;
                                }
                                mesh.colors = colorArray;
                            }
                        }
                        z = this.selectedObj.transform.localScale.z;
                        if ((this.selectedObj.name.Contains("boulder2") || this.selectedObj.name.Contains("boulder3")) || this.selectedObj.name.Contains("field2"))
                        {
                            z *= 0.01f;
                        }
                        num41 = 10f + (((z * num36) * 1.2f) / 2f);
                        this.selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + (Camera.main.transform.forward.x * num41), Camera.main.transform.position.y + (Camera.main.transform.forward.y * 10f), Camera.main.transform.position.z + (Camera.main.transform.forward.z * num41));
                        this.selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
                        name = this.selectedObj.name;
                        string[] strArray3 = new string[0x15];
                        strArray3[0] = name;
                        strArray3[1] = ",";
                        strArray3[2] = (string) settingsOld[0x45];
                        strArray3[3] = ",";
                        strArray3[4] = (string) settingsOld[70];
                        strArray3[5] = ",";
                        strArray3[6] = (string) settingsOld[0x47];
                        strArray3[7] = ",";
                        strArray3[8] = (string) settingsOld[0x48];
                        strArray3[9] = ",";
                        strArray3[10] = settingsOld[0x4c].ToString();
                        strArray3[11] = ",";
                        float num42 = (float) settingsOld[0x49];
                        strArray3[12] = num42.ToString();
                        strArray3[13] = ",";
                        num42 = (float) settingsOld[0x4a];
                        strArray3[14] = num42.ToString();
                        strArray3[15] = ",";
                        strArray3[0x10] = ((float) settingsOld[0x4b]).ToString();
                        strArray3[0x11] = ",";
                        strArray3[0x12] = (string) settingsOld[0x4f];
                        strArray3[0x13] = ",";
                        strArray3[20] = (string) settingsOld[80];
                        this.selectedObj.name = string.Concat(strArray3);
                        this.unloadAssetsEditor();
                    }
                    else if (this.selectedObj.name.StartsWith("misc"))
                    {
                        if ((this.selectedObj.name.Contains("barrier") || this.selectedObj.name.Contains("region")) || this.selectedObj.name.Contains("racing"))
                        {
                            num36 = 1f;
                            num37 = this.selectedObj.transform.localScale.x * Convert.ToSingle((string) settingsOld[70]);
                            num37 -= 0.001f;
                            num38 = this.selectedObj.transform.localScale.y * Convert.ToSingle((string) settingsOld[0x47]);
                            num39 = this.selectedObj.transform.localScale.z * Convert.ToSingle((string) settingsOld[0x48]);
                            this.selectedObj.transform.localScale = new Vector3(num37, num38, num39);
                            z = this.selectedObj.transform.localScale.z;
                            num41 = 10f + (((z * num36) * 1.2f) / 2f);
                            this.selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + (Camera.main.transform.forward.x * num41), Camera.main.transform.position.y + (Camera.main.transform.forward.y * 10f), Camera.main.transform.position.z + (Camera.main.transform.forward.z * num41));
                            if (!this.selectedObj.name.Contains("region"))
                            {
                                this.selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
                            }
                            name = this.selectedObj.name;
                            this.selectedObj.name = name + "," + ((string) settingsOld[70]) + "," + ((string) settingsOld[0x47]) + "," + ((string) settingsOld[0x48]);
                        }
                    }
                    else if (this.selectedObj.name.StartsWith("racing"))
                    {
                        num36 = 1f;
                        num37 = this.selectedObj.transform.localScale.x * Convert.ToSingle((string) settingsOld[70]);
                        num37 -= 0.001f;
                        num38 = this.selectedObj.transform.localScale.y * Convert.ToSingle((string) settingsOld[0x47]);
                        num39 = this.selectedObj.transform.localScale.z * Convert.ToSingle((string) settingsOld[0x48]);
                        this.selectedObj.transform.localScale = new Vector3(num37, num38, num39);
                        z = this.selectedObj.transform.localScale.z;
                        num41 = 10f + (((z * num36) * 1.2f) / 2f);
                        this.selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + (Camera.main.transform.forward.x * num41), Camera.main.transform.position.y + (Camera.main.transform.forward.y * 10f), Camera.main.transform.position.z + (Camera.main.transform.forward.z * num41));
                        this.selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
                        name = this.selectedObj.name;
                        this.selectedObj.name = name + "," + ((string) settingsOld[70]) + "," + ((string) settingsOld[0x47]) + "," + ((string) settingsOld[0x48]);
                    }
                    else
                    {
                        this.selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + (Camera.main.transform.forward.x * 10f), Camera.main.transform.position.y + (Camera.main.transform.forward.y * 10f), Camera.main.transform.position.z + (Camera.main.transform.forward.z * 10f));
                        this.selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
                    }
                    Screen.lockCursor = true;
                    GUI.FocusControl(null);
                }
            }
            else if (GameMenu.Paused)
            {
            }
            else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                if (Time.timeScale <= 0.1f)
                {
                    num7 = ((float) Screen.width) / 2f;
                    num8 = ((float) Screen.height) / 2f;
                    GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
                    GUI.DrawTexture(new Rect(num7 - 98f, num8 - 48f, 196f, 96f), this.textureBackgroundBlue);
                    GUI.Box(new Rect(num7 - 100f, num8 - 50f, 200f, 100f), string.Empty);
                    if (this.pauseWaitTime <= 3f)
                    {
                        GUI.Label(new Rect(num7 - 43f, num8 - 15f, 200f, 22f), "Unpausing in:");
                        GUI.Label(new Rect(num7 - 8f, num8 + 5f, 200f, 22f), this.pauseWaitTime.ToString("F1"));
                    }
                    else
                    {
                        GUI.Label(new Rect(num7 - 43f, num8 - 10f, 200f, 22f), "Game Paused.");
                    }
                }
                else if (!logicLoaded || !customLevelLoaded)
                {
                    num7 = ((float) Screen.width) / 2f;
                    num8 = ((float) Screen.height) / 2f;
                    GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
                    GUI.DrawTexture(new Rect(0f, 0f, (float) Screen.width, (float) Screen.height), this.textureBackgroundBlack);
                    GUI.DrawTexture(new Rect(num7 - 98f, num8 - 48f, 196f, 146f), this.textureBackgroundBlue);
                    GUI.Box(new Rect(num7 - 100f, num8 - 50f, 200f, 150f), string.Empty);
                    int length = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.currentLevel]).Length;
                    int num50 = RCextensions.returnStringFromObject(PhotonNetwork.masterClient.customProperties[PhotonPlayerProperty.currentLevel]).Length;
                    GUI.Label(new Rect(num7 - 60f, num8 - 30f, 200f, 22f), "Loading Level (" + length.ToString() + "/" + num50.ToString() + ")");
                    this.retryTime += Time.deltaTime;
                    if (GUI.Button(new Rect(num7 - 20f, num8 + 50f, 40f, 30f), "Quit"))
                    {
                        PhotonNetwork.Disconnect();
                        IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
                        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
                        this.DestroyAllExistingCloths();
                        UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
                        Application.LoadLevel("menu");
                    }
                }
            }
        }
    }

    public void OnJoinedRoom()
    {
        this.maxPlayers = PhotonNetwork.room.maxPlayers;
        this.playerList = string.Empty;
        char[] separator = new char[] { "`"[0] };
        UnityEngine.MonoBehaviour.print("OnJoinedRoom " + PhotonNetwork.room.name + "    >>>>   " + LevelInfo.getInfo(PhotonNetwork.room.name.Split(separator)[1]).mapName);
        this.gameTimesUp = false;
        char[] chArray3 = new char[] { "`"[0] };
        string[] strArray = PhotonNetwork.room.name.Split(chArray3);
        level = strArray[1];
        if (strArray[2] == "normal")
        {
            this.difficulty = 0;
        }
        else if (strArray[2] == "hard")
        {
            this.difficulty = 1;
        }
        else if (strArray[2] == "abnormal")
        {
            this.difficulty = 2;
        }
        IN_GAME_MAIN_CAMERA.difficulty = this.difficulty;
        this.time = int.Parse(strArray[3]);
        this.time *= 60;
        IN_GAME_MAIN_CAMERA.gamemode = LevelInfo.getInfo(level).type;
        PhotonNetwork.LoadLevel(LevelInfo.getInfo(level).mapName);
        this.name = SettingsManager.ProfileSettings.Name.Value;
        LoginFengKAI.player.name = this.name;
        LoginFengKAI.player.guildname = SettingsManager.ProfileSettings.Guild.Value;
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add(PhotonPlayerProperty.name, LoginFengKAI.player.name);
        hashtable.Add(PhotonPlayerProperty.guildName, LoginFengKAI.player.guildname);
        hashtable.Add(PhotonPlayerProperty.kills, 0);
        hashtable.Add(PhotonPlayerProperty.max_dmg, 0);
        hashtable.Add(PhotonPlayerProperty.total_dmg, 0);
        hashtable.Add(PhotonPlayerProperty.deaths, 0);
        hashtable.Add(PhotonPlayerProperty.dead, true);
        hashtable.Add(PhotonPlayerProperty.isTitan, 0);
        hashtable.Add(PhotonPlayerProperty.RCteam, 0);
        hashtable.Add(PhotonPlayerProperty.currentLevel, string.Empty);
        ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        this.humanScore = 0;
        this.titanScore = 0;
        this.PVPtitanScore = 0;
        this.PVPhumanScore = 0;
        this.wave = 1;
        this.highestwave = 1;
        this.localRacingResult = string.Empty;
        this.needChooseSide = true;
        this.chatContent = new ArrayList();
        this.killInfoGO = new ArrayList();
        InRoomChat.messages.Clear();
        if (!PhotonNetwork.isMasterClient)
        {
            base.photonView.RPC("RequireStatus", PhotonTargets.MasterClient, new object[0]);
        }
        this.assetCacheTextures = new Dictionary<string, Texture2D>();
        this.customMapMaterials = new Dictionary<string, Material>();
        this.isFirstLoad = true;
        if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
        {
            ServerRequestAuthentication(PrivateServerAuthPass);
        }
        MapCeiling.CreateMapCeiling();
    }

    public void OnLeftLobby()
    {
        UnityEngine.MonoBehaviour.print("OnLeftLobby");
    }

    public void OnLeftRoom()
    {
        PhotonPlayer.CleanProperties();
        InRoomChat.messages.Clear();
        if (Application.loadedLevel != 0)
        {
            Time.timeScale = 1f;
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            this.resetSettings(true);
            this.loadconfig();
            IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
            this.gameStart = false;
            this.DestroyAllExistingCloths();
            JustLeftRoom = true;
            UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
            Application.LoadLevel("menu");
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        SkyboxCustomSkinLoader.SkyboxMaterial = null;
        if ((level != 0) && ((Application.loadedLevelName != "characterCreation") && (Application.loadedLevelName != "SnapShot")))
        {
            UIManager.SetMenu(MenuType.Game);
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
            {
                if (!((obj2.GetPhotonView() != null) && obj2.GetPhotonView().owner.isMasterClient))
                {
                    UnityEngine.Object.Destroy(obj2);
                }
            }
            this.isWinning = false;
            this.gameStart = true;
            this.ShowHUDInfoCenter(string.Empty);
            GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("MainCamera_mono"), GameObject.Find("cameraDefaultPosition").transform.position, GameObject.Find("cameraDefaultPosition").transform.rotation);
            UnityEngine.Object.Destroy(GameObject.Find("cameraDefaultPosition"));
            obj3.name = "MainCamera";
            this.ui = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI_IN_GAME"));
            this.ui.name = "UI_IN_GAME";
            this.ui.SetActive(true);
            NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[0], true);
            NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[1], false);
            NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[2], false);
            NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[3], false);
            LevelInfo info = LevelInfo.getInfo(FengGameManagerMKII.level);
            this.cache();
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
            this.loadskin();
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                this.single_kills = 0;
                this.single_maxDamage = 0;
                this.single_totalDamage = 0;
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
                Camera.main.GetComponent<SpectatorMovement>().disable = true;
                Camera.main.GetComponent<MouseLook>().disable = true;
                IN_GAME_MAIN_CAMERA.gamemode = LevelInfo.getInfo(FengGameManagerMKII.level).type;
                this.SpawnPlayer(IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper(), "playerRespawn");
                int abnormal = 90;
                if (this.difficulty == 1)
                {
                    abnormal = 70;
                }
                this.spawnTitanCustom("titanRespawn", abnormal, info.enemyNumber, false);
            }
            else
            {
                PVPcheckPoint.chkPts = new ArrayList();
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = false;
                Camera.main.GetComponent<CameraShake>().enabled = false;
                IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.MULTIPLAYER;
                if (info.type == GAMEMODE.TROST)
                {
                    GameObject.Find("playerRespawn").SetActive(false);
                    UnityEngine.Object.Destroy(GameObject.Find("playerRespawn"));
                    GameObject.Find("rock").animation["lift"].speed = 0f;
                    GameObject.Find("door_fine").SetActive(false);
                    GameObject.Find("door_broke").SetActive(true);
                    UnityEngine.Object.Destroy(GameObject.Find("ppl"));
                }
                else if (info.type == GAMEMODE.BOSS_FIGHT_CT)
                {
                    GameObject.Find("playerRespawnTrost").SetActive(false);
                    UnityEngine.Object.Destroy(GameObject.Find("playerRespawnTrost"));
                }
                if (this.needChooseSide)
                {
                    string joinButton = SettingsManager.InputSettings.Human.Flare1.ToString();
                    this.ShowHUDInfoTopCenterADD("\n\nPRESS " + joinButton + " TO ENTER GAME");
                }
                else if (!SettingsManager.LegacyGeneralSettings.SpecMode.Value)
                {
                    if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
                    {
                        Screen.lockCursor = true;
                    }
                    else
                    {
                        Screen.lockCursor = false;
                    }
                    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
                    {
                        if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
                        {
                            this.checkpoint = GameObject.Find("PVPchkPtT");
                        }
                        else
                        {
                            this.checkpoint = GameObject.Find("PVPchkPtH");
                        }
                    }
                    if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
                    {
                        this.SpawnNonAITitan2(this.myLastHero, "titanRespawn");
                    }
                    else
                    {
                        this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
                    }
                }
                if (info.type == GAMEMODE.BOSS_FIGHT_CT)
                {
                    UnityEngine.Object.Destroy(GameObject.Find("rock"));
                }
                if (PhotonNetwork.isMasterClient)
                {
                    if (info.type == GAMEMODE.TROST)
                    {
                        if (!this.isPlayerAllDead2())
                        {
                            PhotonNetwork.Instantiate("TITAN_EREN_trost", new Vector3(-200f, 0f, -194f), Quaternion.Euler(0f, 180f, 0f), 0).GetComponent<TITAN_EREN>().rockLift = true;
                            int rate = 90;
                            if (this.difficulty == 1)
                            {
                                rate = 70;
                            }
                            GameObject[] objArray2 = GameObject.FindGameObjectsWithTag("titanRespawn");
                            GameObject obj4 = GameObject.Find("titanRespawnTrost");
                            if (obj4 != null)
                            {
                                foreach (GameObject obj5 in objArray2)
                                {
                                    if (obj5.transform.parent.gameObject == obj4)
                                    {
                                        this.spawnTitan(rate, obj5.transform.position, obj5.transform.rotation, false);
                                    }
                                }
                            }
                        }
                    }
                    else if (info.type == GAMEMODE.BOSS_FIGHT_CT)
                    {
                        if (!this.isPlayerAllDead2())
                        {
                            PhotonNetwork.Instantiate("COLOSSAL_TITAN", (Vector3) (-Vector3.up * 10000f), Quaternion.Euler(0f, 180f, 0f), 0);
                        }
                    }
                    else if (((info.type == GAMEMODE.KILL_TITAN) || (info.type == GAMEMODE.ENDLESS_TITAN)) || (info.type == GAMEMODE.SURVIVE_MODE))
                    {
                        if ((info.name == "Annie") || (info.name == "Annie II"))
                        {
                            PhotonNetwork.Instantiate("FEMALE_TITAN", GameObject.Find("titanRespawn").transform.position, GameObject.Find("titanRespawn").transform.rotation, 0);
                        }
                        else
                        {
                            int num4 = 90;
                            if (this.difficulty == 1)
                            {
                                num4 = 70;
                            }
                            this.spawnTitanCustom("titanRespawn", num4, info.enemyNumber, false);
                        }
                    }
                    else if ((info.type != GAMEMODE.TROST) && ((info.type == GAMEMODE.PVP_CAPTURE) && (LevelInfo.getInfo(FengGameManagerMKII.level).mapName == "OutSide")))
                    {
                        GameObject[] objArray3 = GameObject.FindGameObjectsWithTag("titanRespawn");
                        if (objArray3.Length <= 0)
                        {
                            return;
                        }
                        for (int i = 0; i < objArray3.Length; i++)
                        {
                            this.spawnTitanRaw(objArray3[i].transform.position, objArray3[i].transform.rotation).GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
                        }
                    }
                }
                if (!info.supply)
                {
                    UnityEngine.Object.Destroy(GameObject.Find("aot_supply"));
                }
                if (!PhotonNetwork.isMasterClient)
                {
                    base.photonView.RPC("RequireStatus", PhotonTargets.MasterClient, new object[0]);
                }
                if (LevelInfo.getInfo(FengGameManagerMKII.level).lavaMode)
                {
                    UnityEngine.Object.Instantiate(Resources.Load("levelBottom"), new Vector3(0f, -29.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
                    GameObject.Find("aot_supply").transform.position = GameObject.Find("aot_supply_lava_position").transform.position;
                    GameObject.Find("aot_supply").transform.rotation = GameObject.Find("aot_supply_lava_position").transform.rotation;
                }
                if (SettingsManager.LegacyGeneralSettings.SpecMode.Value)
                {
                    this.EnterSpecMode(true);
                }
            }
        }
        MapCeiling.CreateMapCeiling();
        unloadAssets(immediate: true);
    }

    public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        if (!noRestart)
        {
            if (PhotonNetwork.isMasterClient)
            {
                this.restartingMC = true;
                if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
                {
                    this.restartingTitan = true;
                }
                if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
                {
                    this.restartingBomb = true;
                }
                if (SettingsManager.LegacyGameSettings.AllowHorses.Value)
                {
                    this.restartingHorse = true;
                }
                if (!SettingsManager.LegacyGameSettings.KickShifters.Value)
                {
                    this.restartingEren = true;
                }
            }
            this.resetSettings(false);
            if (!LevelInfo.getInfo(level).teamTitan)
            {
                ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                propertiesToSet.Add(PhotonPlayerProperty.isTitan, 1);
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            }
            if (!(this.gameTimesUp || !PhotonNetwork.isMasterClient))
            {
                this.restartGame2(true);
                base.photonView.RPC("setMasterRC", PhotonTargets.All, new object[0]);
            }
        }
        noRestart = false;
    }

    public void OnPhotonCreateRoomFailed()
    {
        UnityEngine.MonoBehaviour.print("OnPhotonCreateRoomFailed");
    }

    public void OnPhotonCustomRoomPropertiesChanged()
    {
        return;
        if (Time.time - LastRoomPropertyCheckTime > 5f)
        {
            LastRoomPropertyCheckTime = Time.time;
            if (PhotonNetwork.isMasterClient)
            {
                if (!PhotonNetwork.room.open)
                {
                    PhotonNetwork.room.open = true;
                }
                if (!PhotonNetwork.room.visible)
                {
                    PhotonNetwork.room.visible = true;
                }
                if (PhotonNetwork.room.maxPlayers != this.maxPlayers)
                {
                    PhotonNetwork.room.maxPlayers = this.maxPlayers;
                }
                if (!PhotonNetwork.room.autoCleanUp)
                {
                    //clean up off, returning on
                    ExitGames.Client.Photon.Hashtable property1 = new ExitGames.Client.Photon.Hashtable();
                    property1.Add((byte)249, (bool)true);
                    PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(property1, true, 0);
                }
            }
            else
            {
                this.maxPlayers = PhotonNetwork.room.maxPlayers;
            }
        }
    }

    public void OnPhotonInstantiate()
    {
        UnityEngine.MonoBehaviour.print("OnPhotonInstantiate");
    }

    public void OnPhotonJoinRoomFailed()
    {
        UnityEngine.MonoBehaviour.print("OnPhotonJoinRoomFailed");
    }

    public void OnPhotonMaxCccuReached()
    {
        UnityEngine.MonoBehaviour.print("OnPhotonMaxCccuReached");
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonView photonView = base.photonView;
            if (banHash.ContainsValue(RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name])))
            {
                this.kickPlayerRC(player, false, "banned.");
            }
            else
            {
                int num = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statACL]);
                int num2 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statBLA]);
                int num3 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statGAS]);
                int num4 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statSPD]);
                if ((((num > 150) || (num2 > 0x7d)) || (num3 > 150)) || (num4 > 140))
                {
                    this.kickPlayerRC(player, true, "excessive stats.");
                    return;
                }
                if (SettingsManager.LegacyGameSettings.PreserveKDR.Value)
                {
                    base.StartCoroutine(this.WaitAndReloadKDR(player));
                }
                if (level.StartsWith("Custom"))
                {
                    base.StartCoroutine(this.customlevelE(new List<PhotonPlayer> { player }));
                }
                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
                {
                    hashtable.Add("bomb", 1);
                }
                if (SettingsManager.LegacyGameSettings.GlobalMinimapDisable.Value)
                {
                    hashtable.Add("globalDisableMinimap", 1);
                }
                if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
                {
                    hashtable.Add("team", SettingsManager.LegacyGameSettings.TeamMode.Value);
                }
                if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
                {
                    hashtable.Add("point", SettingsManager.LegacyGameSettings.PointModeAmount.Value);
                }
                if (!SettingsManager.LegacyGameSettings.RockThrowEnabled.Value)
                {
                    hashtable.Add("rock", 1);
                }
                if (SettingsManager.LegacyGameSettings.TitanExplodeEnabled.Value)
                {
                    hashtable.Add("explode", SettingsManager.LegacyGameSettings.TitanExplodeRadius.Value);
                }
                if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value > 0)
                {
                    hashtable.Add("healthMode", SettingsManager.LegacyGameSettings.TitanHealthMode.Value);
                    hashtable.Add("healthLower", SettingsManager.LegacyGameSettings.TitanHealthMin.Value);
                    hashtable.Add("healthUpper", SettingsManager.LegacyGameSettings.TitanHealthMax.Value);
                }
                if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
                {
                    hashtable.Add("infection", SettingsManager.LegacyGameSettings.InfectionModeAmount.Value);
                }
                if (SettingsManager.LegacyGameSettings.KickShifters.Value)
                {
                    hashtable.Add("eren", 1);
                }
                if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
                {
                    hashtable.Add("titanc", SettingsManager.LegacyGameSettings.TitanNumber.Value);
                }
                if (SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value)
                {
                    hashtable.Add("damage", SettingsManager.LegacyGameSettings.TitanArmor.Value);
                }
                if (SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value)
                {
                    hashtable.Add("sizeMode", SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value);
                    hashtable.Add("sizeLower", SettingsManager.LegacyGameSettings.TitanSizeMin.Value);
                    hashtable.Add("sizeUpper", SettingsManager.LegacyGameSettings.TitanSizeMax.Value);
                }
                if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
                {
                    hashtable.Add("spawnMode", 1);
                    hashtable.Add("nRate", SettingsManager.LegacyGameSettings.TitanSpawnNormal.Value);
                    hashtable.Add("aRate", SettingsManager.LegacyGameSettings.TitanSpawnAberrant.Value);
                    hashtable.Add("jRate", SettingsManager.LegacyGameSettings.TitanSpawnJumper.Value);
                    hashtable.Add("cRate", SettingsManager.LegacyGameSettings.TitanSpawnCrawler.Value);
                    hashtable.Add("pRate", SettingsManager.LegacyGameSettings.TitanSpawnPunk.Value);
                }
                if (SettingsManager.LegacyGameSettings.TitanPerWavesEnabled.Value)
                {
                    hashtable.Add("waveModeOn", 1);
                    hashtable.Add("waveModeNum", SettingsManager.LegacyGameSettings.TitanPerWaves.Value);
                }
                if (SettingsManager.LegacyGameSettings.FriendlyMode.Value)
                {
                    hashtable.Add("friendly", 1);
                }
                if (SettingsManager.LegacyGameSettings.BladePVP.Value > 0)
                {
                    hashtable.Add("pvp", SettingsManager.LegacyGameSettings.BladePVP.Value);
                }
                if (SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value)
                {
                    hashtable.Add("maxwave", SettingsManager.LegacyGameSettings.TitanMaxWaves.Value);
                }
                if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
                {
                    hashtable.Add("endless", SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value);
                }
                if (SettingsManager.LegacyGameSettings.Motd.Value != string.Empty)
                {
                    hashtable.Add("motd", SettingsManager.LegacyGameSettings.Motd.Value);
                }
                if (SettingsManager.LegacyGameSettings.AllowHorses.Value)
                {
                    hashtable.Add("horse", 1);
                }
                if (!SettingsManager.LegacyGameSettings.AHSSAirReload.Value)
                {
                    hashtable.Add("ahssReload", 1);
                }
                if (!SettingsManager.LegacyGameSettings.PunksEveryFive.Value)
                {
                    hashtable.Add("punkWaves", 1);
                }
                if (SettingsManager.LegacyGameSettings.CannonsFriendlyFire.Value)
                {
                    hashtable.Add("deadlycannons", 1);
                }
                if (SettingsManager.LegacyGameSettings.RacingEndless.Value)
                {
                    hashtable.Add("asoracing", 1);
                }
                if ((ignoreList != null) && (ignoreList.Count > 0))
                {
                    photonView.RPC("ignorePlayerArray", player, new object[] { ignoreList.ToArray() });
                }
                photonView.RPC("settingRPC", player, new object[] { hashtable });
                photonView.RPC("setMasterRC", player, new object[0]);
                if ((Time.timeScale <= 0.1f) && (this.pauseWaitTime > 3f))
                {
                    photonView.RPC("pauseRPC", player, new object[] { true });
                    object[] parameters = new object[] { "<color=#FFCC00>MasterClient has paused the game.</color>", "" };
                    photonView.RPC("Chat", player, parameters);
                }
            }
        }
        this.RecompilePlayerList(0.1f);
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (!this.gameTimesUp)
        {
            this.oneTitanDown(string.Empty, true);
            this.someOneIsDead(0);
        }
        if (ignoreList.Contains(player.ID))
        {
            ignoreList.Remove(player.ID);
        }
        InstantiateTracker.instance.TryRemovePlayer(player.ID);
        if (PhotonNetwork.isMasterClient)
        {
            base.photonView.RPC("verifyPlayerHasLeft", PhotonTargets.All, new object[] { player.ID });
            if (SettingsManager.LegacyGameSettings.PreserveKDR.Value)
            {
                string key = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
                if (this.PreservedPlayerKDR.ContainsKey(key))
                {
                    this.PreservedPlayerKDR.Remove(key);
                }
                int[] numArray2 = new int[] { RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.kills]), RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.deaths]), RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.max_dmg]), RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.total_dmg]) };
                this.PreservedPlayerKDR.Add(key, numArray2);
            }
        }
        this.RecompilePlayerList(0.1f);
    }

    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        this.RecompilePlayerList(0.1f);
        if (((playerAndUpdatedProps != null) && (playerAndUpdatedProps.Length >= 2)) && (((PhotonPlayer) playerAndUpdatedProps[0]) == PhotonNetwork.player))
        {
            ExitGames.Client.Photon.Hashtable hashtable2;
            ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable) playerAndUpdatedProps[1];
            if (hashtable.ContainsKey("name") && (RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]) != this.name))
            {
                hashtable2 = new ExitGames.Client.Photon.Hashtable();
                hashtable2.Add(PhotonPlayerProperty.name, this.name);
                PhotonNetwork.player.SetCustomProperties(hashtable2);
            }
            if (((hashtable.ContainsKey("statACL") || hashtable.ContainsKey("statBLA")) || hashtable.ContainsKey("statGAS")) || hashtable.ContainsKey("statSPD"))
            {
                PhotonPlayer player = PhotonNetwork.player;
                int num = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statACL]);
                int num2 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statBLA]);
                int num3 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statGAS]);
                int num4 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statSPD]);
                if (num > 150)
                {
                    hashtable2 = new ExitGames.Client.Photon.Hashtable();
                    hashtable2.Add(PhotonPlayerProperty.statACL, 100);
                    PhotonNetwork.player.SetCustomProperties(hashtable2);
                    num = 100;
                }
                if (num2 > 0x7d)
                {
                    hashtable2 = new ExitGames.Client.Photon.Hashtable();
                    hashtable2.Add(PhotonPlayerProperty.statBLA, 100);
                    PhotonNetwork.player.SetCustomProperties(hashtable2);
                    num2 = 100;
                }
                if (num3 > 150)
                {
                    hashtable2 = new ExitGames.Client.Photon.Hashtable();
                    hashtable2.Add(PhotonPlayerProperty.statGAS, 100);
                    PhotonNetwork.player.SetCustomProperties(hashtable2);
                    num3 = 100;
                }
                if (num4 > 140)
                {
                    hashtable2 = new ExitGames.Client.Photon.Hashtable();
                    hashtable2.Add(PhotonPlayerProperty.statSPD, 100);
                    PhotonNetwork.player.SetCustomProperties(hashtable2);
                    num4 = 100;
                }
            }
        }
    }

    public void OnPhotonRandomJoinFailed()
    {
        UnityEngine.MonoBehaviour.print("OnPhotonRandomJoinFailed");
    }

    public void OnPhotonSerializeView()
    {
        UnityEngine.MonoBehaviour.print("OnPhotonSerializeView");
    }

    public void OnReceivedRoomListUpdate()
    {
    }

    public void OnUpdate()
    {
        if (RCEvents.ContainsKey("OnUpdate"))
        {
            if (this.updateTime > 0f)
            {
                this.updateTime -= Time.deltaTime;
            }
            else
            {
                ((RCEvent) RCEvents["OnUpdate"]).checkEvent();
                this.updateTime = 1f;
            }
        }
    }

    public void OnUpdatedFriendList()
    {
        UnityEngine.MonoBehaviour.print("OnUpdatedFriendList");
    }

    public int operantType(string str, int condition)
    {
        switch (condition)
        {
            case 0:
            case 3:
                if (!str.StartsWith("Equals"))
                {
                    if (str.StartsWith("NotEquals"))
                    {
                        return 5;
                    }
                    if (!str.StartsWith("LessThan"))
                    {
                        if (str.StartsWith("LessThanOrEquals"))
                        {
                            return 1;
                        }
                        if (str.StartsWith("GreaterThanOrEquals"))
                        {
                            return 3;
                        }
                        if (str.StartsWith("GreaterThan"))
                        {
                            return 4;
                        }
                    }
                    return 0;
                }
                return 2;

            case 1:
            case 4:
            case 5:
                if (!str.StartsWith("Equals"))
                {
                    if (str.StartsWith("NotEquals"))
                    {
                        return 5;
                    }
                    return 0;
                }
                return 2;

            case 2:
                if (!str.StartsWith("Equals"))
                {
                    if (str.StartsWith("NotEquals"))
                    {
                        return 1;
                    }
                    if (str.StartsWith("Contains"))
                    {
                        return 2;
                    }
                    if (str.StartsWith("NotContains"))
                    {
                        return 3;
                    }
                    if (str.StartsWith("StartsWith"))
                    {
                        return 4;
                    }
                    if (str.StartsWith("NotStartsWith"))
                    {
                        return 5;
                    }
                    if (str.StartsWith("EndsWith"))
                    {
                        return 6;
                    }
                    if (str.StartsWith("NotEndsWith"))
                    {
                        return 7;
                    }
                    return 0;
                }
                return 0;
        }
        return 0;
    }

    public RCEvent parseBlock(string[] stringArray, int eventClass, int eventType, RCCondition condition)
    {
        List<RCAction> sentTrueActions = new List<RCAction>();
        RCEvent event2 = new RCEvent(null, null, 0, 0);
        for (int i = 0; i < stringArray.Length; i++)
        {
            int num2;
            int num3;
            int num4;
            int length;
            string[] strArray;
            int num6;
            int num7;
            int index;
            int num9;
            string str;
            int num10;
            int num11;
            int num12;
            string[] strArray2;
            RCCondition condition2;
            RCEvent event3;
            RCAction action;
            if (stringArray[i].StartsWith("If") && (stringArray[i + 1] == "{"))
            {
                num2 = i + 2;
                num3 = i + 2;
                num4 = 0;
                length = i + 2;
                while (length < stringArray.Length)
                {
                    if (stringArray[length] == "{")
                    {
                        num4++;
                    }
                    if (stringArray[length] == "}")
                    {
                        if (num4 > 0)
                        {
                            num4--;
                        }
                        else
                        {
                            num3 = length - 1;
                            length = stringArray.Length;
                        }
                    }
                    length++;
                }
                strArray = new string[(num3 - num2) + 1];
                num6 = 0;
                num7 = num2;
                while (num7 <= num3)
                {
                    strArray[num6] = stringArray[num7];
                    num6++;
                    num7++;
                }
                index = stringArray[i].IndexOf("(");
                num9 = stringArray[i].LastIndexOf(")");
                str = stringArray[i].Substring(index + 1, (num9 - index) - 1);
                num10 = this.conditionType(str);
                num11 = str.IndexOf('.');
                str = str.Substring(num11 + 1);
                num12 = this.operantType(str, num10);
                index = str.IndexOf('(');
                num9 = str.LastIndexOf(")");
                strArray2 = str.Substring(index + 1, (num9 - index) - 1).Split(new char[] { ',' });
                condition2 = new RCCondition(num12, num10, this.returnHelper(strArray2[0]), this.returnHelper(strArray2[1]));
                event3 = this.parseBlock(strArray, 1, 0, condition2);
                action = new RCAction(0, 0, event3, null);
                event2 = event3;
                sentTrueActions.Add(action);
                i = num3;
            }
            else if (stringArray[i].StartsWith("While") && (stringArray[i + 1] == "{"))
            {
                num2 = i + 2;
                num3 = i + 2;
                num4 = 0;
                length = i + 2;
                while (length < stringArray.Length)
                {
                    if (stringArray[length] == "{")
                    {
                        num4++;
                    }
                    if (stringArray[length] == "}")
                    {
                        if (num4 > 0)
                        {
                            num4--;
                        }
                        else
                        {
                            num3 = length - 1;
                            length = stringArray.Length;
                        }
                    }
                    length++;
                }
                strArray = new string[(num3 - num2) + 1];
                num6 = 0;
                num7 = num2;
                while (num7 <= num3)
                {
                    strArray[num6] = stringArray[num7];
                    num6++;
                    num7++;
                }
                index = stringArray[i].IndexOf("(");
                num9 = stringArray[i].LastIndexOf(")");
                str = stringArray[i].Substring(index + 1, (num9 - index) - 1);
                num10 = this.conditionType(str);
                num11 = str.IndexOf('.');
                str = str.Substring(num11 + 1);
                num12 = this.operantType(str, num10);
                index = str.IndexOf('(');
                num9 = str.LastIndexOf(")");
                strArray2 = str.Substring(index + 1, (num9 - index) - 1).Split(new char[] { ',' });
                condition2 = new RCCondition(num12, num10, this.returnHelper(strArray2[0]), this.returnHelper(strArray2[1]));
                event3 = this.parseBlock(strArray, 3, 0, condition2);
                action = new RCAction(0, 0, event3, null);
                sentTrueActions.Add(action);
                i = num3;
            }
            else if (stringArray[i].StartsWith("ForeachTitan") && (stringArray[i + 1] == "{"))
            {
                num2 = i + 2;
                num3 = i + 2;
                num4 = 0;
                length = i + 2;
                while (length < stringArray.Length)
                {
                    if (stringArray[length] == "{")
                    {
                        num4++;
                    }
                    if (stringArray[length] == "}")
                    {
                        if (num4 > 0)
                        {
                            num4--;
                        }
                        else
                        {
                            num3 = length - 1;
                            length = stringArray.Length;
                        }
                    }
                    length++;
                }
                strArray = new string[(num3 - num2) + 1];
                num6 = 0;
                num7 = num2;
                while (num7 <= num3)
                {
                    strArray[num6] = stringArray[num7];
                    num6++;
                    num7++;
                }
                index = stringArray[i].IndexOf("(");
                num9 = stringArray[i].LastIndexOf(")");
                str = stringArray[i].Substring(index + 2, (num9 - index) - 3);
                num10 = 0;
                event3 = this.parseBlock(strArray, 2, num10, null);
                event3.foreachVariableName = str;
                action = new RCAction(0, 0, event3, null);
                sentTrueActions.Add(action);
                i = num3;
            }
            else if (stringArray[i].StartsWith("ForeachPlayer") && (stringArray[i + 1] == "{"))
            {
                num2 = i + 2;
                num3 = i + 2;
                num4 = 0;
                length = i + 2;
                while (length < stringArray.Length)
                {
                    if (stringArray[length] == "{")
                    {
                        num4++;
                    }
                    if (stringArray[length] == "}")
                    {
                        if (num4 > 0)
                        {
                            num4--;
                        }
                        else
                        {
                            num3 = length - 1;
                            length = stringArray.Length;
                        }
                    }
                    length++;
                }
                strArray = new string[(num3 - num2) + 1];
                num6 = 0;
                num7 = num2;
                while (num7 <= num3)
                {
                    strArray[num6] = stringArray[num7];
                    num6++;
                    num7++;
                }
                index = stringArray[i].IndexOf("(");
                num9 = stringArray[i].LastIndexOf(")");
                str = stringArray[i].Substring(index + 2, (num9 - index) - 3);
                num10 = 1;
                event3 = this.parseBlock(strArray, 2, num10, null);
                event3.foreachVariableName = str;
                action = new RCAction(0, 0, event3, null);
                sentTrueActions.Add(action);
                i = num3;
            }
            else if (stringArray[i].StartsWith("Else") && (stringArray[i + 1] == "{"))
            {
                num2 = i + 2;
                num3 = i + 2;
                num4 = 0;
                length = i + 2;
                while (length < stringArray.Length)
                {
                    if (stringArray[length] == "{")
                    {
                        num4++;
                    }
                    if (stringArray[length] == "}")
                    {
                        if (num4 > 0)
                        {
                            num4--;
                        }
                        else
                        {
                            num3 = length - 1;
                            length = stringArray.Length;
                        }
                    }
                    length++;
                }
                strArray = new string[(num3 - num2) + 1];
                num6 = 0;
                for (num7 = num2; num7 <= num3; num7++)
                {
                    strArray[num6] = stringArray[num7];
                    num6++;
                }
                if (stringArray[i] == "Else")
                {
                    event3 = this.parseBlock(strArray, 0, 0, null);
                    action = new RCAction(0, 0, event3, null);
                    event2.setElse(action);
                    i = num3;
                }
                else if (stringArray[i].StartsWith("Else If"))
                {
                    index = stringArray[i].IndexOf("(");
                    num9 = stringArray[i].LastIndexOf(")");
                    str = stringArray[i].Substring(index + 1, (num9 - index) - 1);
                    num10 = this.conditionType(str);
                    num11 = str.IndexOf('.');
                    str = str.Substring(num11 + 1);
                    num12 = this.operantType(str, num10);
                    index = str.IndexOf('(');
                    num9 = str.LastIndexOf(")");
                    strArray2 = str.Substring(index + 1, (num9 - index) - 1).Split(new char[] { ',' });
                    condition2 = new RCCondition(num12, num10, this.returnHelper(strArray2[0]), this.returnHelper(strArray2[1]));
                    event3 = this.parseBlock(strArray, 1, 0, condition2);
                    action = new RCAction(0, 0, event3, null);
                    event2.setElse(action);
                    i = num3;
                }
            }
            else
            {
                int num13;
                int num14;
                int num15;
                int num16;
                string str2;
                string[] strArray3;
                RCActionHelper helper;
                RCActionHelper helper2;
                RCActionHelper helper3;
                if (stringArray[i].StartsWith("VariableInt"))
                {
                    num13 = 1;
                    num14 = stringArray[i].IndexOf('.');
                    num15 = stringArray[i].IndexOf('(');
                    num16 = stringArray[i].LastIndexOf(')');
                    str2 = stringArray[i].Substring(num14 + 1, (num15 - num14) - 1);
                    strArray3 = stringArray[i].Substring(num15 + 1, (num16 - num15) - 1).Split(new char[] { ',' });
                    if (str2.StartsWith("SetRandom"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        helper3 = this.returnHelper(strArray3[2]);
                        action = new RCAction(num13, 12, null, new RCActionHelper[] { helper, helper2, helper3 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Set"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 0, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Add"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 1, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Subtract"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 2, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Multiply"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 3, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Divide"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 4, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Modulo"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 5, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Power"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 6, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                }
                else if (stringArray[i].StartsWith("VariableBool"))
                {
                    num13 = 2;
                    num14 = stringArray[i].IndexOf('.');
                    num15 = stringArray[i].IndexOf('(');
                    num16 = stringArray[i].LastIndexOf(')');
                    str2 = stringArray[i].Substring(num14 + 1, (num15 - num14) - 1);
                    strArray3 = stringArray[i].Substring(num15 + 1, (num16 - num15) - 1).Split(new char[] { ',' });
                    if (str2.StartsWith("SetToOpposite"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        action = new RCAction(num13, 11, null, new RCActionHelper[] { helper });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("SetRandom"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        action = new RCAction(num13, 12, null, new RCActionHelper[] { helper });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Set"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 0, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                }
                else if (stringArray[i].StartsWith("VariableString"))
                {
                    num13 = 3;
                    num14 = stringArray[i].IndexOf('.');
                    num15 = stringArray[i].IndexOf('(');
                    num16 = stringArray[i].LastIndexOf(')');
                    str2 = stringArray[i].Substring(num14 + 1, (num15 - num14) - 1);
                    strArray3 = stringArray[i].Substring(num15 + 1, (num16 - num15) - 1).Split(new char[] { ',' });
                    if (str2.StartsWith("Set"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 0, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Concat"))
                    {
                        RCActionHelper[] helpers = new RCActionHelper[strArray3.Length];
                        for (length = 0; length < strArray3.Length; length++)
                        {
                            helpers[length] = this.returnHelper(strArray3[length]);
                        }
                        action = new RCAction(num13, 7, null, helpers);
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Append"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 8, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Replace"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        helper3 = this.returnHelper(strArray3[2]);
                        action = new RCAction(num13, 10, null, new RCActionHelper[] { helper, helper2, helper3 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Remove"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 9, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                }
                else if (stringArray[i].StartsWith("VariableFloat"))
                {
                    num13 = 4;
                    num14 = stringArray[i].IndexOf('.');
                    num15 = stringArray[i].IndexOf('(');
                    num16 = stringArray[i].LastIndexOf(')');
                    str2 = stringArray[i].Substring(num14 + 1, (num15 - num14) - 1);
                    strArray3 = stringArray[i].Substring(num15 + 1, (num16 - num15) - 1).Split(new char[] { ',' });
                    if (str2.StartsWith("SetRandom"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        helper3 = this.returnHelper(strArray3[2]);
                        action = new RCAction(num13, 12, null, new RCActionHelper[] { helper, helper2, helper3 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Set"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 0, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Add"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 1, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Subtract"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 2, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Multiply"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 3, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Divide"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 4, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Modulo"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 5, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                    else if (str2.StartsWith("Power"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 6, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                }
                else if (stringArray[i].StartsWith("VariablePlayer"))
                {
                    num13 = 5;
                    num14 = stringArray[i].IndexOf('.');
                    num15 = stringArray[i].IndexOf('(');
                    num16 = stringArray[i].LastIndexOf(')');
                    str2 = stringArray[i].Substring(num14 + 1, (num15 - num14) - 1);
                    strArray3 = stringArray[i].Substring(num15 + 1, (num16 - num15) - 1).Split(new char[] { ',' });
                    if (str2.StartsWith("Set"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 0, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                }
                else if (stringArray[i].StartsWith("VariableTitan"))
                {
                    num13 = 6;
                    num14 = stringArray[i].IndexOf('.');
                    num15 = stringArray[i].IndexOf('(');
                    num16 = stringArray[i].LastIndexOf(')');
                    str2 = stringArray[i].Substring(num14 + 1, (num15 - num14) - 1);
                    strArray3 = stringArray[i].Substring(num15 + 1, (num16 - num15) - 1).Split(new char[] { ',' });
                    if (str2.StartsWith("Set"))
                    {
                        helper = this.returnHelper(strArray3[0]);
                        helper2 = this.returnHelper(strArray3[1]);
                        action = new RCAction(num13, 0, null, new RCActionHelper[] { helper, helper2 });
                        sentTrueActions.Add(action);
                    }
                }
                else
                {
                    RCActionHelper helper4;
                    if (stringArray[i].StartsWith("Player"))
                    {
                        num13 = 7;
                        num14 = stringArray[i].IndexOf('.');
                        num15 = stringArray[i].IndexOf('(');
                        num16 = stringArray[i].LastIndexOf(')');
                        str2 = stringArray[i].Substring(num14 + 1, (num15 - num14) - 1);
                        strArray3 = stringArray[i].Substring(num15 + 1, (num16 - num15) - 1).Split(new char[] { ',' });
                        if (str2.StartsWith("KillPlayer"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 0, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SpawnPlayerAt"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            helper3 = this.returnHelper(strArray3[2]);
                            helper4 = this.returnHelper(strArray3[3]);
                            action = new RCAction(num13, 2, null, new RCActionHelper[] { helper, helper2, helper3, helper4 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SpawnPlayer"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            action = new RCAction(num13, 1, null, new RCActionHelper[] { helper });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("MovePlayer"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            helper3 = this.returnHelper(strArray3[2]);
                            helper4 = this.returnHelper(strArray3[3]);
                            action = new RCAction(num13, 3, null, new RCActionHelper[] { helper, helper2, helper3, helper4 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SetKills"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 4, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SetDeaths"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 5, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SetMaxDmg"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 6, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SetTotalDmg"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 7, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SetName"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 8, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SetGuildName"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 9, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SetTeam"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 10, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SetCustomInt"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 11, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SetCustomBool"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 12, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SetCustomString"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 13, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SetCustomFloat"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 14, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                    }
                    else if (stringArray[i].StartsWith("Titan"))
                    {
                        num13 = 8;
                        num14 = stringArray[i].IndexOf('.');
                        num15 = stringArray[i].IndexOf('(');
                        num16 = stringArray[i].LastIndexOf(')');
                        str2 = stringArray[i].Substring(num14 + 1, (num15 - num14) - 1);
                        strArray3 = stringArray[i].Substring(num15 + 1, (num16 - num15) - 1).Split(new char[] { ',' });
                        if (str2.StartsWith("KillTitan"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            helper3 = this.returnHelper(strArray3[2]);
                            action = new RCAction(num13, 0, null, new RCActionHelper[] { helper, helper2, helper3 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SpawnTitanAt"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            helper3 = this.returnHelper(strArray3[2]);
                            helper4 = this.returnHelper(strArray3[3]);
                            RCActionHelper helper5 = this.returnHelper(strArray3[4]);
                            RCActionHelper helper6 = this.returnHelper(strArray3[5]);
                            RCActionHelper helper7 = this.returnHelper(strArray3[6]);
                            action = new RCAction(num13, 2, null, new RCActionHelper[] { helper, helper2, helper3, helper4, helper5, helper6, helper7 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SpawnTitan"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            helper3 = this.returnHelper(strArray3[2]);
                            helper4 = this.returnHelper(strArray3[3]);
                            action = new RCAction(num13, 1, null, new RCActionHelper[] { helper, helper2, helper3, helper4 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("SetHealth"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            action = new RCAction(num13, 3, null, new RCActionHelper[] { helper, helper2 });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("MoveTitan"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            helper2 = this.returnHelper(strArray3[1]);
                            helper3 = this.returnHelper(strArray3[2]);
                            helper4 = this.returnHelper(strArray3[3]);
                            action = new RCAction(num13, 4, null, new RCActionHelper[] { helper, helper2, helper3, helper4 });
                            sentTrueActions.Add(action);
                        }
                    }
                    else if (stringArray[i].StartsWith("Game"))
                    {
                        num13 = 9;
                        num14 = stringArray[i].IndexOf('.');
                        num15 = stringArray[i].IndexOf('(');
                        num16 = stringArray[i].LastIndexOf(')');
                        str2 = stringArray[i].Substring(num14 + 1, (num15 - num14) - 1);
                        strArray3 = stringArray[i].Substring(num15 + 1, (num16 - num15) - 1).Split(new char[] { ',' });
                        if (str2.StartsWith("PrintMessage"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            action = new RCAction(num13, 0, null, new RCActionHelper[] { helper });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("LoseGame"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            action = new RCAction(num13, 2, null, new RCActionHelper[] { helper });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("WinGame"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            action = new RCAction(num13, 1, null, new RCActionHelper[] { helper });
                            sentTrueActions.Add(action);
                        }
                        else if (str2.StartsWith("Restart"))
                        {
                            helper = this.returnHelper(strArray3[0]);
                            action = new RCAction(num13, 3, null, new RCActionHelper[] { helper });
                            sentTrueActions.Add(action);
                        }
                    }
                }
            }
        }
        return new RCEvent(condition, sentTrueActions, eventClass, eventType);
    }

    [RPC]
    public void pauseRPC(bool pause, PhotonMessageInfo info)
    {
         if (info.sender.isMasterClient)
         {
             if (pause)
             {
                 this.pauseWaitTime = 100000f;
                 Time.timeScale = 1E-06f;
             }
            else
             {
                 this.pauseWaitTime = 3f;
             }
         }
    }

    public void playerKillInfoSingleUpdate(int dmg)
    {
        this.single_kills++;
        this.single_maxDamage = Mathf.Max(dmg, this.single_maxDamage);
        this.single_totalDamage += dmg;
    }

    public void playerKillInfoUpdate(PhotonPlayer player, int dmg)
    {
        ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.kills, ((int) player.customProperties[PhotonPlayerProperty.kills]) + 1);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new ExitGames.Client.Photon.Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.max_dmg, Mathf.Max(dmg, (int) player.customProperties[PhotonPlayerProperty.max_dmg]));
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new ExitGames.Client.Photon.Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.total_dmg, ((int) player.customProperties[PhotonPlayerProperty.total_dmg]) + dmg);
        player.SetCustomProperties(propertiesToSet);
    }

    public GameObject randomSpawnOneTitan(string place, int rate)
    {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag(place);
        int index = UnityEngine.Random.Range(0, objArray.Length);
        GameObject obj2 = objArray[index];
        return this.spawnTitan(rate, obj2.transform.position, obj2.transform.rotation, false);
    }

    public void randomSpawnTitan(string place, int rate, int num, bool punk = false)
    {
        if (num == -1)
        {
            num = 1;
        }
        GameObject[] objArray = GameObject.FindGameObjectsWithTag(place);
        List<GameObject> list = new List<GameObject>(objArray);
        if (objArray.Length > 0)
        {
            for (int i = 0; i < num; i++)
            {
                if (list.Count <= 0)
                    list = new List<GameObject>(objArray);
                int index = UnityEngine.Random.Range(0, list.Count);
                GameObject obj = list[index];
                list.RemoveAt(index);
                this.spawnTitan(rate, obj.transform.position, obj.transform.rotation, punk);
            }
        }
    }

    public Texture2D RCLoadTexture(string tex)
    {
        if (this.assetCacheTextures == null)
        {
            this.assetCacheTextures = new Dictionary<string, Texture2D>();
        }
        if (this.assetCacheTextures.ContainsKey(tex))
        {
            return this.assetCacheTextures[tex];
        }
        Texture2D textured2 = (Texture2D) RCassets.Load(tex);
        this.assetCacheTextures.Add(tex, textured2);
        return textured2;
    }

    public void RecompilePlayerList(float time)
    {
        if (!this.isRecompiling)
        {
            this.isRecompiling = true;
            base.StartCoroutine(this.WaitAndRecompilePlayerList(time));
        }
    }

    [RPC]
    private void refreshPVPStatus(int score1, int score2)
    {
        this.PVPhumanScore = score1;
        this.PVPtitanScore = score2;
    }

    [RPC]
    private void refreshPVPStatus_AHSS(int[] score1)
    {
        this.teamScores = score1;
    }

    private void refreshRacingResult()
    {
        this.localRacingResult = "Result\n";
        IComparer comparer = new IComparerRacingResult();
        this.racingResult.Sort(comparer);
        int num = Mathf.Min(this.racingResult.Count, 6);
        for (int i = 0; i < num; i++)
        {
            string localRacingResult = this.localRacingResult;
            object[] objArray1 = new object[] { localRacingResult, "Rank ", i + 1, " : " };
            this.localRacingResult = string.Concat(objArray1);
            this.localRacingResult = this.localRacingResult + (this.racingResult[i] as RacingResult).name;
            this.localRacingResult = this.localRacingResult + "   " + ((((int) ((this.racingResult[i] as RacingResult).time * 100f)) * 0.01f)).ToString() + "s";
            this.localRacingResult = this.localRacingResult + "\n";
        }
        object[] parameters = new object[] { this.localRacingResult };
        base.photonView.RPC("netRefreshRacingResult", PhotonTargets.All, parameters);
    }

    private void refreshRacingResult2()
    {
        this.localRacingResult = "Result\n";
        IComparer comparer = new IComparerRacingResult();
        this.racingResult.Sort(comparer);
        int num = Mathf.Min(this.racingResult.Count, 10);
        for (int i = 0; i < num; i++)
        {
            string localRacingResult = this.localRacingResult;
            object[] objArray2 = new object[] { localRacingResult, "Rank ", i + 1, " : " };
            this.localRacingResult = string.Concat(objArray2);
            this.localRacingResult = this.localRacingResult + (this.racingResult[i] as RacingResult).name;
            this.localRacingResult = this.localRacingResult + "   " + ((((int) ((this.racingResult[i] as RacingResult).time * 100f)) * 0.01f)).ToString() + "s";
            this.localRacingResult = this.localRacingResult + "\n";
        }
        object[] parameters = new object[] { this.localRacingResult };
        base.photonView.RPC("netRefreshRacingResult", PhotonTargets.All, parameters);
    }

    [RPC]
    private void refreshStatus(int score1, int score2, int wav, int highestWav, float time1, float time2, bool startRacin, bool endRacin, PhotonMessageInfo info)
    {
        if (info.sender == PhotonNetwork.masterClient && !PhotonNetwork.isMasterClient)
        {
            this.humanScore = score1;
            this.titanScore = score2;
            this.wave = wav;
            this.highestwave = highestWav;
            this.roundTime = time1;
            this.timeTotalServer = time2;
            this.startRacing = startRacin;
            this.endRacing = endRacin;
            if (this.startRacing && (GameObject.Find("door") != null))
            {
                GameObject.Find("door").SetActive(false);
            }
        }
    }

    public IEnumerator reloadSky(bool specmode = false)
    {
        yield return new WaitForSeconds(0.5f);
        Material skyMaterial = SkyboxCustomSkinLoader.SkyboxMaterial;
        if ((skyMaterial != null) && (Camera.main.GetComponent<Skybox>().material != skyMaterial))
        {
            Camera.main.GetComponent<Skybox>().material = skyMaterial;
        }
    }

    public void removeCT(COLOSSAL_TITAN titan)
    {
        this.cT.Remove(titan);
    }

    public void removeET(TITAN_EREN hero)
    {
        this.eT.Remove(hero);
    }

    public void removeFT(FEMALE_TITAN titan)
    {
        this.fT.Remove(titan);
    }

    public void removeHero(HERO hero)
    {
        this.heroes.Remove(hero);
    }

    public void removeHook(Bullet h)
    {
        this.hooks.Remove(h);
    }

    public void removeTitan(TITAN titan)
    {
        this.titans.Remove(titan);
    }

    [RPC]
    private void RequireStatus()
    {
        object[] parameters = new object[] { this.humanScore, this.titanScore, this.wave, this.highestwave, this.roundTime, this.timeTotalServer, this.startRacing, this.endRacing };
        base.photonView.RPC("refreshStatus", PhotonTargets.Others, parameters);
        object[] objArray2 = new object[] { this.PVPhumanScore, this.PVPtitanScore };
        base.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, objArray2);
        object[] objArray3 = new object[] { this.teamScores };
        base.photonView.RPC("refreshPVPStatus_AHSS", PhotonTargets.Others, objArray3);
    }

    private void resetGameSettings()
    {
        SettingsManager.LegacyGameSettings.SetDefault();
    }

    private void resetSettings(bool isLeave)
    {
        this.name = LoginFengKAI.player.name;
        masterRC = false;
        ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.RCteam, 0);
        if (isLeave)
        {
            currentLevel = string.Empty;
            propertiesToSet.Add(PhotonPlayerProperty.currentLevel, string.Empty);
            this.levelCache = new List<string[]>();
            this.titanSpawns.Clear();
            this.playerSpawnsC.Clear();
            this.playerSpawnsM.Clear();
            this.titanSpawners.Clear();
            intVariables.Clear();
            boolVariables.Clear();
            stringVariables.Clear();
            floatVariables.Clear();
            globalVariables.Clear();
            RCRegions.Clear();
            RCEvents.Clear();
            RCVariableNames.Clear();
            playerVariables.Clear();
            titanVariables.Clear();
            RCRegionTriggers.Clear();
            propertiesToSet.Add(PhotonPlayerProperty.statACL, 100);
            propertiesToSet.Add(PhotonPlayerProperty.statBLA, 100);
            propertiesToSet.Add(PhotonPlayerProperty.statGAS, 100);
            propertiesToSet.Add(PhotonPlayerProperty.statSPD, 100);
            this.restartingTitan = false;
            this.restartingMC = false;
            this.restartingHorse = false;
            this.restartingEren = false;
            this.restartingBomb = false;
        }
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        this.resetGameSettings();
        banHash = new ExitGames.Client.Photon.Hashtable();
        imatitan = new ExitGames.Client.Photon.Hashtable();
        oldScript = string.Empty;
        ignoreList = new List<int>();
        this.restartCount = new List<float>();
        heroHash = new ExitGames.Client.Photon.Hashtable();
    }

    private IEnumerator respawnE(float seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            if (!this.isLosing && !this.isWinning)
            {
                for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
                {
                    PhotonPlayer targetPlayer = PhotonNetwork.playerList[j];
                    if (((targetPlayer.customProperties[PhotonPlayerProperty.RCteam] == null) && RCextensions.returnBoolFromObject(targetPlayer.customProperties[PhotonPlayerProperty.dead])) && (RCextensions.returnIntFromObject(targetPlayer.customProperties[PhotonPlayerProperty.isTitan]) != 2))
                    {
                        this.photonView.RPC("respawnHeroInNewRound", targetPlayer, new object[0]);
                    }
                }
            }
        }
    }

    [RPC]
    private void respawnHeroInNewRound()
    {
        if (!this.needChooseSide && GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
        {
            this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
            this.ShowHUDInfoCenter(string.Empty);
        }
    }

    public IEnumerator restartE(float time)
    {
        yield return new WaitForSeconds(time);
        this.restartGame2(false);
    }

    public void restartGame2(bool masterclientSwitched = false)
    {
        if (!this.gameTimesUp)
        {
            this.PVPtitanScore = 0;
            this.PVPhumanScore = 0;
            this.startRacing = false;
            this.endRacing = false;
            this.checkpoint = null;
            this.timeElapse = 0f;
            this.roundTime = 0f;
            this.isWinning = false;
            this.isLosing = false;
            this.isPlayer1Winning = false;
            this.isPlayer2Winning = false;
            this.wave = 1;
            this.myRespawnTime = 0f;
            this.kicklist = new ArrayList();
            this.killInfoGO = new ArrayList();
            this.racingResult = new ArrayList();
            this.ShowHUDInfoCenter(string.Empty);
            this.isRestarting = true;
            this.DestroyAllExistingCloths();
            PhotonNetwork.DestroyAll();
            ExitGames.Client.Photon.Hashtable hash = this.checkGameGUI();
            base.photonView.RPC("settingRPC", PhotonTargets.Others, new object[] { hash });
            base.photonView.RPC("RPCLoadLevel", PhotonTargets.All, new object[0]);
            this.setGameSettings(hash);
            if (masterclientSwitched)
            {
                this.sendChatContentInfo("<color=#A8FF24>MasterClient has switched to </color>" + ((string) PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]).hexColor());
            }
        }
    }

    [RPC]
    private void restartGameByClient()
    {
    }

    public void restartGameSingle2()
    {
        this.startRacing = false;
        this.endRacing = false;
        this.checkpoint = null;
        this.single_kills = 0;
        this.single_maxDamage = 0;
        this.single_totalDamage = 0;
        this.timeElapse = 0f;
        this.roundTime = 0f;
        this.timeTotalServer = 0f;
        this.isWinning = false;
        this.isLosing = false;
        this.isPlayer1Winning = false;
        this.isPlayer2Winning = false;
        this.wave = 1;
        this.myRespawnTime = 0f;
        this.ShowHUDInfoCenter(string.Empty);
        this.DestroyAllExistingCloths();
        Application.LoadLevel(Application.loadedLevel);
    }

    public void restartRC()
    {
        intVariables.Clear();
        boolVariables.Clear();
        stringVariables.Clear();
        floatVariables.Clear();
        playerVariables.Clear();
        titanVariables.Clear();
        if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
        {
            this.endGameInfectionRC();
        }
        else
        {
            this.endGameRC();
        }
    }

    public RCActionHelper returnHelper(string str)
    {
        float num;
        int num3;
        string[] strArray = str.Split(new char[] { '.' });
        if (float.TryParse(str, out num))
        {
            strArray = new string[] { str };
        }
        List<RCActionHelper> list = new List<RCActionHelper>();
        int sentType = 0;
        for (num3 = 0; num3 < strArray.Length; num3++)
        {
            string str2;
            RCActionHelper helper;
            if (list.Count == 0)
            {
                str2 = strArray[num3];
                if (str2.StartsWith("\"") && str2.EndsWith("\""))
                {
                    helper = new RCActionHelper(0, 0, str2.Substring(1, str2.Length - 2));
                    list.Add(helper);
                    sentType = 2;
                }
                else
                {
                    int num4;
                    if (int.TryParse(str2, out num4))
                    {
                        helper = new RCActionHelper(0, 0, num4);
                        list.Add(helper);
                        sentType = 0;
                    }
                    else
                    {
                        float num5;
                        if (float.TryParse(str2, out num5))
                        {
                            helper = new RCActionHelper(0, 0, num5);
                            list.Add(helper);
                            sentType = 3;
                        }
                        else if ((str2.ToLower() == "true") || (str2.ToLower() == "false"))
                        {
                            helper = new RCActionHelper(0, 0, Convert.ToBoolean(str2.ToLower()));
                            list.Add(helper);
                            sentType = 1;
                        }
                        else
                        {
                            int index;
                            int num7;
                            if (str2.StartsWith("Variable"))
                            {
                                index = str2.IndexOf('(');
                                num7 = str2.LastIndexOf(')');
                                if (str2.StartsWith("VariableInt"))
                                {
                                    str2 = str2.Substring(index + 1, (num7 - index) - 1);
                                    helper = new RCActionHelper(1, 0, this.returnHelper(str2));
                                    list.Add(helper);
                                    sentType = 0;
                                }
                                else if (str2.StartsWith("VariableBool"))
                                {
                                    str2 = str2.Substring(index + 1, (num7 - index) - 1);
                                    helper = new RCActionHelper(1, 1, this.returnHelper(str2));
                                    list.Add(helper);
                                    sentType = 1;
                                }
                                else if (str2.StartsWith("VariableString"))
                                {
                                    str2 = str2.Substring(index + 1, (num7 - index) - 1);
                                    helper = new RCActionHelper(1, 2, this.returnHelper(str2));
                                    list.Add(helper);
                                    sentType = 2;
                                }
                                else if (str2.StartsWith("VariableFloat"))
                                {
                                    str2 = str2.Substring(index + 1, (num7 - index) - 1);
                                    helper = new RCActionHelper(1, 3, this.returnHelper(str2));
                                    list.Add(helper);
                                    sentType = 3;
                                }
                                else if (str2.StartsWith("VariablePlayer"))
                                {
                                    str2 = str2.Substring(index + 1, (num7 - index) - 1);
                                    helper = new RCActionHelper(1, 4, this.returnHelper(str2));
                                    list.Add(helper);
                                    sentType = 4;
                                }
                                else if (str2.StartsWith("VariableTitan"))
                                {
                                    str2 = str2.Substring(index + 1, (num7 - index) - 1);
                                    helper = new RCActionHelper(1, 5, this.returnHelper(str2));
                                    list.Add(helper);
                                    sentType = 5;
                                }
                            }
                            else if (str2.StartsWith("Region"))
                            {
                                index = str2.IndexOf('(');
                                num7 = str2.LastIndexOf(')');
                                if (str2.StartsWith("RegionRandomX"))
                                {
                                    str2 = str2.Substring(index + 1, (num7 - index) - 1);
                                    helper = new RCActionHelper(4, 0, this.returnHelper(str2));
                                    list.Add(helper);
                                    sentType = 3;
                                }
                                else if (str2.StartsWith("RegionRandomY"))
                                {
                                    str2 = str2.Substring(index + 1, (num7 - index) - 1);
                                    helper = new RCActionHelper(4, 1, this.returnHelper(str2));
                                    list.Add(helper);
                                    sentType = 3;
                                }
                                else if (str2.StartsWith("RegionRandomZ"))
                                {
                                    str2 = str2.Substring(index + 1, (num7 - index) - 1);
                                    helper = new RCActionHelper(4, 2, this.returnHelper(str2));
                                    list.Add(helper);
                                    sentType = 3;
                                }
                            }
                        }
                    }
                }
                continue;
            }
            if (list.Count <= 0)
            {
                continue;
            }
            str2 = strArray[num3];
            if (list[list.Count - 1].helperClass != 1)
            {
                goto Label_0AF5;
            }
            switch (list[list.Count - 1].helperType)
            {
                case 4:
                {
                    if (!str2.StartsWith("GetTeam()"))
                    {
                        break;
                    }
                    helper = new RCActionHelper(2, 1, null);
                    list.Add(helper);
                    sentType = 0;
                    continue;
                }
                case 5:
                {
                    if (!str2.StartsWith("GetType()"))
                    {
                        goto Label_0918;
                    }
                    helper = new RCActionHelper(3, 0, null);
                    list.Add(helper);
                    sentType = 0;
                    continue;
                }
                default:
                    goto Label_0A1C;
            }
            if (str2.StartsWith("GetType()"))
            {
                helper = new RCActionHelper(2, 0, null);
                list.Add(helper);
                sentType = 0;
            }
            else if (str2.StartsWith("GetIsAlive()"))
            {
                helper = new RCActionHelper(2, 2, null);
                list.Add(helper);
                sentType = 1;
            }
            else if (str2.StartsWith("GetTitan()"))
            {
                helper = new RCActionHelper(2, 3, null);
                list.Add(helper);
                sentType = 0;
            }
            else if (str2.StartsWith("GetKills()"))
            {
                helper = new RCActionHelper(2, 4, null);
                list.Add(helper);
                sentType = 0;
            }
            else if (str2.StartsWith("GetDeaths()"))
            {
                helper = new RCActionHelper(2, 5, null);
                list.Add(helper);
                sentType = 0;
            }
            else if (str2.StartsWith("GetMaxDmg()"))
            {
                helper = new RCActionHelper(2, 6, null);
                list.Add(helper);
                sentType = 0;
            }
            else if (str2.StartsWith("GetTotalDmg()"))
            {
                helper = new RCActionHelper(2, 7, null);
                list.Add(helper);
                sentType = 0;
            }
            else if (str2.StartsWith("GetCustomInt()"))
            {
                helper = new RCActionHelper(2, 8, null);
                list.Add(helper);
                sentType = 0;
            }
            else if (str2.StartsWith("GetCustomBool()"))
            {
                helper = new RCActionHelper(2, 9, null);
                list.Add(helper);
                sentType = 1;
            }
            else if (str2.StartsWith("GetCustomString()"))
            {
                helper = new RCActionHelper(2, 10, null);
                list.Add(helper);
                sentType = 2;
            }
            else if (str2.StartsWith("GetCustomFloat()"))
            {
                helper = new RCActionHelper(2, 11, null);
                list.Add(helper);
                sentType = 3;
            }
            else if (str2.StartsWith("GetPositionX()"))
            {
                helper = new RCActionHelper(2, 14, null);
                list.Add(helper);
                sentType = 3;
            }
            else if (str2.StartsWith("GetPositionY()"))
            {
                helper = new RCActionHelper(2, 15, null);
                list.Add(helper);
                sentType = 3;
            }
            else if (str2.StartsWith("GetPositionZ()"))
            {
                helper = new RCActionHelper(2, 0x10, null);
                list.Add(helper);
                sentType = 3;
            }
            else if (str2.StartsWith("GetName()"))
            {
                helper = new RCActionHelper(2, 12, null);
                list.Add(helper);
                sentType = 2;
            }
            else if (str2.StartsWith("GetGuildName()"))
            {
                helper = new RCActionHelper(2, 13, null);
                list.Add(helper);
                sentType = 2;
            }
            else if (str2.StartsWith("GetSpeed()"))
            {
                helper = new RCActionHelper(2, 0x11, null);
                list.Add(helper);
                sentType = 3;
            }
            continue;
        Label_0918:
            if (str2.StartsWith("GetSize()"))
            {
                helper = new RCActionHelper(3, 1, null);
                list.Add(helper);
                sentType = 3;
            }
            else if (str2.StartsWith("GetHealth()"))
            {
                helper = new RCActionHelper(3, 2, null);
                list.Add(helper);
                sentType = 0;
            }
            else if (str2.StartsWith("GetPositionX()"))
            {
                helper = new RCActionHelper(3, 3, null);
                list.Add(helper);
                sentType = 3;
            }
            else if (str2.StartsWith("GetPositionY()"))
            {
                helper = new RCActionHelper(3, 4, null);
                list.Add(helper);
                sentType = 3;
            }
            else if (str2.StartsWith("GetPositionZ()"))
            {
                helper = new RCActionHelper(3, 5, null);
                list.Add(helper);
                sentType = 3;
            }
            continue;
        Label_0A1C:
            if (str2.StartsWith("ConvertToInt()"))
            {
                helper = new RCActionHelper(5, sentType, null);
                list.Add(helper);
                sentType = 0;
            }
            else if (str2.StartsWith("ConvertToBool()"))
            {
                helper = new RCActionHelper(5, sentType, null);
                list.Add(helper);
                sentType = 1;
            }
            else if (str2.StartsWith("ConvertToString()"))
            {
                helper = new RCActionHelper(5, sentType, null);
                list.Add(helper);
                sentType = 2;
            }
            else if (str2.StartsWith("ConvertToFloat()"))
            {
                helper = new RCActionHelper(5, sentType, null);
                list.Add(helper);
                sentType = 3;
            }
            continue;
        Label_0AF5:
            if (str2.StartsWith("ConvertToInt()"))
            {
                helper = new RCActionHelper(5, sentType, null);
                list.Add(helper);
                sentType = 0;
            }
            else if (str2.StartsWith("ConvertToBool()"))
            {
                helper = new RCActionHelper(5, sentType, null);
                list.Add(helper);
                sentType = 1;
            }
            else if (str2.StartsWith("ConvertToString()"))
            {
                helper = new RCActionHelper(5, sentType, null);
                list.Add(helper);
                sentType = 2;
            }
            else if (str2.StartsWith("ConvertToFloat()"))
            {
                helper = new RCActionHelper(5, sentType, null);
                list.Add(helper);
                sentType = 3;
            }
        }
        for (num3 = list.Count - 1; num3 > 0; num3--)
        {
            list[num3 - 1].setNextHelper(list[num3]);
        }
        return list[0];
    }

    public static PeerStates returnPeerState(int peerstate)
    {
        switch (peerstate)
        {
            case 0:
                return PeerStates.Authenticated;

            case 1:
                return PeerStates.ConnectedToMaster;

            case 2:
                return PeerStates.DisconnectingFromMasterserver;

            case 3:
                return PeerStates.DisconnectingFromGameserver;

            case 4:
                return PeerStates.DisconnectingFromNameServer;
        }
        return PeerStates.ConnectingToMasterserver;
    }

    [RPC]
    private void RPCLoadLevel(PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            this.DestroyAllExistingCloths();
            PhotonNetwork.LoadLevel(LevelInfo.getInfo(level).mapName);
        }
        else if (PhotonNetwork.isMasterClient)
        {
            this.kickPlayerRC(info.sender, true, "false restart.");
        }
        else if (!masterRC)
        {
            this.restartCount.Add(Time.time);
            foreach (float num in this.restartCount)
            {
                if ((Time.time - num) > 60f)
                {
                    this.restartCount.Remove(num);
                }
            }
            if (this.restartCount.Count < 6)
            {
                this.DestroyAllExistingCloths();
                PhotonNetwork.LoadLevel(LevelInfo.getInfo(level).mapName);
            }
        }
    }

    public void sendChatContentInfo(string content)
    {
        object[] parameters = new object[] { content, string.Empty };
        base.photonView.RPC("Chat", PhotonTargets.All, parameters);
    }

    public void sendKillInfo(bool t1, string killer, bool t2, string victim, int dmg = 0)
    {
        object[] parameters = new object[] { t1, killer, t2, victim, dmg };
        base.photonView.RPC("updateKillInfo", PhotonTargets.All, parameters);
    }

    public static void ServerCloseConnection(PhotonPlayer targetPlayer, bool requestIpBan, string inGameName = null)
    {
        RaiseEventOptions options = new RaiseEventOptions {
            TargetActors = new int[] { targetPlayer.ID }
        };
        if (requestIpBan)
        {
            ExitGames.Client.Photon.Hashtable eventContent = new ExitGames.Client.Photon.Hashtable();
            eventContent[(byte) 0] = true;
            if ((inGameName != null) && (inGameName.Length > 0))
            {
                eventContent[(byte) 1] = inGameName;
            }
            PhotonNetwork.RaiseEvent(0xcb, eventContent, true, options);
        }
        else
        {
            PhotonNetwork.RaiseEvent(0xcb, null, true, options);
        }
    }

    public static void ServerRequestAuthentication(string authPassword)
    {
        if (!string.IsNullOrEmpty(authPassword))
        {
            ExitGames.Client.Photon.Hashtable eventContent = new ExitGames.Client.Photon.Hashtable();
            eventContent[(byte) 0] = authPassword;
            PhotonNetwork.RaiseEvent(0xc6, eventContent, true, new RaiseEventOptions());
        }
    }

    public static void ServerRequestUnban(string bannedAddress)
    {
        if (!string.IsNullOrEmpty(bannedAddress))
        {
            ExitGames.Client.Photon.Hashtable eventContent = new ExitGames.Client.Photon.Hashtable();
            eventContent[(byte) 0] = bannedAddress;
            PhotonNetwork.RaiseEvent(0xc7, eventContent, true, new RaiseEventOptions());
        }
    }

    private void setGameSettings(ExitGames.Client.Photon.Hashtable hash)
    {
        string str;
        ExitGames.Client.Photon.Hashtable hashtable;
        this.restartingEren = false;
        this.restartingBomb = false;
        this.restartingHorse = false;
        this.restartingTitan = false;
        LegacyGameSettings settings = SettingsManager.LegacyGameSettings;
        if (hash.ContainsKey("bomb"))
        {
            if (!settings.BombModeEnabled.Value)
            {
                settings.BombModeEnabled.Value = true;
                this.chatRoom.addLINE("<color=#FFCC00>PVP Bomb Mode enabled.</color>");
            }
        }
        else if (settings.BombModeEnabled.Value)
        {
            settings.BombModeEnabled.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>PVP Bomb Mode disabled.</color>");
            if (PhotonNetwork.isMasterClient)
            {
                this.restartingBomb = true;
            }
        }
        if (hash.ContainsKey("globalDisableMinimap"))
        {
            if (!settings.GlobalMinimapDisable.Value)
            {
                settings.GlobalMinimapDisable.Value = true;
                this.chatRoom.addLINE("<color=#FFCC00>Minimaps are not allowed.</color>");
            }
        }
        else if (settings.GlobalMinimapDisable.Value)
        {
            settings.GlobalMinimapDisable.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Minimaps are allowed.</color>");
        }
        if (hash.ContainsKey("horse"))
        {
            if (!settings.AllowHorses.Value)
            {
                settings.AllowHorses.Value = true;
                this.chatRoom.addLINE("<color=#FFCC00>Horses enabled.</color>");
            }
        }
        else if (settings.AllowHorses.Value)
        {
            settings.AllowHorses.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Horses disabled.</color>");
            if (PhotonNetwork.isMasterClient)
            {
                this.restartingHorse = true;
            }
        }
        if (hash.ContainsKey("punkWaves"))
        {
            if (settings.PunksEveryFive.Value) // reversed
            {
                settings.PunksEveryFive.Value = false;
                this.chatRoom.addLINE("<color=#FFCC00>Punks every 5 waves disabled.</color>");
            }
        }
        else if (!settings.PunksEveryFive.Value)
        {
            settings.PunksEveryFive.Value = true;
            this.chatRoom.addLINE("<color=#FFCC00>Punks ever 5 waves enabled.</color>");
        }
        if (hash.ContainsKey("ahssReload"))
        {
            if (settings.AHSSAirReload.Value) // reversed
            {
                settings.AHSSAirReload.Value = false;
                this.chatRoom.addLINE("<color=#FFCC00>AHSS Air-Reload disabled.</color>");
            }
        }
        else if (!settings.AHSSAirReload.Value)
        {
            settings.AHSSAirReload.Value = true;
            this.chatRoom.addLINE("<color=#FFCC00>AHSS Air-Reload allowed.</color>");
        }
        if (hash.ContainsKey("team"))
        {
            if (settings.TeamMode.Value != ((int)hash["team"]))
            {
                settings.TeamMode.Value = (int)hash["team"];
                str = string.Empty;
                if (settings.TeamMode.Value == 1)
                {
                    str = "no sort";
                }
                else if (settings.TeamMode.Value == 2)
                {
                    str = "locked by size";
                }
                else if (settings.TeamMode.Value == 3)
                {
                    str = "locked by skill";
                }
                this.chatRoom.addLINE("<color=#FFCC00>Team Mode enabled (" + str + ").</color>");
                if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 0)
                {
                    this.setTeam(3);
                }
            }
        }
        else if (settings.TeamMode.Value != 0)
        {
            settings.TeamMode.Value = 0;
            this.setTeam(0);
            this.chatRoom.addLINE("<color=#FFCC00>Team mode disabled.</color>");
        }
        if (hash.ContainsKey("point"))
        {
            if (!settings.PointModeEnabled.Value || settings.PointModeAmount.Value != (int)hash["point"])
            {
                settings.PointModeEnabled.Value = true;
                settings.PointModeAmount.Value = (int)hash["point"];
                this.chatRoom.addLINE("<color=#FFCC00>Point limit enabled (" + Convert.ToString(settings.PointModeAmount.Value) + ").</color>");
            }
        }
        else if (settings.PointModeEnabled.Value)
        {
            settings.PointModeEnabled.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Point limit disabled.</color>");
        }
        if (hash.ContainsKey("rock"))
        {
            if (settings.RockThrowEnabled.Value) // reversed
            {
                settings.RockThrowEnabled.Value = false;
                this.chatRoom.addLINE("<color=#FFCC00>Punk rock throwing disabled.</color>");
            }
        }
        else if (!settings.RockThrowEnabled.Value)
        {
            settings.RockThrowEnabled.Value = true;
            this.chatRoom.addLINE("<color=#FFCC00>Punk rock throwing enabled.</color>");
        }
        if (hash.ContainsKey("explode"))
        {
            if (!settings.TitanExplodeEnabled.Value || settings.TitanExplodeRadius.Value != (int)hash["explode"])
            {
                settings.TitanExplodeEnabled.Value = true;
                settings.TitanExplodeRadius.Value = (int)hash["explode"];
                this.chatRoom.addLINE("<color=#FFCC00>Titan Explode Mode enabled (Radius " + Convert.ToString(settings.TitanExplodeRadius.Value) + ").</color>");
            }
        }
        else if (settings.TitanExplodeEnabled.Value)
        {
            settings.TitanExplodeEnabled.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Titan Explode Mode disabled.</color>");
        }
        if ((hash.ContainsKey("healthMode") && hash.ContainsKey("healthLower")) && hash.ContainsKey("healthUpper"))
        {
            if (((settings.TitanHealthMode.Value != ((int)hash["healthMode"])) || (settings.TitanHealthMin.Value != ((int)hash["healthLower"]))) || (settings.TitanHealthMax.Value != ((int)hash["healthUpper"])))
            {
                settings.TitanHealthMode.Value = (int)hash["healthMode"];
                settings.TitanHealthMin.Value = (int)hash["healthLower"];
                settings.TitanHealthMax.Value = (int)hash["healthUpper"];
                str = "Static";
                if (settings.TitanHealthMode.Value == 2)
                {
                    str = "Scaled";
                }
                this.chatRoom.addLINE("<color=#FFCC00>Titan Health (" + str + ", " + settings.TitanHealthMin.Value.ToString() + " to " + settings.TitanHealthMax.Value.ToString() + ") enabled.</color>");
            }
        }
        else if (settings.TitanHealthMode.Value > 0)
        {
            settings.TitanHealthMode.Value = 0;
            this.chatRoom.addLINE("<color=#FFCC00>Titan Health disabled.</color>");
        }
        if (hash.ContainsKey("infection"))
        {
            if (!settings.InfectionModeEnabled.Value)
            {
                settings.InfectionModeEnabled.Value = true;
                settings.InfectionModeAmount.Value = (int)hash["infection"];
                this.name = LoginFengKAI.player.name;
                hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable.Add(PhotonPlayerProperty.RCteam, 0);
                PhotonNetwork.player.SetCustomProperties(hashtable);
                this.chatRoom.addLINE("<color=#FFCC00>Infection mode (" + Convert.ToString(settings.InfectionModeAmount.Value) + ") enabled. Make sure your first character is human.</color>");
            }
        }
        else if (settings.InfectionModeEnabled.Value)
        {
            settings.InfectionModeEnabled.Value = false;
            hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.isTitan, 1);
            PhotonNetwork.player.SetCustomProperties(hashtable);
            this.chatRoom.addLINE("<color=#FFCC00>Infection Mode disabled.</color>");
            if (PhotonNetwork.isMasterClient)
            {
                this.restartingTitan = true;
            }
        }
        if (hash.ContainsKey("eren"))
        {
            if (!settings.KickShifters.Value)
            {
                settings.KickShifters.Value = true;
                this.chatRoom.addLINE("<color=#FFCC00>Anti-Eren enabled. Using eren transform will get you kicked.</color>");
                if (PhotonNetwork.isMasterClient)
                {
                    this.restartingEren = true;
                }
            }
        }
        else if (settings.KickShifters.Value)
        {
            settings.KickShifters.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Anti-Eren disabled. Eren transform is allowed.</color>");
        }
        if (hash.ContainsKey("titanc"))
        {
            if (!settings.TitanNumberEnabled.Value || settings.TitanNumber.Value != (int)hash["titanc"])
            {
                settings.TitanNumberEnabled.Value = true;
                settings.TitanNumber.Value = (int)hash["titanc"];
                this.chatRoom.addLINE("<color=#FFCC00>" + Convert.ToString(settings.TitanNumber.Value) + " titans will spawn each round.</color>");
            }
        }
        else if (settings.TitanNumberEnabled.Value)
        {
            settings.TitanNumberEnabled.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Default titans will spawn each round.</color>");
        }
        if (hash.ContainsKey("damage"))
        {
            if (!settings.TitanArmorEnabled.Value || settings.TitanArmor.Value != (int)hash["damage"])
            {
                settings.TitanArmorEnabled.Value = true;
                settings.TitanArmor.Value = (int)hash["damage"];
                this.chatRoom.addLINE("<color=#FFCC00>Nape minimum damage (" + Convert.ToString(settings.TitanArmor.Value) + ") enabled.</color>");
            }
        }
        else if (settings.TitanArmorEnabled.Value)
        {
            settings.TitanArmorEnabled.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Nape minimum damage disabled.</color>");
        }
        if ((hash.ContainsKey("sizeMode") && hash.ContainsKey("sizeLower")) && hash.ContainsKey("sizeUpper"))
        {
            if (!settings.TitanSizeEnabled.Value || settings.TitanSizeMin.Value != (float)hash["sizeLower"] || settings.TitanSizeMax.Value != (float)hash["sizeUpper"])
            {
                settings.TitanSizeEnabled.Value = true;
                settings.TitanSizeMin.Value = (float)hash["sizeLower"];
                settings.TitanSizeMax.Value = (float)hash["sizeUpper"];
                this.chatRoom.addLINE("<color=#FFCC00>Custom titan size (" + settings.TitanSizeMin.Value.ToString("F2") + "," + settings.TitanSizeMax.Value.ToString("F2") + ") enabled.</color>");
            }
        }
        else if (settings.TitanSizeEnabled.Value)
        {
            settings.TitanSizeEnabled.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Custom titan size disabled.</color>");
        }
        if ((((hash.ContainsKey("spawnMode") && hash.ContainsKey("nRate")) && (hash.ContainsKey("aRate") && hash.ContainsKey("jRate"))) && hash.ContainsKey("cRate")) && hash.ContainsKey("pRate"))
        {
            if (!settings.TitanSpawnEnabled.Value || settings.TitanSpawnNormal.Value != (float)hash["nRate"] || settings.TitanSpawnAberrant.Value != (float)hash["aRate"] || 
                settings.TitanSpawnJumper.Value != (float)hash["jRate"] || settings.TitanSpawnCrawler.Value != (float)hash["cRate"] || settings.TitanSpawnPunk.Value != (float)hash["pRate"])
            {
                settings.TitanSpawnEnabled.Value = true;
                settings.TitanSpawnNormal.Value = (float)hash["nRate"];
                settings.TitanSpawnAberrant.Value = (float)hash["aRate"];
                settings.TitanSpawnJumper.Value = (float)hash["jRate"];
                settings.TitanSpawnCrawler.Value = (float)hash["cRate"];
                settings.TitanSpawnPunk.Value = (float)hash["pRate"];
                this.chatRoom.addLINE("<color=#FFCC00>Custom spawn rate enabled (" + settings.TitanSpawnNormal.Value.ToString("F2") + "% Normal, " + settings.TitanSpawnAberrant.Value.ToString("F2") + "% Abnormal, " +
                    settings.TitanSpawnJumper.Value.ToString("F2") + "% Jumper, " + settings.TitanSpawnCrawler.Value.ToString("F2") + "% Crawler, " + settings.TitanSpawnPunk.Value.ToString("F2") + "% Punk </color>");
            }
        }
        else if (settings.TitanSpawnEnabled.Value)
        {
            settings.TitanSpawnEnabled.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Custom spawn rate disabled.</color>");
        }
        if (hash.ContainsKey("waveModeOn") && hash.ContainsKey("waveModeNum"))
        {
            if (!settings.TitanPerWavesEnabled.Value || (settings.TitanPerWaves.Value != ((int)hash["waveModeNum"])))
            {
                settings.TitanPerWavesEnabled.Value = true;
                settings.TitanPerWaves.Value = (int)hash["waveModeNum"];
                this.chatRoom.addLINE("<color=#FFCC00>Custom wave mode (" + settings.TitanPerWaves.Value.ToString() + ") enabled.</color>");
            }
        }
        else if (settings.TitanPerWavesEnabled.Value)
        {
            settings.TitanPerWavesEnabled.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Custom wave mode disabled.</color>");
        }
        if (hash.ContainsKey("friendly"))
        {
            if (!settings.FriendlyMode.Value)
            {
                settings.FriendlyMode.Value = true;
                this.chatRoom.addLINE("<color=#FFCC00>PVP is prohibited.</color>");
            }
        }
        else if (settings.FriendlyMode.Value)
        {
            settings.FriendlyMode.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>PVP is allowed.</color>");
        }
        if (hash.ContainsKey("pvp"))
        {
            if (settings.BladePVP.Value != ((int)hash["pvp"]))
            {
                settings.BladePVP.Value = (int)hash["pvp"];
                str = string.Empty;
                if (settings.BladePVP.Value == 1)
                {
                    str = "Team-Based";
                }
                else if (settings.BladePVP.Value == 2)
                {
                    str = "FFA";
                }
                this.chatRoom.addLINE("<color=#FFCC00>Blade/AHSS PVP enabled (" + str + ").</color>");
            }
        }
        else if (settings.BladePVP.Value != 0)
        {
            settings.BladePVP.Value = 0;
            this.chatRoom.addLINE("<color=#FFCC00>Blade/AHSS PVP disabled.</color>");
        }
        if (hash.ContainsKey("maxwave"))
        {
            if (!settings.TitanMaxWavesEnabled.Value || settings.TitanMaxWaves.Value != (int)hash["maxwave"])
            {
                settings.TitanMaxWavesEnabled.Value = true;
                settings.TitanMaxWaves.Value = (int)hash["maxwave"];
                this.chatRoom.addLINE("<color=#FFCC00>Max wave is " + settings.TitanMaxWaves.Value.ToString() + ".</color>");
            }
        }
        else if (settings.TitanMaxWavesEnabled.Value)
        {
            settings.TitanMaxWavesEnabled.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Max wave set to default.</color>");
        }
        if (hash.ContainsKey("endless"))
        {
            if (!settings.EndlessRespawnEnabled.Value || settings.EndlessRespawnTime.Value != (int)hash["endless"])
            {
                settings.EndlessRespawnEnabled.Value = true;
                settings.EndlessRespawnTime.Value = (int)hash["endless"];
                this.chatRoom.addLINE("<color=#FFCC00>Endless respawn enabled (" + settings.EndlessRespawnTime.Value.ToString() + " seconds).</color>");
            }
        }
        else if (settings.EndlessRespawnEnabled.Value)
        {
            settings.EndlessRespawnEnabled.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Endless respawn disabled.</color>");
        }
        if (hash.ContainsKey("motd"))
        {
            if (settings.Motd.Value != ((string)hash["motd"]))
            {
                settings.Motd.Value = (string)hash["motd"];
                this.chatRoom.addLINE("<color=#FFCC00>MOTD:" + settings.Motd.Value + "</color>");
            }
        }
        else if (settings.Motd.Value != string.Empty)
        {
            settings.Motd.Value = string.Empty;
        }
        if (hash.ContainsKey("deadlycannons"))
        {
            if (!settings.CannonsFriendlyFire.Value)
            {
                settings.CannonsFriendlyFire.Value = true;
                this.chatRoom.addLINE("<color=#FFCC00>Cannons will now kill players.</color>");
            }
        }
        else if (settings.CannonsFriendlyFire.Value)
        {
            settings.CannonsFriendlyFire.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Cannons will no longer kill players.</color>");
        }
        if (hash.ContainsKey("asoracing"))
        {
            if (!settings.RacingEndless.Value)
            {
                settings.RacingEndless.Value = true;
                this.chatRoom.addLINE("<color=#FFCC00>Racing will not restart on win.</color>");
            }
        }
        else if (settings.RacingEndless.Value)
        {
            settings.RacingEndless.Value = false;
            this.chatRoom.addLINE("<color=#FFCC00>Racing will restart on win.</color>");
        }
    }

    private IEnumerator setGuildFeng()
    {
        WWW iteratorVariable1;
        WWWForm form = new WWWForm();
        form.AddField("name", LoginFengKAI.player.name);
        form.AddField("guildname", LoginFengKAI.player.guildname);
        if (Application.isWebPlayer)
        {
            iteratorVariable1 = new WWW("http://aotskins.com/version/guild.php", form);
        }
        else
        {
            iteratorVariable1 = new WWW("http://fenglee.com/game/aog/change_guild_name.php", form);
        }
        yield return iteratorVariable1;
    }

    [RPC]
    private void setMasterRC(PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            masterRC = true;
        }
    }

    private void setTeam(int setting)
    {
        if (setting == 0)
        {
            this.name = LoginFengKAI.player.name;
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.RCteam, 0);
            propertiesToSet.Add(PhotonPlayerProperty.name, this.name);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
        else if (setting == 1)
        {
            ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
            hashtable2.Add(PhotonPlayerProperty.RCteam, 1);
            string name = LoginFengKAI.player.name;
            while (name.Contains("[") && (name.Length >= (name.IndexOf("[") + 8)))
            {
                int index = name.IndexOf("[");
                name = name.Remove(index, 8);
            }
            if (!name.StartsWith("[00FFFF]"))
            {
                name = "[00FFFF]" + name;
            }
            this.name = name;
            hashtable2.Add(PhotonPlayerProperty.name, this.name);
            PhotonNetwork.player.SetCustomProperties(hashtable2);
        }
        else if (setting == 2)
        {
            ExitGames.Client.Photon.Hashtable hashtable3 = new ExitGames.Client.Photon.Hashtable();
            hashtable3.Add(PhotonPlayerProperty.RCteam, 2);
            string str2 = LoginFengKAI.player.name;
            while (str2.Contains("[") && (str2.Length >= (str2.IndexOf("[") + 8)))
            {
                int startIndex = str2.IndexOf("[");
                str2 = str2.Remove(startIndex, 8);
            }
            if (!str2.StartsWith("[FF00FF]"))
            {
                str2 = "[FF00FF]" + str2;
            }
            this.name = str2;
            hashtable3.Add(PhotonPlayerProperty.name, this.name);
            PhotonNetwork.player.SetCustomProperties(hashtable3);
        }
        else if (setting == 3)
        {
            int num3 = 0;
            int num4 = 0;
            int num5 = 1;
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                int num7 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.RCteam]);
                if (num7 > 0)
                {
                    if (num7 == 1)
                    {
                        num3++;
                    }
                    else if (num7 == 2)
                    {
                        num4++;
                    }
                }
            }
            if (num3 > num4)
            {
                num5 = 2;
            }
            this.setTeam(num5);
        }
        if (((setting == 0) || (setting == 1)) || (setting == 2))
        {
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (obj2.GetPhotonView().isMine)
                {
                    base.photonView.RPC("labelRPC", PhotonTargets.All, new object[] { obj2.GetPhotonView().viewID });
                }
            }
        }
    }

    [RPC]
    private void setTeamRPC(int setting, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient || info.sender.isLocal)
        {
            this.setTeam(setting);
        }
    }

    [RPC]
    private void settingRPC(ExitGames.Client.Photon.Hashtable hash, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            this.setGameSettings(hash);
        }
    }

    private void showChatContent(string content)
    {
        this.chatContent.Add(content);
        if (this.chatContent.Count > 10)
        {
            this.chatContent.RemoveAt(0);
        }
        GameObject.Find("LabelChatContent").GetComponent<UILabel>().text = string.Empty;
        for (int i = 0; i < this.chatContent.Count; i++)
        {
            UILabel component = GameObject.Find("LabelChatContent").GetComponent<UILabel>();
            component.text = component.text + this.chatContent[i];
        }
    }

    public void ShowHUDInfoCenter(string content)
    {
        GameObject obj2 = GameObject.Find("LabelInfoCenter");
        if (obj2 != null)
        {
            obj2.GetComponent<UILabel>().text = content;
        }
    }

    public void ShowHUDInfoCenterADD(string content)
    {
        GameObject obj2 = GameObject.Find("LabelInfoCenter");
        if (obj2 != null)
        {
            UILabel component = obj2.GetComponent<UILabel>();
            component.text = component.text + content;
        }
    }

    private void ShowHUDInfoTopCenter(string content)
    {
        GameObject obj2 = GameObject.Find("LabelInfoTopCenter");
        if (obj2 != null)
        {
            obj2.GetComponent<UILabel>().text = content;
        }
    }

    private void ShowHUDInfoTopCenterADD(string content)
    {
        GameObject obj2 = GameObject.Find("LabelInfoTopCenter");
        if (obj2 != null)
        {
            UILabel component = obj2.GetComponent<UILabel>();
            component.text = component.text + content;
        }
    }

    private void ShowHUDInfoTopLeft(string content)
    {
        GameObject obj2 = GameObject.Find("LabelInfoTopLeft");
        if (obj2 != null)
        {
            obj2.GetComponent<UILabel>().text = content;
        }
    }

    private void ShowHUDInfoTopRight(string content)
    {
        GameObject obj2 = GameObject.Find("LabelInfoTopRight");
        if (obj2 != null)
        {
            obj2.GetComponent<UILabel>().text = content;
        }
    }

    private void ShowHUDInfoTopRightMAPNAME(string content)
    {
        GameObject obj2 = GameObject.Find("LabelInfoTopRight");
        if (obj2 != null)
        {
            UILabel component = obj2.GetComponent<UILabel>();
            component.text = component.text + content;
        }
    }

    [RPC]
    private void showResult(string text0, string text1, string text2, string text3, string text4, string text6, PhotonMessageInfo t)
    {
        if (!(this.gameTimesUp || !t.sender.isMasterClient))
        {
            this.gameTimesUp = true;
            GameObject obj2 = GameObject.Find("UI_IN_GAME");
            NGUITools.SetActive(obj2.GetComponent<UIReferArray>().panels[0], false);
            NGUITools.SetActive(obj2.GetComponent<UIReferArray>().panels[1], false);
            NGUITools.SetActive(obj2.GetComponent<UIReferArray>().panels[2], true);
            NGUITools.SetActive(obj2.GetComponent<UIReferArray>().panels[3], false);
            GameObject.Find("LabelName").GetComponent<UILabel>().text = text0;
            GameObject.Find("LabelKill").GetComponent<UILabel>().text = text1;
            GameObject.Find("LabelDead").GetComponent<UILabel>().text = text2;
            GameObject.Find("LabelMaxDmg").GetComponent<UILabel>().text = text3;
            GameObject.Find("LabelTotalDmg").GetComponent<UILabel>().text = text4;
            GameObject.Find("LabelResultTitle").GetComponent<UILabel>().text = text6;
            IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
            this.gameStart = false;
        }
        else if (!(t.sender.isMasterClient || !PhotonNetwork.player.isMasterClient))
        {
            this.kickPlayerRC(t.sender, true, "false game end.");
        }
    }

    private void SingleShowHUDInfoTopCenter(string content)
    {
        GameObject obj2 = GameObject.Find("LabelInfoTopCenter");
        if (obj2 != null)
        {
            obj2.GetComponent<UILabel>().text = content;
        }
    }

    private void SingleShowHUDInfoTopLeft(string content)
    {
        GameObject obj2 = GameObject.Find("LabelInfoTopLeft");
        if (obj2 != null)
        {
            content = content.Replace("[0]", "[*^_^*]");
            obj2.GetComponent<UILabel>().text = content;
        }
    }

    [RPC]
    public void someOneIsDead(int id = -1)
    {
        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
        {
            if (id != 0)
            {
                this.PVPtitanScore += 2;
            }
            this.checkPVPpts();
            object[] parameters = new object[] { this.PVPhumanScore, this.PVPtitanScore };
            base.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters);
        }
        else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
        {
            this.titanScore++;
        }
        else if (((IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN) || (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)) || ((IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT) || (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.TROST)))
        {
            if (this.isPlayerAllDead2())
            {
                this.gameLose2();
            }
        }
        else if (((IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS) && (SettingsManager.LegacyGameSettings.BladePVP.Value == 0)) && (!SettingsManager.LegacyGameSettings.BombModeEnabled.Value))
        {
            if (this.isPlayerAllDead2())
            {
                this.gameLose2();
                this.teamWinner = 0;
            }
            if (this.isTeamAllDead2(1))
            {
                this.teamWinner = 2;
                this.gameWin2();
            }
            if (this.isTeamAllDead2(2))
            {
                this.teamWinner = 1;
                this.gameWin2();
            }
        }
    }

    public void SpawnNonAITitan(string id, string tag = "titanRespawn")
    {
        GameObject obj3;
        GameObject[] objArray = GameObject.FindGameObjectsWithTag(tag);
        GameObject obj2 = objArray[UnityEngine.Random.Range(0, objArray.Length)];
        this.myLastHero = id.ToUpper();
        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
        {
            obj3 = PhotonNetwork.Instantiate("TITAN_VER3.1", this.checkpoint.transform.position + new Vector3((float) UnityEngine.Random.Range(-20, 20), 2f, (float) UnityEngine.Random.Range(-20, 20)), this.checkpoint.transform.rotation, 0);
        }
        else
        {
            obj3 = PhotonNetwork.Instantiate("TITAN_VER3.1", obj2.transform.position, obj2.transform.rotation, 0);
        }
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObjectASTITAN(obj3);
        obj3.GetComponent<TITAN>().nonAI = true;
        obj3.GetComponent<TITAN>().speed = 30f;
        obj3.GetComponent<TITAN_CONTROLLER>().enabled = true;
        if ((id == "RANDOM") && (UnityEngine.Random.Range(0, 100) < 7))
        {
            obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
        }
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
        GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
        GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add("dead", false);
        ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add(PhotonPlayerProperty.isTitan, 2);
        propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        this.ShowHUDInfoCenter(string.Empty);
    }

    public void SpawnNonAITitan2(string id, string tag = "titanRespawn")
    {
        if (logicLoaded && customLevelLoaded)
        {
            GameObject obj3;
            GameObject[] objArray = GameObject.FindGameObjectsWithTag(tag);
            GameObject obj2 = objArray[UnityEngine.Random.Range(0, objArray.Length)];
            Vector3 position = obj2.transform.position;
            if (level.StartsWith("Custom") && (this.titanSpawns.Count > 0))
            {
                position = this.titanSpawns[UnityEngine.Random.Range(0, this.titanSpawns.Count)];
            }
            this.myLastHero = id.ToUpper();
            if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
            {
                obj3 = PhotonNetwork.Instantiate("TITAN_VER3.1", this.checkpoint.transform.position + new Vector3((float) UnityEngine.Random.Range(-20, 20), 2f, (float) UnityEngine.Random.Range(-20, 20)), this.checkpoint.transform.rotation, 0);
            }
            else
            {
                obj3 = PhotonNetwork.Instantiate("TITAN_VER3.1", position, obj2.transform.rotation, 0);
            }
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObjectASTITAN(obj3);
            obj3.GetComponent<TITAN>().nonAI = true;
            obj3.GetComponent<TITAN>().speed = 30f;
            obj3.GetComponent<TITAN_CONTROLLER>().enabled = true;
            if ((id == "RANDOM") && (UnityEngine.Random.Range(0, 100) < 7))
            {
                obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
            }
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
            GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
            GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("dead", false);
            ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.isTitan, 2);
            propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            this.ShowHUDInfoCenter(string.Empty);
        }
        else
        {
            this.NOTSpawnNonAITitanRC(id);
        }
    }

    public void SpawnPlayer(string id, string tag = "playerRespawn")
    {
        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
        {
            this.SpawnPlayerAt2(id, this.checkpoint);
        }
        else
        {
            this.myLastRespawnTag = tag;
            GameObject[] objArray = GameObject.FindGameObjectsWithTag(tag);
            GameObject pos = objArray[UnityEngine.Random.Range(0, objArray.Length)];
            this.SpawnPlayerAt2(id, pos);
        }
    }

    public void SpawnPlayerAt2(string id, GameObject pos)
    {
        if (!logicLoaded || !customLevelLoaded)
        {
            this.NOTSpawnPlayerRC(id);
        }
        else
        {
            Vector3 position = pos.transform.position;
            if (this.racingSpawnPointSet)
            {
                position = this.racingSpawnPoint;
            }
            else if (level.StartsWith("Custom"))
            {
                if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 0)
                {
                    List<Vector3> list = new List<Vector3>();
                    foreach (Vector3 vector2 in this.playerSpawnsC)
                    {
                        list.Add(vector2);
                    }
                    foreach (Vector3 vector2 in this.playerSpawnsM)
                    {
                        list.Add(vector2);
                    }
                    if (list.Count > 0)
                    {
                        position = list[UnityEngine.Random.Range(0, list.Count)];
                    }
                }
                else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 1)
                {
                    if (this.playerSpawnsC.Count > 0)
                    {
                        position = this.playerSpawnsC[UnityEngine.Random.Range(0, this.playerSpawnsC.Count)];
                    }
                }
                else if ((RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 2) && (this.playerSpawnsM.Count > 0))
                {
                    position = this.playerSpawnsM[UnityEngine.Random.Range(0, this.playerSpawnsM.Count)];
                }
            }
            IN_GAME_MAIN_CAMERA component = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>();
            this.myLastHero = id.ToUpper();
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                if (IN_GAME_MAIN_CAMERA.singleCharacter == "TITAN_EREN")
                {
                    component.setMainObject((GameObject) UnityEngine.Object.Instantiate(Resources.Load("TITAN_EREN"), pos.transform.position, pos.transform.rotation), true, false);
                }
                else
                {
                    component.setMainObject((GameObject) UnityEngine.Object.Instantiate(Resources.Load("AOTTG_HERO 1"), pos.transform.position, pos.transform.rotation), true, false);
                    if (((IN_GAME_MAIN_CAMERA.singleCharacter == "SET 1") || (IN_GAME_MAIN_CAMERA.singleCharacter == "SET 2")) || (IN_GAME_MAIN_CAMERA.singleCharacter == "SET 3"))
                    {
                        HeroCostume costume = CostumeConeveter.LocalDataToHeroCostume(IN_GAME_MAIN_CAMERA.singleCharacter);
                        costume.checkstat();
                        CostumeConeveter.HeroCostumeToLocalData(costume, IN_GAME_MAIN_CAMERA.singleCharacter);
                        component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
                        if (costume != null)
                        {
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = costume;
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = costume.stat;
                        }
                        else
                        {
                            costume = HeroCostume.costumeOption[3];
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = costume;
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(costume.name.ToUpper());
                        }
                        component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
                        component.main_object.GetComponent<HERO>().setStat2();
                        component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
                    }
                    else
                    {
                        for (int i = 0; i < HeroCostume.costume.Length; i++)
                        {
                            if (HeroCostume.costume[i].name.ToUpper() == IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper())
                            {
                                int index = (HeroCostume.costume[i].id + CheckBoxCostume.costumeSet) - 1;
                                if (HeroCostume.costume[index].name != HeroCostume.costume[i].name)
                                {
                                    index = HeroCostume.costume[i].id + 1;
                                }
                                component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
                                component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[index];
                                component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[index].name.ToUpper());
                                component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
                                component.main_object.GetComponent<HERO>().setStat2();
                                component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                component.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, pos.transform.rotation, 0), true, false);
                id = id.ToUpper();
                if (((id == "SET 1") || (id == "SET 2")) || (id == "SET 3"))
                {
                    HeroCostume costume2 = CostumeConeveter.LocalDataToHeroCostume(id);
                    costume2.checkstat();
                    CostumeConeveter.HeroCostumeToLocalData(costume2, id);
                    if (costume2.uniform_type == UNIFORM_TYPE.CasualAHSS && SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
                        costume2 = HeroCostume.costume[6];
                    component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
                    if (costume2 != null)
                    {
                        component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = costume2;
                        component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = costume2.stat;
                    }
                    else
                    {
                        costume2 = HeroCostume.costumeOption[3];
                        component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = costume2;
                        component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(costume2.name.ToUpper());
                    }
                    component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
                    component.main_object.GetComponent<HERO>().setStat2();
                    component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
                }
                else
                {
                    for (int j = 0; j < HeroCostume.costume.Length; j++)
                    {
                        if (HeroCostume.costume[j].name.ToUpper() == id.ToUpper())
                        {
                            int num4 = HeroCostume.costume[j].id;
                            if (id.ToUpper() != "AHSS")
                            {
                                num4 += CheckBoxCostume.costumeSet - 1;
                            }
                            if (HeroCostume.costume[num4].name != HeroCostume.costume[j].name)
                            {
                                num4 = HeroCostume.costume[j].id + 1;
                            }
                            //convert to levi
                            if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value && id.ToUpper() == "AHSS")
                                num4 = 6; 
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[num4];
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[num4].name.ToUpper());
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
                            component.main_object.GetComponent<HERO>().setStat2();
                            component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
                            break;
                        }
                    }
                }
                CostumeConeveter.HeroCostumeToPhotonData2(component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume, PhotonNetwork.player);
                if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
                {
                    Transform transform = component.main_object.transform;
                    transform.position += new Vector3((float) UnityEngine.Random.Range(-20, 20), 2f, (float) UnityEngine.Random.Range(-20, 20));
                }
                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable.Add("dead", false);
                ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable.Add(PhotonPlayerProperty.isTitan, 1);
                propertiesToSet = hashtable;
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            }
            component.enabled = true;
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
            GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
            GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
            component.gameOver = false;
            this.isLosing = false;
            this.ShowHUDInfoCenter(string.Empty);
        }
    }

    [RPC]
    public void spawnPlayerAtRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
    {
        if (((info.sender.isMasterClient && logicLoaded) && (customLevelLoaded && !this.needChooseSide)) && Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
        {
            Vector3 position = new Vector3(posX, posY, posZ);
            IN_GAME_MAIN_CAMERA component = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>();
            component.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, new Quaternion(0f, 0f, 0f, 1f), 0), true, false);
            string slot = this.myLastHero.ToUpper();
            switch (slot)
            {
                case "SET 1":
                case "SET 2":
                case "SET 3":
                    {
                        HeroCostume costume = CostumeConeveter.LocalDataToHeroCostume(slot);
                        costume.checkstat();
                        CostumeConeveter.HeroCostumeToLocalData(costume, slot);
                        if (costume.uniform_type == UNIFORM_TYPE.CasualAHSS && SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
                            costume = HeroCostume.costume[6];
                        component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
                        if (costume != null)
                        {
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = costume;
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = costume.stat;
                        }
                        else
                        {
                            costume = HeroCostume.costumeOption[3];
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = costume;
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(costume.name.ToUpper());
                        }
                        component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
                        component.main_object.GetComponent<HERO>().setStat2();
                        component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
                        break;
                    }
                default:
                    for (int i = 0; i < HeroCostume.costume.Length; i++)
                    {
                        if (HeroCostume.costume[i].name.ToUpper() == slot.ToUpper())
                        {
                            int id = HeroCostume.costume[i].id;
                            if (slot.ToUpper() != "AHSS")
                            {
                                id += CheckBoxCostume.costumeSet - 1;
                            }
                            if (HeroCostume.costume[id].name != HeroCostume.costume[i].name)
                            {
                                id = HeroCostume.costume[i].id + 1;
                            }
                            if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value && slot.ToUpper() == "AHSS")
                                id = 6;
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[id];
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[id].name.ToUpper());
                            component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
                            component.main_object.GetComponent<HERO>().setStat2();
                            component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
                            break;
                        }
                    }
                    break;
            }
            CostumeConeveter.HeroCostumeToPhotonData2(component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume, PhotonNetwork.player);
            if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
            {
                Transform transform = component.main_object.transform;
                transform.position += new Vector3((float) UnityEngine.Random.Range(-20, 20), 2f, (float) UnityEngine.Random.Range(-20, 20));
            }
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("dead", false);
            ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.isTitan, 1);
            propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            component.enabled = true;
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
            GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
            GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
            component.gameOver = false;
            this.isLosing = false;
            this.ShowHUDInfoCenter(string.Empty);
        }
    }

    private void spawnPlayerCustomMap()
    {
        if (!this.needChooseSide && GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
        {
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
            if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
            {
                this.SpawnNonAITitan2(this.myLastHero, "titanRespawn");
            }
            else
            {
                this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
            }
            this.ShowHUDInfoCenter(string.Empty);
        }
    }

    public GameObject spawnTitan(int rate, Vector3 position, Quaternion rotation, bool punk = false)
    {
        GameObject obj3;
        GameObject obj2 = this.spawnTitanRaw(position, rotation);
        if (punk)
        {
            obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, false);
        }
        else if (UnityEngine.Random.Range(0, 100) < rate)
        {
            if (IN_GAME_MAIN_CAMERA.difficulty == 2)
            {
                if ((UnityEngine.Random.Range((float) 0f, (float) 1f) < 0.7f) || LevelInfo.getInfo(level).noCrawler)
                {
                    obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
                }
                else
                {
                    obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, false);
                }
            }
        }
        else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
        {
            if ((UnityEngine.Random.Range((float) 0f, (float) 1f) < 0.7f) || LevelInfo.getInfo(level).noCrawler)
            {
                obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
            }
            else
            {
                obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, false);
            }
        }
        else if (UnityEngine.Random.Range(0, 100) < rate)
        {
            if ((UnityEngine.Random.Range((float) 0f, (float) 1f) < 0.8f) || LevelInfo.getInfo(level).noCrawler)
            {
                obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, false);
            }
            else
            {
                obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, false);
            }
        }
        else if ((UnityEngine.Random.Range((float) 0f, (float) 1f) < 0.8f) || LevelInfo.getInfo(level).noCrawler)
        {
            obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
        }
        else
        {
            obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, false);
        }
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            obj3 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanSpawn"), obj2.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        }
        else
        {
            obj3 = PhotonNetwork.Instantiate("FX/FXtitanSpawn", obj2.transform.position, Quaternion.Euler(-90f, 0f, 0f), 0);
        }
        obj3.transform.localScale = obj2.transform.localScale;
        return obj2;
    }

    public void spawnTitanAction(int type, float size, int health, int number)
    {
        Vector3 position = new Vector3(UnityEngine.Random.Range((float) -400f, (float) 400f), 0f, UnityEngine.Random.Range((float) -400f, (float) 400f));
        Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
        if (this.titanSpawns.Count > 0)
        {
            position = this.titanSpawns[UnityEngine.Random.Range(0, this.titanSpawns.Count)];
        }
        else
        {
            GameObject[] objArray = GameObject.FindGameObjectsWithTag("titanRespawn");
            if (objArray.Length > 0)
            {
                int index = UnityEngine.Random.Range(0, objArray.Length);
                GameObject obj2 = objArray[index];
                position = obj2.transform.position;
                rotation = obj2.transform.rotation;
            }
        }
        for (int i = 0; i < number; i++)
        {
            GameObject obj3 = this.spawnTitanRaw(position, rotation);
            obj3.GetComponent<TITAN>().resetLevel(size);
            obj3.GetComponent<TITAN>().hasSetLevel = true;
            if (health > 0f)
            {
                obj3.GetComponent<TITAN>().currentHealth = health;
                obj3.GetComponent<TITAN>().maxHealth = health;
            }
            switch (type)
            {
                case 0:
                    obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, false);
                    break;

                case 1:
                    obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, false);
                    break;

                case 2:
                    obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
                    break;

                case 3:
                    obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
                    break;

                case 4:
                    obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, false);
                    break;
            }
        }
    }

    public void spawnTitanAtAction(int type, float size, int health, int number, float posX, float posY, float posZ)
    {
        Vector3 position = new Vector3(posX, posY, posZ);
        Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
        for (int i = 0; i < number; i++)
        {
            GameObject obj2 = this.spawnTitanRaw(position, rotation);
            obj2.GetComponent<TITAN>().resetLevel(size);
            obj2.GetComponent<TITAN>().hasSetLevel = true;
            if (health > 0f)
            {
                obj2.GetComponent<TITAN>().currentHealth = health;
                obj2.GetComponent<TITAN>().maxHealth = health;
            }
            switch (type)
            {
                case 0:
                    obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, false);
                    break;

                case 1:
                    obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, false);
                    break;

                case 2:
                    obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
                    break;

                case 3:
                    obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
                    break;

                case 4:
                    obj2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, false);
                    break;
            }
        }
    }

    public void spawnTitanCustom(string type, int abnormal, int rate, bool punk)
    {
        int num8;
        Vector3 position;
        Quaternion rotation;
        int num9;
        GameObject obj2;
        int moreTitans = rate;
        if (!SettingsManager.LegacyGameSettings.PunksEveryFive.Value)
            punk = false;
        if (level.StartsWith("Custom"))
        {
            moreTitans = 5;
            if (SettingsManager.LegacyGameSettings.GameType.Value == 1)
            {
                moreTitans = 3;
            }
            else if ((SettingsManager.LegacyGameSettings.GameType.Value == 2) || (SettingsManager.LegacyGameSettings.GameType.Value == 3))
            {
                moreTitans = 0;
            }
        }
        if ((SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value) || (((!SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value) && level.StartsWith("Custom")) && (SettingsManager.LegacyGameSettings.GameType.Value >= 2)))
        {
            moreTitans = SettingsManager.LegacyGameSettings.TitanNumber.Value;
            if (!SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
                moreTitans = 0;
        }
        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
        {
            if (punk)
            {
                moreTitans = rate;
            }
            else
            {
                int waveModeNum;
                if (!SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
                {
                    waveModeNum = 1;
                    if (SettingsManager.LegacyGameSettings.TitanPerWavesEnabled.Value)
                    {
                        waveModeNum = SettingsManager.LegacyGameSettings.TitanPerWaves.Value;
                    }
                    moreTitans += (this.wave - 1) * (waveModeNum - 1);
                }
                else if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
                {
                    waveModeNum = 1;
                    if (SettingsManager.LegacyGameSettings.TitanPerWavesEnabled.Value)
                    {
                        waveModeNum = SettingsManager.LegacyGameSettings.TitanPerWaves.Value;
                    }
                    moreTitans += (this.wave - 1) * waveModeNum;
                }
            }
        }
        moreTitans = Math.Min(100, moreTitans);
        if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
        {
            float nRate = SettingsManager.LegacyGameSettings.TitanSpawnNormal.Value;
            float aRate = SettingsManager.LegacyGameSettings.TitanSpawnAberrant.Value;
            float jRate = SettingsManager.LegacyGameSettings.TitanSpawnJumper.Value;
            float cRate = SettingsManager.LegacyGameSettings.TitanSpawnCrawler.Value;
            float pRate = SettingsManager.LegacyGameSettings.TitanSpawnPunk.Value;
            if (punk)
            {
                nRate = 0f;
                aRate = 0f;
                jRate = 0f;
                cRate = 0f;
                pRate = 100f;
                moreTitans = rate;
            }
            GameObject[] objArray = GameObject.FindGameObjectsWithTag("titanRespawn");
            List<GameObject> list = new List<GameObject>(objArray);
            for (num8 = 0; num8 < moreTitans; num8++)
            {
                position = new Vector3(UnityEngine.Random.Range((float)-400f, (float)400f), 0f, UnityEngine.Random.Range((float)-400f, (float)400f));
                rotation = new Quaternion(0f, 0f, 0f, 1f);
                if (this.titanSpawns.Count > 0)
                {
                    position = this.titanSpawns[UnityEngine.Random.Range(0, this.titanSpawns.Count)];
                }
                else if (objArray.Length > 0)
                {
                    if (list.Count <= 0)
                        list = new List<GameObject>(objArray);
                    int index = UnityEngine.Random.Range(0, list.Count);
                    GameObject obj = list[index];
                    position = obj.transform.position;
                    rotation = obj.transform.rotation;
                    list.RemoveAt(index);
                }
                float num10 = UnityEngine.Random.Range((float)0f, (float)100f);
                if (num10 <= ((((nRate + aRate) + jRate) + cRate) + pRate))
                {
                    GameObject obj3 = this.spawnTitanRaw(position, rotation);
                    if (num10 < nRate)
                    {
                        obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, false);
                    }
                    else if ((num10 >= nRate) && (num10 < (nRate + aRate)))
                    {
                        obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, false);
                    }
                    else if ((num10 >= (nRate + aRate)) && (num10 < ((nRate + aRate) + jRate)))
                    {
                        obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
                    }
                    else if ((num10 >= ((nRate + aRate) + jRate)) && (num10 < (((nRate + aRate) + jRate) + cRate)))
                    {
                        obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, true);
                    }
                    else if ((num10 >= (((nRate + aRate) + jRate) + cRate)) && (num10 < ((((nRate + aRate) + jRate) + cRate) + pRate)))
                    {
                        obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, false);
                    }
                    else
                    {
                        obj3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, false);
                    }
                }
                else
                {
                    this.spawnTitan(abnormal, position, rotation, punk);
                }
            }
        }
        else if (level.StartsWith("Custom"))
        {
            GameObject[] objArray = GameObject.FindGameObjectsWithTag("titanRespawn");
            List<GameObject> list = new List<GameObject>(objArray);
            for (num8 = 0; num8 < moreTitans; num8++)
            {
                position = new Vector3(UnityEngine.Random.Range((float)-400f, (float)400f), 0f, UnityEngine.Random.Range((float)-400f, (float)400f));
                rotation = new Quaternion(0f, 0f, 0f, 1f);
                if (this.titanSpawns.Count > 0)
                {
                    position = this.titanSpawns[UnityEngine.Random.Range(0, this.titanSpawns.Count)];
                }
                else if (objArray.Length > 0)
                {
                    if (list.Count <= 0)
                        list = new List<GameObject>(objArray);
                    int index = UnityEngine.Random.Range(0, list.Count);
                    GameObject obj = list[index];
                    position = obj.transform.position;
                    rotation = obj.transform.rotation;
                    list.RemoveAt(index);
                }
                this.spawnTitan(abnormal, position, rotation, punk);
            }
        }
        else
        {
            this.randomSpawnTitan("titanRespawn", abnormal, moreTitans, punk);
        }
    }

    private GameObject spawnTitanRaw(Vector3 position, Quaternion rotation)
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            return (GameObject) UnityEngine.Object.Instantiate(Resources.Load("TITAN_VER3.1"), position, rotation);
        }
        return PhotonNetwork.Instantiate("TITAN_VER3.1", position, rotation, 0);
    }

    [RPC]
    private void spawnTitanRPC(PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            foreach (TITAN titan in this.titans)
            {
                if (titan.photonView.isMine && !(PhotonNetwork.isMasterClient && !titan.nonAI))
                {
                    PhotonNetwork.Destroy(titan.gameObject);
                }
            }
            this.SpawnNonAITitan2(this.myLastHero, "titanRespawn");
        }
    }

    private void Start()
    {
        instance = this;
        base.gameObject.name = "MultiplayerManager";
        HeroCostume.init2();
        CharacterMaterials.init();
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        this.heroes = new ArrayList();
        this.eT = new ArrayList();
        this.titans = new ArrayList();
        this.fT = new ArrayList();
        this.cT = new ArrayList();
        this.hooks = new ArrayList();
        this.name = string.Empty;
        if (nameField == null)
        {
            nameField = "GUEST" + UnityEngine.Random.Range(0, 0x186a0);
        }
        if (privateServerField == null)
            privateServerField = string.Empty;
        if (privateLobbyField == null)
            privateLobbyField = string.Empty;
        usernameField = string.Empty;
        passwordField = string.Empty;
        this.resetGameSettings();
        banHash = new ExitGames.Client.Photon.Hashtable();
        imatitan = new ExitGames.Client.Photon.Hashtable();
        oldScript = string.Empty;
        currentLevel = string.Empty;
        this.titanSpawns = new List<Vector3>();
        this.playerSpawnsC = new List<Vector3>();
        this.playerSpawnsM = new List<Vector3>();
        this.playersRPC = new List<PhotonPlayer>();
        this.levelCache = new List<string[]>();
        this.titanSpawners = new List<TitanSpawner>();
        this.restartCount = new List<float>();
        ignoreList = new List<int>();
        this.groundList = new List<GameObject>();
        noRestart = false;
        masterRC = false;
        this.isSpawning = false;
        intVariables = new ExitGames.Client.Photon.Hashtable();
        heroHash = new ExitGames.Client.Photon.Hashtable();
        boolVariables = new ExitGames.Client.Photon.Hashtable();
        stringVariables = new ExitGames.Client.Photon.Hashtable();
        floatVariables = new ExitGames.Client.Photon.Hashtable();
        globalVariables = new ExitGames.Client.Photon.Hashtable();
        RCRegions = new ExitGames.Client.Photon.Hashtable();
        RCEvents = new ExitGames.Client.Photon.Hashtable();
        RCVariableNames = new ExitGames.Client.Photon.Hashtable();
        RCRegionTriggers = new ExitGames.Client.Photon.Hashtable();
        playerVariables = new ExitGames.Client.Photon.Hashtable();
        titanVariables = new ExitGames.Client.Photon.Hashtable();
        logicLoaded = false;
        customLevelLoaded = false;
        oldScriptLogic = string.Empty;
        customMapMaterials = new Dictionary<string, Material>();
        this.retryTime = 0f;
        this.playerList = string.Empty;
        this.updateTime = 0f;
        if (this.textureBackgroundBlack == null)
        {
            this.textureBackgroundBlack = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            this.textureBackgroundBlack.SetPixel(0, 0, new Color(0f, 0f, 0f, 1f));
            this.textureBackgroundBlack.Apply();
        }
        if (this.textureBackgroundBlue == null)
        {
            this.textureBackgroundBlue = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            this.textureBackgroundBlue.SetPixel(0, 0, new Color(0.08f, 0.3f, 0.4f, 1f));
            this.textureBackgroundBlue.Apply();
        }
        this.loadconfig();
        List<string> panels = new List<string> { "PanelLogin", "LOGIN", "VERSION", "LabelNetworkStatus" };
        List<string> animatedMenu = new List<string> { "AOTTG_HERO", "Colossal", "Icosphere", "Cube", "colossal", "CITY", "city", "rock" };
        if (!SettingsManager.GraphicsSettings.AnimatedIntro.Value)
            panels.AddRange(animatedMenu);
        foreach (GameObject obj2 in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            foreach (string str in panels)
            {
                if (obj2.name.Contains(str))
                    UnityEngine.Object.Destroy(obj2);
            }
        }
    }

    public void titanGetKill(PhotonPlayer player, int Damage, string name)
    {
        Damage = Mathf.Max(10, Damage);
        object[] parameters = new object[] { Damage };
        base.photonView.RPC("netShowDamage", player, parameters);
        object[] objArray2 = new object[] { name, false };
        base.photonView.RPC("oneTitanDown", PhotonTargets.MasterClient, objArray2);
        this.sendKillInfo(false, (string) player.customProperties[PhotonPlayerProperty.name], true, name, Damage);
        this.playerKillInfoUpdate(player, Damage);
    }

    public void titanGetKillbyServer(int Damage, string name)
    {
        Damage = Mathf.Max(10, Damage);
        this.sendKillInfo(false, LoginFengKAI.player.name, true, name, Damage);
        this.netShowDamage(Damage);
        this.oneTitanDown(name, false);
        this.playerKillInfoUpdate(PhotonNetwork.player, Damage);
    }

    private void tryKick(KickState tmp)
    {
        this.sendChatContentInfo(string.Concat(new object[] { "kicking #", tmp.name, ", ", tmp.getKickCount(), "/", (int) (PhotonNetwork.playerList.Length * 0.5f), "vote" }));
        if (tmp.getKickCount() >= ((int) (PhotonNetwork.playerList.Length * 0.5f)))
        {
            this.kickPhotonPlayer(tmp.name.ToString());
        }
    }

    public void unloadAssets(bool immediate = false)
    {
        if (immediate)
            Resources.UnloadUnusedAssets();
        else if (!this.isUnloading)
        {
            this.isUnloading = true;
            base.StartCoroutine(this.unloadAssetsE(10f));
        }
    }

    public IEnumerator unloadAssetsE(float time)
    {
        yield return new WaitForSeconds(time);
        Resources.UnloadUnusedAssets();
        this.isUnloading = false;
    }

    public void unloadAssetsEditor()
    {
        if (!this.isUnloading)
        {
            this.isUnloading = true;
            base.StartCoroutine(this.unloadAssetsE(30f));
        }
    }

    private void Update()
    {
        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && (GameObject.Find("LabelNetworkStatus") != null))
        {
            GameObject.Find("LabelNetworkStatus").GetComponent<UILabel>().text = PhotonNetwork.connectionStateDetailed.ToString();
            if (PhotonNetwork.connected)
            {
                UILabel component = GameObject.Find("LabelNetworkStatus").GetComponent<UILabel>();
                component.text = component.text + " ping:" + PhotonNetwork.GetPing();
            }
        }
        if (this.gameStart)
        {
            IEnumerator enumerator = this.heroes.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ((HERO) enumerator.Current).update2();
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                	disposable.Dispose();
            }
            IEnumerator enumerator2 = this.hooks.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    ((Bullet) enumerator2.Current).update();
                }
            }
            finally
            {
                IDisposable disposable2 = enumerator2 as IDisposable;
                if (disposable2 != null)
                	disposable2.Dispose();
            }
            IEnumerator enumerator3 = this.eT.GetEnumerator();
            try
            {
                while (enumerator3.MoveNext())
                {
                    ((TITAN_EREN) enumerator3.Current).update();
                }
            }
            finally
            {
                IDisposable disposable3 = enumerator3 as IDisposable;
                if (disposable3 != null)
                	disposable3.Dispose();
            }
            IEnumerator enumerator4 = this.titans.GetEnumerator();
            try
            {
                while (enumerator4.MoveNext())
                {
                    ((TITAN) enumerator4.Current).update2();
                }
            }
            finally
            {
                IDisposable disposable4 = enumerator4 as IDisposable;
                if (disposable4 != null)
                	disposable4.Dispose();
            }
            IEnumerator enumerator5 = this.fT.GetEnumerator();
            try
            {
                while (enumerator5.MoveNext())
                {
                    ((FEMALE_TITAN) enumerator5.Current).update();
                }
            }
            finally
            {
                IDisposable disposable5 = enumerator5 as IDisposable;
                if (disposable5 != null)
                	disposable5.Dispose();
            }
            IEnumerator enumerator6 = this.cT.GetEnumerator();
            try
            {
                while (enumerator6.MoveNext())
                {
                    ((COLOSSAL_TITAN) enumerator6.Current).update2();
                }
            }
            finally
            {
                IDisposable disposable6 = enumerator6 as IDisposable;
                if (disposable6 != null)
                	disposable6.Dispose();
            }
            if (this.mainCamera != null)
            {
                this.mainCamera.update2();
            }
        }
    }

    [RPC]
    private void updateKillInfo(bool t1, string killer, bool t2, string victim, int dmg)
    {
        GameObject obj4;
        GameObject obj2 = GameObject.Find("UI_IN_GAME");
        GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI/KillInfo"));
        for (int i = 0; i < this.killInfoGO.Count; i++)
        {
            obj4 = (GameObject) this.killInfoGO[i];
            if (obj4 != null)
            {
                obj4.GetComponent<KillInfoComponent>().moveOn();
            }
        }
        if (this.killInfoGO.Count > 4)
        {
            obj4 = (GameObject) this.killInfoGO[0];
            if (obj4 != null)
            {
                obj4.GetComponent<KillInfoComponent>().destory();
            }
            this.killInfoGO.RemoveAt(0);
        }
        obj3.transform.parent = obj2.GetComponent<UIReferArray>().panels[0].transform;
        obj3.GetComponent<KillInfoComponent>().show(t1, killer, t2, victim, dmg);
        this.killInfoGO.Add(obj3);
        ReportKillToChatFeed(killer, victim, dmg);
    }

    public void ReportKillToChatFeed(string killer, string victim, int damage)
    {
        if (SettingsManager.UISettings.GameFeed.Value)
        {
            string str2 = ("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> ") + killer.hexColor() + " killed ";
            string newLine = str2 + victim.hexColor() + " for " + damage.ToString() + " damage.";
            this.chatRoom.addLINE(newLine);
        }
    }

    [RPC]
    public void verifyPlayerHasLeft(int ID, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient && (PhotonPlayer.Find(ID) != null))
        {
            PhotonPlayer player = PhotonPlayer.Find(ID);
            string str = string.Empty;
            str = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
            banHash.Add(ID, str);
        }
    }

    public IEnumerator WaitAndRecompilePlayerList(float time)
    {
        int num16;
        string str2;
        int num17;
        int num18;
        int num19;
        int num20;
        object[] objArray2;
        yield return new WaitForSeconds(time);
        string iteratorVariable1 = string.Empty;
        if (SettingsManager.LegacyGameSettings.TeamMode.Value == 0)
        {
            foreach (PhotonPlayer player7 in PhotonNetwork.playerList)
            {
                if (player7.customProperties[PhotonPlayerProperty.dead] != null)
                {
                    if (ignoreList.Contains(player7.ID))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[FF0000][X] ";
                    }
                    if (player7.isLocal)
                    {
                        iteratorVariable1 = iteratorVariable1 + "[00CC00]";
                    }
                    else
                    {
                        iteratorVariable1 = iteratorVariable1 + "[FFCC00]";
                    }
                    iteratorVariable1 = iteratorVariable1 + "[" + Convert.ToString(player7.ID) + "] ";
                    if (player7.isMasterClient)
                    {
                        iteratorVariable1 = iteratorVariable1 + "[ffffff][M] ";
                    }
                    if (RCextensions.returnBoolFromObject(player7.customProperties[PhotonPlayerProperty.dead]))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_red + "] *dead* ";
                    }
                    if (RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.isTitan]) < 2)
                    {
                        num16 = RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.team]);
                        if (num16 < 2)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human + "] H ";
                        }
                        else if (num16 == 2)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human_1 + "] A ";
                        }
                    }
                    else if (RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.isTitan]) == 2)
                    {
                        iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_titan_player + "] <T> ";
                    }
                    string iteratorVariable0 = iteratorVariable1;
                    str2 = string.Empty;
                    str2 = RCextensions.returnStringFromObject(player7.customProperties[PhotonPlayerProperty.name]);
                    num17 = 0;
                    num17 = RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.kills]);
                    num18 = 0;
                    num18 = RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.deaths]);
                    num19 = 0;
                    num19 = RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.max_dmg]);
                    num20 = 0;
                    num20 = RCextensions.returnIntFromObject(player7.customProperties[PhotonPlayerProperty.total_dmg]);
                    objArray2 = new object[] { iteratorVariable0, string.Empty, str2, "[ffffff]:", num17, "/", num18, "/", num19, "/", num20 };
                    iteratorVariable1 = string.Concat(objArray2);
                    if (RCextensions.returnBoolFromObject(player7.customProperties[PhotonPlayerProperty.dead]))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[-]";
                    }
                    iteratorVariable1 = iteratorVariable1 + "\n";
                }
            }
        }
        else
        {
            int num11;
            string str;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            int num9 = 0;
            Dictionary<int, PhotonPlayer> dictionary = new Dictionary<int, PhotonPlayer>();
            Dictionary<int, PhotonPlayer> dictionary2 = new Dictionary<int, PhotonPlayer>();
            Dictionary<int, PhotonPlayer> dictionary3 = new Dictionary<int, PhotonPlayer>();
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if ((player.customProperties[PhotonPlayerProperty.dead] != null) && !ignoreList.Contains(player.ID))
                {
                    num11 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.RCteam]);
                    switch (num11)
                    {
                        case 0:
                            dictionary3.Add(player.ID, player);
                            break;

                        case 1:
                            dictionary.Add(player.ID, player);
                            num2 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.kills]);
                            num4 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.deaths]);
                            num6 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.max_dmg]);
                            num8 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.total_dmg]);
                            break;

                        case 2:
                            dictionary2.Add(player.ID, player);
                            num3 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.kills]);
                            num5 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.deaths]);
                            num7 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.max_dmg]);
                            num9 += RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.total_dmg]);
                            break;
                    }
                }
            }
            this.cyanKills = num2;
            this.magentaKills = num3;
            if (PhotonNetwork.isMasterClient)
            {
                if (SettingsManager.LegacyGameSettings.TeamMode.Value != 2)
                {
                    if (SettingsManager.LegacyGameSettings.TeamMode.Value == 3)
                    {
                        foreach (PhotonPlayer player3 in PhotonNetwork.playerList)
                        {
                            int num13 = 0;
                            num11 = RCextensions.returnIntFromObject(player3.customProperties[PhotonPlayerProperty.RCteam]);
                            if (num11 > 0)
                            {
                                switch (num11)
                                {
                                    case 1:
                                    {
                                        int num14 = 0;
                                        num14 = RCextensions.returnIntFromObject(player3.customProperties[PhotonPlayerProperty.kills]);
                                        if (((num3 + num14) + 7) < (num2 - num14))
                                        {
                                            num13 = 2;
                                            num3 += num14;
                                            num2 -= num14;
                                        }
                                        break;
                                    }
                                    case 2:
                                    {
                                        int num15 = 0;
                                        num15 = RCextensions.returnIntFromObject(player3.customProperties[PhotonPlayerProperty.kills]);
                                        if (((num2 + num15) + 7) < (num3 - num15))
                                        {
                                            num13 = 1;
                                            num2 += num15;
                                            num3 -= num15;
                                        }
                                        break;
                                    }
                                }
                                if (num13 > 0)
                                {
                                    this.photonView.RPC("setTeamRPC", player3, new object[] { num13 });
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (PhotonPlayer player2 in PhotonNetwork.playerList)
                    {
                        int num12 = 0;
                        if (dictionary.Count > (dictionary2.Count + 1))
                        {
                            num12 = 2;
                            if (dictionary.ContainsKey(player2.ID))
                            {
                                dictionary.Remove(player2.ID);
                            }
                            if (!dictionary2.ContainsKey(player2.ID))
                            {
                                dictionary2.Add(player2.ID, player2);
                            }
                        }
                        else if (dictionary2.Count > (dictionary.Count + 1))
                        {
                            num12 = 1;
                            if (!dictionary.ContainsKey(player2.ID))
                            {
                                dictionary.Add(player2.ID, player2);
                            }
                            if (dictionary2.ContainsKey(player2.ID))
                            {
                                dictionary2.Remove(player2.ID);
                            }
                        }
                        if (num12 > 0)
                        {
                            this.photonView.RPC("setTeamRPC", player2, new object[] { num12 });
                        }
                    }
                }
            }
            iteratorVariable1 = string.Concat(new object[] { iteratorVariable1, "[00FFFF]TEAM CYAN", "[ffffff]:", this.cyanKills, "/", num4, "/", num6, "/", num8, "\n" });
            foreach (PhotonPlayer player4 in dictionary.Values)
            {
                num11 = RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.RCteam]);
                if ((player4.customProperties[PhotonPlayerProperty.dead] != null) && (num11 == 1))
                {
                    if (ignoreList.Contains(player4.ID))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[FF0000][X] ";
                    }
                    if (player4.isLocal)
                    {
                        iteratorVariable1 = iteratorVariable1 + "[00CC00]";
                    }
                    else
                    {
                        iteratorVariable1 = iteratorVariable1 + "[FFCC00]";
                    }
                    iteratorVariable1 = iteratorVariable1 + "[" + Convert.ToString(player4.ID) + "] ";
                    if (player4.isMasterClient)
                    {
                        iteratorVariable1 = iteratorVariable1 + "[ffffff][M] ";
                    }
                    if (RCextensions.returnBoolFromObject(player4.customProperties[PhotonPlayerProperty.dead]))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_red + "] *dead* ";
                    }
                    if (RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.isTitan]) < 2)
                    {
                        num16 = RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.team]);
                        if (num16 < 2)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human + "] H ";
                        }
                        else if (num16 == 2)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human_1 + "] A ";
                        }
                    }
                    else if (RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.isTitan]) == 2)
                    {
                        iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_titan_player + "] <T> ";
                    }
                    str = iteratorVariable1;
                    str2 = string.Empty;
                    str2 = RCextensions.returnStringFromObject(player4.customProperties[PhotonPlayerProperty.name]);
                    num17 = 0;
                    num17 = RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.kills]);
                    num18 = 0;
                    num18 = RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.deaths]);
                    num19 = 0;
                    num19 = RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.max_dmg]);
                    num20 = 0;
                    num20 = RCextensions.returnIntFromObject(player4.customProperties[PhotonPlayerProperty.total_dmg]);
                    iteratorVariable1 = string.Concat(new object[] { str, string.Empty, str2, "[ffffff]:", num17, "/", num18, "/", num19, "/", num20 });
                    if (RCextensions.returnBoolFromObject(player4.customProperties[PhotonPlayerProperty.dead]))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[-]";
                    }
                    iteratorVariable1 = iteratorVariable1 + "\n";
                }
            }
            iteratorVariable1 = string.Concat(new object[] { iteratorVariable1, " \n", "[FF00FF]TEAM MAGENTA", "[ffffff]:", this.magentaKills, "/", num5, "/", num7, "/", num9, "\n" });
            foreach (PhotonPlayer player5 in dictionary2.Values)
            {
                num11 = RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.RCteam]);
                if ((player5.customProperties[PhotonPlayerProperty.dead] != null) && (num11 == 2))
                {
                    if (ignoreList.Contains(player5.ID))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[FF0000][X] ";
                    }
                    if (player5.isLocal)
                    {
                        iteratorVariable1 = iteratorVariable1 + "[00CC00]";
                    }
                    else
                    {
                        iteratorVariable1 = iteratorVariable1 + "[FFCC00]";
                    }
                    iteratorVariable1 = iteratorVariable1 + "[" + Convert.ToString(player5.ID) + "] ";
                    if (player5.isMasterClient)
                    {
                        iteratorVariable1 = iteratorVariable1 + "[ffffff][M] ";
                    }
                    if (RCextensions.returnBoolFromObject(player5.customProperties[PhotonPlayerProperty.dead]))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_red + "] *dead* ";
                    }
                    if (RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.isTitan]) < 2)
                    {
                        num16 = RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.team]);
                        if (num16 < 2)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human + "] H ";
                        }
                        else if (num16 == 2)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human_1 + "] A ";
                        }
                    }
                    else if (RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.isTitan]) == 2)
                    {
                        iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_titan_player + "] <T> ";
                    }
                    str = iteratorVariable1;
                    str2 = string.Empty;
                    str2 = RCextensions.returnStringFromObject(player5.customProperties[PhotonPlayerProperty.name]);
                    num17 = 0;
                    num17 = RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.kills]);
                    num18 = 0;
                    num18 = RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.deaths]);
                    num19 = 0;
                    num19 = RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.max_dmg]);
                    num20 = 0;
                    num20 = RCextensions.returnIntFromObject(player5.customProperties[PhotonPlayerProperty.total_dmg]);
                    iteratorVariable1 = string.Concat(new object[] { str, string.Empty, str2, "[ffffff]:", num17, "/", num18, "/", num19, "/", num20 });
                    if (RCextensions.returnBoolFromObject(player5.customProperties[PhotonPlayerProperty.dead]))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[-]";
                    }
                    iteratorVariable1 = iteratorVariable1 + "\n";
                }
            }
            iteratorVariable1 = string.Concat(new object[] { iteratorVariable1, " \n", "[00FF00]INDIVIDUAL\n" });
            foreach (PhotonPlayer player6 in dictionary3.Values)
            {
                num11 = RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.RCteam]);
                if ((player6.customProperties[PhotonPlayerProperty.dead] != null) && (num11 == 0))
                {
                    if (ignoreList.Contains(player6.ID))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[FF0000][X] ";
                    }
                    if (player6.isLocal)
                    {
                        iteratorVariable1 = iteratorVariable1 + "[00CC00]";
                    }
                    else
                    {
                        iteratorVariable1 = iteratorVariable1 + "[FFCC00]";
                    }
                    iteratorVariable1 = iteratorVariable1 + "[" + Convert.ToString(player6.ID) + "] ";
                    if (player6.isMasterClient)
                    {
                        iteratorVariable1 = iteratorVariable1 + "[ffffff][M] ";
                    }
                    if (RCextensions.returnBoolFromObject(player6.customProperties[PhotonPlayerProperty.dead]))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_red + "] *dead* ";
                    }
                    if (RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.isTitan]) < 2)
                    {
                        num16 = RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.team]);
                        if (num16 < 2)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human + "] H ";
                        }
                        else if (num16 == 2)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human_1 + "] A ";
                        }
                    }
                    else if (RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.isTitan]) == 2)
                    {
                        iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_titan_player + "] <T> ";
                    }
                    str = iteratorVariable1;
                    str2 = string.Empty;
                    str2 = RCextensions.returnStringFromObject(player6.customProperties[PhotonPlayerProperty.name]);
                    num17 = 0;
                    num17 = RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.kills]);
                    num18 = 0;
                    num18 = RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.deaths]);
                    num19 = 0;
                    num19 = RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.max_dmg]);
                    num20 = 0;
                    num20 = RCextensions.returnIntFromObject(player6.customProperties[PhotonPlayerProperty.total_dmg]);
                    iteratorVariable1 = string.Concat(new object[] { str, string.Empty, str2, "[ffffff]:", num17, "/", num18, "/", num19, "/", num20 });
                    if (RCextensions.returnBoolFromObject(player6.customProperties[PhotonPlayerProperty.dead]))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[-]";
                    }
                    iteratorVariable1 = iteratorVariable1 + "\n";
                }
            }
        }
        this.playerList = iteratorVariable1;
        if (PhotonNetwork.isMasterClient && ((!this.isWinning && !this.isLosing) && (this.roundTime >= 5f)))
        {
            int num22;
            if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
            {
                int num21 = 0;
                for (num22 = 0; num22 < PhotonNetwork.playerList.Length; num22++)
                {
                    PhotonPlayer targetPlayer = PhotonNetwork.playerList[num22];
                    if ((!ignoreList.Contains(targetPlayer.ID) && (targetPlayer.customProperties[PhotonPlayerProperty.dead] != null)) && (targetPlayer.customProperties[PhotonPlayerProperty.isTitan] != null))
                    {
                        if (RCextensions.returnIntFromObject(targetPlayer.customProperties[PhotonPlayerProperty.isTitan]) == 1)
                        {
                            if (RCextensions.returnBoolFromObject(targetPlayer.customProperties[PhotonPlayerProperty.dead]) && (RCextensions.returnIntFromObject(targetPlayer.customProperties[PhotonPlayerProperty.deaths]) > 0))
                            {
                                if (!imatitan.ContainsKey(targetPlayer.ID))
                                {
                                    imatitan.Add(targetPlayer.ID, 2);
                                }
                                ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                                propertiesToSet.Add(PhotonPlayerProperty.isTitan, 2);
                                targetPlayer.SetCustomProperties(propertiesToSet);
                                this.photonView.RPC("spawnTitanRPC", targetPlayer, new object[0]);
                            }
                            else if (imatitan.ContainsKey(targetPlayer.ID))
                            {
                                for (int j = 0; j < this.heroes.Count; j++)
                                {
                                    HERO hero = (HERO) this.heroes[j];
                                    if (hero.photonView.owner == targetPlayer)
                                    {
                                        hero.markDie();
                                        hero.photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "no switching in infection" });
                                    }
                                }
                            }
                        }
                        else if (!((RCextensions.returnIntFromObject(targetPlayer.customProperties[PhotonPlayerProperty.isTitan]) != 2) || RCextensions.returnBoolFromObject(targetPlayer.customProperties[PhotonPlayerProperty.dead])))
                        {
                            num21++;
                        }
                    }
                }
                if ((num21 <= 0) && (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.KILL_TITAN))
                {
                    this.gameWin2();
                }
            }
            else if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
            {
                if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
                {
                    if (this.cyanKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
                    {
                        object[] parameters = new object[] { "<color=#00FFFF>Team Cyan wins! </color>", string.Empty };
                        this.photonView.RPC("Chat", PhotonTargets.All, parameters);
                        this.gameWin2();
                    }
                    else if (this.magentaKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
                    {
                        objArray2 = new object[] { "<color=#FF00FF>Team Magenta wins! </color>", string.Empty };
                        this.photonView.RPC("Chat", PhotonTargets.All, objArray2);
                        this.gameWin2();
                    }
                }
                else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 0)
                {
                    for (num22 = 0; num22 < PhotonNetwork.playerList.Length; num22++)
                    {
                        PhotonPlayer player9 = PhotonNetwork.playerList[num22];
                        if (RCextensions.returnIntFromObject(player9.customProperties[PhotonPlayerProperty.kills]) >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
                        {
                            object[] objArray4 = new object[] { "<color=#FFCC00>" + RCextensions.returnStringFromObject(player9.customProperties[PhotonPlayerProperty.name]).hexColor() + " wins!</color>", string.Empty };
                            this.photonView.RPC("Chat", PhotonTargets.All, objArray4);
                            this.gameWin2();
                        }
                    }
                }
            }
            else if ((!SettingsManager.LegacyGameSettings.PointModeEnabled.Value) && ((SettingsManager.LegacyGameSettings.BombModeEnabled.Value) || (SettingsManager.LegacyGameSettings.BladePVP.Value > 0)))
            {
                if ((SettingsManager.LegacyGameSettings.TeamMode.Value > 0) && (PhotonNetwork.playerList.Length > 1))
                {
                    int num24 = 0;
                    int num25 = 0;
                    int num26 = 0;
                    int num27 = 0;
                    for (num22 = 0; num22 < PhotonNetwork.playerList.Length; num22++)
                    {
                        PhotonPlayer player10 = PhotonNetwork.playerList[num22];
                        if ((!ignoreList.Contains(player10.ID) && (player10.customProperties[PhotonPlayerProperty.RCteam] != null)) && (player10.customProperties[PhotonPlayerProperty.dead] != null))
                        {
                            if (RCextensions.returnIntFromObject(player10.customProperties[PhotonPlayerProperty.RCteam]) == 1)
                            {
                                num26++;
                                if (!RCextensions.returnBoolFromObject(player10.customProperties[PhotonPlayerProperty.dead]))
                                {
                                    num24++;
                                }
                            }
                            else if (RCextensions.returnIntFromObject(player10.customProperties[PhotonPlayerProperty.RCteam]) == 2)
                            {
                                num27++;
                                if (!RCextensions.returnBoolFromObject(player10.customProperties[PhotonPlayerProperty.dead]))
                                {
                                    num25++;
                                }
                            }
                        }
                    }
                    if ((num26 > 0) && (num27 > 0))
                    {
                        if (num24 == 0)
                        {
                            object[] objArray5 = new object[] { "<color=#FF00FF>Team Magenta wins! </color>", string.Empty };
                            this.photonView.RPC("Chat", PhotonTargets.All, objArray5);
                            this.gameWin2();
                        }
                        else if (num25 == 0)
                        {
                            object[] objArray6 = new object[] { "<color=#00FFFF>Team Cyan wins! </color>", string.Empty };
                            this.photonView.RPC("Chat", PhotonTargets.All, objArray6);
                            this.gameWin2();
                        }
                    }
                }
                else if ((SettingsManager.LegacyGameSettings.TeamMode.Value == 0) && (PhotonNetwork.playerList.Length > 1))
                {
                    int num28 = 0;
                    string text = "Nobody";
                    PhotonPlayer player11 = PhotonNetwork.playerList[0];
                    for (num22 = 0; num22 < PhotonNetwork.playerList.Length; num22++)
                    {
                        PhotonPlayer player12 = PhotonNetwork.playerList[num22];
                        if (!((player12.customProperties[PhotonPlayerProperty.dead] == null) || RCextensions.returnBoolFromObject(player12.customProperties[PhotonPlayerProperty.dead])))
                        {
                            text = RCextensions.returnStringFromObject(player12.customProperties[PhotonPlayerProperty.name]).hexColor();
                            player11 = player12;
                            num28++;
                        }
                    }
                    if (num28 <= 1)
                    {
                        string str4 = " 5 points added.";
                        if (text == "Nobody")
                        {
                            str4 = string.Empty;
                        }
                        else
                        {
                            for (num22 = 0; num22 < 5; num22++)
                            {
                                this.playerKillInfoUpdate(player11, 0);
                            }
                        }
                        object[] objArray7 = new object[] { "<color=#FFCC00>" + text.hexColor() + " wins." + str4 + "</color>", string.Empty };
                        this.photonView.RPC("Chat", PhotonTargets.All, objArray7);
                        this.gameWin2();
                    }
                }
            }
        }
        this.isRecompiling = false;
    }

    public IEnumerator WaitAndReloadKDR(PhotonPlayer player)
    {
        yield return new WaitForSeconds(5f);
        string key = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
        if (this.PreservedPlayerKDR.ContainsKey(key))
        {
            int[] numArray = this.PreservedPlayerKDR[key];
            this.PreservedPlayerKDR.Remove(key);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.kills, numArray[0]);
            propertiesToSet.Add(PhotonPlayerProperty.deaths, numArray[1]);
            propertiesToSet.Add(PhotonPlayerProperty.max_dmg, numArray[2]);
            propertiesToSet.Add(PhotonPlayerProperty.total_dmg, numArray[3]);
            player.SetCustomProperties(propertiesToSet);
        }
    }

    public IEnumerator WaitAndResetRestarts()
    {
        yield return new WaitForSeconds(10f);
        this.restartingBomb = false;
        this.restartingEren = false;
        this.restartingHorse = false;
        this.restartingMC = false;
        this.restartingTitan = false;
    }

    public IEnumerator WaitAndRespawn1(float time, string str)
    {
        yield return new WaitForSeconds(time);
        this.SpawnPlayer(this.myLastHero, str);
    }

    public IEnumerator WaitAndRespawn2(float time, GameObject pos)
    {
        yield return new WaitForSeconds(time);
        this.SpawnPlayerAt2(this.myLastHero, pos);
    }



    private enum LoginStates
    {
        notlogged,
        loggingin,
        loginfailed,
        loggedin
    }
}

