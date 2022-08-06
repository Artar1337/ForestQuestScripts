using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public Item item;

    public void Pick()
    {
        if (item != null)
        {
            if (!Inventory.instance.AddItem(item))
                return;
            Destroy(gameObject);
        }
    }
}
