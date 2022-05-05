using Constants;
using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Settings;
using GameProgress;

class Bomb : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;
    private Vector3 correctPlayerVelocity = Vector3.zero;
    public bool Disabled;
    public float SmoothingDelay = 10f;
    public float BombRadius;
    TITAN _collidedTitan;
    SphereCollider _sphereCollider;
    List<GameObject> _hideUponDestroy;
    ParticleSystem _trail;
    ParticleSystem _flame;
    float _DisabledTrailFadeMultiplier = 0.6f;
    HERO _owner;

    //backwards compatibility
    bool _receivedNonZeroVelocity = false;
    bool _ownerIsUpdated = false;

    public void Setup(HERO owner, float bombRadius)
    {
        _owner = owner;
        BombRadius = bombRadius;
    }


    public void Awake()
    {
        if (photonView != null)
        {
            photonView.observed = this;
            correctPlayerPos = transform.position;
            correctPlayerRot = transform.rotation;
            PhotonPlayer owner = photonView.owner;
            _trail = transform.Find("Trail").GetComponent<ParticleSystem>();
            _flame = transform.Find("Flame").GetComponent<ParticleSystem>();
            _sphereCollider = GetComponent<SphereCollider>();
            _hideUponDestroy = new List<GameObject>();
            _hideUponDestroy.Add(transform.Find("Flame").gameObject);
            _hideUponDestroy.Add(transform.Find("ThunderSpearModel").gameObject);
            if (SettingsManager.AbilitySettings.ShowBombColors.Value)
            {
                Color color = BombUtil.GetBombColor(owner, 1f);
                _trail.startColor = color;
                _flame.startColor = color;
            }
            if (photonView.isMine)
                photonView.RPC("IsUpdatedRPC", PhotonTargets.All, new object[0]);
        }
    }

    [RPC]
    void IsUpdatedRPC(PhotonMessageInfo info)
    {
        if (info.sender != photonView.owner)
            return;
        _ownerIsUpdated = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (photonView.isMine && !Disabled)
            Explode(BombRadius);
    }

    public void DestroySelf()
    {
        if (photonView.isMine && !Disabled)
        {
            photonView.RPC("DisableRPC", PhotonTargets.All, new object[0]);
            StartCoroutine(WaitAndFinishDestroyCoroutine(1.5f));
        }
    }

    IEnumerator WaitAndFinishDestroyCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (_collidedTitan != null)
            _collidedTitan.isThunderSpear = false;
        PhotonNetwork.Destroy(gameObject);
    }

    [RPC]
    public void DisableRPC(PhotonMessageInfo info = null)
    {
        if (Disabled)
            return;
        if (info != null && info.sender != photonView.owner)
            return;
        foreach (GameObject obj in _hideUponDestroy)
            obj.SetActive(false);
        _sphereCollider.enabled = false;
        SetDisabledTrailFade();
        rigidbody.velocity = Vector3.zero;
        Disabled = true;
    }

    void SetDisabledTrailFade()
    {
        int particleCount = _trail.particleCount;
        float newLifetime = _trail.startLifetime * _DisabledTrailFadeMultiplier;
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleCount];
        _trail.GetParticles(particles);
        for (int i = 0; i < particleCount; i++)
        {
            particles[i].lifetime *= _DisabledTrailFadeMultiplier;
            particles[i].startLifetime = newLifetime;
        }
        _trail.SetParticles(particles, particleCount);
    }

    public void Explode(float radius)
    {
        if (!Disabled)
        {
            PhotonNetwork.Instantiate("RCAsset/BombExplodeMain", transform.position, Quaternion.Euler(0f, 0f, 0f), 0);
            KillPlayersInRadius(radius);
            DestroySelf();
        }
    }

    void KillPlayersInRadius(float radius)
    {
        foreach (HERO hero in FengGameManagerMKII.instance.getPlayers())
        {
            GameObject gameObject = hero.gameObject;
            if (Vector3.Distance(gameObject.transform.position, transform.position) < radius && !gameObject.GetPhotonView().isMine && !hero.bombImmune)
            {
                PhotonPlayer owner = gameObject.GetPhotonView().owner;
                if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0 && PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam] != null && owner.customProperties[PhotonPlayerProperty.RCteam] != null)
                {
                    int myTeam = RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]);
                    int otherTeam = RCextensions.returnIntFromObject(owner.customProperties[PhotonPlayerProperty.RCteam]);
                    if (myTeam == 0 || myTeam != otherTeam)
                        KillPlayer(hero);
                }
                else
                    KillPlayer(hero);
            }
        }
    }

    void KillPlayer(HERO hero)
    {
        hero.markDie();
        hero.photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]) + " " });
        FengGameManagerMKII.instance.playerKillInfoUpdate(PhotonNetwork.player, 0);
        GameProgressManager.RegisterHumanKill(_owner.gameObject, hero, KillWeapon.ThunderSpear);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rigidbody.velocity);
        }
        else
        {
            correctPlayerPos = (Vector3) stream.ReceiveNext();
            correctPlayerRot = (Quaternion) stream.ReceiveNext();
            correctPlayerVelocity = (Vector3) stream.ReceiveNext();
        }
    }

    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * SmoothingDelay);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * SmoothingDelay);
            rigidbody.velocity = correctPlayerVelocity;

            //backwards compatibility, assume a non-moving bomb after some time is exploded
            if (rigidbody.velocity != Vector3.zero)
                _receivedNonZeroVelocity = true;
            else if (!_ownerIsUpdated && _receivedNonZeroVelocity && !Disabled)
            {
                Disabled = true;
                foreach (GameObject obj in _hideUponDestroy)
                    obj.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        if (!Disabled && photonView.isMine)
        {
            CheckCollide();
        }
    }
    void CheckCollide()
    {
        RaycastHit hit;
        LayerMask mask = (1 << PhysicsLayer.PlayerAttackBox) | (1 << PhysicsLayer.PlayerHitBox);
        Collider[] colliders = Physics.OverlapSphere(transform.position + _sphereCollider.center, _sphereCollider.radius, mask);
        foreach (Collider collider in colliders)
        {
            if (collider.name.Contains("PlayerDetectorRC"))
            {
                TITAN component = collider.transform.root.gameObject.GetComponent<TITAN>();
                if (component != null)
                {
                    if (_collidedTitan == null)
                    {
                        _collidedTitan = component;
                        _collidedTitan.isThunderSpear = true;
                    }
                    else if (_collidedTitan != component)
                    {
                        _collidedTitan.isThunderSpear = false;
                        _collidedTitan = component;
                        _collidedTitan.isThunderSpear = true;
                    }
                }
            }
            else if (collider.gameObject.layer == PhysicsLayer.PlayerHitBox)
            {
                HERO hero = collider.transform.root.gameObject.GetComponent<HERO>();
                if (!hero.photonView.isMine)
                    Explode(BombRadius);
            }
        }
    }
}
