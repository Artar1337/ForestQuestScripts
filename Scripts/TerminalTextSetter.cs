using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalTextSetter : MonoBehaviour
{
    public string textID;
    public bool alsoRandomizeSpawnBook = true;
    public GameObject bookPrefab;
    // Start is called before the first frame update
    void Start()
    {
        char ch = (char)StringResources.instance.random.Next('A', 'Z' + 1);
        transform.Find("Text Display").Find("Canvas").
            Find("Editable Text").GetComponent<Text>().text =
            StringResources.instance.ElementAt
            (StringResources.LocalDictionaryType.speech, textID)[0].Replace('?', ch);
        Transform bookshelfs = transform.Find(ch.ToString()).Find("Bookshelfs");
        bookshelfs = bookshelfs.GetChild(StringResources.instance.random.Next(0, bookshelfs.childCount)).
            Find("PlacesToSpawn");
        Transform parent = bookshelfs.GetChild(StringResources.instance.random.Next(0, bookshelfs.childCount));
        GameObject g = Instantiate(bookPrefab, parent.position, Quaternion.identity, parent);
        g.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
