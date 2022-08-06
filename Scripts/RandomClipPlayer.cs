using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomClipPlayer : MonoBehaviour
{
    private AudioSource src;
    public AudioClip[] clips;
    public bool canPlayMultiplyAtOnce = false;
    public bool playClipOnCollisionEnter = false;
    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public void PlayRandomClip()
    {
        if (!canPlayMultiplyAtOnce && src.isPlaying)
            return;
        int index = StringResources.instance.random.Next(0, clips.Length);
        src.PlayOneShot(clips[index]);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playClipOnCollisionEnter)
            PlayRandomClip();
    }
}
