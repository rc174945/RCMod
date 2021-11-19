




using System;
using UnityEngine;

public class ParticleScaling : MonoBehaviour
{
    public void OnWillRenderObject()
    {
        base.GetComponent<ParticleSystem>().renderer.material.SetVector("_Center", base.transform.position);
        base.GetComponent<ParticleSystem>().renderer.material.SetVector("_Scaling", base.transform.lossyScale);
        base.GetComponent<ParticleSystem>().renderer.material.SetMatrix("_Camera", Camera.current.worldToCameraMatrix);
        base.GetComponent<ParticleSystem>().renderer.material.SetMatrix("_CameraInv", Camera.current.worldToCameraMatrix.inverse);
    }
}

