




using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LoginFengKAI : MonoBehaviour
{
    private string ChangeGuildURL = "http://aotskins.com/version/guild.php";
    private string ChangePasswordURL = "http://fenglee.com/game/aog/change_password.php";
    private string CheckUserURL = "http://aotskins.com/version/login.php";
    private string ForgetPasswordURL = "http://fenglee.com/game/aog/forget_password.php";
    public string formText = string.Empty;
    private string GetInfoURL = "http://aotskins.com/version/getinfo.php";
    public PanelLoginGroupManager loginGroup;
    public GameObject output;
    public GameObject output2;
    public GameObject panelChangeGUILDNAME;
    public GameObject panelChangePassword;
    public GameObject panelForget;
    public GameObject panelLogin;
    public GameObject panelRegister;
    public GameObject panelStatus;
    public static PlayerInfoPHOTON player;
    private static string playerGUILDName = string.Empty;
    private static string playerName = string.Empty;
    private static string playerPassword = string.Empty;
    private string RegisterURL = "http://fenglee.com/game/aog/signup_check.php";

    public void cGuild(string name)
    {
        if (playerName == string.Empty)
        {
            this.logout();
            NGUITools.SetActive(this.panelChangeGUILDNAME, false);
            NGUITools.SetActive(this.panelLogin, true);
            this.output.GetComponent<UILabel>().text = "Please sign in.";
        }
        else
        {
            base.StartCoroutine(this.changeGuild(name));
        }
    }

    [DebuggerHidden]
    private IEnumerator changeGuild(string name)
    {
        return new changeGuildcIterator5 { name = name, Sname = name, fthis = this };
    }

    [DebuggerHidden]
    private IEnumerator changePassword(string oldpassword, string password, string password2)
    {
        return new changePasswordcIterator4 { oldpassword = oldpassword, password = password, password2 = password2, Soldpassword = oldpassword, Spassword = password, Spassword2 = password2, fthis = this };
    }

    private void clearCOOKIE()
    {
        playerName = string.Empty;
        playerPassword = string.Empty;
    }

    public void cpassword(string oldpassword, string password, string password2)
    {
        if (playerName == string.Empty)
        {
            this.logout();
            NGUITools.SetActive(this.panelChangePassword, false);
            NGUITools.SetActive(this.panelLogin, true);
            this.output.GetComponent<UILabel>().text = "Please sign in.";
        }
        else
        {
            base.StartCoroutine(this.changePassword(oldpassword, password, password2));
        }
    }

    [DebuggerHidden]
    private IEnumerator ForgetPassword(string email)
    {
        return new ForgetPasswordcIterator6 { email = email, Semail = email, fthis = this };
    }

    [DebuggerHidden]
    private IEnumerator getInfo()
    {
        return new getInfocIterator2 { fthis = this };
    }

    public void login(string name, string password)
    {
        base.StartCoroutine(this.Login(name, password));
    }

    [DebuggerHidden]
    private IEnumerator Login(string name, string password)
    {
        return new LogincIterator1 { name = name, password = password, Sname = name, Spassword = password, fthis = this };
    }

    public void logout()
    {
        this.clearCOOKIE();
        player = new PlayerInfoPHOTON();
        player.initAsGuest();
        this.output.GetComponent<UILabel>().text = "Welcome," + player.name;
    }

    [DebuggerHidden]
    private IEnumerator Register(string name, string password, string password2, string email)
    {
        return new RegistercIterator3 { name = name, password = password, password2 = password2, email = email, Sname = name, Spassword = password, Spassword2 = password2, Semail = email, fthis = this };
    }

    public void resetPassword(string email)
    {
        base.StartCoroutine(this.ForgetPassword(email));
    }

    public void signup(string name, string password, string password2, string email)
    {
        base.StartCoroutine(this.Register(name, password, password2, email));
    }

    private void Start()
    {
        if (player == null)
        {
            player = new PlayerInfoPHOTON();
            player.initAsGuest();
        }
        if (playerName != string.Empty)
        {
            NGUITools.SetActive(this.panelLogin, false);
            NGUITools.SetActive(this.panelStatus, true);
            base.StartCoroutine(this.getInfo());
        }
        else
        {
            this.output.GetComponent<UILabel>().text = "Welcome," + player.name;
        }
    }

    [CompilerGenerated]
    private sealed class changeGuildcIterator5 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal string Sname;
        internal LoginFengKAI fthis;
        internal WWWForm form0;
        internal WWW w1;
        internal string name;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.form0 = new WWWForm();
                    this.form0.AddField("name", LoginFengKAI.playerName);
                    this.form0.AddField("guildname", this.name);
                    this.w1 = new WWW(this.fthis.ChangeGuildURL, this.form0);
                    this.Scurrent = this.w1;
                    this.SPC = 1;
                    return true;

                case 1:
                    if (this.w1.error == null)
                    {
                        this.fthis.output.GetComponent<UILabel>().text = this.w1.text;
                        if (this.w1.text.Contains("Guild name set."))
                        {
                            NGUITools.SetActive(this.fthis.panelChangeGUILDNAME, false);
                            NGUITools.SetActive(this.fthis.panelStatus, true);
                            this.fthis.StartCoroutine(this.fthis.getInfo());
                        }
                        this.w1.Dispose();
                        break;
                    }
                    MonoBehaviour.print(this.w1.error);
                    break;

                default:
                    goto Label_0135;
            }
            this.SPC = -1;
        Label_0135:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }

    [CompilerGenerated]
    private sealed class changePasswordcIterator4 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal string Soldpassword;
        internal string Spassword;
        internal string Spassword2;
        internal LoginFengKAI fthis;
        internal WWWForm form0;
        internal WWW w1;
        internal string oldpassword;
        internal string password;
        internal string password2;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.form0 = new WWWForm();
                    this.form0.AddField("userid", LoginFengKAI.playerName);
                    this.form0.AddField("old_password", this.oldpassword);
                    this.form0.AddField("password", this.password);
                    this.form0.AddField("password2", this.password2);
                    this.w1 = new WWW(this.fthis.ChangePasswordURL, this.form0);
                    this.Scurrent = this.w1;
                    this.SPC = 1;
                    return true;

                case 1:
                    if (this.w1.error == null)
                    {
                        this.fthis.output.GetComponent<UILabel>().text = this.w1.text;
                        if (this.w1.text.Contains("Thanks, Your password changed successfully"))
                        {
                            NGUITools.SetActive(this.fthis.panelChangePassword, false);
                            NGUITools.SetActive(this.fthis.panelLogin, true);
                        }
                        this.w1.Dispose();
                        break;
                    }
                    MonoBehaviour.print(this.w1.error);
                    break;

                default:
                    goto Label_014A;
            }
            this.SPC = -1;
        Label_014A:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }

    [CompilerGenerated]
    private sealed class ForgetPasswordcIterator6 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal string Semail;
        internal LoginFengKAI fthis;
        internal WWWForm form0;
        internal WWW w1;
        internal string email;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.form0 = new WWWForm();
                    this.form0.AddField("email", this.email);
                    this.w1 = new WWW(this.fthis.ForgetPasswordURL, this.form0);
                    this.Scurrent = this.w1;
                    this.SPC = 1;
                    return true;

                case 1:
                    if (this.w1.error == null)
                    {
                        this.fthis.output.GetComponent<UILabel>().text = this.w1.text;
                        this.w1.Dispose();
                        NGUITools.SetActive(this.fthis.panelForget, false);
                        NGUITools.SetActive(this.fthis.panelLogin, true);
                        break;
                    }
                    MonoBehaviour.print(this.w1.error);
                    break;

                default:
                    goto Label_00FA;
            }
            this.fthis.clearCOOKIE();
            this.SPC = -1;
        Label_00FA:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }

    [CompilerGenerated]
    private sealed class getInfocIterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal LoginFengKAI fthis;
        internal WWWForm form0;
        internal string[] result2;
        internal WWW w1;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.form0 = new WWWForm();
                    this.form0.AddField("userid", LoginFengKAI.playerName);
                    this.form0.AddField("password", LoginFengKAI.playerPassword);
                    this.w1 = new WWW(this.fthis.GetInfoURL, this.form0);
                    this.Scurrent = this.w1;
                    this.SPC = 1;
                    return true;

                case 1:
                    if (this.w1.error == null)
                    {
                        if (this.w1.text.Contains("Error,please sign in again."))
                        {
                            NGUITools.SetActive(this.fthis.panelLogin, true);
                            NGUITools.SetActive(this.fthis.panelStatus, false);
                            this.fthis.output.GetComponent<UILabel>().text = this.w1.text;
                            LoginFengKAI.playerName = string.Empty;
                            LoginFengKAI.playerPassword = string.Empty;
                        }
                        else
                        {
                            char[] separator = new char[] { '|' };
                            this.result2 = this.w1.text.Split(separator);
                            LoginFengKAI.playerGUILDName = this.result2[0];
                            this.fthis.output2.GetComponent<UILabel>().text = this.result2[1];
                            LoginFengKAI.player.name = LoginFengKAI.playerName;
                            LoginFengKAI.player.guildname = LoginFengKAI.playerGUILDName;
                        }
                        this.w1.Dispose();
                        break;
                    }
                    MonoBehaviour.print(this.w1.error);
                    break;

                default:
                    goto Label_01A7;
            }
            this.SPC = -1;
        Label_01A7:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }

    [CompilerGenerated]
    private sealed class LogincIterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal string Sname;
        internal string Spassword;
        internal LoginFengKAI fthis;
        internal WWWForm form0;
        internal WWW w1;
        internal string name;
        internal string password;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.form0 = new WWWForm();
                    this.form0.AddField("userid", this.name);
                    this.form0.AddField("password", this.password);
                    this.form0.AddField("version", UIMainReferences.Version);
                    this.w1 = new WWW(this.fthis.CheckUserURL, this.form0);
                    this.Scurrent = this.w1;
                    this.SPC = 1;
                    return true;

                case 1:
                    this.fthis.clearCOOKIE();
                    if (this.w1.error == null)
                    {
                        this.fthis.output.GetComponent<UILabel>().text = this.w1.text;
                        this.fthis.formText = this.w1.text;
                        this.w1.Dispose();
                        if (this.fthis.formText.Contains("Welcome back") && this.fthis.formText.Contains("(^o^)/~"))
                        {
                            NGUITools.SetActive(this.fthis.panelLogin, false);
                            NGUITools.SetActive(this.fthis.panelStatus, true);
                            LoginFengKAI.playerName = this.name;
                            LoginFengKAI.playerPassword = this.password;
                            this.fthis.StartCoroutine(this.fthis.getInfo());
                        }
                        break;
                    }
                    MonoBehaviour.print(this.w1.error);
                    break;

                default:
                    goto Label_019C;
            }
            this.SPC = -1;
        Label_019C:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }

    [CompilerGenerated]
    private sealed class RegistercIterator3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal string Semail;
        internal string Sname;
        internal string Spassword;
        internal string Spassword2;
        internal LoginFengKAI fthis;
        internal WWWForm form0;
        internal WWW w1;
        internal string email;
        internal string name;
        internal string password;
        internal string password2;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.form0 = new WWWForm();
                    this.form0.AddField("userid", this.name);
                    this.form0.AddField("password", this.password);
                    this.form0.AddField("password2", this.password2);
                    this.form0.AddField("email", this.email);
                    this.w1 = new WWW(this.fthis.RegisterURL, this.form0);
                    this.Scurrent = this.w1;
                    this.SPC = 1;
                    return true;

                case 1:
                    if (this.w1.error == null)
                    {
                        this.fthis.output.GetComponent<UILabel>().text = this.w1.text;
                        if (this.w1.text.Contains("Final step,to activate your account, please click the link in the activation email"))
                        {
                            NGUITools.SetActive(this.fthis.panelRegister, false);
                            NGUITools.SetActive(this.fthis.panelLogin, true);
                        }
                        this.w1.Dispose();
                        break;
                    }
                    MonoBehaviour.print(this.w1.error);
                    break;

                default:
                    goto Label_0156;
            }
            this.fthis.clearCOOKIE();
            this.SPC = -1;
        Label_0156:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }
}

