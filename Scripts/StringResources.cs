using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEngine.Video;
using System;

public class StringResources : MonoBehaviour
{

    #region Singleton
    public static StringResources instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Strings instance error!");
            return;
        }
        Initialize();
        instance = this;
    }
    #endregion

    private Dictionary<string, string[]> UI, speech;
    private Dictionary<string, Sprite> UIsprites;
    private Dictionary<string, VideoClip> videos;
    private int coinCount = 0;
    private int maxCoinCount = 10;
    public System.Random random;
    public Item inviteToParty;

    public enum LocalDictionaryType
    {
        UI,
        speech
    }

    public enum Localization
    {
        Русский,
        English
    }
    public Localization currentLocalization = Localization.Русский;

    private int GetAllFilesLength()
    {
        int len = 0; // длина текущего скрипта
        var files = Directory.GetFiles(@"D:\projects\ForestQuest\Assets\Scripts", "*.cs");
        foreach(string fname in files)
        {
            len += File.ReadAllText(fname).Split('\n').Length;
        }
        return len;
    }

    private void Initialize()
    {
        Debug.Log(GetAllFilesLength() + " strings in all project files");

        UI = new Dictionary<string, string[]>();
        speech = new Dictionary<string, string[]>();
        UIsprites = new Dictionary<string, Sprite>();
        videos = new Dictionary<string, VideoClip>();
        random = new System.Random();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites");
        if(sprites == null)
            throw new System.Exception("CRITICAL ERROR LOADING SPRITE RESOURCES!");
        foreach(Sprite sprite in sprites)
        {
            UIsprites.Add(sprite.name, sprite);
            Debug.Log("Loaded sprite: " + sprite.name);
        }

        VideoClip[] vids = Resources.LoadAll<VideoClip>("Video");
        if (vids == null)
            throw new System.Exception("CRITICAL ERROR LOADING VIDEO RESOURCES!");
        foreach (VideoClip v in vids)
        {
            videos.Add(v.name, v);
            Debug.Log("Loaded video: " + v.name);
        }

        //настройки по умолчанию
        //h = 1080 (высота)
        //w = 1920 (ширина)
        //q = 5 (качество на "великолепном")
        //v = 1.0f (громкость, 1 - максимум, 0 - минимум)
        //f = 1 (полный экран 1/0)
        //l = язык, 0 - русский, 1 - английский
        if (!PlayerPrefs.HasKey("h"))
            PlayerPrefs.SetInt("h", Screen.currentResolution.height);
        if (!PlayerPrefs.HasKey("w"))
            PlayerPrefs.SetInt("w", Screen.currentResolution.width);
        if (!PlayerPrefs.HasKey("q"))
            PlayerPrefs.SetInt("q", 5);
        if (!PlayerPrefs.HasKey("v"))
            PlayerPrefs.SetFloat("v", 1f);
        if (!PlayerPrefs.HasKey("f"))
            PlayerPrefs.SetInt("f", 1);
        if (!PlayerPrefs.HasKey("l"))
            PlayerPrefs.SetInt("l", 1);

        switch (PlayerPrefs.GetInt("l"))
        {
            case 0:
                currentLocalization = Localization.Русский;
                break;
            default: // английский по умолчанию (ID = 1)
                currentLocalization = Localization.English;
                break;
        }

        Debug.Log(System.String.Format("Player prefs: \nw={0}\nh={1}\nq={2}\nv={3}\nf={4}",
            GetConfigIntParameter("w"), GetConfigIntParameter("h"), GetConfigIntParameter("q"),
            GetConfigFloatParameter("v"), GetConfigIntParameter("f"))); 

        string path = "Localization/";
        switch (currentLocalization)
        {
            case Localization.Русский:
                path += "RU";
                break;
            case Localization.English:
                path += "EN";
                break;
            default:
                throw new System.Exception("CRITICAL ERROR LOADING TEXT RESOURCES!");
        }
        //загружаем документ
        TextAsset obj = (TextAsset)Resources.Load(path);
        if(obj == null)
            throw new System.Exception("ERROR FINDING OR LOADING TEXT RESOURCES!");
        XmlDocument document = new XmlDocument();
        document.Load(new StringReader(obj.text));
        //получаем корневой элемент (обозначен в файле как root)
        XmlElement root = document.DocumentElement;
        if (root != null)
        {
            int index = 0;
            foreach(XmlElement mainNode in root)
            {
                Dictionary<string, string[]> dictionary = 
                    new Dictionary<string, string[]>();

                //для каждого line в UI / speech
                foreach (XmlElement node in mainNode)
                {
                    //получаем атрибут "name" в "line"
                    XmlNode attr = node.Attributes.GetNamedItem("name");
                    if(attr == null)
                        throw new System.Exception("Error with parsing XML!");
                    string valueName = attr?.Value;
                    if (dictionary.ContainsKey(valueName))
                    {
                        throw new System.Exception("Two identical names in XML!");
                    }
                    dictionary.Add(valueName, null);
                    List<string> list = new List<string>();

                    foreach(XmlNode child in node.ChildNodes)
                    {
                        if(child.Name == "content")
                        {
                            list.Add(child.InnerText.Replace("_", System.Environment.NewLine));
                            continue;
                        }
                        throw new System.Exception("Unknown name in XML: " + child.Name);
                    }

                    dictionary[valueName] = list.ToArray();
                }

                FillDictionary(index, dictionary);
                index++;
            }
            //Успешное завершение парсинга!
            return;
        }
        throw new System.Exception("Root element not found in XML!");
    }

    private void FillDictionary(int index, Dictionary<string, string[]> dictionary, bool toConsole = true)
    {
        if (index == 0)
            UI = dictionary;
        else if(index == 1)
            speech = dictionary;
        else
            throw new System.Exception("Too many elements in root! Should be two or one!");

        if (!toConsole)
            return;
        if (index == 0)
            Debug.Log("UI:");
        else if (index == 1)
            Debug.Log("SPEECH:");
        foreach (KeyValuePair<string,string[]> p in dictionary)
            foreach(string s in p.Value)
                Debug.Log(p.Key + " => " + s);
    }

    public void SetConfigIntParameter(string parameter, int value)
    {
        if (PlayerPrefs.HasKey(parameter))
        {
            PlayerPrefs.SetInt(parameter, value);
        }
    }

    public void SetConfigFloatParameter(string parameter, float value)
    {
        if (PlayerPrefs.HasKey(parameter))
        {
            PlayerPrefs.SetFloat(parameter, value);
        }
    }

    public int GetConfigIntParameter(string parameter)
    {
        return PlayerPrefs.GetInt(parameter);
    }

    public float GetConfigFloatParameter(string parameter)
    {
        return PlayerPrefs.GetFloat(parameter);
    }

    public string[] ElementAt(LocalDictionaryType dictionary, string key)
    {
        if(dictionary == LocalDictionaryType.UI)
        {
            return GetElement(UI, key, dictionary);
        }
        else if(dictionary == LocalDictionaryType.speech)
        {
            return GetElement(speech, key, dictionary);
        }
        else 
            throw new System.Exception("Can't find dictionary: " + dictionary);
    }

    private string[] GetElement(Dictionary<string,string[]> dictionary, string key, LocalDictionaryType type)
    {
        if (!dictionary.ContainsKey(key))
            throw new System.Exception("Can't find key " + key + " in " + type);
        if (dictionary[key] == null)
            throw new System.Exception("Key " + key + " in " + type + " is null!");
        return dictionary[key];
    }

    private Dictionary<string, string[]> SetElement(Dictionary<string, string[]> dictionary, string key, LocalDictionaryType type, string[] newValue)
    {
        if (!dictionary.ContainsKey(key))
            throw new System.Exception("Can't find key " + key + " in " + type);
        dictionary[key] = newValue;
        foreach(string s in newValue)
            Debug.Log("Set " + key + " => " + s);
        return dictionary;
    }

    public void SetElementAt(LocalDictionaryType dictionary, string key, string[] newValue)
    {
        if (dictionary == LocalDictionaryType.UI)
        {
            UI = SetElement(UI, key, dictionary, newValue);
        }
        else if (dictionary == LocalDictionaryType.speech)
        {
            speech = SetElement(speech, key, dictionary, newValue);
        }
        else
            throw new System.Exception("Can't find dictionary: " + dictionary);
    }

    public Sprite GetSpriteByName(string name)
    {
        if(!UIsprites.ContainsKey(name))
            throw new System.Exception("Can't find key " + name + " in UISprites");
        return UIsprites[name];
    }

    public VideoClip GetVideoByName(string name)
    {
        if (!videos.ContainsKey(name))
            throw new System.Exception("Can't find key " + name + " in videos");
        return videos[name];
    }

    public bool AddCoin()
    {
        if(coinCount < maxCoinCount - 1)
        {
            coinCount++;
            return false;
        }
        coinCount++;
        Inventory.instance.AddItem(inviteToParty);
        return true;
    }

    public KeyValuePair<int, int> GetCoinsCountAndMaxCoinsCount()
    {
        return new KeyValuePair<int, int>(coinCount, maxCoinCount);
    }
}
