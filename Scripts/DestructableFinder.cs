using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableFinder : MonoBehaviour
{
    public void FindAndDestroy()
    {
        transform.Find("Destructable").GetComponent<Destructable>().DestroyObject();
    }
}
