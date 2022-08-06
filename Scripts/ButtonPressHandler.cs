using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ButtonPressHandler : MonoBehaviour
{
    public VideoClip clipRU, clipEN;
    public VideoPlayer screen;
    public Material buttonOFF, buttonON;
    public bool buttonIsActive = false;
    public AnimatorStopperAndButtonEvents door;
    [Range(0,2)]
    public int buttonType = 0;
    public GameObject objectToInteract;

    private AudioSource src;
    new private Renderer renderer;
    private bool watchedVideo = false;
    

    private void Awake()
    {
        src = GetComponent<AudioSource>();
        renderer = GetComponent<Renderer>();
        if (renderer != null)
            renderer.material = buttonOFF;
        if (buttonIsActive)
            renderer.material = buttonON;
    }

    public void SetButtonActive()
    {
        buttonIsActive = !buttonIsActive;
        renderer.material = buttonOFF;
        if (buttonIsActive)
            renderer.material = buttonON;
    }

    public void OnButtonPressAnimationEvent()
    {
        src.Play();
        if(buttonIsActive)
        {
            if (screen.isPlaying)
                screen.Stop();

            switch (StringResources.instance.currentLocalization)
            {
                case StringResources.Localization.Русский:
                    screen.clip = clipRU;
                    break;
                default: //по умолчанию - англ
                    screen.clip = clipEN;
                    break;
            }
            
            screen.Play();
            if (!watchedVideo)
            {
                //обычная кнопка
                if(buttonType == 0)
                {
                    //activate door and lamp;
                    door.LightMaterialSetEvent();
                }
                //кнопка активации другой кнопки + анимации убирания подстаки
                else if (buttonType == 1)
                {
                    objectToInteract.GetComponent<ButtonPressHandler>().SetButtonActive();
                    transform.parent.transform.parent.transform.
                        parent.GetComponent<DoorOpener>().ChangeDoorState();
                    transform.parent.transform.parent.Find("NO").Find("Button").
                        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
                //кнопка активации двери + исчезновение денег + анимации подставки + смена концовки
                else if (buttonType == 2)
                {
                    objectToInteract.SetActive(false);
                    door.LightMaterialSetEvent();
                    transform.parent.transform.parent.transform.
                        parent.GetComponent<DoorOpener>().ChangeDoorState();
                    transform.parent.transform.parent.Find("YES").Find("Button").
                        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
                watchedVideo = true;
            }
        }
    }
}
