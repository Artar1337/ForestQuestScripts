using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchAnimationEvents : MonoBehaviour
{
    private PlayerMovement movement;
    private Transform mainCam;

    public float cameraOffset = 0.847f;

    private void Awake()
    {
        movement = GetComponentInParent<PlayerMovement>();
        mainCam = movement.gameObject.transform.Find("Main Camera").transform;
    }

    public void ChangeCameraPosition()
    {
        bool isCrouching = !movement.IsPlayerCrouching();
        if (isCrouching)
        {
            mainCam.position = new Vector3(mainCam.position.x,
                mainCam.position.y + cameraOffset, mainCam.position.z);
            return;
        }
        mainCam.position = new Vector3(mainCam.position.x,
                mainCam.position.y - cameraOffset, mainCam.position.z);
    }

    public void SetLocalPositionToZero()
    {
        transform.localPosition = Vector3.zero;
        transform.position = Vector3.zero;
    }

    public void PlayCurrentStepSound()
    {
        FootStepSounds.instance.PlayRandomFootStepSoundOnLayer
            (PlayerFootsteps.instance.currentName);
    }
}
