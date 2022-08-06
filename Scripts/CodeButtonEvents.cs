using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeButtonEvents : MonoBehaviour
{
    private CodePanel password;

    void Awake()
    {
        password = transform.parent.transform.parent.GetComponent<CodePanel>();
    }

    public void PressCodeButton()
    {
        string button = gameObject.name;
        if (button == "OK")
        {
            password.ConfirmPassword();
            password.ClearCharacters(false);
            return;
        }
        else if(button == "DEL")
        {
            password.ClearCharacters();
            return;
        }
        password.AddCharacter(button[0]);
    }
}
