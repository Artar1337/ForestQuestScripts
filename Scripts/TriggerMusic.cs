using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusic : MonoBehaviour
{
    private AudioSource src;
    private bool triggered = false;
    public AudioClip music;
    public bool stoppingMusic = false;
    public bool playAdditional = false;
    public bool playIfNotPlaying = false;
    public bool showTitle = true;
    private GameTitleShower title;
    private void Awake()
    {
        src = GameObject.Find("FPS Controller").transform.
            Find("Background Music").GetComponent<AudioSource>();
        if(showTitle)
            title = GetComponent<GameTitleShower>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (triggered)
            return;
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;
        if(playIfNotPlaying && !src.isPlaying)
        {
            src.PlayOneShot(music);
            triggered = true;
            return;
        }
        if (src.isPlaying && !playAdditional)
            src.Stop();
        if (!stoppingMusic)
            src.PlayOneShot(music);
        if (title != null && showTitle)
            title.PlayAnimation();
        triggered = true;
    }
}
