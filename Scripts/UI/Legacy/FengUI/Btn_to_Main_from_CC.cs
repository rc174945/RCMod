using System;
using UnityEngine;

public class Btn_to_Main_from_CC : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();
        Screen.lockCursor = false;
        Screen.showCursor = true;
        IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
        UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
        Application.LoadLevel("menu");
    }
}
