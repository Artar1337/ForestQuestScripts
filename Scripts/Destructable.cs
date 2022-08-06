using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject objectToSpawn;

    public void DestroyObject()
    {
        Instantiate(objectToSpawn, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }
}
