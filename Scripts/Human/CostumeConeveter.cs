




using ExitGames.Client.Photon;
using System;
using UnityEngine;

public class CostumeConeveter
{
    private static int DivisionToInt(DIVISION id)
    {
        if (id == DIVISION.TheGarrison)
        {
            return 0;
        }
        if (id == DIVISION.TheMilitaryPolice)
        {
            return 1;
        }
        if ((id != DIVISION.TheSurveryCorps) && (id == DIVISION.TraineesSquad))
        {
            return 3;
        }
        return 2;
    }

    public static void HeroCostumeToLocalData(HeroCostume costume, string slot)
    {
        slot = slot.ToUpper();
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.sex, SexToInt(costume.sex));
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.costumeId, costume.costumeId);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.heroCostumeId, costume.id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.cape, !costume.cape ? 0 : 1);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.hairInfo, costume.hairInfo.id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.eye_texture_id, costume.eye_texture_id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.beard_texture_id, costume.beard_texture_id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.glass_texture_id, costume.glass_texture_id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.skin_color, costume.skin_color);
        PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color1, costume.hair_color.r);
        PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color2, costume.hair_color.g);
        PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color3, costume.hair_color.b);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.division, DivisionToInt(costume.division));
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statSPD, costume.stat.SPD);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statGAS, costume.stat.GAS);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statBLA, costume.stat.BLA);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statACL, costume.stat.ACL);
        PlayerPrefs.SetString(slot + PhotonPlayerProperty.statSKILL, costume.stat.skillId);
    }

    public static void HeroCostumeToPhotonData2(HeroCostume costume, PhotonPlayer player)
    {
        Hashtable propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.sex, SexToInt(costume.sex));
        int costumeId = costume.costumeId;
        if (costumeId == 0x1a)
        {
            costumeId = 0x19;
        }
        propertiesToSet.Add(PhotonPlayerProperty.costumeId, costumeId);
        propertiesToSet.Add(PhotonPlayerProperty.heroCostumeId, costume.id);
        propertiesToSet.Add(PhotonPlayerProperty.cape, costume.cape);
        propertiesToSet.Add(PhotonPlayerProperty.hairInfo, costume.hairInfo.id);
        propertiesToSet.Add(PhotonPlayerProperty.eye_texture_id, costume.eye_texture_id);
        propertiesToSet.Add(PhotonPlayerProperty.beard_texture_id, costume.beard_texture_id);
        propertiesToSet.Add(PhotonPlayerProperty.glass_texture_id, costume.glass_texture_id);
        propertiesToSet.Add(PhotonPlayerProperty.skin_color, costume.skin_color);
        propertiesToSet.Add(PhotonPlayerProperty.hair_color1, costume.hair_color.r);
        propertiesToSet.Add(PhotonPlayerProperty.hair_color2, costume.hair_color.g);
        propertiesToSet.Add(PhotonPlayerProperty.hair_color3, costume.hair_color.b);
        propertiesToSet.Add(PhotonPlayerProperty.division, DivisionToInt(costume.division));
        propertiesToSet.Add(PhotonPlayerProperty.statSPD, costume.stat.SPD);
        propertiesToSet.Add(PhotonPlayerProperty.statGAS, costume.stat.GAS);
        propertiesToSet.Add(PhotonPlayerProperty.statBLA, costume.stat.BLA);
        propertiesToSet.Add(PhotonPlayerProperty.statACL, costume.stat.ACL);
        propertiesToSet.Add(PhotonPlayerProperty.statSKILL, costume.stat.skillId);
        player.SetCustomProperties(propertiesToSet);
    }

    private static DIVISION IntToDivision(int id)
    {
        if (id == 0)
        {
            return DIVISION.TheGarrison;
        }
        if (id == 1)
        {
            return DIVISION.TheMilitaryPolice;
        }
        if ((id != 2) && (id == 3))
        {
            return DIVISION.TraineesSquad;
        }
        return DIVISION.TheSurveryCorps;
    }

    private static SEX IntToSex(int id)
    {
        if (id == 0)
        {
            return SEX.FEMALE;
        }
        if (id == 1)
        {
            return SEX.MALE;
        }
        return SEX.MALE;
    }

    private static UNIFORM_TYPE IntToUniformType(int id)
    {
        if (id == 0)
        {
            return UNIFORM_TYPE.CasualA;
        }
        if (id == 1)
        {
            return UNIFORM_TYPE.CasualB;
        }
        if (id != 2)
        {
            if (id == 3)
            {
                return UNIFORM_TYPE.UniformB;
            }
            if (id == 4)
            {
                return UNIFORM_TYPE.CasualAHSS;
            }
        }
        return UNIFORM_TYPE.UniformA;
    }

    public static HeroCostume LocalDataToHeroCostume(string slot)
    {
        slot = slot.ToUpper();
        if (!PlayerPrefs.HasKey(slot + PhotonPlayerProperty.sex))
        {
            return HeroCostume.costume[0];
        }
        HeroCostume costume = new HeroCostume();
        costume = new HeroCostume {
            sex = IntToSex(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.sex)),
            id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.heroCostumeId),
            costumeId = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.costumeId),
            cape = (PlayerPrefs.GetInt(slot + PhotonPlayerProperty.cape) != 1) ? false : true,
            hairInfo = (IntToSex(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.sex)) == SEX.FEMALE ? (CostumeHair.hairsF[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.hairInfo)]) : CostumeHair.hairsM[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.hairInfo)]),
            //hairInfo = (costume.sex != SEX.MALE) ? CostumeHair.hairsF[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.hairInfo)] : CostumeHair.hairsM[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.hairInfo)],
            eye_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.eye_texture_id),
            beard_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.beard_texture_id),
            glass_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.glass_texture_id),
            skin_color = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.skin_color),
            hair_color = new Color(PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color1), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color2), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color3)),
            division = IntToDivision(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.division)),
            stat = new HeroStat()
        };
        costume.stat.SPD = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statSPD);
        costume.stat.GAS = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statGAS);
        costume.stat.BLA = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statBLA);
        costume.stat.ACL = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statACL);
        costume.stat.skillId = PlayerPrefs.GetString(slot + PhotonPlayerProperty.statSKILL);
        costume.setBodyByCostumeId(-1);
        costume.setMesh2();
        costume.setTexture();
        return costume;
    }

    public static HeroCostume PhotonDataToHeroCostume2(PhotonPlayer player)
    {
        HeroCostume costume = new HeroCostume();
        SEX sex = IntToSex((int) player.customProperties[PhotonPlayerProperty.sex]);
        costume = new HeroCostume {
            sex = sex,
            costumeId = (int) player.customProperties[PhotonPlayerProperty.costumeId],
            id = (int) player.customProperties[PhotonPlayerProperty.heroCostumeId],
            cape = (bool) player.customProperties[PhotonPlayerProperty.cape],
            hairInfo = (sex != SEX.MALE) ? CostumeHair.hairsF[(int) player.customProperties[PhotonPlayerProperty.hairInfo]] : CostumeHair.hairsM[(int) player.customProperties[PhotonPlayerProperty.hairInfo]],
            eye_texture_id = (int) player.customProperties[PhotonPlayerProperty.eye_texture_id],
            beard_texture_id = (int) player.customProperties[PhotonPlayerProperty.beard_texture_id],
            glass_texture_id = (int) player.customProperties[PhotonPlayerProperty.glass_texture_id],
            skin_color = (int) player.customProperties[PhotonPlayerProperty.skin_color],
            hair_color = new Color((float) player.customProperties[PhotonPlayerProperty.hair_color1], (float) player.customProperties[PhotonPlayerProperty.hair_color2], (float) player.customProperties[PhotonPlayerProperty.hair_color3]),
            division = IntToDivision((int) player.customProperties[PhotonPlayerProperty.division]),
            stat = new HeroStat()
        };
        costume.stat.SPD = (int) player.customProperties[PhotonPlayerProperty.statSPD];
        costume.stat.GAS = (int) player.customProperties[PhotonPlayerProperty.statGAS];
        costume.stat.BLA = (int) player.customProperties[PhotonPlayerProperty.statBLA];
        costume.stat.ACL = (int) player.customProperties[PhotonPlayerProperty.statACL];
        costume.stat.skillId = (string) player.customProperties[PhotonPlayerProperty.statSKILL];
        if ((costume.costumeId == 0x19) && (costume.sex == SEX.FEMALE))
        {
            costume.costumeId = 0x1a;
        }
        costume.setBodyByCostumeId(-1);
        costume.setMesh2();
        costume.setTexture();
        return costume;
    }

    private static int SexToInt(SEX id)
    {
        if (id == SEX.FEMALE)
        {
            return 0;
        }
        if (id == SEX.MALE)
        {
            return 1;
        }
        return 1;
    }

    private static int UniformTypeToInt(UNIFORM_TYPE id)
    {
        if (id == UNIFORM_TYPE.CasualA)
        {
            return 0;
        }
        if (id == UNIFORM_TYPE.CasualB)
        {
            return 1;
        }
        if (id != UNIFORM_TYPE.UniformA)
        {
            if (id == UNIFORM_TYPE.UniformB)
            {
                return 3;
            }
            if (id == UNIFORM_TYPE.CasualAHSS)
            {
                return 4;
            }
        }
        return 2;
    }
}

