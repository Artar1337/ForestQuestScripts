using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStopperAndButtonEvents : MonoBehaviour
{
    private Animator a;
    private Renderer lightRenderer;
    private bool lightSet = false;
    private GameObject[] layersToChange;

    public bool justAnimatorStopperEvent = false;
    public Color lightColor;
    public ApplyDistortionAndSuperSpeed superSpeed;

    private void Awake()
    {
        a = GetComponent<Animator>();
        if (justAnimatorStopperEvent)
            return;
        lightRenderer = transform.Find("Door Light").GetComponent<Renderer>();
        RaycastTarget[] targets = GetComponentsInChildren<RaycastTarget>();
        if (targets == null)
            return;
        layersToChange = new GameObject[targets.Length];
        for(int i = 0; i < layersToChange.Length; i++)
        {
            layersToChange[i] = targets[i].gameObject;
        }
    }

    public void Stop()
    {
        a.speed = 0;
        a.StopPlayback();
    }

    public void SetAnimationSpeed(float speed)
    {
        a.speed = speed;
    }

    public void PlayerForceGetUp()
    {
        if (superSpeed != null)
            superSpeed.DeactivateEffects();
        GameObject.Find("FPS Controller").GetComponent<PlayerMovement>().SetPlayerGetUp();
    }

    public void Pause()
    {
        if (a.speed < 0.5f)
            a.speed = 1;
        else
            a.speed = 0;
    }

    public void LightMaterialSetEvent()
    {
        if (lightSet)
            return;
        lightRenderer.material.SetColor("_EmissionColor", lightColor);
        lightSet = true;
        OpenTheDoor();
    }

    public void OpenTheEndingScene()
    {
        if (transform.parent.Find("Money").gameObject.activeInHierarchy)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ending",
                UnityEngine.SceneManagement.LoadSceneMode.Single);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("FalseEnding",
                UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void EnableMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void OpenTheDoor()
    {
        foreach(GameObject obj in layersToChange)
        {
            obj.layer = LayerMask.NameToLayer("Raycast Target");
        }
    }
}
