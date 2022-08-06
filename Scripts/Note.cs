using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public string ID;
    public bool speakTextAfterNote = false;
    private string noteText;
    private bool isReading = false;
    private float currentCooldown = -1f, timeCooldown = 3f;
    private LineSpeaker speaker;

    private PlayerRaycast playerRaycast;
    private PlayerMovement playerMovement;
    private PlayerLookDirectionChecker directionChecker;

    void Awake()
    {
        speaker = GetComponent<LineSpeaker>();
        playerMovement = GameObject.Find("FPS Controller").GetComponent<PlayerMovement>();
        directionChecker = playerMovement.gameObject.transform.Find("Main Camera").GetComponent<PlayerLookDirectionChecker>();
        playerRaycast = directionChecker.gameObject.GetComponent<PlayerRaycast>();
    }

    void Update()
    {
        if (isReading)
        {
            if (currentCooldown > 0f)
            {
                currentCooldown -= Time.deltaTime;
                return;
            }
            float interaction = Input.GetAxis("Interact");
            if (interaction > 0f)
            {
                isReading = false;
                playerRaycast.enabled = true;
                playerMovement.enabled = true;
                directionChecker.enabled = true;
                directionChecker.canMove = true;
                SubtitleReader.instance.DisableNote();
                if (speakTextAfterNote)
                    speaker.Speak();
            }
        }
    }

    public void Read()
    {
        if (isReading)
            return;
        playerRaycast.enabled = false;
        playerMovement.enabled = false;
        directionChecker.enabled = false;
        directionChecker.canMove = false;
        noteText = StringResources.instance.ElementAt(StringResources.LocalDictionaryType.speech, ID)[0];
        SubtitleReader.instance.SetNoteWithText(noteText);
        currentCooldown = timeCooldown;
        isReading = true;
    }
}
