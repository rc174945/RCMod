using Settings;
using System;
using UnityEngine;

public class LevelTriggerHint : MonoBehaviour
{
    public string content;
    public HintType myhint;
    private bool on;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.on = true;
        }
    }

    private void Start()
    {
        if (!LevelInfo.getInfo(FengGameManagerMKII.level).hint)
        {
            base.enabled = false;
        }
        if (this.content == string.Empty)
        {
            switch (this.myhint)
            {
                case HintType.MOVE:
                {
                    string[] textArray2 = new string[] { "Hello soldier!\nWelcome to Attack On Titan Tribute Game!\n Press [F7D358]", SettingsManager.InputSettings.General.Forward.ToString(), SettingsManager.InputSettings.General.Left.ToString(),
                        SettingsManager.InputSettings.General.Back.ToString(), SettingsManager.InputSettings.General.Right.ToString(), "[-] to Move." };
                    this.content = string.Concat(textArray2);
                    break;
                }
                case HintType.TELE:
                    this.content = "Move to [82FA58]green warp point[-] to proceed.";
                    break;

                case HintType.CAMA:
                {
                    string[] textArray3 = new string[] { "Press [F7D358]", SettingsManager.InputSettings.General.ChangeCamera.ToString(), "[-] to change camera mode\nPress [F7D358]", SettingsManager.InputSettings.General.HideUI.ToString(), "[-] to hide or show the cursor." };
                    this.content = string.Concat(textArray3);
                    break;
                }
                case HintType.JUMP:
                    this.content = "Press [F7D358]" + SettingsManager.InputSettings.Human.Jump.ToString() + "[-] to Jump.";
                    break;

                case HintType.JUMP2:
                    this.content = "Press [F7D358]" + SettingsManager.InputSettings.General.Forward.ToString() + "[-] towards a wall to perform a wall-run.";
                    break;

                case HintType.HOOK:
                {
                    string[] textArray4 = new string[] { "Press and Hold[F7D358] ", SettingsManager.InputSettings.Human.HookLeft.ToString(), "[-] or [F7D358]", SettingsManager.InputSettings.Human.HookRight.ToString(), "[-] to launch your grapple.\nNow Try hooking to the [>3<] box. " };
                    this.content = string.Concat(textArray4);
                    break;
                }
                case HintType.HOOK2:
                {
                    string[] textArray5 = new string[] { "Press and Hold[F7D358] ", SettingsManager.InputSettings.Human.HookBoth.ToString(), "[-] to launch both of your grapples at the same Time.\n\nNow aim between the two black blocks. \nYou will see the mark '<' and '>' appearing on the blocks. \nThen press ", SettingsManager.InputSettings.Human.HookBoth.ToString(), " to hook the blocks." };
                    this.content = string.Concat(textArray5);
                    break;
                }
                case HintType.SUPPLY:
                    this.content = "Press [F7D358]" + SettingsManager.InputSettings.Human.Reload.ToString() + "[-] to reload your blades.\n Move to the supply station to refill your gas and blades.";
                    break;

                case HintType.DODGE:
                    this.content = "Press [F7D358]" + SettingsManager.InputSettings.Human.Dodge.ToString() + "[-] to Dodge.";
                    break;

                case HintType.ATTACK:
                {
                    string[] textArray1 = new string[] { "Press [F7D358]", SettingsManager.InputSettings.Human.AttackDefault.ToString(), "[-] to Attack. \nPress [F7D358]", SettingsManager.InputSettings.Human.AttackSpecial.ToString(), "[-] to use special attack.\n***You can only kill a titan by slashing his [FA5858]NAPE[-].***\n\n" };
                    this.content = string.Concat(textArray1);
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (this.on)
        {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ShowHUDInfoCenter(this.content + "\n\n\n\n\n");
            this.on = false;
        }
    }
}

