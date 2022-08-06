using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class ItemLocalizator : MonoBehaviour
{
    //ВАЖНО!
    //предметы в массиве должны быть ОТСОРТИРОВАНЫ ПО ID
    //(в xml документе их сортировать не нужно)

    public Item[] allInGameItems;
    // Start is called before the first frame update
    void Start()
    {
        string path = "Localization/";
        switch (StringResources.instance.currentLocalization)
        {
            case StringResources.Localization.Русский:
                path += "RUitems";
                break;
            case StringResources.Localization.English:
                path += "ENitems";
                break;
            default:
                throw new System.Exception("FATAL ERROR DURING ITEM NAMING!");
        }
        
        //загружаем документ
        TextAsset obj = (TextAsset)Resources.Load(path);
        if (obj == null)
            throw new System.Exception("ERROR FINDING OR LOADING ITEM NAMING TEXT RESOURCES!");
        XmlDocument document = new XmlDocument();
        document.Load(new StringReader(obj.text));
        //получаем корневой элемент (обозначен в файле как root)
        XmlElement root = document.DocumentElement;
        if (root != null)
        {
            //для каждого items (node всего 1)
            foreach (XmlElement mainNode in root)
            {
                int curID = -1;
                //для каждого item в items
                foreach (XmlElement node in mainNode)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.Name == "id")
                        {
                            curID = Convert.ToInt32(child.InnerText);
                            continue;
                        }
                        else if(child.Name == "name")
                        {
                            allInGameItems[curID - 1].name = child.InnerText.Replace("_", System.Environment.NewLine);
                            continue;
                        }
                        else if(child.Name == "info")
                        {
                            allInGameItems[curID - 1].info = child.InnerText.Replace("_", System.Environment.NewLine);
                            continue;
                        }
                        throw new System.Exception("Unknown name in XML: " + child.Name);
                    }
                }
            }
            //Успешное завершение парсинга!
            return;
        }
        throw new System.Exception("Root element not found in XML!");
    }

}
