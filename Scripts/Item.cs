using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    //имя предмета
    new public string name = "";
    //фотография
    public Sprite photo = null;
    //описание предмета
    public string info = "";
    //уникальный ID
    public int ID = 0;
    //звук поднятия предмета
    public AudioClip pickSound = null;
    //звук использования
    public AudioClip useSound = null;
}
