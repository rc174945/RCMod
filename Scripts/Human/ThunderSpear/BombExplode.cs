using Photon;
using System;
using UnityEngine;
using System.Collections;
using Settings;

public class BombExplode : Photon.MonoBehaviour
{
    public static float _sizeMultiplier = 1.1f;
    public void Awake()
    {
        if (photonView != null)
        {
            PhotonPlayer owner = photonView.owner;
            float bombRadius = RCextensions.returnFloatFromObject(owner.customProperties[PhotonPlayerProperty.RCBombRadius]);
            float size = Mathf.Clamp(bombRadius, 20f, 60f) * 2f * _sizeMultiplier;
            ParticleSystem particle = GetComponent<ParticleSystem>();
            if (SettingsManager.AbilitySettings.UseOldEffect.Value)
            {
                particle.Stop();
                particle.Clear();
                particle = transform.Find("OldExplodeEffect").GetComponent<ParticleSystem>();
                particle.gameObject.SetActive(true);
                size = size / _sizeMultiplier;
            }
            if (SettingsManager.AbilitySettings.ShowBombColors.Value)
                particle.startColor = BombUtil.GetBombColor(owner);
            particle.startSize = size;
            if (photonView.isMine)
                StartCoroutine(WaitAndDestroyCoroutine(1.5f));
        }
    }
    private IEnumerator WaitAndDestroyCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(gameObject);
    }
}