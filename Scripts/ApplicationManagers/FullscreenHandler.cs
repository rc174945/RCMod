using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using Utility;
using Settings;
using UI;

namespace ApplicationManagers
{
    class FullscreenHandler : MonoBehaviour
    {
        static FullscreenHandler _instance;
        static bool _exclusiveFullscreen;
        static bool _fullscreen;

        // windows minimize functions
        [DllImport("user32.dll", EntryPoint = "GetActiveWindow")]
        private static extern int GetActiveWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(int hWnd, int nCmdShow);

        // consts
        static int WindowedWidth;
        static int WindowedHeight;
        static int FullscreenWidth;
        static int FullscreenHeight;
        static readonly string RootPath = Application.dataPath + "/FullscreenFix";
        static readonly string BorderlessPath = RootPath + "/mainDataBorderless";
        static readonly string ExclusivePath = RootPath + "/mainDataExclusive";
        static readonly string MainDataPath = Application.dataPath + "/mainData";

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            _fullscreen = Screen.fullScreen;
            _exclusiveFullscreen = SettingsManager.GraphicsSettings.ExclusiveFullscreen.Value;
            if (_fullscreen)
            {
                WindowedWidth = 960;
                WindowedHeight = 600;
                FullscreenWidth = Screen.width;
                FullscreenHeight = Screen.height;
            }
            else
            {
                WindowedWidth = Screen.width;
                WindowedHeight = Screen.height;
                FullscreenWidth = Screen.currentResolution.width;
                FullscreenHeight = Screen.currentResolution.height;
            }
        }

        public static void ToggleFullscreen()
        {
            SetFullscreen(!_fullscreen);
            _fullscreen = !_fullscreen;
        }

        static void SetFullscreen(bool fullscreen)
        {
            bool change = fullscreen != Screen.fullScreen;
            if (fullscreen && !Screen.fullScreen)
                Screen.SetResolution(FullscreenWidth, FullscreenHeight, true);
            else if (!fullscreen && Screen.fullScreen)
                Screen.SetResolution(WindowedWidth, WindowedHeight, false);
            if (change)
            {
                _instance.StartCoroutine(_instance.WaitAndRefreshHUD());
                CursorManager.RefreshCursorLock();
                if (UIManager.CurrentMenu != null)
                    UIManager.CurrentMenu.ApplyScale();
            }
        }

        public void OnApplicationFocus(bool hasFocus)
        {
            if (!Supported())
                return;
            if (_exclusiveFullscreen)
            {
                if (hasFocus)
                    SetFullscreen(_fullscreen);
                else
                {
                    // need to manually minimize when alt-tabbing on exclusive-fullscreen
                    SetFullscreen(false);
                    int handle = GetActiveWindow();
                    ShowWindow(handle, 2);
                }
            }
            else
            {
                // need to refresh minimap when alt-tabbing on borderless-fullscreen
                if (hasFocus)
                    _instance.StartCoroutine(WaitAndRefreshMinimap());
            }
            CursorManager.RefreshCursorLock();
        }

        IEnumerator WaitAndRefreshHUD()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            IN_GAME_MAIN_CAMERA.needSetHUD = true;
            Minimap.OnScreenResolutionChanged();
            GameObject stylish = GameObject.Find("Stylish");
            if (stylish != null)
                stylish.GetComponent<StylishComponent>().OnResolutionChange();
        }

        IEnumerator WaitAndRefreshMinimap()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            Minimap.OnScreenResolutionChanged();
        }

        public static void SetMainData(bool trueFullscreen)
        {
            if (!Supported())
                return;
            try
            {
                if (trueFullscreen)
                    File.Copy(ExclusivePath, MainDataPath, true);
                else
                    File.Copy(BorderlessPath, MainDataPath, true);
            }
            catch (Exception ex)
            {
                Debug.Log("FullscreenHandler error setting main data: " + ex.Message);
            }
        }
        static bool Supported()
        {
            return Application.platform == RuntimePlatform.WindowsPlayer;
        }
    }
}