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

        protected override void LoadLegacy()
        {
            // human
            Human.Sets.Value.Clear();
            for (int i = 0; i < 3; i++)
            {
                string suffix = string.Empty;
                if (i > 0)
                    suffix = (i + 1).ToString();
                HumanCustomSkinSet humanSet = new HumanCustomSkinSet();
                humanSet.Horse.Value = PlayerPrefs.GetString("horse" + suffix, string.Empty);
                humanSet.Hair.Value = PlayerPrefs.GetString("hair" + suffix, string.Empty);
                humanSet.Eye.Value = PlayerPrefs.GetString("eye" + suffix, string.Empty);
                humanSet.Glass.Value = PlayerPrefs.GetString("glass" + suffix, string.Empty);
                humanSet.Face.Value = PlayerPrefs.GetString("face" + suffix, string.Empty);
                humanSet.Skin.Value = PlayerPrefs.GetString("skin" + suffix, string.Empty);
                humanSet.Costume.Value = PlayerPrefs.GetString("costume" + suffix, string.Empty);
                humanSet.Logo.Value = PlayerPrefs.GetString("logo" + suffix, string.Empty);
                humanSet.GearL.Value = PlayerPrefs.GetString("bladel" + suffix, string.Empty);
                humanSet.GearR.Value = PlayerPrefs.GetString("blader" + suffix, string.Empty);
                humanSet.Gas.Value = PlayerPrefs.GetString("gas" + suffix, string.Empty);
                humanSet.Hoodie.Value = PlayerPrefs.GetString("hoodie" + suffix, string.Empty);
                if (i > 0)
                    humanSet.WeaponTrail.Value = PlayerPrefs.GetString("trail" + suffix, string.Empty);
                else
                    humanSet.WeaponTrail.Value = PlayerPrefs.GetString("trailskin" + suffix, string.Empty);
                humanSet.Name.Value = "Set " + (i + 1).ToString();
                Human.Sets.Value.Add(humanSet);
            }

            // titan
            TitanCustomSkinSet titanSet = Titan.Sets.Value[0];
            titanSet.Hairs.Value.Clear();
            titanSet.Eyes.Value.Clear();
            titanSet.Bodies.Value.Clear();
            titanSet.HairModels.Value.Clear();
            for (int i = 0; i < 5; i++)
            {
                string suffix = (i + 1).ToString();
                titanSet.Hairs.Value.Add(new StringSetting(PlayerPrefs.GetString("titanhair" + suffix, string.Empty)));
                titanSet.Eyes.Value.Add(new StringSetting(PlayerPrefs.GetString("titaneye" + suffix, string.Empty)));
                titanSet.Bodies.Value.Add(new StringSetting(PlayerPrefs.GetString("titanbody" + suffix, string.Empty)));
                titanSet.HairModels.Value.Add(new IntSetting(PlayerPrefs.GetInt("titantype" + suffix, -1) + 1));
            }
            titanSet.Name.Value = "Set 1";

            // shifter
            ShifterCustomSkinSet shifterSet = Shifter.Sets.Value[0];
            shifterSet.Eren.Value = PlayerPrefs.GetString("eren", string.Empty);
            shifterSet.Annie.Value = PlayerPrefs.GetString("annie", string.Empty);
            shifterSet.Colossal.Value = PlayerPrefs.GetString("colossal", string.Empty);
            shifterSet.Name.Value = "Set 1";

            // skybox
            Skybox.Sets.Value.Clear();
            string[] prefixes = new string[] { "forestsky", "citysky", "customsky" };
            for (int i = 0; i < 3; i++)
            {
                SkyboxCustomSkinSet skyBoxSet = new SkyboxCustomSkinSet();
                skyBoxSet.Front.Value = PlayerPrefs.GetString(prefixes[i] + "front", string.Empty);
                skyBoxSet.Back.Value = PlayerPrefs.GetString(prefixes[i] + "back", string.Empty);
                skyBoxSet.Left.Value = PlayerPrefs.GetString(prefixes[i] + "left", string.Empty);
                skyBoxSet.Right.Value = PlayerPrefs.GetString(prefixes[i] + "right", string.Empty);
                skyBoxSet.Up.Value = PlayerPrefs.GetString(prefixes[i] + "up", string.Empty);
                skyBoxSet.Down.Value = PlayerPrefs.GetString(prefixes[i] + "down", string.Empty);
                skyBoxSet.Name.Value = "Set " + (i + 1).ToString();
                Skybox.Sets.Value.Add(skyBoxSet);
            }

            // forest
            ForestCustomSkinSet forestSet = Forest.Sets.Value[0];
            forestSet.TreeTrunks.Value.Clear();
            forestSet.TreeLeafs.Value.Clear();
            for (int i = 0; i < 8; i++)
            {
                string suffix = (i + 1).ToString();
                forestSet.TreeTrunks.Value.Add(new StringSetting(PlayerPrefs.GetString("tree" + suffix, string.Empty)));
                forestSet.TreeLeafs.Value.Add(new StringSetting(PlayerPrefs.GetString("leaf" + suffix, string.Empty)));
            }
            forestSet.Ground.Value = PlayerPrefs.GetString("forestG", string.Empty);
            forestSet.RandomizedPairs.Value = Convert.ToBoolean(PlayerPrefs.GetInt("forestR", 0));
            forestSet.Name.Value = "Set 1";

            // city
            CityCustomSkinSet citySet = City.Sets.Value[0];
            citySet.Houses.Value.Clear();
            for (int i = 0; i < 8; i++)
            {
                string suffix = (i + 1).ToString();
                citySet.Houses.Value.Add(new StringSetting(PlayerPrefs.GetString("house" + suffix, string.Empty)));
            }
            citySet.Ground.Value = PlayerPrefs.GetString("cityG", string.Empty);
            citySet.Wall.Value = PlayerPrefs.GetString("cityW", string.Empty);
            citySet.Gate.Value = PlayerPrefs.GetString("cityH", string.Empty);
            citySet.Name.Value = "Set 1";

            // custom
            CustomLevelCustomSkinSet customSet = CustomLevel.Sets.Value[0];
            customSet.Ground.Value = PlayerPrefs.GetString("customGround", string.Empty);
            customSet.Name.Value = "Set 1";
        }
    }
}
