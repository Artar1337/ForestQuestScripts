using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public Animator workingAnimator;
    public bool disableAfterOpened = false;
    //�������� �� �������� ���������� �� ������ - �����, ���������������, ��� �� ���
    //(�� ���� �������� �����, �� �������� ���������� ��� ������ �� ������������� ���������)
    public bool pauseBasedAnimationEvent = false;
    //cooldown ����� �����������
    public float interactCooldown = 1.6f;
    //������� ����� cooldown
    private float currentCooldown = -0.01f;

    public void ChangeDoorState()
    {
        if (currentCooldown > 0f)
            return;
        bool closed = workingAnimator.GetBool("Closed");
        if(closed)
            workingAnimator.SetTrigger("Open");
        else
            workingAnimator.SetTrigger("Close");
        
        if (pauseBasedAnimationEvent)
        {
            if (!closed)
                workingAnimator.speed = 1f;
        }

        workingAnimator.SetBool("Closed", !closed);
        currentCooldown = interactCooldown;
        if (disableAfterOpened)
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    //������ ������� �� ������� ������ (��� ������ ��������)
    private void FixedUpdate()
    {
        if (currentCooldown > 0f)
            currentCooldown -= Time.fixedDeltaTime;
    }
}
