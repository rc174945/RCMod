using System;
using UnityEngine;
using Settings;

public class BTN_Server_ASIA : MonoBehaviour
{
    private void OnClick()
    {
        SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.ASIA);
    }
}
