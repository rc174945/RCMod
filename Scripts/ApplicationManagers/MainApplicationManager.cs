using UnityEngine;
using Utility;
using Settings;
using UI;
using Weather;
using System.Collections;
using GameProgress;
using Map;

namespace ApplicationManagers
{
    class MainApplicationManager : MonoBehaviour
    {
        static bool _firstLaunch = true;
        static MainApplicationManager _instance;

        public static void Init()
        {
            if (_firstLaunch)
            {
                _firstLaunch = false;
                _instance = SingletonFactory.CreateSingleton(_instance);
                ApplicationStart();
            }
            else if (AssetBundleManager.Status == AssetBundleStatus.Ready)
            {
                UIManager.SetMenu(MenuType.Main);
                GameProgressManager.OnMainMenu();
            }
            IN_GAME_MAIN_CAMERA.ApplyGraphicsSettings();
        }

        static void ApplicationStart()
        {
            // set legacy feng locale to english
            Language.init();
            Language.type = 0;

            // initialize other app managers
            DebugConsole.Init();
            ApplicationConfig.Init();
            AutoUpdateManager.Init();
            LevelInfo.Init();
            SettingsManager.Init();
            FullscreenHandler.Init();
            UIManager.Init();
            AssetBundleManager.Init();
            SnapshotManager.Init();
            CursorManager.Init();
            WeatherManager.Init();
            GameProgressManager.Init();
            MapManager.Init();
            if (ApplicationConfig.DevelopmentMode)
            {
                DebugTesting.Init();
                DebugTesting.RunTests();
            }
        }

        public static void FinishLoadAssets()
        {
            GameProgressManager.FinishLoadAssets();
            UIManager.FinishLoadAssets();
            CursorManager.FinishLoadAssets();
            WeatherManager.FinishLoadAssets();
        }
    }
}