using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour
{
    //камера, из которой летит луч
    private Camera cam;
    //ссылка на изображение прицела
    private Image crosshair;
    //текущая цель
    private GameObject target = null;
    //в данный момент можем активировать предмет
    private bool canInteract = false;
    //длина пускаемого луча
    public float rayLength = 0.9f;
    //cooldown после интерактива
    private float interactCooldown = 0.5f;
    //текущее время cooldown
    private float currentCooldown = -0.01f;
    //спрайт прицела по умолчанию
    public Sprite defaultSprite;
    //слой, который реагирует на пересечение с пускаемым лучом
    public LayerMask targetLayer;

    // Start is called before the first frame update
    private void Start()
    {
        cam = GetComponent<Camera>();
        crosshair = GameObject.Find("Main Canvas").
            transform.Find("Crosshair").GetComponent<Image>();
    }

    private void Update()
    {
        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
            return;
        }
        if (!canInteract || SubtitleReader.instance.InReadMode())
            return;

        float interaction = Input.GetAxis("Interact");
        if (interaction > 0f && target != null)
        {
            //Debug.Log("Interaction: "+ interaction);
            target.GetComponent<RaycastTarget>().Interact();
            currentCooldown = interactCooldown;
        }
    }

    private void FixedUpdate()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength, targetLayer))
        {
            if (hit.collider.gameObject == target)
            {
                //Debug.Log("Still hitting " + target.name);
                return;
            }
            target = hit.collider.gameObject;
            //Debug.Log("Hit " + target.name);
            canInteract = true;
            Sprite s = target.GetComponent<RaycastTarget>().crosshairSprite;
            if (s != null)
                crosshair.sprite = s;
            else
                crosshair.sprite = defaultSprite;
        }
        else if(canInteract)
        {
            crosshair.sprite = defaultSprite;
            canInteract = false;
            target = null;
        }
    }
}
