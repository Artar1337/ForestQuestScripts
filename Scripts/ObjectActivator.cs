using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    public GameObject[] targets;
    public bool activateOnce = true;
    private bool activated = false;
    public bool activateMeshRendererAndBoxCollider = false;
    public bool activateOnTriggerEnter = false;
    public bool activateOnAwake = false;

    private void Awake()
    {
        if (activateOnAwake)
        {
            Activate();
        }
    }

    public void Activate()
    {
        if (activated && activateOnce)
            return;

        if (activateMeshRendererAndBoxCollider)
        {
            foreach (GameObject obj in targets)
            {
                obj.GetComponent<MeshRenderer>().enabled = true;
                obj.GetComponent<BoxCollider>().enabled = true;
            }
            return;
        }

        foreach(GameObject obj in targets)
        {
            obj.SetActive(!obj.activeInHierarchy);
        }
        
        activated = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activateOnTriggerEnter)
            return;
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Activate();
        }
    }
}
