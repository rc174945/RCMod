using Anticheat;
using ExitGames.Client.Photon;
using Photon;
using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class InRoomChat : Photon.MonoBehaviour
{
    private bool AlignBottom = true;
    public static readonly string ChatRPC = "Chat";
    public static Rect GuiRect = new Rect(0f, 100f, 300f, 470f);
    public static Rect GuiRect2 = new Rect(30f, 575f, 300f, 25f);
    private string inputLine = string.Empty;
    public bool IsVisible = true;
    public static LinkedList<string> messages = new LinkedList<string>();
    float deltaTime = 0.0f;
    int _maxLines = 15;

    private void ShowFPS()
    {
        Rect rect = new Rect(Screen.width / 4f - 75f, 10f, 150f, 30f);
        int fps = (int)Math.Round(1.0f / deltaTime);
        GUI.Label(rect, string.Format("FPS: {0}", fps));
    }

    private void ShowMessageWindow()
    {
        GUI.SetNextControlName(string.Empty);
        GUILayout.BeginArea(GuiRect);
        GUILayout.FlexibleSpace();
        string text = string.Empty;
        foreach (string message in messages)
            text = text + message + "\n";
        GUILayout.Label(text, new GUILayoutOption[0]);
        GUILayout.EndArea();
    }

    public void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    public void addLINE(string newLine)
    {
        newLine = ChatFilter.FilterSizeTag(newLine);
        messages.AddLast(newLine);
        while (messages.Count > _maxLines)
            messages.RemoveFirst();
    }

    public void OnGUI()
    {
        if (SettingsManager.GraphicsSettings.ShowFPS.Value)
            ShowFPS();
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && SettingsManager.UISettings.GameFeed.Value)
            ShowMessageWindow();
        if (!this.IsVisible || (PhotonNetwork.connectionStateDetailed != PeerStates.Joined))
        {
            return;
        }
        if (Event.current.type == EventType.KeyDown)
        {
            if ((((Event.current.keyCode == KeyCode.Tab) || (Event.current.character == '\t')) && !GameMenu.Paused) && (!SettingsManager.InputSettings.General.Chat.Contains(KeyCode.Tab)))
            {
                Event.current.Use();
                goto Label_219C;
            }
        }
        else if ((Event.current.type == EventType.KeyUp) && (((Event.current.keyCode != KeyCode.None) && (SettingsManager.InputSettings.General.Chat.Contains(Event.current.keyCode))) && (GUI.GetNameOfFocusedControl() != "ChatInput")))
        {
            if (Event.current.keyCode != KeyCode.KeypadEnter && Event.current.keyCode != KeyCode.Return)
            {
                this.inputLine = string.Empty;
                GUI.FocusControl("ChatInput");
                goto Label_219C;
            }
        }
        if ((Event.current.type == EventType.KeyDown) && ((Event.current.keyCode == KeyCode.KeypadEnter) || (Event.current.keyCode == KeyCode.Return)))
        {
            if (!string.IsNullOrEmpty(this.inputLine))
            {
                string str2;
                if (this.inputLine == "\t")
                {
                    this.inputLine = string.Empty;
                    GUI.FocusControl(string.Empty);
                    return;
                }
                if (FengGameManagerMKII.RCEvents.ContainsKey("OnChatInput"))
                {
                    string key = (string) FengGameManagerMKII.RCVariableNames["OnChatInput"];
                    if (FengGameManagerMKII.stringVariables.ContainsKey(key))
                    {
                        FengGameManagerMKII.stringVariables[key] = this.inputLine;
                    }
                    else
                    {
                        FengGameManagerMKII.stringVariables.Add(key, this.inputLine);
                    }
                    ((RCEvent) FengGameManagerMKII.RCEvents["OnChatInput"]).checkEvent();
                }
                if (!this.inputLine.StartsWith("/"))
                {
                    str2 = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]).hexColor();
                    if (str2 == string.Empty)
                    {
                        str2 = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]);
                        if (PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam] != null)
                        {
                            if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 1)
                            {
                                str2 = "<color=#00FFFF>" + str2 + "</color>";
                            }
                            else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 2)
                            {
                                str2 = "<color=#FF00FF>" + str2 + "</color>";
                            }
                        }
                    }
                    object[] parameters = new object[] { this.inputLine, str2 };
                    FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters);
                }
                else if (this.inputLine.StartsWith("/aso"))
                {
                    if (PhotonNetwork.isMasterClient)
                    {
                        LegacyGameSettings settings = SettingsManager.LegacyGameSettings;
                        LegacyGameSettings settingsUI = SettingsManager.LegacyGameSettingsUI;
                        switch (this.inputLine.Substring(5))
                        {
                            case "kdr":
                                if (!settings.PreserveKDR.Value)
                                {
                                    settings.PreserveKDR.Value = true;
                                    settingsUI.PreserveKDR.Value = true;
                                    this.addLINE("<color=#FFCC00>KDRs will be preserved from disconnects.</color>");
                                }
                                else
                                {
                                    settings.PreserveKDR.Value = false;
                                    settingsUI.PreserveKDR.Value = false;
                                    this.addLINE("<color=#FFCC00>KDRs will not be preserved from disconnects.</color>");
                                }
                                break;

                            case "racing":
                                if (!settings.RacingEndless.Value)
                                {
                                    settings.RacingEndless.Value = settingsUI.RacingEndless.Value = true;
                                    this.addLINE("<color=#FFCC00>Endless racing enabled.</color>");
                                }
                                else
                                {
                                    settings.RacingEndless.Value = settingsUI.RacingEndless.Value = false;
                                    this.addLINE("<color=#FFCC00>Endless racing disabled.</color>");
                                }
                                break;
                        }
                    }
                }
                else
                {
                    object[] objArray3;
                    if (this.inputLine == "/pause")
                    {
                        if (PhotonNetwork.isMasterClient)
                        {
                            FengGameManagerMKII.instance.photonView.RPC("pauseRPC", PhotonTargets.All, new object[] { true });
                            objArray3 = new object[] { "<color=#FFCC00>MasterClient has paused the game.</color>", "" };
                            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                        }
                        else
                        {
                            this.addLINE("<color=#FFCC00>error: not master client</color>");
                        }
                    }
                    else if (this.inputLine == "/unpause")
                    {
                        if (PhotonNetwork.isMasterClient)
                        {
                            FengGameManagerMKII.instance.photonView.RPC("pauseRPC", PhotonTargets.All, new object[] { false });
                            objArray3 = new object[] { "<color=#FFCC00>MasterClient has unpaused the game.</color>", "" };
                            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                        }
                        else
                        {
                            this.addLINE("<color=#FFCC00>error: not master client</color>");
                        }
                    }
                    else if (this.inputLine == "/checklevel")
                    {
                        foreach (PhotonPlayer player in PhotonNetwork.playerList)
                        {
                            this.addLINE(RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.currentLevel]));
                        }
                    }
                    else if (this.inputLine == "/isrc")
                    {
                        if (FengGameManagerMKII.masterRC)
                        {
                            this.addLINE("is RC");
                        }
                        else
                        {
                            this.addLINE("not RC");
                        }
                    }
                    else if (this.inputLine == "/ignorelist")
                    {
                        foreach (int num2 in FengGameManagerMKII.ignoreList)
                        {
                            this.addLINE(num2.ToString());
                        }
                    }
                    else if (this.inputLine.StartsWith("/room"))
                    {
                        if (PhotonNetwork.isMasterClient)
                        {
                            if (this.inputLine.Substring(6).StartsWith("max"))
                            {
                                int num3 = Convert.ToInt32(this.inputLine.Substring(10));
                                FengGameManagerMKII.instance.maxPlayers = num3;
                                PhotonNetwork.room.maxPlayers = num3;
                                objArray3 = new object[] { "<color=#FFCC00>Max players changed to " + this.inputLine.Substring(10) + "!</color>", "" };
                                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                            }
                            else if (this.inputLine.Substring(6).StartsWith("time"))
                            {
                                FengGameManagerMKII.instance.addTime(Convert.ToSingle(this.inputLine.Substring(11)));
                                objArray3 = new object[] { "<color=#FFCC00>" + this.inputLine.Substring(11) + " seconds added to the clock.</color>", "" };
                                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                            }
                        }
                        else
                        {
                            this.addLINE("<color=#FFCC00>error: not master client</color>");
                        }
                    }
                    else if (this.inputLine.StartsWith("/resetkd"))
                    {
                        Hashtable hashtable;
                        if (this.inputLine == "/resetkdall")
                        {
                            if (PhotonNetwork.isMasterClient)
                            {
                                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                                {
                                    hashtable = new Hashtable();
                                    hashtable.Add(PhotonPlayerProperty.kills, 0);
                                    hashtable.Add(PhotonPlayerProperty.deaths, 0);
                                    hashtable.Add(PhotonPlayerProperty.max_dmg, 0);
                                    hashtable.Add(PhotonPlayerProperty.total_dmg, 0);
                                    player.SetCustomProperties(hashtable);
                                }
                                objArray3 = new object[] { "<color=#FFCC00>All stats have been reset.</color>", "" };
                                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                            }
                            else
                            {
                                this.addLINE("<color=#FFCC00>error: not master client</color>");
                            }
                        }
                        else
                        {
                            hashtable = new Hashtable();
                            hashtable.Add(PhotonPlayerProperty.kills, 0);
                            hashtable.Add(PhotonPlayerProperty.deaths, 0);
                            hashtable.Add(PhotonPlayerProperty.max_dmg, 0);
                            hashtable.Add(PhotonPlayerProperty.total_dmg, 0);
                            PhotonNetwork.player.SetCustomProperties(hashtable);
                            this.addLINE("<color=#FFCC00>Your stats have been reset. </color>");
                        }
                    }
                    else if (this.inputLine.StartsWith("/pm"))
                    {
                        string[] strArray = this.inputLine.Split(new char[] { ' ' });
                        PhotonPlayer targetPlayer = PhotonPlayer.Find(Convert.ToInt32(strArray[1]));
                        str2 = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]).hexColor();
                        if (str2 == string.Empty)
                        {
                            str2 = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]);
                            if (PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam] != null)
                            {
                                if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 1)
                                {
                                    str2 = "<color=#00FFFF>" + str2 + "</color>";
                                }
                                else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 2)
                                {
                                    str2 = "<color=#FF00FF>" + str2 + "</color>";
                                }
                            }
                        }
                        string str3 = RCextensions.returnStringFromObject(targetPlayer.customProperties[PhotonPlayerProperty.name]).hexColor();
                        if (str3 == string.Empty)
                        {
                            str3 = RCextensions.returnStringFromObject(targetPlayer.customProperties[PhotonPlayerProperty.name]);
                            if (targetPlayer.customProperties[PhotonPlayerProperty.RCteam] != null)
                            {
                                if (RCextensions.returnIntFromObject(targetPlayer.customProperties[PhotonPlayerProperty.RCteam]) == 1)
                                {
                                    str3 = "<color=#00FFFF>" + str3 + "</color>";
                                }
                                else if (RCextensions.returnIntFromObject(targetPlayer.customProperties[PhotonPlayerProperty.RCteam]) == 2)
                                {
                                    str3 = "<color=#FF00FF>" + str3 + "</color>";
                                }
                            }
                        }
                        string str4 = string.Empty;
                        for (int i = 2; i < strArray.Length; i++)
                        {
                            str4 = str4 + strArray[i] + " ";
                        }
                        FengGameManagerMKII.instance.photonView.RPC("ChatPM", targetPlayer, new object[] { str2, str4 });
                        this.addLINE("<color=#FFC000>TO [" + targetPlayer.ID.ToString() + "]</color> " + str3 + ":" + str4);
                    }
                    else if (this.inputLine.StartsWith("/team"))
                    {
                        if (SettingsManager.LegacyGameSettings.TeamMode.Value == 1)
                        {
                            if ((this.inputLine.Substring(6) == "1") || (this.inputLine.Substring(6) == "cyan"))
                            {
                                FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, new object[] { 1 });
                                this.addLINE("<color=#00FFFF>You have joined team cyan.</color>");
                                foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("Player"))
                                {
                                    if (obj2.GetPhotonView().isMine)
                                    {
                                        obj2.GetComponent<HERO>().markDie();
                                        obj2.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                                    }
                                }
                            }
                            else if ((this.inputLine.Substring(6) == "2") || (this.inputLine.Substring(6) == "magenta"))
                            {
                                FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, new object[] { 2 });
                                this.addLINE("<color=#FF00FF>You have joined team magenta.</color>");
                                foreach (GameObject obj3 in GameObject.FindGameObjectsWithTag("Player"))
                                {
                                    if (obj3.GetPhotonView().isMine)
                                    {
                                        obj3.GetComponent<HERO>().markDie();
                                        obj3.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                                    }
                                }
                            }
                            else if ((this.inputLine.Substring(6) == "0") || (this.inputLine.Substring(6) == "individual"))
                            {
                                FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, new object[] { 0 });
                                this.addLINE("<color=#00FF00>You have joined individuals.</color>");
                                foreach (GameObject obj4 in GameObject.FindGameObjectsWithTag("Player"))
                                {
                                    if (obj4.GetPhotonView().isMine)
                                    {
                                        obj4.GetComponent<HERO>().markDie();
                                        obj4.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                                    }
                                }
                            }
                            else
                            {
                                this.addLINE("<color=#FFCC00>error: invalid team code. Accepted values are 0,1, and 2.</color>");
                            }
                        }
                        else
                        {
                            this.addLINE("<color=#FFCC00>error: teams are locked or disabled. </color>");
                        }
                    }
                    else if (this.inputLine == "/restart")
                    {
                        if (PhotonNetwork.isMasterClient)
                        {
                            objArray3 = new object[] { "<color=#FFCC00>MasterClient has restarted the game!</color>", "" };
                            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                            FengGameManagerMKII.instance.restartRC();
                        }
                        else
                        {
                            this.addLINE("<color=#FFCC00>error: not master client</color>");
                        }
                    }
                    else if (this.inputLine.StartsWith("/specmode"))
                    {
                        SettingsManager.LegacyGeneralSettings.SpecMode.Value = !SettingsManager.LegacyGeneralSettings.SpecMode.Value;
                        if (SettingsManager.LegacyGeneralSettings.SpecMode.Value)
                        {
                            FengGameManagerMKII.instance.EnterSpecMode(true);
                            this.addLINE("<color=#FFCC00>You have entered spectator mode.</color>");
                        }
                        else
                        {
                            FengGameManagerMKII.instance.EnterSpecMode(false);
                            this.addLINE("<color=#FFCC00>You have exited spectator mode.</color>");
                        }
                    }
                    else if (this.inputLine.StartsWith("/fov"))
                    {
                        int num6 = Convert.ToInt32(this.inputLine.Substring(5));
                        Camera.main.fieldOfView = num6;
                        this.addLINE("<color=#FFCC00>Field of vision set to " + num6.ToString() + ".</color>");
                    }
                    else if (this.inputLine == "/clear")
                    {
                        messages.Clear();
                    }
                    else
                    {
                        int num8;
                        if (this.inputLine.StartsWith("/spectate"))
                        {
                            num8 = Convert.ToInt32(this.inputLine.Substring(10));
                            foreach (GameObject obj5 in GameObject.FindGameObjectsWithTag("Player"))
                            {
                                if (obj5.GetPhotonView().owner.ID == num8)
                                {
                                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(obj5, true, false);
                                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
                                }
                            }
                        }
                        else if (!this.inputLine.StartsWith("/kill"))
                        {
                            object[] objArray5;
                            if (this.inputLine.StartsWith("/revive"))
                            {
                                if (PhotonNetwork.isMasterClient)
                                {
                                    if (this.inputLine == "/reviveall")
                                    {
                                        objArray5 = new object[] { "<color=#FFCC00>All players have been revived.</color>", string.Empty };
                                        FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray5);
                                        foreach (PhotonPlayer player in PhotonNetwork.playerList)
                                        {
                                            if (((player.customProperties[PhotonPlayerProperty.dead] != null) && RCextensions.returnBoolFromObject(player.customProperties[PhotonPlayerProperty.dead])) && (RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.isTitan]) != 2))
                                            {
                                                FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", player, new object[0]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        num8 = Convert.ToInt32(this.inputLine.Substring(8));
                                        foreach (PhotonPlayer player in PhotonNetwork.playerList)
                                        {
                                            if (player.ID == num8)
                                            {
                                                this.addLINE("<color=#FFCC00>Player " + num8.ToString() + " has been revived.</color>");
                                                if (((player.customProperties[PhotonPlayerProperty.dead] != null) && RCextensions.returnBoolFromObject(player.customProperties[PhotonPlayerProperty.dead])) && (RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.isTitan]) != 2))
                                                {
                                                    objArray5 = new object[] { "<color=#FFCC00>You have been revived by the master client.</color>", string.Empty };
                                                    FengGameManagerMKII.instance.photonView.RPC("Chat", player, objArray5);
                                                    FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", player, new object[0]);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    this.addLINE("<color=#FFCC00>error: not master client</color>");
                                }
                            }
                            else if (this.inputLine.StartsWith("/unban"))
                            {
                                if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
                                {
                                    FengGameManagerMKII.ServerRequestUnban(this.inputLine.Substring(7));
                                }
                                else if (PhotonNetwork.isMasterClient)
                                {
                                    int num9 = Convert.ToInt32(this.inputLine.Substring(7));
                                    if (FengGameManagerMKII.banHash.ContainsKey(num9))
                                    {
                                        objArray5 = new object[] { "<color=#FFCC00>" + ((string) FengGameManagerMKII.banHash[num9]) + " has been unbanned from the server. </color>", string.Empty };
                                        FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray5);
                                        FengGameManagerMKII.banHash.Remove(num9);
                                    }
                                    else
                                    {
                                        this.addLINE("error: no such player");
                                    }
                                }
                                else
                                {
                                    this.addLINE("<color=#FFCC00>error: not master client</color>");
                                }
                            }
                            else if (this.inputLine.StartsWith("/rules"))
                            {
                                this.addLINE("<color=#FFCC00>Currently activated gamemodes:</color>");
                                if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Bomb mode is on.</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
                                {
                                    if (SettingsManager.LegacyGameSettings.TeamMode.Value == 1)
                                    {
                                        this.addLINE("<color=#FFCC00>Team mode is on (no sort).</color>");
                                    }
                                    else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 2)
                                    {
                                        this.addLINE("<color=#FFCC00>Team mode is on (sort by size).</color>");
                                    }
                                    else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 3)
                                    {
                                        this.addLINE("<color=#FFCC00>Team mode is on (sort by skill).</color>");
                                    }
                                }
                                if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Point mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.PointModeAmount.Value) + ").</color>");
                                }
                                if (!SettingsManager.LegacyGameSettings.RockThrowEnabled.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Punk Rock-Throwing is disabled.</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Custom spawn rate is on (" + SettingsManager.LegacyGameSettings.TitanSpawnNormal.Value.ToString("F2") + "% Normal, " + 
                                        SettingsManager.LegacyGameSettings.TitanSpawnAberrant.Value.ToString("F2") + "% Abnormal, " + 
                                        SettingsManager.LegacyGameSettings.TitanSpawnJumper.Value.ToString("F2") + "% Jumper, " + 
                                        SettingsManager.LegacyGameSettings.TitanSpawnCrawler.Value.ToString("F2") + "% Crawler, " + 
                                        SettingsManager.LegacyGameSettings.TitanSpawnPunk.Value.ToString("F2") + "% Punk </color>");
                                }
                                if (SettingsManager.LegacyGameSettings.TitanExplodeEnabled.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Titan explode mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanExplodeRadius.Value) + ").</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value > 0)
                                {
                                    this.addLINE("<color=#FFCC00>Titan health mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanHealthMin.Value) + "-" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanHealthMax.Value) + ").</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Infection mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.InfectionModeAmount.Value) + ").</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Nape armor is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanArmor.Value) + ").</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Custom titan # is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanNumber.Value) + ").</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Custom titan size is on (" + SettingsManager.LegacyGameSettings.TitanSizeMin.Value.ToString("F2") + "," + 
                                        SettingsManager.LegacyGameSettings.TitanSizeMax.Value.ToString("F2") + ").</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.KickShifters.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Anti-shifter is on. Using shifters will get you kicked.</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Custom wave mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanMaxWaves.Value) + ").</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.FriendlyMode.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Friendly-Fire disabled. PVP is prohibited.</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.BladePVP.Value > 0)
                                {
                                    if (SettingsManager.LegacyGameSettings.BladePVP.Value == 1)
                                    {
                                        this.addLINE("<color=#FFCC00>AHSS/Blade PVP is on (team-based).</color>");
                                    }
                                    else if (SettingsManager.LegacyGameSettings.BladePVP.Value == 2)
                                    {
                                        this.addLINE("<color=#FFCC00>AHSS/Blade PVP is on (FFA).</color>");
                                    }
                                }
                                if (SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Max Wave set to " + SettingsManager.LegacyGameSettings.TitanMaxWaves.Value.ToString() + "</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.AllowHorses.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Horses are enabled.</color>");
                                }
                                if (!SettingsManager.LegacyGameSettings.AHSSAirReload.Value)
                                {
                                    this.addLINE("<color=#FFCC00>AHSS Air-Reload disabled.</color>");
                                }
                                if (!SettingsManager.LegacyGameSettings.PunksEveryFive.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Punks will not spawn every five waves.</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Endless Respawn is enabled (" + SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value.ToString() + " seconds).</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.GlobalMinimapDisable.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Minimaps are disabled.</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.Motd.Value != string.Empty)
                                {
                                    this.addLINE("<color=#FFCC00>MOTD:" + SettingsManager.LegacyGameSettings.Motd.Value + "</color>");
                                }
                                if (SettingsManager.LegacyGameSettings.CannonsFriendlyFire.Value)
                                {
                                    this.addLINE("<color=#FFCC00>Cannons will kill humans.</color>");
                                }
                            }
                            else
                            {
                                object[] objArray6;
                                bool flag2;
                                object[] objArray7;
                                if (this.inputLine.StartsWith("/kick"))
                                {
                                    num8 = Convert.ToInt32(this.inputLine.Substring(6));
                                    if (num8 == PhotonNetwork.player.ID)
                                    {
                                        this.addLINE("error:can't kick yourself.");
                                    }
                                    else if (!(SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN || PhotonNetwork.isMasterClient))
                                    {
                                        objArray6 = new object[] { "/kick #" + Convert.ToString(num8), LoginFengKAI.player.name };
                                        FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray6);
                                    }
                                    else
                                    {
                                        flag2 = false;
                                        foreach (PhotonPlayer player3 in PhotonNetwork.playerList)
                                        {
                                            if (num8 == player3.ID)
                                            {
                                                flag2 = true;
                                                if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
                                                {
                                                    FengGameManagerMKII.instance.kickPlayerRC(player3, false, "");
                                                }
                                                else if (PhotonNetwork.isMasterClient)
                                                {
                                                    FengGameManagerMKII.instance.kickPlayerRC(player3, false, "");
                                                    objArray7 = new object[] { "<color=#FFCC00>" + RCextensions.returnStringFromObject(player3.customProperties[PhotonPlayerProperty.name]) + " has been kicked from the server!</color>", string.Empty };
                                                    FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray7);
                                                }
                                            }
                                        }
                                        if (!flag2)
                                        {
                                            this.addLINE("error:no such player.");
                                        }
                                    }
                                }
                                else if (this.inputLine.StartsWith("/ban"))
                                {
                                    if (this.inputLine == "/banlist")
                                    {
                                        this.addLINE("<color=#FFCC00>List of banned players:</color>");
                                        foreach (int num10 in FengGameManagerMKII.banHash.Keys)
                                        {
                                            this.addLINE("<color=#FFCC00>" + Convert.ToString(num10) + ":" + ((string) FengGameManagerMKII.banHash[num10]) + "</color>");
                                        }
                                    }
                                    else
                                    {
                                        num8 = Convert.ToInt32(this.inputLine.Substring(5));
                                        if (num8 == PhotonNetwork.player.ID)
                                        {
                                            this.addLINE("error:can't kick yourself.");
                                        }
                                        else if (!(SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN || PhotonNetwork.isMasterClient))
                                        {
                                            objArray6 = new object[] { "/kick #" + Convert.ToString(num8), LoginFengKAI.player.name };
                                            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray6);
                                        }
                                        else
                                        {
                                            flag2 = false;
                                            foreach (PhotonPlayer player3 in PhotonNetwork.playerList)
                                            {
                                                if (num8 == player3.ID)
                                                {
                                                    flag2 = true;
                                                    if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
                                                    {
                                                        FengGameManagerMKII.instance.kickPlayerRC(player3, true, "");
                                                    }
                                                    else if (PhotonNetwork.isMasterClient)
                                                    {
                                                        FengGameManagerMKII.instance.kickPlayerRC(player3, true, "");
                                                        objArray7 = new object[] { "<color=#FFCC00>" + RCextensions.returnStringFromObject(player3.customProperties[PhotonPlayerProperty.name]) + " has been banned from the server!</color>", string.Empty };
                                                        FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray7);
                                                    }
                                                }
                                            }
                                            if (!flag2)
                                            {
                                                this.addLINE("error:no such player.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                this.inputLine = string.Empty;
                GUI.FocusControl(string.Empty);
                return;
            }
            this.inputLine = "\t";
            GUI.FocusControl("ChatInput");
        }
    Label_219C:
        ShowMessageWindow();
        GUILayout.BeginArea(GuiRect2);
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUI.SetNextControlName("ChatInput");
        this.inputLine = GUILayout.TextField(this.inputLine, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public void setPosition()
    {
        if (this.AlignBottom)
        {
            GuiRect = new Rect(0f, (float) (Screen.height - 500), 300f, 470f);
            GuiRect2 = new Rect(SettingsManager.UISettings.CenteredFlares.Value ? 0f : 30f, (float) ((Screen.height - 300) + 0x113), 300f, 25f);
        }
    }

    public void Start()
    {
        this.setPosition();
    }
}

