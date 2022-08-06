using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    //��� ��������
    new public string name = "";
    //����������
    public Sprite photo = null;
    //�������� ��������
    public string info = "";
    //���������� ID
    public int ID = 0;
    //���� �������� ��������
    public AudioClip pickSound = null;
    //���� �������������
    public AudioClip useSound = null;
}
