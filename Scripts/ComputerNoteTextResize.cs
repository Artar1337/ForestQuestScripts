using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerNoteTextResize : MonoBehaviour
{
    //для блокнота: объект текста TMP и его границы
    private TMPro.TMP_Text note = null;
    private RectTransform noteBar = null;
    private TMPro.TMP_Dropdown dropdown = null;
    private UnityEngine.UI.Scrollbar bar;

    #region Singleton
    public static ComputerNoteTextResize instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Strings instance error!");
            return;
        }
        Resize(-1);
        instance = this;
    }
    #endregion

    public void Resize(int value)
    {
        if (note == null)
        {
            Transform tmp = transform.parent.transform.parent.
                transform.Find("Scroll Area").transform;
            note = tmp.Find("Text").GetComponent<TMPro.TMP_Text>();
            noteBar = note.GetComponent<RectTransform>();
            dropdown = GetComponent<TMPro.TMP_Dropdown>();
            bar = tmp.Find("Scrollbar").GetComponent<UnityEngine.UI.Scrollbar>();
        }
        if (value > -1)
        {
            dropdown.value = value;
            return;
        }
        note.fontSize = System.Convert.ToInt32(dropdown.captionText.text.Split(' ')[0]);
        noteBar.sizeDelta = new Vector2(noteBar.sizeDelta.x, 640);
        if (note.preferredHeight > 640)
            noteBar.sizeDelta = new Vector2(noteBar.sizeDelta.x, note.preferredHeight);
        StartCoroutine(BarInitiateValue(1f));
    }

    private IEnumerator BarInitiateValue(float value = 0.5f)
    {
        //ждем ОДИН кадр, потому что unity после того, как меняет размер scrollbar, ставит его в середину всегда!
        yield return null;
        bar.value = value;
    }
}
