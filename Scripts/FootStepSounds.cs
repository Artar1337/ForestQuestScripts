using System.Collections.Generic;
using UnityEngine;

public class FootStepSounds : MonoBehaviour
{

    #region Singleton
    public static FootStepSounds instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Inventory instance error!");
            return;
        }
        Initialize();
        instance = this;
    }
    #endregion

    public Dictionary<string, AudioClip[]> sounds;

    private AudioSource src;
    private System.Random rnd;

    private void Initialize()
    {
        sounds = new Dictionary<string, AudioClip[]>();
        List<string> layers = new List<string>() 
        { "Wood", "Floor", "Forest", "Carpet", "Gravel" };
        string root = "Audio/Footsteps/";

        foreach(string layer in layers)
        {
            sounds.Add(layer, Resources.LoadAll<AudioClip>(root + layer));
            //Debug.Log(layer + ": " + sounds[layer].Length + " clips");
        }

        src = GetComponent<AudioSource>();
        rnd = new System.Random();
    }

    public void PlayRandomFootStepSoundOnLayer(string layer)
    {
        if (sounds.ContainsKey(layer))
            src.PlayOneShot(sounds[layer][rnd.Next(0, sounds[layer].Length)]);
    }
}
