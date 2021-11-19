




using System;
using UnityEngine;

public class MovementUpdate1 : MonoBehaviour
{
    public bool disabled;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 lastVelocity;

    private void Start()
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            this.disabled = true;
            base.enabled = false;
        }
        else if (base.networkView.isMine)
        {
            object[] args = new object[] { base.transform.position, base.transform.rotation, base.transform.lossyScale };
            base.networkView.RPC("updateMovement1", RPCMode.OthersBuffered, args);
        }
        else
        {
            base.enabled = false;
        }
    }

    private void Update()
    {
        if (!this.disabled)
        {
            object[] args = new object[] { base.transform.position, base.transform.rotation, base.transform.lossyScale };
            base.networkView.RPC("updateMovement1", RPCMode.Others, args);
        }
    }

    [RPC]
    private void updateMovement1(Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
    {
        base.transform.position = newPosition;
        base.transform.rotation = newRotation;
        base.transform.localScale = newScale;
    }
}

