using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSubtitle : MonoBehaviour
{
    private LineSpeaker speaker;
    public bool disableAfter = false;
    private bool isActive = true;

    private void Awake()
    {
        speaker = GetComponent<LineSpeaker>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        if (!isActive)
            return;
        speaker.Speak();
        if (disableAfter)
            isActive = false;
    }
}
