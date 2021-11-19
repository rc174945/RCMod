using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Xft;
using System.Collections;
using Settings;

namespace CustomSkins
{
    class HumanCustomSkinLoader : BaseCustomSkinLoader
    {
        protected override string RendererIdPrefix { get { return "human"; } }
        private int _horseViewId;

        public override IEnumerator LoadSkinsFromRPC(object[] data)
        {
            _horseViewId = (int)data[0];
            string[] skinUrls = ((string)data[1]).Split(',');
            foreach (int partId in GetCustomSkinPartIds(typeof(HumanCustomSkinPartId)))
            {
                if (partId == (int)HumanCustomSkinPartId.Horse && _horseViewId < 0)
                    continue;
                else if (partId == (int)HumanCustomSkinPartId.WeaponTrail && !_owner.GetComponent<HERO>().IsMine())
                    continue;
                else if (partId == (int)HumanCustomSkinPartId.Gas && !SettingsManager.CustomSkinSettings.Human.GasEnabled.Value)
                    continue;
                BaseCustomSkinPart part = GetCustomSkinPart(partId);
                if (!part.LoadCache(skinUrls[partId]))
                    yield return StartCoroutine(part.LoadSkin(skinUrls[partId]));
            }
            FengGameManagerMKII.instance.unloadAssets();
        }

        protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
        {
            HERO hero = _owner.GetComponent<HERO>();
            List<Renderer> renderers = new List<Renderer>();
            switch ((HumanCustomSkinPartId)partId)
            {
                case HumanCustomSkinPartId.Horse:
                    AddRenderersMatchingName(renderers, PhotonView.Find(_horseViewId).gameObject, "HORSE");
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.Hair:
                    AddRendererIfExists(renderers, hero.setup.part_hair);
                    AddRendererIfExists(renderers, hero.setup.part_hair_1);
                    return new HumanHairCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium, hero.setup.myCostume.hairInfo);
                case HumanCustomSkinPartId.Eye:
                    AddRendererIfExists(renderers, hero.setup.part_eye);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall, new Vector2(8f, 8f));
                case HumanCustomSkinPartId.Glass:
                    AddRendererIfExists(renderers, hero.setup.part_glass);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall, new Vector2(8f, 8f));
                case HumanCustomSkinPartId.Face:
                    AddRendererIfExists(renderers, hero.setup.part_face);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall, new Vector2(8f, 8f));
                case HumanCustomSkinPartId.Skin:
                    AddRendererIfExists(renderers, hero.setup.part_hand_l);
                    AddRendererIfExists(renderers, hero.setup.part_hand_r);
                    AddRendererIfExists(renderers, hero.setup.part_head);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.Costume:
                    AddRendererIfExists(renderers, hero.setup.part_chest_3);
                    AddRendererIfExists(renderers, hero.setup.part_upper_body);
                    AddRendererIfExists(renderers, hero.setup.part_arm_l);
                    AddRendererIfExists(renderers, hero.setup.part_arm_r);
                    AddRendererIfExists(renderers, hero.setup.part_leg);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeLarge);
                case HumanCustomSkinPartId.Logo:
                    AddRendererIfExists(renderers, hero.setup.part_cape);
                    AddRendererIfExists(renderers, hero.setup.part_brand_1);
                    AddRendererIfExists(renderers, hero.setup.part_brand_2);
                    AddRendererIfExists(renderers, hero.setup.part_brand_3);
                    AddRendererIfExists(renderers, hero.setup.part_brand_4);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case HumanCustomSkinPartId.GearL:
                    AddRendererIfExists(renderers, hero.setup.part_3dmg);
                    AddRendererIfExists(renderers, hero.setup.part_3dmg_belt);
                    AddRendererIfExists(renderers, hero.setup.part_3dmg_gas_l);
                    AddRendererIfExists(renderers, hero.setup.part_blade_l);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.GearR:
                    AddRendererIfExists(renderers, hero.setup.part_3dmg);
                    AddRendererIfExists(renderers, hero.setup.part_3dmg_belt);
                    AddRendererIfExists(renderers, hero.setup.part_3dmg_gas_r);
                    AddRendererIfExists(renderers, hero.setup.part_blade_r);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.Gas:
                    AddRendererIfExists(renderers, hero.transform.Find("3dmg_smoke").gameObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case HumanCustomSkinPartId.Hoodie:
                    if (hero.setup.part_chest_1 != null && hero.setup.part_chest_1.name.Contains("character_cap_casual"))
                        AddRendererIfExists(renderers, hero.setup.part_chest_1);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case HumanCustomSkinPartId.WeaponTrail:
                    List<XWeaponTrail> trails = new List<XWeaponTrail>();
                    trails.Add(hero.leftbladetrail);
                    trails.Add(hero.leftbladetrail2);
                    trails.Add(hero.rightbladetrail);
                    trails.Add(hero.rightbladetrail2);
                    return new WeaponTrailCustomSkinPart(this, trails, GetRendererId(partId), MaxSizeSmall);
                case HumanCustomSkinPartId.ThunderspearL:
                    if (hero.ThunderSpearLModel != null)
                        AddRendererIfExists(renderers, hero.ThunderSpearLModel);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.ThunderspearR:
                    if (hero.ThunderSpearRModel != null)
                        AddRendererIfExists(renderers, hero.ThunderSpearRModel);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                default:
                    return null;
            }
        }
    }

    public enum HumanCustomSkinPartId
    {
        Horse,
        Hair,
        Eye,
        Glass,
        Face,
        Skin,
        Costume,
        Logo,
        GearL,
        GearR,
        Gas,
        Hoodie,
        WeaponTrail,
        ThunderspearL,
        ThunderspearR
    }
}
