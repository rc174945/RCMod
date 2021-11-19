using System;
using UnityEngine;
using Settings;

public class BTN_Server_EU : MonoBehaviour
{
    private void OnClick()
    {
        SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.EU);
    }
}

