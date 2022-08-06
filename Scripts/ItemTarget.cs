using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTarget : MonoBehaviour
{
    public bool destroyItemAfterUse = true;
    public bool playAnimation = false;
    public Animator animator;
    public float waitSeconds = 0f;
    public GameObject[] targets;
    private string resultLayer = "Raycast Target";
    public Item itemNeeded;
    public bool setTargetToOpenable = false;

    public bool flashInteraction = false;

    private string defaultLayer = "Ignore Raycast";
    private PlayerMovement player;
    private LineSpeaker speaker;
    private ComputerInteraction computerInteraction;

    private void Start()
    {
        player = GameObject.Find("FPS Controller").
            transform.GetComponent<PlayerMovement>();
        speaker = GetComponent<LineSpeaker>();
        if (flashInteraction)
        {
            computerInteraction = transform.parent.GetComponent<ComputerInteraction>();
        }
    }

    private void WorkWithTargets()
    {
        if (targets != null)
        {
            foreach (GameObject target in targets)
                    target.gameObject.layer = LayerMask.NameToLayer(resultLayer);
            if (setTargetToOpenable)
            {
                foreach (GameObject target in targets)
                    target.GetComponent<RaycastTarget>().objectType = RaycastTarget.InteractableObject.Openable;
            }
        }
    }

    //CORRECT - DESTROY AFTER
    public KeyValuePair<bool,bool> ChangeLayerIfGotCorrectItem(Item item)
    {
        if (item == null)
        {
            speaker.Speak();
            return new KeyValuePair<bool, bool>(false, destroyItemAfterUse);
        }
            
        if(item.ID == itemNeeded.ID)
        {
            gameObject.layer = LayerMask.NameToLayer(defaultLayer);
            if (!playAnimation)
            {
                WorkWithTargets();
            }
            else
            {
                animator.SetTrigger("Open");
                StartCoroutine(LayerCoroutine());
            }
            return new KeyValuePair<bool, bool>(true, destroyItemAfterUse);
        }
        speaker.Speak(false);
        return new KeyValuePair<bool, bool>(false, destroyItemAfterUse);
    }

    public void BlackScreenAnimationEvent()
    {
        GameObject.Find("Main Canvas").transform.
            Find("Black Screen").GetComponent<ColorChanger>().InitiateAnimation();
    }

    public void TeleportateMainCharacter(int ID)
    {
        Vector3 destination = ID switch
        {
            //photo room
            0 => new Vector3(-550.82f, 0.065f, -299.804f),
            //forest
            1 => new Vector3(-590.673f, 0.03f, -241.26f),
            //big house
            2 => new Vector3(-734.825f, 20.13f, 282.853f),
            //well
            3 => new Vector3(-761.8434f, 4.4263f, -148.1716f),
            //out of well
            4 => new Vector3(-760.6928f, 4.29f, -148.76f),
            //home
            _ => new Vector3(-549.451f, 0.93f, -284.62f),
        };
        if(ID == 3)
        {
            player.SetSmallControllerValues();
            player.gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }
        else if(ID == 4)
        {
            player.SetNormalControllerValues();
            player.gameObject.transform.localScale = new Vector3(0.96f, 0.96f, 0.96f);
        }
        player.SetMCPosition(destination);
    }

    private IEnumerator LayerCoroutine()
    {
        yield return new WaitForSeconds(waitSeconds);

        WorkWithTargets();

        if (flashInteraction)
        {
            ComputerMessageShower.instance.SetSender(computerInteraction);
            ComputerMessageShower.instance.GetSender().InsertFlashDrive();
        }
    }
}
