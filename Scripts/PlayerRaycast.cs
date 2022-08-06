using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour
{
    //������, �� ������� ����� ���
    private Camera cam;
    //������ �� ����������� �������
    private Image crosshair;
    //������� ����
    private GameObject target = null;
    //� ������ ������ ����� ������������ �������
    private bool canInteract = false;
    //����� ���������� ����
    public float rayLength = 0.9f;
    //cooldown ����� �����������
    private float interactCooldown = 0.5f;
    //������� ����� cooldown
    private float currentCooldown = -0.01f;
    //������ ������� �� ���������
    public Sprite defaultSprite;
    //����, ������� ��������� �� ����������� � ��������� �����
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
