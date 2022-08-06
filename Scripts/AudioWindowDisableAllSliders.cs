using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioWindowDisableAllSliders : MonoBehaviour
{

    #region Singleton
    public static AudioWindowDisableAllSliders instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Audio Window instance error!");
            return;
        }
        Initialize();
        instance = this;
    }
    #endregion

    private List<AudioWindowVolumeChanger> volumesAndLengths;
    private List<AudioWindowStartOrPausePlayback> playAndPauses;
    private List<Slider> lengths;

    private void Initialize()
    {
        List<Transform> list = new List<Transform>();
        volumesAndLengths = new List<AudioWindowVolumeChanger>();
        playAndPauses = new List<AudioWindowStartOrPausePlayback>();
        lengths = new List<Slider>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.CompareTag("Audio Window"))
            {
                list.Add(transform.GetChild(i));
            }
        }
        foreach(Transform t in list)
        {
            AudioWindowVolumeChanger[] obj = 
                t.GetComponentsInChildren<AudioWindowVolumeChanger>();
            foreach (AudioWindowVolumeChanger changer in obj)
            {
                volumesAndLengths.Add(changer);
                if (changer.lengthSlider)
                    lengths.Add(changer.GetComponent<Slider>());
            }
            AudioWindowStartOrPausePlayback[] elements = 
                t.GetComponentsInChildren<AudioWindowStartOrPausePlayback>();
            foreach(AudioWindowStartOrPausePlayback element in elements)
            {
                playAndPauses.Add(element);
            }
        }
    }

    public void Disable(bool enable = false)
    {
        if (volumesAndLengths == null || playAndPauses == null || lengths == null)
            return;
        foreach(AudioWindowVolumeChanger obj in volumesAndLengths)
        {
            obj.SetActive(enable);
        }
        foreach (AudioWindowStartOrPausePlayback obj in playAndPauses)
        {
            obj.SetPlaying(enable);
        }
        foreach (Slider obj in lengths)
        {
            obj.value = 0;
        }
    }

    public void OnDisable()
    {
        Disable();
    }
}
