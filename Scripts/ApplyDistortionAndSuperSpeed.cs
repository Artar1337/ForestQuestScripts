using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ApplyDistortionAndSuperSpeed : MonoBehaviour
{
    public AudioClip distortSound, backgroundMusic, tiredSound;
    private float currentTime = 0.2f, maxTime = 60f;
    [SerializeField]
    private PostProcessVolume volume;
    private LensDistortion distortion;
    private Grain grain;
    private ColorGrading grading;
    private PlayerMovement movement;
    private AudioSource backMusic;

    private void Start()
    {
        volume.profile.TryGetSettings(out distortion);
        volume.profile.TryGetSettings(out grain);
        volume.profile.TryGetSettings(out grading);
        movement = GameObject.Find("FPS Controller").GetComponent<PlayerMovement>();
        distortion.enabled.value = false;
        grading.enabled.value = false;
        grain.enabled.value = false;
        backMusic = GameObject.Find("FPS Controller").transform.
            Find("Background Music").GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.fixedDeltaTime;
            if (currentTime < 0f)
            {
                movement.normalSpeed = 2.56f;
                movement.sprintSpeed = 5.12f;
                distortion.enabled.value = false;
                grading.enabled.value = false;
                grain.enabled.value = false;
                backMusic.Stop();
            }
        }
    }

    public void DeactivateEffects()
    {
        currentTime = 0.01f;
    }

    public void ActivateDistortionAndSpeed()
    {
        GetComponent<AudioSource>().PlayOneShot(distortSound);
        movement.normalSpeed = 5.12f;
        movement.sprintSpeed = 10.24f;
        distortion.enabled.value = true;
        grading.enabled.value = true;
        grain.enabled.value = true;
        currentTime = maxTime;
        backMusic.Stop();
        backMusic.PlayOneShot(backgroundMusic);
    }
}
