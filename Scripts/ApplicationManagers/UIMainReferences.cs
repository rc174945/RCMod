using System;
using System.Collections;
using UnityEngine;
using ApplicationManagers;
using UI;
using GameProgress;

public class UIMainReferences : MonoBehaviour
{
    public GameObject panelCredits;
    public GameObject PanelDisconnect;
    public GameObject panelMain;
    public GameObject PanelMultiJoinPrivate;
    public GameObject PanelMultiPWD;
    public GameObject panelMultiROOM;
    public GameObject panelMultiSet;
    public GameObject panelMultiStart;
    public GameObject PanelMultiWait;
    public GameObject panelOption;
    public GameObject panelSingleSet;
    public GameObject PanelSnapShot;

    // legacy
    public static string Version = "01042015";

    private void Awake()
    {
        NGUITools.SetActive(panelMain, false);
        Destroy(GameObject.Find("PopupListLang"));
        MainApplicationManager.Init();
    }
}
