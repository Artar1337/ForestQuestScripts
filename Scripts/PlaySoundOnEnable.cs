using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{
    public AudioClip clip;
    public bool alsoDisableEverythingExceptOfFirstTwoElements = false;

    private void OnEnable()
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
        if (alsoDisableEverythingExceptOfFirstTwoElements)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            if (transform.childCount > 1)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}
