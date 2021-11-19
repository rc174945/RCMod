using System;
using System.Collections;
using UnityEngine;
using Settings;
using GameProgress;

public class TriggerColliderWeapon : MonoBehaviour
{
    public bool active_me;
    public GameObject currentCamera;
    public ArrayList currentHits = new ArrayList();
    public ArrayList currentHitsII = new ArrayList();
    public AudioSource meatDie;
    public int myTeam = 1;
    public float scoreMulti = 1f;

    private bool checkIfBehind(GameObject titan)
    {
        Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
        Vector3 to = base.transform.position - transform.transform.position;
        return (Vector3.Angle(-transform.transform.forward, to) < 70f);
    }

    public void clearHits()
    {
        this.currentHitsII = new ArrayList();
        this.currentHits = new ArrayList();
    }

    private void napeMeat(Vector3 vkill, Transform titan)
    {
        Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
        GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("titanNapeMeat"), transform.position, transform.rotation);
        obj2.transform.localScale = titan.localScale;
        obj2.rigidbody.AddForce((Vector3) (vkill.normalized * 15f), ForceMode.Impulse);
        obj2.rigidbody.AddForce((Vector3) (-titan.forward * 10f), ForceMode.Impulse);
        obj2.rigidbody.AddTorque(new Vector3((float) UnityEngine.Random.Range(-100, 100), (float) UnityEngine.Random.Range(-100, 100), (float) UnityEngine.Random.Range(-100, 100)), ForceMode.Impulse);
    }

    private void OnTriggerStay(Collider other)
    {
        if (this.active_me)
        {
            HERO me = transform.root.GetComponent<HERO>();
            if (!this.currentHitsII.Contains(other.gameObject))
            {
                this.currentHitsII.Add(other.gameObject);
                this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.1f, 0.1f, 0.95f);
                if (other.transform.root.gameObject.tag == "titan")
                {
                    GameObject obj2;
                    me.slashHit.Play();
                    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                    {
                        obj2 = PhotonNetwork.Instantiate("hitMeat", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
                    }
                    else
                    {
                        obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("hitMeat"));
                    }
                    obj2.transform.position = base.transform.position;
                    me.useBlade(0);
                }
            }
            if (other.gameObject.tag == "playerHitbox")
            {
                if (LevelInfo.getInfo(FengGameManagerMKII.level).pvp)
                {
                    float b = 1f - (Vector3.Distance(other.gameObject.transform.position, base.transform.position) * 0.05f);
                    b = Mathf.Min(1f, b);
                    HitBox component = other.gameObject.GetComponent<HitBox>();
                    if ((((component != null) && (component.transform.root != null)) && (component.transform.root.GetComponent<HERO>().myTeam != this.myTeam)) && !component.transform.root.GetComponent<HERO>().isInvincible())
                    {
                        HERO hero = component.transform.root.GetComponent<HERO>();
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            if (!hero.isGrabbed)
                            {
                                Vector3 vector = hero.transform.position - base.transform.position;
                                hero.die((Vector3) (((vector.normalized * b) * 1000f) + (Vector3.up * 50f)), false);
                                GameProgressManager.RegisterHumanKill(me.gameObject, hero, KillWeapon.Blade);
                            }
                        }
                        else if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && !hero.HasDied()) && !hero.isGrabbed)
                        {
                            hero.markDie();
                            object[] parameters = new object[5];
                            Vector3 vector2 = component.transform.root.position - base.transform.position;
                            parameters[0] = (Vector3) (((vector2.normalized * b) * 1000f) + (Vector3.up * 50f));
                            parameters[1] = false;
                            parameters[2] = base.transform.root.gameObject.GetPhotonView().viewID;
                            parameters[3] = PhotonView.Find(base.transform.root.gameObject.GetPhotonView().viewID).owner.customProperties[PhotonPlayerProperty.name];
                            parameters[4] = false;
                            hero.photonView.RPC("netDie", PhotonTargets.All, parameters);
                            GameProgressManager.RegisterHumanKill(me.gameObject, hero, KillWeapon.Blade);
                        }
                    }
                }
            }
            else if (other.gameObject.tag == "titanneck")
            {
                HitBox item = other.gameObject.GetComponent<HitBox>();
                if (((item != null) && this.checkIfBehind(item.transform.root.gameObject)) && !this.currentHits.Contains(item))
                {
                    item.hitPosition = (Vector3) ((base.transform.position + item.transform.position) * 0.5f);
                    this.currentHits.Add(item);
                    this.meatDie.Play();
                    TITAN titan = item.transform.root.GetComponent<TITAN>();
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if ((titan != null) && !titan.hasDie)
                        {
                            Vector3 vector3 = me.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                            int num2 = (int) ((vector3.magnitude * 10f) * this.scoreMulti);
                            num2 = Mathf.Max(10, num2);
                            if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
                            {
                                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num2, item.transform.root.gameObject, 0.02f);
                            }
                            titan.die();
                            this.napeMeat(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, item.transform.root);
                            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().netShowDamage(num2);
                            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ReportKillToChatFeed("You", "Titan", num2);
                            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().playerKillInfoSingleUpdate(num2);
                            GameProgressManager.RegisterDamage(me.gameObject, titan.gameObject, KillWeapon.Blade, num2);
                            GameProgressManager.RegisterTitanKill(me.gameObject, titan, KillWeapon.Blade);
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (titan != null)
                        {
                            if (!titan.hasDie)
                            {
                                Vector3 vector4 = me.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                                int num3 = (int) ((vector4.magnitude * 10f) * this.scoreMulti);
                                num3 = Mathf.Max(10, num3);
                                if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
                                {
                                    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num3, item.transform.root.gameObject, 0.02f);
                                    item.transform.root.GetComponent<TITAN>().asClientLookTarget = false;
                                }
                                object[] objArray2 = new object[] { me.gameObject.GetPhotonView().viewID, num3 };
                                item.transform.root.GetComponent<TITAN>().photonView.RPC("titanGetHit", titan.photonView.owner, objArray2);
                                GameProgressManager.RegisterDamage(me.gameObject, titan.gameObject, KillWeapon.Blade, num3);
                                if (titan.WillDIe(num3))
                                    GameProgressManager.RegisterTitanKill(me.gameObject, titan, KillWeapon.Blade);
                            }
                        }
                        else if (item.transform.root.GetComponent<FEMALE_TITAN>() != null)
                        {
                            base.transform.root.GetComponent<HERO>().useBlade(0x7fffffff);
                            Vector3 vector5 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                            int num4 = (int) ((vector5.magnitude * 10f) * this.scoreMulti);
                            num4 = Mathf.Max(10, num4);
                            if (!item.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                object[] objArray3 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num4 };
                                item.transform.root.GetComponent<FEMALE_TITAN>().photonView.RPC("titanGetHit", item.transform.root.GetComponent<FEMALE_TITAN>().photonView.owner, objArray3);
                            }
                        }
                        else if (item.transform.root.GetComponent<COLOSSAL_TITAN>() != null)
                        {
                            base.transform.root.GetComponent<HERO>().useBlade(0x7fffffff);
                            if (!item.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                            {
                                Vector3 vector6 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                                int num5 = (int) ((vector6.magnitude * 10f) * this.scoreMulti);
                                num5 = Mathf.Max(10, num5);
                                object[] objArray4 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num5 };
                                item.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.RPC("titanGetHit", item.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.owner, objArray4);
                            }
                        }
                    }
                    else if (titan != null)
                    {
                        if (!titan.hasDie)
                        {
                            Vector3 vector7 = me.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                            int num6 = (int) ((vector7.magnitude * 10f) * this.scoreMulti);
                            num6 = Mathf.Max(10, num6);
                            if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
                            {
                                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num6, item.transform.root.gameObject, 0.02f);
                            }
                            titan.titanGetHit(me.gameObject.GetPhotonView().viewID, num6);
                            GameProgressManager.RegisterDamage(me.gameObject, titan.gameObject, KillWeapon.Blade, num6);
                            if (titan.WillDIe(num6))
                                GameProgressManager.RegisterTitanKill(me.gameObject, titan, KillWeapon.Blade);
                        }
                    }
                    else if (item.transform.root.GetComponent<FEMALE_TITAN>() != null)
                    {
                        base.transform.root.GetComponent<HERO>().useBlade(0x7fffffff);
                        if (!item.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            Vector3 vector8 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                            int num7 = (int) ((vector8.magnitude * 10f) * this.scoreMulti);
                            num7 = Mathf.Max(10, num7);
                            if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
                            {
                                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num7, null, 0.02f);
                            }
                            item.transform.root.GetComponent<FEMALE_TITAN>().titanGetHit(base.transform.root.gameObject.GetPhotonView().viewID, num7);
                        }
                    }
                    else if (item.transform.root.GetComponent<COLOSSAL_TITAN>() != null)
                    {
                        base.transform.root.GetComponent<HERO>().useBlade(0x7fffffff);
                        if (!item.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                        {
                            Vector3 vector9 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - item.transform.root.rigidbody.velocity;
                            int num8 = (int) ((vector9.magnitude * 10f) * this.scoreMulti);
                            num8 = Mathf.Max(10, num8);
                            if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
                            {
                                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num8, null, 0.02f);
                            }
                            item.transform.root.GetComponent<COLOSSAL_TITAN>().titanGetHit(base.transform.root.gameObject.GetPhotonView().viewID, num8);
                        }
                    }
                    this.showCriticalHitFX();
                }
            }
            else if (other.gameObject.tag == "titaneye")
            {
                if (!this.currentHits.Contains(other.gameObject))
                {
                    this.currentHits.Add(other.gameObject);
                    GameObject gameObject = other.gameObject.transform.root.gameObject;
                    if (gameObject.GetComponent<FEMALE_TITAN>() != null)
                    {
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                gameObject.GetComponent<FEMALE_TITAN>().hitEye();
                            }
                        }
                        else if (!PhotonNetwork.isMasterClient)
                        {
                            if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                object[] objArray5 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID };
                                gameObject.GetComponent<FEMALE_TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, objArray5);
                            }
                        }
                        else if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject.GetComponent<FEMALE_TITAN>().hitEyeRPC(base.transform.root.gameObject.GetPhotonView().viewID);
                        }
                    }
                    else if (gameObject.GetComponent<TITAN>().abnormalType != AbnormalType.TYPE_CRAWLER)
                    {
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            if (!gameObject.GetComponent<TITAN>().hasDie)
                            {
                                gameObject.GetComponent<TITAN>().hitEye();
                            }
                        }
                        else if (!PhotonNetwork.isMasterClient)
                        {
                            if (!gameObject.GetComponent<TITAN>().hasDie)
                            {
                                object[] objArray6 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID };
                                gameObject.GetComponent<TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, objArray6);
                            }
                        }
                        else if (!gameObject.GetComponent<TITAN>().hasDie)
                        {
                            gameObject.GetComponent<TITAN>().hitEyeRPC(base.transform.root.gameObject.GetPhotonView().viewID);
                        }
                        this.showCriticalHitFX();
                    }
                }
            }
            else if ((other.gameObject.tag == "titanankle") && !this.currentHits.Contains(other.gameObject))
            {
                this.currentHits.Add(other.gameObject);
                GameObject obj4 = other.gameObject.transform.root.gameObject;
                Vector3 vector10 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - obj4.rigidbody.velocity;
                int num9 = (int) ((vector10.magnitude * 10f) * this.scoreMulti);
                num9 = Mathf.Max(10, num9);
                if ((obj4.GetComponent<TITAN>() != null) && (obj4.GetComponent<TITAN>().abnormalType != AbnormalType.TYPE_CRAWLER))
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (!obj4.GetComponent<TITAN>().hasDie)
                        {
                            obj4.GetComponent<TITAN>().hitAnkle();
                        }
                    }
                    else
                    {
                        if (!PhotonNetwork.isMasterClient)
                        {
                            if (!obj4.GetComponent<TITAN>().hasDie)
                            {
                                object[] objArray7 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID };
                                obj4.GetComponent<TITAN>().photonView.RPC("hitAnkleRPC", PhotonTargets.MasterClient, objArray7);
                            }
                        }
                        else if (!obj4.GetComponent<TITAN>().hasDie)
                        {
                            obj4.GetComponent<TITAN>().hitAnkle();
                        }
                        this.showCriticalHitFX();
                    }
                }
                else if (obj4.GetComponent<FEMALE_TITAN>() != null)
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (other.gameObject.name == "ankleR")
                        {
                            if ((obj4.GetComponent<FEMALE_TITAN>() != null) && !obj4.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                obj4.GetComponent<FEMALE_TITAN>().hitAnkleR(num9);
                            }
                        }
                        else if ((obj4.GetComponent<FEMALE_TITAN>() != null) && !obj4.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            obj4.GetComponent<FEMALE_TITAN>().hitAnkleL(num9);
                        }
                    }
                    else if (other.gameObject.name == "ankleR")
                    {
                        if (!PhotonNetwork.isMasterClient)
                        {
                            if (!obj4.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                object[] objArray8 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num9 };
                                obj4.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleRRPC", PhotonTargets.MasterClient, objArray8);
                            }
                        }
                        else if (!obj4.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            obj4.GetComponent<FEMALE_TITAN>().hitAnkleRRPC(base.transform.root.gameObject.GetPhotonView().viewID, num9);
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (!obj4.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            object[] objArray9 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num9 };
                            obj4.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleLRPC", PhotonTargets.MasterClient, objArray9);
                        }
                    }
                    else if (!obj4.GetComponent<FEMALE_TITAN>().hasDie)
                    {
                        obj4.GetComponent<FEMALE_TITAN>().hitAnkleLRPC(base.transform.root.gameObject.GetPhotonView().viewID, num9);
                    }
                    this.showCriticalHitFX();
                }
            }
        }
    }

    private void showCriticalHitFX()
    {
        GameObject obj2;
        this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f, 0.95f);
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            obj2 = PhotonNetwork.Instantiate("redCross", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        }
        else
        {
            obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("redCross"));
        }
        obj2.transform.position = base.transform.position;
    }

    private void Start()
    {
        this.currentCamera = GameObject.Find("MainCamera");
    }
}

