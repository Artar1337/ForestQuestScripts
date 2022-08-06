using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public string currentName = "Forest";
    private int currentLayer = 0;
    private LayerMask mask;
    private PlayerMovement movement;
    private Transform legs;

    #region Singleton
    public static PlayerFootsteps instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Footsteps instance error!");
            return;
        }
        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        mask = movement.groundLayers;
        legs = transform.Find("Leg Position").transform;
    }

    // Update is called once per physics frame
    private void FixedUpdate()
    {
        if (movement.GetIsGrounded())
        {
            Collider[] colliders = Physics.OverlapSphere(legs.position, movement.groundDistance, 
                mask, QueryTriggerInteraction.Ignore);

            //default
            currentLayer = LayerMask.NameToLayer("Default");
            //получаем максимальный по значению слой, его звук потом воспроизводим
            foreach(Collider col in colliders)
            {
                if (col.gameObject.layer > currentLayer)
                    currentLayer = col.gameObject.layer;
            }

            if (currentLayer < LayerMask.NameToLayer("Forest"))
                currentName = "Forest";
            else
                currentName = LayerMask.LayerToName(currentLayer);
        }
    }
}
