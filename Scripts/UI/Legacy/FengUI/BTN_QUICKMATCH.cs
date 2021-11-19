using Settings;
using System;
using UnityEngine;

public class BTN_QUICKMATCH : MonoBehaviour
{
    private void OnClick()
    {
        SettingsManager.MultiplayerSettings.ConnectOffline();
    }
}