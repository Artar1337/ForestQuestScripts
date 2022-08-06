using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioWindowStartOrPausePlayback : MonoBehaviour
{
    private AudioWindowVolumeChanger volumeChanger, lengthChanger;
    private bool isPlaying;
    public AudioClip clipToPlay;
    private UnityEngine.UI.Slider progress;

    private UnityEngine.UI.Image image;

    private Color paused, playing;
    private bool pause;

    private void Awake()
    {
        volumeChanger = transform.parent.Find("Volume").GetComponent<AudioWindowVolumeChanger>();
        lengthChanger = transform.parent.Find("Progress").GetComponent<AudioWindowVolumeChanger>();
        image = GetComponent<UnityEngine.UI.Image>();
        isPlaying = false;
        pause = false;
        paused = image.color;
        playing = image.color - new Color(0f, 0f, 0f, 0.5f);
        if (clipToPlay == null)
        {
            image.color = playing;
            return;
        }
        progress = lengthChanger.gameObject.GetComponent<UnityEngine.UI.Slider>();
        progress.maxValue = clipToPlay.length;
        progress.value = 0;
    }

    public void SetPlaying(bool value)
    {
        isPlaying = value;
        if (clipToPlay == null)
        {
            pause = false;
            image.color = playing;
            return;
        }
        if(!isPlaying)
            image.sprite = StringResources.instance.GetSpriteByName("play");
        else
            image.sprite = StringResources.instance.GetSpriteByName("stop");
    }

    public void StartOrStopPlayback()
    {
        Sprite sprite;
        volumeChanger.GetTarget().time = 0f;
        bool oldIsPlaying = isPlaying;
        AudioWindowDisableAllSliders.instance.Disable();
        if (oldIsPlaying)
        {
            sprite = StringResources.instance.GetSpriteByName("play");
            volumeChanger.GetTarget().Stop();
            progress.value = 0;
        }
        else
        {
            volumeChanger.GetTarget().clip = clipToPlay;
            volumeChanger.SetActive(true);
            lengthChanger.SetActive(true);
            isPlaying = true;
            volumeChanger.OnAudioSliderVolumeChange();
            volumeChanger.GetTarget().Play();
            sprite = StringResources.instance.GetSpriteByName("stop");
        }
        image.sprite = sprite;
    }

    public void PausePlayback()
    {
        if (!volumeChanger.GetActive())
            return;
        pause = !pause;
        if (pause)
        {
            image.color = paused;
            volumeChanger.GetTarget().Pause();
        }
        else
        {
            image.color = playing;
            volumeChanger.GetTarget().Play();
        }
    }

    //���������� progress bar
    private void FixedUpdate()
    {
        //�� �������� �� pause script, �� ��� �� ����� ���� (��� � ��������)
        if (clipToPlay != null)
        {
            //���� ������� ���������� ����� �� ������� - �������, �� �� � ��� �������
            if (!lengthChanger.GetActive())
                return;

            progress.value = volumeChanger.GetTarget().time;

            if(progress.value + Time.fixedDeltaTime >= progress.maxValue)
            {
                isPlaying = false;
                image.sprite = StringResources.instance.GetSpriteByName("play");
            }
        }
    }
}
