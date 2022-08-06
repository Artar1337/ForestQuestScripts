using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public string stringIDTrueEnding;
    public string stringIDFalseEnding;
    public AudioClip clipToPlay;
    public bool parseOnStart = false;
    private void Start()
    {
        if (parseOnStart)
        {
            transform.Find("screen").Find("Text Display").Find("Canvas").
                Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt
                (StringResources.LocalDictionaryType.speech, stringIDTrueEnding)[0];
        }
    }

    public void End()
    {
        bool trueEnding = transform.parent.transform.parent.Find("Money").gameObject.activeInHierarchy;
        if (trueEnding)
            transform.Find("screen").Find("Text Display").Find("Canvas").
                Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt
                (StringResources.LocalDictionaryType.speech, stringIDTrueEnding)[0];
        else
            transform.Find("screen").Find("Text Display").Find("Canvas").
                Find("Text (TMP)").GetComponent<TMPro.TMP_Text>().text =
                StringResources.instance.ElementAt
                (StringResources.LocalDictionaryType.speech, stringIDFalseEnding)[0];
        AudioSource backMusic = GetComponent<AudioSource>();
        GameObject.Find("FPS Controller").SetActive(false);
        GameObject.Find("Main Canvas").SetActive(false);
        transform.parent.GetComponent<Animator>().SetTrigger("Go");
        backMusic.Stop();
        backMusic.PlayOneShot(clipToPlay);
    }
}
