using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public class ComputerVideoPlayStopPause : MonoBehaviour
{
    [SerializeField]
    internal VideoPlayer player;
    new private AudioSource audio;

    private UnityEngine.UI.Slider progress;
    private UnityEngine.UI.Image image;
    private AudioWindowVolumeChanger volumeChanger, lengthChanger;
    private RectTransform loading;

    private Color paused, playing;
    private bool isPlaying = false;
    public bool pauseButton = false;
    private bool canPlayVideo = false;

    private WebClient client;

    private void Awake()
    {
        audio = transform.parent.transform.parent.GetComponent<AudioSource>();
        player = transform.parent.transform.parent.Find("Video").GetComponent<VideoPlayer>();
        volumeChanger = transform.parent.Find("Volume").GetComponent<AudioWindowVolumeChanger>();
        lengthChanger = transform.parent.Find("Progress").GetComponent<AudioWindowVolumeChanger>();
        loading = transform.parent.transform.parent.Find("Loading").GetComponent<RectTransform>();
        progress = lengthChanger.GetComponent<UnityEngine.UI.Slider>();
        image = GetComponent<UnityEngine.UI.Image>();
        isPlaying = false;

        paused = image.color;
        playing = image.color - new Color(0f, 0f, 0f, 0.5f);
    }

    // Start is called before the first frame update
    public void Initialize(bool local, string video, string vidName="video")
    {
        if (!pauseButton)
            image.sprite = StringResources.instance.GetSpriteByName("play");
        isPlaying = false;
        canPlayVideo = false;
        transform.parent.transform.parent.GetComponent<ComputerResizeWindow>().Resize(false);
        loading.gameObject.SetActive(true);
        player.audioOutputMode = VideoAudioOutputMode.AudioSource;
        player.SetTargetAudioSource(0, audio);
        if (local)
        {
            StartCoroutine(PrepareVideo(video, false));
            return;
        }
        player.source = VideoSource.Url;
        LoadVideoFromURL(video, vidName);
    }

    private void OnDisable()
    {
        if (client != null)
        {
            try
            {
                client.CancelAsync();
                client.Dispose();
                Debug.Log("Client disposed");
            }
            catch (WebException)
            {
                ComputerMessageShower.instance.ShowMessage(StringResources.instance.
                    ElementAt(StringResources.LocalDictionaryType.UI, "message_network_error")[0]);
            }
        }
    }

    private void LoadVideoFromURL(string url,string vidName)
    {
        string path = Application.persistentDataPath + "/" + vidName + ".mp4";
        OnDisable();
        if (File.Exists(path))
        {
            try
            {
                File.Delete(path);
                Debug.Log("Deleted temp video file from " + path);
            }
            catch (IOException)
            {
                Debug.LogError("Error deleting " + path);
            }
        }

        client = new WebClient();
        var uri = new Uri(url);
        Debug.Log("Downloading file to " + path + " from " + url);
        try
        {
            client.DownloadFileCompleted += (sender, e) => { 
                Debug.Log("Done downloading from " + url);
                if(player.isActiveAndEnabled)
                    StartCoroutine(PrepareVideo(path, true));
            };
            
            client.DownloadFileAsync(uri, path);
        }
        catch (WebException)
        {
            ComputerMessageShower.instance.ShowMessage(StringResources.instance.
                ElementAt(StringResources.LocalDictionaryType.UI, "message_network_error")[0]);
            return;
        }
    }

    private IEnumerator PrepareVideo(string url, bool isURL)
    {
        if (isURL)
        {
            player.source = VideoSource.Url;
            player.url = url;
        }
        else
        {
            player.source = VideoSource.VideoClip;
            player.clip = StringResources.instance.GetVideoByName(url);
        }
        player.Prepare();

        while (player.isPrepared == false)
        { yield return null; }

        Debug.Log("Can play video now! Enjoy!");
        canPlayVideo = true;
        loading.gameObject.SetActive(false);
        progress.maxValue = (float)player.length;
        progress.value = 0;
    }

    public void PauseVideo()
    {
        if (player.time < 0.3f)
            return;

        if (player.isPlaying)
        {
            player.Pause();
            image.color = playing;
        }
        else
        {
            player.Play();
            image.color = paused;
        }
    }

    public void PlayOrStopVideo()
    {
        //сообщение о том, что видео готовится
        if (!canPlayVideo)
        {
            ComputerMessageShower.instance.ShowMessage(StringResources.instance.
                ElementAt(StringResources.LocalDictionaryType.UI, "message_video_not_ready")[0]);
            return;
        }
        Sprite sprite;
        if (isPlaying)
        {
            sprite = StringResources.instance.GetSpriteByName("play");
            volumeChanger.SetActive(false);
            lengthChanger.SetActive(false);
            isPlaying = false;
            player.Stop();
        }
        else
        {
            isPlaying = true;
            sprite = StringResources.instance.GetSpriteByName("stop");
            volumeChanger.SetActive(true);
            lengthChanger.SetActive(true);
            //проигрываем видео
            player.Play();
        }
        image.sprite = sprite;
    }

    //обновление progress bar
    private void FixedUpdate()
    {
        if (pauseButton)
            return;
        //анимация загрузки
        if (!canPlayVideo)
        {
            loading.Rotate(0f, 0f, 1f, Space.Self);
            return;
        }
        if (isPlaying && player.isPlaying)
        {
            progress.value = (float)player.time;
            //Debug.Log(progress.value + " " + progress.maxValue);

            if (progress.value + 0.3f >= progress.maxValue)
            {
                PlayOrStopVideo();
                progress.value = 0;
            }
        }
    }

}
