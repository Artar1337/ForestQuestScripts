using System.Collections.Generic;
using UnityEngine;

public class LineSpeaker : MonoBehaviour
{
    //ID реплики из XML файла
    public string ID;
    //выбирается случайная реплика из всех возможных
    private string[] lines;
    //выбирается некоторый цвет по ID (чтобы не задавать каждый раз, тк высока вероятность ошибиться)
    //0 - игрок
    //1 - рассказчик
    //2 - ?
    [Range(0, 2)]
    public int speakerID;

    private Color[] colors = new Color[3];
    private System.Random random = new System.Random();
    
    private void Awake()
    {
        colors[0] = new Color(0f, 1f, 174f / 255f, 160f / 255f);
        colors[1] = new Color(244f / 255f, 1f, 208f / 255f, 160f / 255f);
        colors[2] = new Color(86f / 255f, 182f / 255f, 1f, 160f / 255f);
    }

    public void Speak(bool correctItem = true)
    {
        if (ID == null)
            return;
        lines = StringResources.instance.ElementAt(StringResources.LocalDictionaryType.speech, ID);
        //вывод количества монет
        if (ID == "coin")
        {
            string str = lines[0];
            bool haveAllCoins = StringResources.instance.AddCoin();
            KeyValuePair<int, int> pair = StringResources.instance.GetCoinsCountAndMaxCoinsCount();
            str = str.Replace("-", pair.Key.ToString()).Replace("+", pair.Value.ToString());
            SubtitleReader.instance.SpeakLine(str, colors[0]);
            //собрали все монеты - больше ничего не выводим
            if (haveAllCoins)
            {
                ID = null;
            }
            return;
        }
        //вывод неверного предмета в руке
        if (!correctItem)
        {
            string[] tmp = StringResources.instance.ElementAt
                (StringResources.LocalDictionaryType.speech, "wrong_item");
            SubtitleReader.instance.SpeakLine(tmp[random.Next(0, tmp.Length)], colors[0]);
            return;
        }
        //обычный вывод
        SubtitleReader.instance.SpeakLine
            (lines[random.Next(0, lines.Length)], colors[speakerID]);
    }
}
