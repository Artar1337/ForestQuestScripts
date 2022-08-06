using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    private void Awake()
    {
        int ID = StringResources.instance.random.Next(0, transform.childCount);
        Transform child = transform.GetChild(ID);
        Instantiate(objectToSpawn, child.position, child.rotation, child);
        //������ ��� ����, ����� � ���������� ���� �����, � ����� ����� �������� ������
        child.gameObject.name = "Box with the magnet";
    }
}
