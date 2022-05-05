




using Settings;
using System;
using UnityEngine;

public class TITAN_CONTROLLER : MonoBehaviour
{
    public bool bite;
    public bool bitel;
    public bool biter;
    public bool chopl;
    public bool chopr;
    public bool choptl;
    public bool choptr;
    public bool cover;
    public Camera currentCamera;
    public float currentDirection;
    public bool grabbackl;
    public bool grabbackr;
    public bool grabfrontl;
    public bool grabfrontr;
    public bool grabnapel;
    public bool grabnaper;
    public bool isAttackDown;
    public bool isAttackIIDown;
    public bool isHorse;
    public bool isJumpDown;
    public bool isSuicide;
    public bool isWALKDown;
    public bool sit;
    public float targetDirection;

    private void Start()
    {
        this.currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            base.enabled = false;
        }
    }

    private void Update()
    {
        int num;
        int num2;
        float y;
        float num4;
        float num5;
        float num6;
        if (this.isHorse)
        {
            if (SettingsManager.InputSettings.General.Forward.GetKey())
            {
                num = 1;
            }
            else if (SettingsManager.InputSettings.General.Back.GetKey())
            {
                num = -1;
            }
            else
            {
                num = 0;
            }
            if (SettingsManager.InputSettings.General.Left.GetKey())
            {
                num2 = -1;
            }
            else if (SettingsManager.InputSettings.General.Right.GetKey())
            {
                num2 = 1;
            }
            else
            {
                num2 = 0;
            }
            if ((num2 != 0) || (num != 0))
            {
                y = this.currentCamera.transform.rotation.eulerAngles.y;
                num4 = Mathf.Atan2((float) num, (float) num2) * 57.29578f;
                num4 = -num4 + 90f;
                num5 = y + num4;
                this.targetDirection = num5;
            }
            else
            {
                this.targetDirection = -874f;
            }
            this.isAttackDown = false;
            this.isAttackIIDown = false;
            if (this.targetDirection != -874f)
            {
                this.currentDirection = this.targetDirection;
            }
            num6 = this.currentCamera.transform.rotation.eulerAngles.y - this.currentDirection;
            if (num6 >= 180f)
            {
                num6 -= 360f;
            }
            if (SettingsManager.InputSettings.Human.HorseJump.GetKey())
            {
                this.isAttackDown = true;
            }
            this.isWALKDown = SettingsManager.InputSettings.Human.HorseWalk.GetKey();
        }
        else
        {
            if (SettingsManager.InputSettings.General.Forward.GetKey())
            {
                num = 1;
            }
            else if (SettingsManager.InputSettings.General.Back.GetKey())
            {
                num = -1;
            }
            else
            {
                num = 0;
            }
            if (SettingsManager.InputSettings.General.Left.GetKey())
            {
                num2 = -1;
            }
            else if (SettingsManager.InputSettings.General.Right.GetKey())
            {
                num2 = 1;
            }
            else
            {
                num2 = 0;
            }
            if ((num2 != 0) || (num != 0))
            {
                y = this.currentCamera.transform.rotation.eulerAngles.y;
                num4 = Mathf.Atan2((float) num, (float) num2) * 57.29578f;
                num4 = -num4 + 90f;
                num5 = y + num4;
                this.targetDirection = num5;
            }
            else
            {
                this.targetDirection = -874f;
            }
            this.isAttackDown = false;
            this.isJumpDown = false;
            this.isAttackIIDown = false;
            this.isSuicide = false;
            this.grabbackl = false;
            this.grabbackr = false;
            this.grabfrontl = false;
            this.grabfrontr = false;
            this.grabnapel = false;
            this.grabnaper = false;
            this.choptl = false;
            this.chopr = false;
            this.chopl = false;
            this.choptr = false;
            this.bite = false;
            this.bitel = false;
            this.biter = false;
            this.cover = false;
            this.sit = false;
            if (this.targetDirection != -874f)
            {
                this.currentDirection = this.targetDirection;
            }
            num6 = this.currentCamera.transform.rotation.eulerAngles.y - this.currentDirection;
            if (num6 >= 180f)
            {
                num6 -= 360f;
            }
            if (SettingsManager.InputSettings.Titan.AttackPunch.GetKey())
            {
                this.isAttackDown = true;
            }
            if (SettingsManager.InputSettings.Titan.AttackSlam.GetKey())
            {
                this.isAttackIIDown = true;
            }
            if (SettingsManager.InputSettings.Titan.Jump.GetKey())
            {
                this.isJumpDown = true;
            }
            if (SettingsManager.InputSettings.General.ChangeCharacter.GetKey())
            {
                this.isSuicide = true;
            }
            if (SettingsManager.InputSettings.Titan.CoverNape.GetKey())
            {
                this.cover = true;
            }
            if (SettingsManager.InputSettings.Titan.Sit.GetKey())
            {
                this.sit = true;
            }
            if (SettingsManager.InputSettings.Titan.AttackGrabFront.GetKey() && (num6 >= 0f))
            {
                this.grabfrontr = true;
            }
            if (SettingsManager.InputSettings.Titan.AttackGrabFront.GetKey() && (num6 < 0f))
            {
                this.grabfrontl = true;
            }
            if (SettingsManager.InputSettings.Titan.AttackGrabBack.GetKey() && (num6 >= 0f))
            {
                this.grabbackr = true;
            }
            if (SettingsManager.InputSettings.Titan.AttackGrabBack.GetKey() && (num6 < 0f))
            {
                this.grabbackl = true;
            }
            if (SettingsManager.InputSettings.Titan.AttackGrabNape.GetKey() && (num6 >= 0f))
            {
                this.grabnaper = true;
            }
            if (SettingsManager.InputSettings.Titan.AttackGrabNape.GetKey() && (num6 < 0f))
            {
                this.grabnapel = true;
            }
            if (SettingsManager.InputSettings.Titan.AttackSlap.GetKey() && (num6 >= 0f))
            {
                this.choptr = true;
            }
            if (SettingsManager.InputSettings.Titan.AttackSlap.GetKey() && (num6 < 0f))
            {
                this.choptl = true;
            }
            if (SettingsManager.InputSettings.Titan.AttackBite.GetKey() && (num6 > 7.5f))
            {
                this.biter = true;
            }
            if (SettingsManager.InputSettings.Titan.AttackBite.GetKey() && (num6 < -7.5f))
            {
                this.bitel = true;
            }
            if ((SettingsManager.InputSettings.Titan.AttackBite.GetKey() && (num6 >= -7.5f)) && (num6 <= 7.5f))
            {
                this.bite = true;
            }
            this.isWALKDown = SettingsManager.InputSettings.Titan.Walk.GetKey();
        }
    }
}

