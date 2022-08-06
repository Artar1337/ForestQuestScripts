using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoorActivator : MonoBehaviour
{
    public ButtonPressHandler button;
    private BigPasswordGenerator handler;
    private void Awake()
    {
        handler = transform.parent.transform.parent.GetComponent<BigPasswordGenerator>();
    }

    //���������� ��������������� ������ � ��������
    //� �������� ��������������� �������� �����
    public void Activate(string password)
    {
        button.SetButtonActive();
        handler.SetActive(password);
    }
}
