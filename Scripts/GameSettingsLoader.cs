using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsLoader : MonoBehaviour
{
    #region Singleton
    public static GameSettingsLoader instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Settings instance error!");
            return;
        }
        instance = this;
    }
    #endregion

    private Dictionary<string, int> values = new Dictionary<string, int>();
    private string[] names = { "quality",
                                "volume",
                                "fullscreen",
                                "mouse",
                                "w",
                                "h",
                                "rate"};
    private MenuButtonHandler handler;
    // Start is called before the first frame update
    void Start()
    {
        handler = GetComponent<MenuButtonHandler>();
        LoadSettings();
        handler.SetUI();
    }

    public void LoadSettings()
    {
        bool setDefaults = false;
        values.Clear();
        foreach(string s in names)
        {
            if (!PlayerPrefs.HasKey(s))
            {
                Debug.Log("No settings found! Setting defaults...");
                setDefaults = true;
                break;
            }
            values.Add(s, PlayerPrefs.GetInt(s));
        }
        SetSettings(setDefaults);
    }

    public void UpdateKey(string key, int value)
    {
        if (values.ContainsKey(key))
            values[key] = value;
        PlayerPrefs.SetInt(key, value);
    }

    public void SetSettings(bool toDefault = false)
    {
        //настройки по умолчанию
        if (toDefault)
        {
            values.Clear();
            //качество графики
            values.Add(names[0], 5);
            //громкость, в процентах
            values.Add(names[1], 100);
            //полноэкранный режим (1/0)
            values.Add(names[2], 1);
            //чувствительность мыши
            values.Add(names[3], 500);
            //разрешение по горизонтали
            values.Add(names[4], Screen.currentResolution.width);
            //разрешение по вертикали
            values.Add(names[5], Screen.currentResolution.height);
            //частота обновления
            values.Add(names[6], Screen.currentResolution.refreshRate);
        }
        handler.SetFullscreenMode(values[names[2]] > 0);
        handler.SetQuality(values[names[0]]);
        handler.SetGlobalVolume((float)values[names[1]] / 100f);
        handler.SetMouseFloatSensibility(values[names[3]]);
        handler.SetResolution(values[names[4]], values[names[5]], values[names[6]]);

        Debug.Log("SETTINGS");
        foreach (KeyValuePair<string, int> p in values)
        {
            Debug.Log(p.Key + "=>" + p.Value);
        }
        
    }
}
