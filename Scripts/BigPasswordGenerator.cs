using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPasswordGenerator : MonoBehaviour
{
    public bool[] activated = new bool[3] { false, false, false }; 
    public int singlePasswordSize = 7;
    public Color lightColor;
    private CodePanel[] panels;
    private Renderer[] lights;

    private void Awake()
    {
        Note[] notes = GetComponentsInChildren<Note>();
        lights = transform.Find("Lights").GetComponentsInChildren<Renderer>();
        System.Random r = StringResources.instance.random;
        List<CodePanel> list = new List<CodePanel>();
        foreach(Note note in notes)
        {
            string digits = "";
            for(int i = 0; i < singlePasswordSize; i++)
            {
                digits += r.Next(0, 10);
            }
            string password = StringResources.instance.ElementAt(StringResources.LocalDictionaryType.speech,
                note.ID)[0].Replace("?", digits);
            StringResources.instance.SetElementAt(StringResources.LocalDictionaryType.speech,
                    note.ID, new string[] { password });
            CodePanel panel = note.gameObject.GetComponentInChildren<CodePanel>();
            panel.password = digits;
            list.Add(panel);
        }
        panels = list.ToArray();
    }

    public void SetActive(string senderPassword)
    {
        //�� ������ ����� ������� ������� ���� ������ ������
        int index = 0;
        for (; index < 3; index++)
        {
            if (senderPassword == panels[index].password)
                break;
        }
        //������ �� ������ ��� ��� �������
        if (index > 2 || activated[index])
            return;
        //������ ���� ����������� ����� (������� ���� ��������������� � ����������)
        activated[index] = true;
        lights[index].material.SetColor("_EmissionColor", lightColor);
    }
}
