using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Settings
{
    class CustomSkinSettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "CustomSkins.json"; } }
        public HumanCustomSkinSettings Human = new HumanCustomSkinSettings();
        public BaseCustomSkinSettings<TitanCustomSkinSet> Titan = new BaseCustomSkinSettings<TitanCustomSkinSet>();
        public BaseCustomSkinSettings<ShifterCustomSkinSet> Shifter = new BaseCustomSkinSettings<ShifterCustomSkinSet>();
        public BaseCustomSkinSettings<SkyboxCustomSkinSet> Skybox = new BaseCustomSkinSettings<SkyboxCustomSkinSet>();
        public BaseCustomSkinSettings<ForestCustomSkinSet> Forest = new BaseCustomSkinSettings<ForestCustomSkinSet>();
        public BaseCustomSkinSettings<CityCustomSkinSet> City = new BaseCustomSkinSettings<CityCustomSkinSet>();
        public BaseCustomSkinSettings<CustomLevelCustomSkinSet> CustomLevel = new BaseCustomSkinSettings<CustomLevelCustomSkinSet>();

        // hotfix to load legacy SkinSets variable
        public override void Load()
        {
            string path = GetFilePath();
            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                if (Encrypted)
                {
                    text = new SimpleAES().Decrypt(text);
                }
                DeserializeFromJsonString(text);
                foreach (ICustomSkinSettings settings in new ICustomSkinSettings[] { Human, Titan, Shifter, Skybox, Forest, City, CustomLevel })
                {
                    List<BaseSetting> items = settings.GetSkinSets().GetItems();
                    if (items.Count > 0)
                    {
                        settings.GetSets().Clear();
                        foreach (BaseSetSetting set in settings.GetSkinSets().GetItems())
                            settings.GetSets().AddItem(set);
                    }
                    settings.GetSkinSets().Clear();
                }
            }
            else
            {
                try
                {
                    LoadLegacy();
                }
                catch
                {
                    Debug.Log("Exception occurred while loading legacy settings.");
                }
            }
        }
    }
}
