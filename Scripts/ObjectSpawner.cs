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
        //просто для того, чтобы в инспекторе было видно, в каком ящике появился магнит
        child.gameObject.name = "Box with the magnet";
    }
}
