using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerOnDestroy : MonoBehaviour
{
    private AudioSource src;
    public AudioClip clip;

    private void Start()
    {
        src = GameObject.Find("FPS Controller").transform.
            Find("Main Camera").GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        if (clip != null && src != null)
            src.PlayOneShot(clip);
    }
}
