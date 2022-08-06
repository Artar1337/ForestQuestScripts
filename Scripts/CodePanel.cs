using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodePanel : MonoBehaviour
{
    public int characterLimit = 7;
    public string password;

    public AudioClip correctSound, wrongSound, clickSound, beepSound;
    public bool bigDoorPanel = false;

    private TMPro.TMP_Text textContainer;
    private AudioSource source;
    private RaycastTarget trigger;
    private bool passwordConfirmed = false;

    private void Awake()
    {
        textContainer = transform.Find("Text").GetComponent<TMPro.TMP_Text>();
        source = transform.Find("Buttons").GetComponent<AudioSource>();
        trigger = transform.parent.Find("Trigger").GetComponent<RaycastTarget>();
    }
    
    public void AddCharacter(char c)
    {
        source.PlayOneShot(clickSound);
        if (passwordConfirmed)
            return;
        if (textContainer.text.Length < characterLimit)
        {
            textContainer.text += c;
            //���� ������� �� ������ (���)
            source.PlayOneShot(beepSound);
        }
    }

    public void ClearCharacters(bool playSound = true)
    {
        source.PlayOneShot(clickSound);
        if (passwordConfirmed)
            return;
        //���� ������� �� ������ (���)
        if (playSound)
            source.PlayOneShot(beepSound);
        textContainer.text ="";
    }

    public void ConfirmPassword()
    {
        source.PlayOneShot(clickSound);
        if (passwordConfirmed)
            return;
        if (textContainer.text == password)
        {
            //������ �������� �������� �����
            //���� ��������
            source.PlayOneShot(correctSound);
            passwordConfirmed = true;
            //��������� ������ � ��������
            if (bigDoorPanel)
            {
                trigger.gameObject.GetComponent<FinalDoorActivator>().Activate(password);
                return;
            }
            trigger.objectType = RaycastTarget.InteractableObject.Openable;
            return;
        }
        //���� �������
        source.PlayOneShot(wrongSound);
    }
}
