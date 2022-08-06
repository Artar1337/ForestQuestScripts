using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePasswordGenerator : MonoBehaviour
{
    [Range(2,7)]
    public int lengthToGenerate = 6;
    public bool generateTwoParts = true;
    public string passwordLeftID, passwordRightID;

    private void Awake()
    {
        System.Random r = StringResources.instance.random;
        string digits = "";
        for(int i = 0; i < lengthToGenerate; i++)
        {
            //0 to 9 (� string �� int32 c# �������� �������������)
            digits += r.Next(0, 10);
        }
        //���������� ������ �� ������� ������
        transform.Find("Door").transform.Find("Code Panel").
            GetComponent<CodePanel>().password = digits;

        if (generateTwoParts)
        {
            //������ lengthToGenerate / 2 ����� ����������� � ������
            string passwordLeft = StringResources.instance.ElementAt(StringResources.LocalDictionaryType.speech,
                passwordLeftID)[0].Replace("?", digits.Substring(0, lengthToGenerate / 2));

            //��������� ����� � ����������
            string passwordRight = StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI,
                passwordRightID)[0].Replace("?", digits.Substring(lengthToGenerate / 2));

            StringResources.instance.SetElementAt(StringResources.LocalDictionaryType.speech, 
                passwordLeftID, new string[] { passwordLeft });
            StringResources.instance.SetElementAt(StringResources.LocalDictionaryType.UI,
                passwordRightID, new string[] { passwordRight });
            return;
        }
        string password = StringResources.instance.ElementAt(StringResources.LocalDictionaryType.UI,
                passwordLeftID)[0].Replace("?", digits);
        StringResources.instance.SetElementAt(StringResources.LocalDictionaryType.UI,
                passwordLeftID, new string[] { password });
    }
}
