using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using CustomSkins;
using Settings;

public class TITAN_SETUP : Photon.MonoBehaviour
{
    public GameObject eye;
    private CostumeHair hair;
    private GameObject hair_go_ref;
    private int hairType;
    public bool haseye;
    public GameObject part_hair;
    public int skin;
    private TitanCustomSkinLoader _customSkinLoader;

    private void Awake()
    {
        CostumeHair.init();
        CharacterMaterials.init();
        HeroCostume.init2();
        this.hair_go_ref = new GameObject();
        this.eye.transform.parent = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").transform;
        this.hair_go_ref.transform.position = (Vector3) ((this.eye.transform.position + (Vector3.up * 3.5f)) + (base.transform.forward * 5.2f));
        this.hair_go_ref.transform.rotation = this.eye.transform.rotation;
        this.hair_go_ref.transform.RotateAround(this.eye.transform.position, base.transform.right, -20f);
        this.hair_go_ref.transform.localScale = new Vector3(210f, 210f, 210f);
        this.hair_go_ref.transform.parent = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").transform;
        _customSkinLoader = gameObject.AddComponent<TitanCustomSkinLoader>();
    }

    public IEnumerator loadskinE(int hair, int eye, string hairlink)
    {
        UnityEngine.Object.Destroy(this.part_hair);
        this.hair = CostumeHair.hairsM[hair];
        this.hairType = hair;
        if (this.hair.hair != string.Empty)
        {
            GameObject iteratorVariable1 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
            iteratorVariable1.transform.parent = this.hair_go_ref.transform.parent;
            iteratorVariable1.transform.position = this.hair_go_ref.transform.position;
            iteratorVariable1.transform.rotation = this.hair_go_ref.transform.rotation;
            iteratorVariable1.transform.localScale = this.hair_go_ref.transform.localScale;
            iteratorVariable1.renderer.material = CharacterMaterials.materials[this.hair.texture];
            this.part_hair = iteratorVariable1;
            yield return StartCoroutine(_customSkinLoader.LoadSkinsFromRPC(new object[] { true, hairlink }));
        }
        if (eye >= 0)
        {
            this.setFacialTexture(this.eye, eye);
        }
        yield return null;
    }

    public void setFacialTexture(GameObject go, int id)
    {
        if (id >= 0)
        {
            float num = 0.25f;
            float num2 = 0.125f;
            float x = num2 * ((int) (((float) id) / 8f));
            float y = -num * (id % 4);
            go.renderer.material.mainTextureOffset = new Vector2(x, y);
        }
    }

    public void setHair2()
    {
        int num;
        object[] objArray2;
        BaseCustomSkinSettings<TitanCustomSkinSet> settings = SettingsManager.CustomSkinSettings.Titan;
        if ((settings.SkinsEnabled.Value) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine))
        {
            TitanCustomSkinSet set = (TitanCustomSkinSet)settings.GetSelectedSet();
            Color color;
            num = UnityEngine.Random.Range(0, 9);
            if (num == 3)
            {
                num = 9;
            }
            int index = this.skin;
            if (set.RandomizedPairs.Value)
            {
                index = UnityEngine.Random.Range(0, 5);
            }
            int hairModel = ((IntSetting)set.HairModels.GetItemAt(index)).Value - 1;
            if (hairModel >= 0)
                num = hairModel;
            string hairlink = ((StringSetting)set.Hairs.GetItemAt(index)).Value;
            int eye = UnityEngine.Random.Range(1, 8);
            if (this.haseye)
            {
                eye = 0;
            }
            bool flag2 = false;
            if ((hairlink.EndsWith(".jpg") || hairlink.EndsWith(".png")) || hairlink.EndsWith(".jpeg"))
            {
                flag2 = true;
            }
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && base.photonView.isMine)
            {
                if (flag2)
                {
                    objArray2 = new object[] { num, eye, hairlink };
                    base.photonView.RPC("setHairRPC2", PhotonTargets.AllBuffered, objArray2);
                }
                else
                {
                    color = HeroCostume.costume[UnityEngine.Random.Range(0, HeroCostume.costume.Length - 5)].hair_color;
                    objArray2 = new object[] { num, eye, color.r, color.g, color.b };
                    base.photonView.RPC("setHairPRC", PhotonTargets.AllBuffered, objArray2);
                }
            }
            else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                if (flag2)
                {
                    base.StartCoroutine(this.loadskinE(num, eye, hairlink));
                }
                else
                {
                    color = HeroCostume.costume[UnityEngine.Random.Range(0, HeroCostume.costume.Length - 5)].hair_color;
                    this.setHairPRC(num, eye, color.r, color.g, color.b);
                }
            }
        }
        else
        {
            num = UnityEngine.Random.Range(0, CostumeHair.hairsM.Length);
            if (num == 3)
            {
                num = 9;
            }
            UnityEngine.Object.Destroy(this.part_hair);
            this.hairType = num;
            this.hair = CostumeHair.hairsM[num];
            if (this.hair.hair == string.Empty)
            {
                this.hair = CostumeHair.hairsM[9];
                this.hairType = 9;
            }
            this.part_hair = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
            this.part_hair.transform.parent = this.hair_go_ref.transform.parent;
            this.part_hair.transform.position = this.hair_go_ref.transform.position;
            this.part_hair.transform.rotation = this.hair_go_ref.transform.rotation;
            this.part_hair.transform.localScale = this.hair_go_ref.transform.localScale;
            this.part_hair.renderer.material = CharacterMaterials.materials[this.hair.texture];
            this.part_hair.renderer.material.color = HeroCostume.costume[UnityEngine.Random.Range(0, HeroCostume.costume.Length - 5)].hair_color;
            int id = UnityEngine.Random.Range(1, 8);
            this.setFacialTexture(this.eye, id);
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && base.photonView.isMine)
            {
                objArray2 = new object[] { this.hairType, id, this.part_hair.renderer.material.color.r, this.part_hair.renderer.material.color.g, this.part_hair.renderer.material.color.b };
                base.photonView.RPC("setHairPRC", PhotonTargets.OthersBuffered, objArray2);
            }
        }
    }

    [RPC]
    private void setHairPRC(int type, int eye_type, float c1, float c2, float c3)
    {
        UnityEngine.Object.Destroy(this.part_hair);
        this.hair = CostumeHair.hairsM[type];
        this.hairType = type;
        if (this.hair.hair != string.Empty)
        {
            GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
            obj2.transform.parent = this.hair_go_ref.transform.parent;
            obj2.transform.position = this.hair_go_ref.transform.position;
            obj2.transform.rotation = this.hair_go_ref.transform.rotation;
            obj2.transform.localScale = this.hair_go_ref.transform.localScale;
            obj2.renderer.material = CharacterMaterials.materials[this.hair.texture];
            obj2.renderer.material.color = new Color(c1, c2, c3);
            this.part_hair = obj2;
        }
        this.setFacialTexture(this.eye, eye_type);
    }

    [RPC]
    public void setHairRPC2(int hair, int eye, string hairlink, PhotonMessageInfo info)
    {
        BaseCustomSkinSettings<TitanCustomSkinSet> settings = SettingsManager.CustomSkinSettings.Titan;
        if (info.sender != photonView.owner)
            return;
        if (settings.SkinsEnabled.Value && (!settings.SkinsLocal.Value || photonView.isMine))
        {
            base.StartCoroutine(this.loadskinE(hair, eye, hairlink));
        }
    }

    public void setPunkHair()
    {
        UnityEngine.Object.Destroy(this.part_hair);
        this.hair = CostumeHair.hairsM[3];
        this.hairType = 3;
        GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
        obj2.transform.parent = this.hair_go_ref.transform.parent;
        obj2.transform.position = this.hair_go_ref.transform.position;
        obj2.transform.rotation = this.hair_go_ref.transform.rotation;
        obj2.transform.localScale = this.hair_go_ref.transform.localScale;
        obj2.renderer.material = CharacterMaterials.materials[this.hair.texture];
        switch (UnityEngine.Random.Range(1, 4))
        {
            case 1:
                obj2.renderer.material.color = FengColor.hairPunk1;
                break;

            case 2:
                obj2.renderer.material.color = FengColor.hairPunk2;
                break;

            case 3:
                obj2.renderer.material.color = FengColor.hairPunk3;
                break;
        }
        this.part_hair = obj2;
        this.setFacialTexture(this.eye, 0);
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && base.photonView.isMine)
        {
            object[] parameters = new object[] { this.hairType, 0, this.part_hair.renderer.material.color.r, this.part_hair.renderer.material.color.g, this.part_hair.renderer.material.color.b };
            base.photonView.RPC("setHairPRC", PhotonTargets.OthersBuffered, parameters);
        }
    }

    public void setVar(int skin, bool haseye)
    {
        this.skin = skin;
        this.haseye = haseye;
    }

}

