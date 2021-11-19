




using System;
using UnityEngine;

public class HERO_ON_MENU : MonoBehaviour
{
    private Vector3 cameraOffset;
    private Transform cameraPref;
    public int costumeId;
    private Transform head;
    public float headRotationX;
    public float headRotationY;

    private void LateUpdate()
    {
        this.head.rotation = Quaternion.Euler(this.head.rotation.eulerAngles.x + this.headRotationX, this.head.rotation.eulerAngles.y + this.headRotationY, this.head.rotation.eulerAngles.z);
        if (this.costumeId == 9)
        {
            GameObject.Find("MainCamera_Mono").transform.position = this.cameraPref.position + this.cameraOffset;
        }
    }

    private void Start()
    {
        HERO_SETUP component = base.gameObject.GetComponent<HERO_SETUP>();
        HeroCostume.init2();
        component.init();
        component.myCostume = HeroCostume.costume[this.costumeId];
        component.setCharacterComponent();
        this.head = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
        this.cameraPref = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
        if (this.costumeId == 9)
        {
            this.cameraOffset = GameObject.Find("MainCamera_Mono").transform.position - this.cameraPref.position;
        }
        if (component.myCostume.sex == SEX.FEMALE)
        {
            base.animation.Play("stand");
            base.animation["stand"].normalizedTime = UnityEngine.Random.Range((float) 0f, (float) 1f);
        }
        else
        {
            base.animation.Play("stand_levi");
            base.animation["stand_levi"].normalizedTime = UnityEngine.Random.Range((float) 0f, (float) 1f);
        }
        float num = 0.5f;
        base.animation["stand"].speed = num;
        base.animation["stand_levi"].speed = num;
    }
}

