using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<TMPro.TMP_InputField>().text = "";
    }
}
