using UnityEngine;
using Settings;

public class BombUtil
{
    public static Color GetBombColor(PhotonPlayer player, float minAlpha=0.5f)
    {
        if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
        {
            int team = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.RCteam]);
            if (team == 1)
                return Color.cyan;
            else if (team == 2)
                return Color.magenta;
        }
        return GetBombColorIndividual(player, minAlpha);
    }
    static Color GetBombColorIndividual(PhotonPlayer player, float minAlpha)
    {

        float r = RCextensions.returnFloatFromObject(player.customProperties[PhotonPlayerProperty.RCBombR]);
        float g = RCextensions.returnFloatFromObject(player.customProperties[PhotonPlayerProperty.RCBombG]);
        float b = RCextensions.returnFloatFromObject(player.customProperties[PhotonPlayerProperty.RCBombB]);
        float a = RCextensions.returnFloatFromObject(player.customProperties[PhotonPlayerProperty.RCBombA]);
        a = Mathf.Max(minAlpha, a);
        return new Color(r, g, b, a);
    }
}
