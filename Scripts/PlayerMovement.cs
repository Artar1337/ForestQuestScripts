using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float normalSpeed = 2.56f, crouchSpeed = 1.28f, sprintSpeed = 5.12f;
    public LayerMask groundLayers;
    public float groundDistance = 0.3f;

    private float gravity = -9.81f;
    private bool isGrounded = false;
    private bool isCrouching = false;
    private bool isRunning = false;
    private bool canCrouch = true;
    private Transform legPosition;
    private CharacterController controller;
    private Animator animator;
    private float epsilon = 0.0000001f;

    private Vector3 velocity, oldPosition;
    private PlayerRaycast playerRaycast;
    private int currentLayers;

    private float crouchCooldown = 1f, currentCooldown = -0.01f;

    private float maxEndurance = 10f, currentEndurance = 10f,
        currentEnduranceCooldown = -0.01f, enduranceCooldown = 2f;
    private UnityEngine.UI.Slider enduranceSlider;
    private AudioSource audioSource;
    public AudioClip playerIsTiredSound;

    public bool IsPlayerCrouching()
    {
        return isCrouching;
    }

    //фикс бага с телепортацией модели игрока
    //в неверном направлении из-за анимации ходьбы
    //(запрещаем игроку ходьбу на некотрое время,
    //за которое и телепортируем игрока без проблем)
    public void SetMCPosition(Vector3 pos)
    {
        animator.SetBool("Can Walk", false);
        controller.enabled = false;
        controller.gameObject.transform.position = pos;
        StartCoroutine(WalkCoroutine(0.3f));
    }

    private IEnumerator WalkCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        controller.enabled = true;
        animator.SetBool("Can Walk", true);
    }

    public void SetSmallControllerValues()
    {        
        controller.stepOffset = 0.015625f;
        normalSpeed = 0.25f;
        crouchSpeed = 0.125f;
        sprintSpeed = 0.5f;
        groundDistance = 0.1f;
        playerRaycast.rayLength = 0.25f;
        canCrouch = false;
    }

    public void SetPlayerGetUp()
    {
        isCrouching = false;
        animator.SetBool("Crouching", false);
        currentCooldown = crouchCooldown;
    }

    public void SetNormalControllerValues()
    {
        controller.stepOffset = 0.3f;
        normalSpeed = 2.56f;
        crouchSpeed = 1.28f;
        sprintSpeed = 5.12f;
        groundDistance = 0.2f;
        playerRaycast.rayLength = 1f;
        canCrouch = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLayers = groundLayers;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controller = GetComponent<CharacterController>();
        oldPosition = Vector3.zero;
        legPosition = transform.Find("Leg Position").transform;
        animator = transform.Find("Main Character").GetComponent<Animator>();
        animator.SetBool("Can Walk", true);

        playerRaycast = transform.Find("Main Camera").GetComponent<PlayerRaycast>();

        enduranceSlider = GameObject.Find("Main Canvas").transform.Find("Endurance").
            GetComponent<UnityEngine.UI.Slider>();
        audioSource = transform.Find("Main Camera").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.enabled)
            return;

        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = (transform.right * x + transform.forward * z);

        if(isCrouching)
            controller.Move(move * crouchSpeed * Time.deltaTime);
        else if(isRunning)
            controller.Move(move * sprintSpeed * Time.deltaTime);
        else
            controller.Move(move * normalSpeed * Time.deltaTime);

        isGrounded = Physics.CheckSphere(legPosition.position, groundDistance, 
            currentLayers, QueryTriggerInteraction.Ignore);

        //mgh, m = 1, g = gravity, h по умолчанию всегда 1
        if (isGrounded && velocity.y < 0)
            velocity.y = gravity;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        //не зависящая от фактической скорости передвижения переменная!!!
        float speed = Math.Abs(x) + Math.Abs(z);
        //фикс крутящегося на месте, но при этом идущего персонажа
        if (speed < epsilon)
            speed = 0f;
        //меняется от 0 до 2 (конвертируется в 1 аниматором)
        animator.SetFloat("Speed", speed);
        oldPosition = transform.position;

        float interaction = Input.GetAxis("Crouch");
        if (interaction > 0f && canCrouch)
        {
            //Debug.Log("Crouch: "+ interaction);
            isCrouching = !animator.GetBool("Crouching");
            animator.SetBool("Crouching", isCrouching);
            currentCooldown = crouchCooldown;
        }

        //устанавливаем визуально выносливость
        if (currentEndurance < 0f)
            currentEndurance = 0f;
        else if (currentEndurance > maxEndurance)
            currentEndurance = maxEndurance;
        enduranceSlider.value = currentEndurance;
        //проверка на спринт
        if (!isCrouching)
        {
            if (currentEnduranceCooldown > 0f)
            {
                animator.SetBool("Running", false);
                isRunning = false;
                currentEnduranceCooldown -= Time.deltaTime;
                return;
            }
            interaction = Input.GetAxis("Run");
            if (interaction > 0f)
            {
                isRunning = true;
                animator.SetBool("Running", true);
                //бежим
                if (currentEndurance > 0f)
                {
                    currentEndurance -= 1.5f * Time.deltaTime;
                    return;
                }
                //устал бежать, делаем cooldown для нажатия на run
                //и вопсроизводим звук усталости
                currentEnduranceCooldown = enduranceCooldown;
                isRunning = false;
                animator.SetBool("Running", false);
                audioSource.PlayOneShot(playerIsTiredSound);
            }
            else
            {
                isRunning = false;
                animator.SetBool("Running", false);
                if (currentEndurance < maxEndurance)
                {
                    currentEndurance += Time.deltaTime;
                }
            }
        }
        else if (currentEndurance < maxEndurance)
        {
            currentEndurance += 1.5f * Time.deltaTime;
        }
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }
}
