




using System;
using UnityEngine;

public class PopuplistCharacterSelection : MonoBehaviour
{
    public GameObject ACL;
    public GameObject BLA;
    public GameObject GAS;
    public GameObject SPD;

    private void onCharacterChange()
    {
        HeroStat stat;
        string selection = base.GetComponent<UIPopupList>().selection;
        switch (selection)
        {
            case "Set 1":
            case "Set 2":
            case "Set 3":
            {
                HeroCostume costume = CostumeConeveter.LocalDataToHeroCostume(selection.ToUpper());
                if (costume == null)
                {
                    stat = new HeroStat();
                }
                else
                {
                    stat = costume.stat;
                }
                break;
            }
            default:
                stat = HeroStat.getInfo(base.GetComponent<UIPopupList>().selection);
                break;
        }
        this.SPD.transform.localScale = new Vector3((float) stat.SPD, 20f, 0f);
        this.GAS.transform.localScale = new Vector3((float) stat.GAS, 20f, 0f);
        this.BLA.transform.localScale = new Vector3((float) stat.BLA, 20f, 0f);
        this.ACL.transform.localScale = new Vector3((float) stat.ACL, 20f, 0f);
    }
}

