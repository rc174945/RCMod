using System;
using UnityEngine;
using Settings;

public class BTN_Server_SA : MonoBehaviour
{
    private void OnClick()
    {
        SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.SA);
    }
}
