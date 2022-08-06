using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordChecker : MonoBehaviour
{
    private GameObject login, workspace;
    private TMPro.TMP_InputField input;
    new private ColorChanger animation;
    public string password = "1234";
    private void Awake()
    {
        login = transform.parent.gameObject;
        workspace = transform.parent.transform.parent.Find("Work").gameObject;
        input = login.transform.Find("Password").GetComponent<TMPro.TMP_InputField>();
        animation = login.transform.Find("Wrong").GetComponent<ColorChanger>();
    }

    public void CheckPassword()
    {
        if (input.text == password)
        {
            workspace.SetActive(true);
            login.SetActive(false);
            ComputerMessageShower.instance.GetSender().LogIn();
        }
        else
        {
            //пароли не равны - вывод сообщения
            animation.InitiateAnimation();
        }
    }
}
