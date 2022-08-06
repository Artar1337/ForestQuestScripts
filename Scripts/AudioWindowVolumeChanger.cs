using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AudioWindowVolumeChanger : MonoBehaviour
{
    private AudioSource target;
    private Slider slider;

    public bool lengthSlider = false;
    public bool isActive = false;

    public bool videoControls = false;
    //private VideoPlayer vidClip;

    private void Awake()
    {
        target = transform.parent.
            transform.parent.GetComponent<AudioSource>();
        slider = GetComponent<Slider>();
        if (lengthSlider)
        {
            /*if (videoControls)
                vidClip = transform.parent.transform.parent.
                    Find("Video").GetComponent<VideoPlayer>();*/
            slider.value = 0f;
            return;
        }
        Initiate();
    }

    public void OnAudioSliderVolumeChange()
    {
        if (slider != null && target != null && isActive)
        {
            target.volume = slider.value;
        }
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }

    public bool GetActive()
    {
        return isActive;
    }

    public void OnLengthSliderChange()
    {
        if (slider != null && isActive)
        {
            if (!videoControls && target != null && slider.value < target.clip.length)
            {
                target.time = slider.value;
                return;
            }
            //if (videoControls && vidClip != null && slider.value < vidClip.length)
        }
    }

    public void Initiate()
    {
        target.volume = 0.5f;
        slider.value = 0.5f;
    }

    public AudioSource GetTarget()
    {
        return target;
    }
}
