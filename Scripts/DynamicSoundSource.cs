using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSoundSource : MonoBehaviour
{

    // ������ �� ��������� AudioSource, � �� ������ ��� �� �����, � ������� ��������� �� 0.
    // ��� ����� ������ ��� �������� �������� ����� ����� �����, ���� 
    // ����� �� ������������� �����, �������� ���� �� ���������������!

    //��������� �����
    private AudioSource[] src;
    //�������� ����� ������� ������
    private AudioSource buttonSource;
    //��������� ��� ������ ������������
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
