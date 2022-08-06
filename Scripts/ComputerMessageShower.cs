using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerMessageShower : MonoBehaviour
{
    #region Singleton
    public static ComputerMessageShower instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Message shower instance error!");
            return;
        }
        Initialize();
        instance = this;
    }
    #endregion

    private GameObject messageWindow;
    private TMPro.TMP_Text text;
    private RectTransform window;
    private ComputerInteraction sender;

    private void Initialize()
    {
        messageWindow = transform.Find("Work").transform.Find("MessagePanel").gameObject;
        window = messageWindow.GetComponent<RectTransform>();
        text = messageWindow.transform.Find("Message").transform.
            Find("Text").GetComponent<TMPro.TMP_Text>();
    }

    public void ShowMessage(string text)
    {
        messageWindow.SetActive(true);
        window.SetAsLastSibling();
        this.text.text = text;
    }

    public void SetSender(ComputerInteraction sender)
    {
        this.sender = sender;
    }

    public ComputerInteraction GetSender()
    {
        return sender;
    }

    public void TurnOFFComputer()
    {
        if (sender != null)
            sender.TurnOFFComputer();
    }
}
