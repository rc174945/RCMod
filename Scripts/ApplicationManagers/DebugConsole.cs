using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using Utility;
using System.Diagnostics;

namespace ApplicationManagers
{
    class DebugConsole : MonoBehaviour
    {
        static DebugConsole _instance;
        static bool _enabled;
        static LinkedList<string> _messages = new LinkedList<string>();
        static int _currentCharCount = 0;
        static Vector2 _scrollPosition = Vector2.zero;
        static string _inputLine = string.Empty;
        static bool _needResetScroll;
        const int MaxMessages = 100;
        const int MaxChars = 5000;
        const int PositionX = 20;
        const int PositionY = 20;
        const int Width = 400;
        const int Height = 300;
        const int InputHeight = 25;
        const int Padding = 10;
        const string InputControlName = "DebugInput";
        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            Application.RegisterLogCallback(OnUnityDebugLog);
        }

        static void OnUnityDebugLog(string log, string stackTrace, LogType type)
        {
            AddMessage(stackTrace);
            AddMessage(log);
        }

        static void AddMessage(string message)
        {
            _messages.AddLast(message);
            _currentCharCount += message.Length;
            while (_messages.Count > MaxMessages || _currentCharCount > MaxChars)
            {
                _currentCharCount -= _messages.First.Value.Length;
                _messages.RemoveFirst();
            }
            _needResetScroll = true;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
                _enabled = !_enabled;
        }

        void OnGUI()
        {
            if (_enabled)
            {
                // draw debug console over everything else
                GUI.depth = 1;
                GUI.Box(new Rect(PositionX, PositionY, Width, Height), "");
                DrawMessageWindow();
                DrawInputWindow();
                HandleInput();
                GUI.depth = 0;
            }
        }

        static void DrawMessageWindow()
        {
            int positionX = PositionX + Padding;
            int positionY = PositionY + Padding;
            int width = Width - Padding * 2;
            GUI.Label(new Rect(positionX, positionY, width, InputHeight), "Debug Console (Press F11 to hide)");
            positionY += InputHeight + Padding;
            int scrollViewHeight = Height - Padding * 4 - InputHeight * 2;
            GUIStyle style = new GUIStyle(GUI.skin.box);
            string text = "";
            foreach (string message in _messages)
                text += message + "\n";
            int textWidth = width - Padding * 2;
            int height = (int)style.CalcHeight(new GUIContent(text), textWidth) + Padding;
            _scrollPosition = GUI.BeginScrollView(new Rect(positionX, positionY, width, scrollViewHeight), _scrollPosition,
                new Rect(positionX, positionY, textWidth, height));
            GUI.Label(new Rect(positionX, positionY, textWidth, height), text);
            if (_needResetScroll)
            {
                _needResetScroll = false;
                _scrollPosition = new Vector2(0f, height);
            }
            GUI.EndScrollView();
        }

        static void DrawInputWindow()
        {
            int y = PositionY + Height - InputHeight - Padding;
            GUI.SetNextControlName(InputControlName);
            _inputLine = GUI.TextField(new Rect(PositionX + Padding, y, Width - Padding * 2, InputHeight), _inputLine);
        }

        static void HandleInput()
        {
            if (GUI.GetNameOfFocusedControl() == InputControlName)
            {
                if (IsEnterUp())
                {
                    if (_inputLine != string.Empty)
                    {
                        UnityEngine.Debug.Log(_inputLine);
                        if (_inputLine.StartsWith("/"))
                            DebugTesting.RunDebugCommand(_inputLine.Substring(1));
                        else
                            UnityEngine.Debug.Log("Invalid debug command.");
                        _inputLine = string.Empty;
                    }
                    GUI.FocusControl(string.Empty);
                }
            }
            else if (IsEnterUp())
            {
                GUI.FocusControl(InputControlName);
            }
        }

        static bool IsEnterUp()
        {
            return Event.current.type == EventType.keyUp && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter);
        }
    }
}
