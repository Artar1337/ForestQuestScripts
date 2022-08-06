using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObjectDisabler : MonoBehaviour
{
    private ComputerResizeWindow window;
    private void Awake()
    {
        window = transform.parent.transform.parent.GetComponent<ComputerResizeWindow>();
    }

    public void DisableParent()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void DisableGrandParent(bool andComputer = false)
    {
        if (andComputer)
            ComputerMessageShower.instance.TurnOFFComputer();
        transform.parent.transform.parent.gameObject.SetActive(false);
    }

    public void DisableGrandGrandParent(bool andComputer = false)
    {
        if (andComputer)
            ComputerMessageShower.instance.TurnOFFComputer();
        transform.parent.transform.parent.transform.parent.gameObject.SetActive(false);
    }

    public void ResizeChange()
    {
        window.RevertSize();
    }
}
