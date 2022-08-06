using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsController : MonoBehaviour
{
    private void Awake()
    {
        AudioSource[] src = GetComponentsInChildren<AudioSource>();
        float delay = 0.0f;
        float delayIncrement = 2.5f;
        foreach(AudioSource a in src)
        {
            a.PlayDelayed(delay);
            delay += delayIncrement;
        }
    }

}
