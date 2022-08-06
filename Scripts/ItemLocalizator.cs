using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class ItemLocalizator : MonoBehaviour
{
    //�����!
    //�������� � ������� ������ ���� ������������� �� ID
    //(� xml ��������� �� ����������� �� �����)

    public Item[] allInGameItems;
    // Start is called before the first frame update
    void Start()
    {
        string path = "Localization/";
        switch (StringResources.instance.currentLocalization)
        {
            case StringResources.Localization.�������:
                path += "RUitems";
                break;
            case StringResources.Localization.English:
                path += "ENitems";
                break;
            default:
                throw new System.Exception("FATAL ERROR DURING ITEM NAMING!");
        }
        
        //��������� ��������
        TextAsset obj = (TextAsset)Resources.Load(path);
        if (obj == null)
            throw new System.Exception("ERROR FINDING OR LOADING ITEM NAMING TEXT RESOURCES!");
        XmlDocument document = new XmlDocument();
        document.Load(new StringReader(obj.text));
        //�������� �������� ������� (��������� � ����� ��� root)
        XmlElement root = document.DocumentElement;
        if (root != null)
        {
            //��� ������� items (node ����� 1)
            foreach (XmlElement mainNode in root)
            {
                int curID = -1;
                //��� ������� item � items
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
            //�������� ���������� ��������!
            return;
        }
        throw new System.Exception("Root element not found in XML!");
    }

}
