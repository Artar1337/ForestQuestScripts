using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class RaycastTarget : MonoBehaviour
{
    //������ �������, ��������������� ��� ���������
    public Sprite crosshairSprite;
    //��� �������������� �������
    public enum InteractableObject
    {
        //������ �� ������
        None,
        //����� / ���� / ����� ������ ������ � ���������� � ����� ���������� Open/Close
        Openable,
        //������������� ����
        SoundEffect,
        //��������� �������
        Item,
        //������� ������, ��� ������������� ��������� ��������� item � ���������
        ItemTarget,
        //������� ����� (5 ����), �� ������������ ��-�� ����, ��� ��������� ���������� Openable, ����������,
        //�� ��� ���������� ������� id ��� enum �������� �������� ��� ��������
        CodeInterface,
        //��������� (������ ������ -> ���������� ������� ���� / �������� ������ / ������� � ����-����)
        Computer,
        //������������ ����������� ����� ��� ������������ ������� ����� �����
        RaycastBlocker,
        //�������� �� ������ ��������
        Subtitles,
        //��������/��������� ����
        DynamicSoundSource,
        //�������� ��� ���������: ������
        Collectable,
        //�������
        Note,
        //���������� ������ + ��������
        SpeakingActivator,
        //����� ���� �� ������� �����, �� ������������, ��� ��� ��������� ���������� Note.
        //�� ������� �� ��� �� �������, ��� � CodeInterface
        PartOfCode,
        //������, ������� ����� ��������� �����������
        Pickable,
        //��������� ������������������ ��������
        End
    };
    public InteractableObject objectType = InteractableObject.None;

    private bool canInteract = true;

    public void Interact()
    {
        if (objectType == InteractableObject.None || !canInteract 
            || objectType == InteractableObject.RaycastBlocker)
            return;

        canInteract = false;
        Type t = GetType();

        //�������� ����� �� ������ t (������� �����) � ������, ������ ����� enum ����
        //InteractableObject. ���� �� ��������� - exception
        MethodInfo method = t.GetMethod(objectType.ToString());
        if (method == null) throw new
           Exception(string.Format("����� {0} �� �������� � ���� {1}!",
               objectType.ToString(), t.Name));
        //�������� ����� ��� �������� ������� (this) ��� ���������� (null)
        method.Invoke(this, null);
    }

    public void Openable()
    {
        GetComponent<DoorOpener>().ChangeDoorState();
        canInteract = true;
    }

    public void Pickable()
    {
        GetComponent<Pickable>().PickItem();
        //�� ����� ���������������� � ������� ���������, ���� ���� ��������� ������ � ����!
        canInteract = true;
    }

    public void SoundEffect()
    {
        GetComponent<RandomClipPlayer>().PlayRandomClip();
        canInteract = true;
    }

    public void Item()
    {
        GetComponent<PickableItem>().Pick();
        canInteract = true;
    }

    public void ItemTarget()
    {
        Item item = Inventory.instance.currentItem;
        KeyValuePair<bool, bool> k = GetComponent<ItemTarget>().ChangeLayerIfGotCorrectItem(item);
        if (k.Key && k.Value)
            Inventory.instance.UnEquipAndRemove();
        canInteract = true;
    }

    public void Subtitles()
    {
        GetComponent<LineSpeaker>().Speak();
        canInteract = true;
    }

    public void DynamicSoundSource()
    {
        GetComponent<DynamicSoundSource>().SetVolume();
        canInteract = true;
    }

    public void Collectable()
    {
        GetComponent<LineSpeaker>().Speak();
        Destroy(gameObject);
        canInteract = true;
    }

    public void Note()
    {
        GetComponent<Note>().Read();
        canInteract = true;
    }

    public void SpeakingActivator()
    {
        GetComponent<ObjectActivator>().Activate();
        GetComponent<LineSpeaker>().Speak();
        canInteract = true;
    }

    public void Computer()
    {
        GetComponent<ComputerInteraction>().TurnONComputer();
        canInteract = true;
    }

    public void End()
    {
        GetComponent<EndGame>().End();
        canInteract = true;
    }

}
