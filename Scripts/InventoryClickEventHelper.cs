using System;
using UnityEngine;

public class InventoryClickEventHelper : MonoBehaviour
{
    int index;

    private void Start()
    {
        try
        {
            index = Int32.Parse(name);
        }
        catch (NotFiniteNumberException)
        {
            index = -1;
        }
    }

    public void ItemCheckedClick()
    {
        Inventory.instance.EquipItem(index);
    }
}
