using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Settings;
using GameManagers;

namespace UI
{
    class GameMenu: BaseMenu
    {
        public static Dictionary<string, Texture2D> EmojiTextures = new Dictionary<string, Texture2D>();
        public static List<string> AvailableEmojis = new List<string>() { "Smile", "ThumbsUp", "Cool", "Love", "Shocked", "Crying", "Annoyed", "Angry" };
        public static List<string> AvailableText = new List<string>() { "Help", "Thanks", "Sorry", "Titan here", "Good game", "Nice hit", "Oops", "Welcome" };
        public static List<string> AvailableActions = new List<string>() { "Salute", "Dance", "Flip", "Wave1", "Wave2", "Eat"};
        const float EmoteCooldown = 4f;
        public static bool Paused;
        public static bool WheelMenu;
        public static bool HideCrosshair;
        public List<BasePopup> _emoteTextPopups = new List<BasePopup>();
        public List<BasePopup> _emoteEmojiPopups = new List<BasePopup>();
        public BasePopup _settingsPopup;
        public BasePopup _emoteWheelPopup;
        public BasePopup _itemWheelPopup;
        public RawImage _crosshairImageWhite;
        public RawImage _crosshairImageRed;
        public Text _crosshairLabelWhite;
        public Text _crosshairLabelRed;
        private float _currentEmoteCooldown = 0f;
        private EmoteWheelState _currentEmoteWheelState = EmoteWheelState.Text;

        public override void Setup()
        {
            base.Setup();
            HideCrosshair = false;
            TogglePause(false);
            WheelMenu = false;
            SetupCrosshairs();
        }

        public static bool InMenu()
        {
            return Paused || WheelMenu;
        }

        public static void TogglePause(bool pause)
        {
            Paused = pause;
            if (UIManager.CurrentMenu != null && UIManager.CurrentMenu.GetComponent<GameMenu>() != null)
            {
                GameMenu menu = UIManager.CurrentMenu.GetComponent<GameMenu>();
                if (Paused && !menu._settingsPopup.gameObject.activeSelf)
                {
                    menu._settingsPopup.Show();
                    menu._emoteWheelPopup.Hide();
                    WheelMenu = false;
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        Time.timeScale = 0f;
                    }
                }
                else
                {
                    Paused = false;
                    menu._settingsPopup.Hide();
                    if (!Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled)
                    {
                        Camera.main.GetComponent<SpectatorMovement>().disable = false;
                        Camera.main.GetComponent<MouseLook>().disable = false;
                    }
                }
            }
            if (!Paused && FengGameManagerMKII.instance.pauseWaitTime <= 0f)
                Time.timeScale = 1f;
        }

        public static void OnEmoteTextRPC(int viewId, string text, PhotonMessageInfo info)
        {
            if (UIManager.CurrentMenu == null || !SettingsManager.UISettings.ShowEmotes.Value)
                return;
            GameMenu menu = UIManager.CurrentMenu.GetComponent<GameMenu>();
            Transform t = GetTransformFromViewId(viewId, info);
            if (t != null && menu != null)
                menu.ShowEmoteText(text, t);
        }

        public static void OnEmoteEmojiRPC(int viewId, string emoji, PhotonMessageInfo info)
        {
            if (UIManager.CurrentMenu == null || !SettingsManager.UISettings.ShowEmotes.Value)
                return;
            GameMenu menu = UIManager.CurrentMenu.GetComponent<GameMenu>();
            Transform t = GetTransformFromViewId(viewId, info);
            if (t != null && menu != null)
                menu.ShowEmoteEmoji(emoji, t);
        }

        public static void ToggleEmoteWheel(bool enable)
        {
            if (UIManager.CurrentMenu != null && UIManager.CurrentMenu.GetComponent<GameMenu>() != null)
            {
                GameMenu menu = UIManager.CurrentMenu.GetComponent<GameMenu>();
                if (enable)
                {
                    ((WheelPopup)menu._emoteWheelPopup).Show(SettingsManager.InputSettings.Interaction.EmoteMenu.ToString(),
                        GetEmoteWheelOptions(menu._currentEmoteWheelState), () => menu.OnEmoteWheelSelect());
                    WheelMenu = true;
                }
                else
                {
                    menu._emoteWheelPopup.Hide();
                    WheelMenu = false;
                }
            }
        }

        public static void NextEmoteWheel()
        {
            if (UIManager.CurrentMenu != null && UIManager.CurrentMenu.GetComponent<GameMenu>() != null)
            {
                GameMenu menu = UIManager.CurrentMenu.GetComponent<GameMenu>();
                if (!menu._emoteWheelPopup.gameObject.activeSelf || !WheelMenu)
                    return;
                menu._currentEmoteWheelState++;
                if (menu._currentEmoteWheelState > EmoteWheelState.Action)
                    menu._currentEmoteWheelState = 0;
                ((WheelPopup)menu._emoteWheelPopup).Show(SettingsManager.InputSettings.Interaction.EmoteMenu.ToString(),
                        GetEmoteWheelOptions(menu._currentEmoteWheelState), () => menu.OnEmoteWheelSelect());
            }
        }

        public void ShowEmoteText(string text, Transform parent)
        {
            EmoteTextPopup popup = (EmoteTextPopup)GetAvailablePopup(_emoteTextPopups);
            if (text.Length > 20)
                text = text.Substring(0, 20);
            popup.Show(text, parent);
        }

        public void ShowEmoteEmoji(string emoji, Transform parent)
        {
            EmoteEmojiPopup popup = (EmoteEmojiPopup)GetAvailablePopup(_emoteEmojiPopups);
            popup.Show(emoji, parent);
        }

        private void OnEmoteWheelSelect()
        {
            if (_currentEmoteWheelState != EmoteWheelState.Action)
            {
                if (_currentEmoteCooldown > 0f)
                    return;
                _currentEmoteCooldown = EmoteCooldown;
            }
            HERO hero = RCextensions.GetMyHero();
            if (hero == null)
                return;
            if (_currentEmoteWheelState == EmoteWheelState.Text)
            {
                string text = AvailableText[((WheelPopup)_emoteWheelPopup).SelectedItem];
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    ShowEmoteText(text, hero.transform);
                else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                    CustomRPCManager.PhotonView.RPC("EmoteTextRPC", PhotonTargets.All, new object[] { hero.photonView.viewID, text });
            }
            else if (_currentEmoteWheelState == EmoteWheelState.Emoji)
            {
                string emoji = AvailableEmojis[((WheelPopup)_emoteWheelPopup).SelectedItem];
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    ShowEmoteEmoji(emoji, hero.transform);
                else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                    CustomRPCManager.PhotonView.RPC("EmoteEmojiRPC", PhotonTargets.All, new object[] { hero.photonView.viewID, emoji });
            }
            else if (_currentEmoteWheelState == EmoteWheelState.Action)
            {
                string action = AvailableActions[((WheelPopup)_emoteWheelPopup).SelectedItem];
                if (action == "Salute")
                    hero.EmoteAction("salute");
                else if (action == "Dance")
                    hero.EmoteAction("special_armin");
                else if (action == "Flip")
                    hero.EmoteAction("dodge");
                else if (action == "Wave1")
                    hero.EmoteAction("special_marco_0");
                else if (action == "Wave2")
                    hero.EmoteAction("special_marco_1");
                else if (action == "Eat")
                    hero.EmoteAction("special_sasha");
            }
            hero._flareDelayAfterEmote = 2f;
            _emoteWheelPopup.Hide();
            WheelMenu = false;
        }

        private static Transform GetTransformFromViewId(int viewId, PhotonMessageInfo info)
        {
            PhotonView view = PhotonView.Find(viewId);
            if (view != null && view.owner == info.sender)
            {
                return view.transform;
            }
            return null;
        }

        private static List<string> GetEmoteWheelOptions(EmoteWheelState state)
        {
            if (state == EmoteWheelState.Text)
                return AvailableText;
            else if (state == EmoteWheelState.Emoji)
                return AvailableEmojis;
            return AvailableActions;
        }

        private BasePopup GetAvailablePopup(List<BasePopup> popups)
        {
            foreach (BasePopup popup in popups)
            {
                if (!popup.gameObject.activeSelf)
                    return popup;
            }
            return popups[0];
        }

        protected void SetupCrosshairs()
        {
            _crosshairImageWhite = ElementFactory.InstantiateAndBind(transform, "CrosshairImage").GetComponent<RawImage>();
            _crosshairImageRed = ElementFactory.InstantiateAndBind(transform, "CrosshairImage").GetComponent<RawImage>();
            _crosshairImageRed.color = Color.red;
            _crosshairLabelWhite = _crosshairImageWhite.transform.Find("DefaultLabel").GetComponent<Text>();
            _crosshairLabelRed = _crosshairImageRed.transform.Find("DefaultLabel").GetComponent<Text>();
            ElementFactory.SetAnchor(_crosshairImageWhite.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, Vector2.zero);
            ElementFactory.SetAnchor(_crosshairImageRed.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, Vector2.zero);
            _crosshairImageWhite.gameObject.AddComponent<CrosshairScaler>();
            _crosshairImageRed.gameObject.AddComponent<CrosshairScaler>();
            CursorManager.UpdateCrosshair(_crosshairImageWhite, _crosshairImageRed, _crosshairLabelWhite, _crosshairLabelRed, true);
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
            _settingsPopup = ElementFactory.CreateHeadedPanel<SettingsPopup>(transform).GetComponent<BasePopup>();
            _emoteWheelPopup = ElementFactory.InstantiateAndSetupPanel<WheelPopup>(transform, "WheelMenu").GetComponent<BasePopup>();
            for (int i = 0; i < 5; i++)
            {
                BasePopup emoteTextPopup = ElementFactory.InstantiateAndSetupPanel<EmoteTextPopup>(transform, "EmoteTextPopup").GetComponent<BasePopup>();
                _emoteTextPopups.Add(emoteTextPopup);
                BasePopup emoteEmojiPopup = ElementFactory.InstantiateAndSetupPanel<EmoteEmojiPopup>(transform, "EmoteEmojiPopup").GetComponent<BasePopup>();
                _emoteEmojiPopups.Add(emoteEmojiPopup);
            }
            _popups.Add(_settingsPopup);
            _popups.Add(_emoteWheelPopup);
        }
        private void Update()
        {
            CursorManager.UpdateCrosshair(_crosshairImageWhite, _crosshairImageRed, _crosshairLabelWhite, _crosshairLabelRed);
            _currentEmoteCooldown -= Time.deltaTime;
        }
    }

    public enum EmoteWheelState
    {
        Text,
        Emoji,
        Action
    }
}
