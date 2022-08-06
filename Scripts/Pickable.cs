using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    public Sprite itemSprite;
    public void PickItem()
    {
        if (Inventory.instance.PutItemInHand(itemSprite, gameObject))
        {
            //выключаем объект
            gameObject.SetActive(false);
        }
    }
}
