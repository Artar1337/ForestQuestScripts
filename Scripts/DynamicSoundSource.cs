using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSoundSource : MonoBehaviour
{

    // Скрипт не отключает AudioSource, и не ставит его на паузу, а снижает громкость до 0.
    // Это нужно только для имитации передачи звука через радио, ведь 
    // когда мы останавливаем радио, передача ведь не останавливается!

    //источники звука
    private AudioSource[] src;
    //источник звука нажатия кнопки
    private AudioSource buttonSource;
    //громкость при начале проигрывания
    private float[] volume;

    private void Awake()
    {
        src = GetComponents<AudioSource>();
        volume = new float[src.Length];
        for (int i = 0; i < src.Length; i++)
        {
            volume[i] = src[i].volume;
        } 
        buttonSource = transform.Find("Sound").GetComponent<AudioSource>();
    }

    public void SetVolume()
    {
        if (buttonSource == null || buttonSource.isPlaying)
            return;
        buttonSource.Play();
        if (src[0].volume > 0.01f)
        {
            for (int i = 0; i < src.Length; i++)
                src[i].volume = 0f;
            return;
        }
        for (int i = 0; i < src.Length; i++)
            src[i].volume = volume[i];
    }
}
