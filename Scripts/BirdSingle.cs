using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSingle : MonoBehaviour
{
    private AudioSource src;
    public AudioClip[] clips;
    private float currentTime = -1f;
    //�� 5 �� 60, ������������, ������ �������� � �� ����� ��������
    [Range(5,60)]
    public int rangeMin, rangeMax;
   
    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.fixedDeltaTime;
            return;
        }

        src.PlayOneShot(clips[StringResources.instance.random.Next(0, clips.Length)]);
        currentTime = (float)(StringResources.instance.random.Next(rangeMin, rangeMax) + 
            StringResources.instance.random.NextDouble());
    }
}
