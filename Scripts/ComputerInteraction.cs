using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerInteraction : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerLookDirectionChecker lookChecker;
    private PlayerRaycast raycast;
    private GameObject computerUI;
    private GameObject login, work;
    private PasswordChecker passwordChecker;
    private GameObject flashTrigger = null;

    //данные о компьютере
    private bool flashInside = false;
    private bool loginCompleted = false;
    public RectTransform flashWindow;
    public string password;

    private void Awake()
    {
        movement = GameObject.Find("FPS Controller").GetComponent<PlayerMovement>();
        lookChecker = movement.transform.Find("Main Camera").GetComponent<PlayerLookDirectionChecker>();
        raycast = lookChecker.GetComponent<PlayerRaycast>();
        computerUI = GameObject.Find("Main Canvas").transform.Find("Computer UI").gameObject;
        login = computerUI.transform.Find("Login").gameObject;
        work = computerUI.transform.Find("Work").gameObject;
        passwordChecker = login.transform.Find("OK").GetComponent<PasswordChecker>();
        if (transform.Find("Flash trigger") != null)
            flashTrigger = transform.Find("Flash trigger").gameObject;
    }

    public void TurnONComputer()
    {
        movement.enabled = false;
        lookChecker.enabled = false;
        raycast.enabled = false;
        lookChecker.canMove = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        computerUI.SetActive(true);
        ComputerMessageShower.instance.SetSender(this);
        work.SetActive(false);
        login.SetActive(false);
        passwordChecker.password = password;
        if (flashTrigger != null)
            flashTrigger.SetActive(true);
        if (loginCompleted)
            work.SetActive(true);
        else
            login.SetActive(true);
    }

    public void TurnOFFComputer()
    {
        movement.enabled = true;
        lookChecker.enabled = true;
        lookChecker.canMove = true;
        raycast.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        computerUI.SetActive(false);
    }

    public bool IsFlashDriveInside()
    {
        return flashInside;
    }

    public void InsertFlashDrive()
    {
        flashInside = true;
    }

    public void LogIn()
    {
        loginCompleted = true;
    }
}
