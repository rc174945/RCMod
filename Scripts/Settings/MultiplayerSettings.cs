using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    class MultiplayerSettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "Multiplayer.json"; } }
        public static string PublicLobby = "01042015";
        public static string PrivateLobby = "verified343";
        public static string PublicAppId = "5578b046-8264-438c-99c5-fb15c71b6744";
        public IntSetting LobbyMode = new IntSetting(0);
        public IntSetting AppIdMode = new IntSetting(0);
        public StringSetting CustomLobby = new StringSetting(string.Empty);
        public StringSetting CustomAppId = new StringSetting(string.Empty);
        public StringSetting LanIP = new StringSetting(string.Empty);
        public IntSetting LanPort = new IntSetting(5055);
        public StringSetting LanPassword = new StringSetting(string.Empty);
        public MultiplayerServerType CurrentMultiplayerServerType;
        public readonly Dictionary<MultiplayerRegion, string> CloudAddresses = new Dictionary<MultiplayerRegion, string>()
        {
            { MultiplayerRegion.EU, "app-eu.exitgamescloud.com" },
            { MultiplayerRegion.US, "app-us.exitgamescloud.com" },
            { MultiplayerRegion.SA, "app-sa.exitgames.com" },
            { MultiplayerRegion.ASIA, "app-asia.exitgamescloud.com" }
        };
        public readonly Dictionary<MultiplayerRegion, string> PublicAddresses = new Dictionary<MultiplayerRegion, string>()
        {
            { MultiplayerRegion.EU, "135.125.239.180" },
            { MultiplayerRegion.US, "142.44.242.29" },
            { MultiplayerRegion.SA, "172.107.193.233" },
            { MultiplayerRegion.ASIA, "51.79.164.137" },
        };
        public readonly int DefaultPort = 5055;

        // legacy save compatibility
        public StringSetting Name = new StringSetting("GUEST" + Random.Range(0, 100000), maxLength: 50);
        public StringSetting Guild = new StringSetting(string.Empty, maxLength: 50);

        public void ConnectServer(MultiplayerRegion region)
        {
            FengGameManagerMKII.JustLeftRoom = false;
            PhotonNetwork.Disconnect();
            string address;
            if (AppIdMode.Value == (int)AppIdModeType.Public)
            {
                address = PublicAddresses[region];
                CurrentMultiplayerServerType = MultiplayerServerType.Public;
                PhotonNetwork.ConnectToMaster(address, DefaultPort, string.Empty, GetCurrentLobby());
            }
            else
            {
                address = CloudAddresses[region];
                CurrentMultiplayerServerType = MultiplayerServerType.Cloud;
                PhotonNetwork.ConnectToMaster(address, DefaultPort, CustomAppId.Value, GetCurrentLobby());
            }
        }

        public string GetCurrentLobby()
        {
            if (LobbyMode.Value == (int)LobbyModeType.Public)
                return PublicLobby;
            if (LobbyMode.Value == (int)LobbyModeType.Private)
                return PrivateLobby;
            return CustomLobby.Value;
        }

        public void ConnectLAN()
        {
            PhotonNetwork.Disconnect();
            if (PhotonNetwork.ConnectToMaster(LanIP.Value, LanPort.Value, string.Empty, GetCurrentLobby()))
            {
                CurrentMultiplayerServerType = MultiplayerServerType.LAN;
                FengGameManagerMKII.PrivateServerAuthPass = LanPassword.Value;
            }
        }

        public void ConnectOffline()
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.offlineMode = true;
            CurrentMultiplayerServerType = MultiplayerServerType.Cloud;
            FengGameManagerMKII.instance.OnJoinedLobby();
        }

        protected override void LoadLegacy()
        {
            Name.Value = PlayerPrefs.GetString("name", Name.Value);
            Guild.Value = PlayerPrefs.GetString("guildname", Guild.Value);
        }
    }

    public enum MultiplayerServerType
    {
        LAN,
        Cloud,
        Public
    }

    public enum MultiplayerRegion
    {
        EU,
        US,
        SA,
        ASIA
    }

    public enum LobbyModeType
    {
        Public,
        Private,
        Custom
    }

    public enum AppIdModeType
    {
        Public,
        Custom
    }
}