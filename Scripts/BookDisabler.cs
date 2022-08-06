using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookDisabler : MonoBehaviour
{
    [Range(0, 1)]
    public double destroyMinBorder;
    // Start is called before the first frame update
    void Start()
    {
        int size = transform.childCount;
        for(int i = 0; i < size; i++)
        {
            if (StringResources.instance.random.NextDouble() > destroyMinBorder)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
