using System;
using UnityEngine;
using Settings;

public class BTN_ServerUS : MonoBehaviour
{
    private void OnClick()
    {
        SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.US);
    }
}
