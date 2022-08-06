using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookDirectionChecker : MonoBehaviour
{
    public float mouseSensitivity = 1000f;
    public bool canMove = true;

    private Transform playerBody;
    private PlayerMovement movement;
    private float XRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = GameObject.Find("FPS Controller").transform;
        movement = playerBody.GetComponent<PlayerMovement>();
        mouseSensitivity = MenuButtonHandler.GetMouseSensibility();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        XRotation -= mouseY;
        if(movement.IsPlayerCrouching())
            //при движении на корточках угол обзора вверх существенно снижаетс€ из-за анимации (голова мешает просмотру)
            XRotation = Mathf.Clamp(XRotation, -5f, 76f);
        else
            XRotation = Mathf.Clamp(XRotation, -90f, 76f);
        transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
