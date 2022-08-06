using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSequenceController : MonoBehaviour
{
    private ButtonPressHandler handler;
    private DoorOpener opener;
    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        handler = GetComponent<ButtonPressHandler>();
        opener = GetComponent<DoorOpener>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;
        //спуск рампы на корутине + активация видеоклипа
        if (!activated)
        {
            if (handler.screen.isPlaying)
                handler.screen.Stop();

            switch (StringResources.instance.currentLocalization)
            {
                case StringResources.Localization.Русский:
                    handler.screen.clip = handler.clipRU;
                    StartCoroutine(StartAnimationAfterSeconds((float)handler.clipRU.length));
                    break;
                default:
                    handler.screen.clip = handler.clipEN;
                    StartCoroutine(StartAnimationAfterSeconds((float)handler.clipEN.length));
                    break;
            }
            
            handler.screen.Play();
            activated = true;
        }
    }

    private IEnumerator StartAnimationAfterSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        opener.ChangeDoorState();
    }
}
