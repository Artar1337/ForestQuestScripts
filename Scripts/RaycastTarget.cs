using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class RaycastTarget : MonoBehaviour
{
    //спрайт прицела, устанавливаемый при наведении
    public Sprite crosshairSprite;
    //тип интерактивного объекта
    public enum InteractableObject
    {
        //ничего не делать
        None,
        //дверь / окно / любой другой объект с аниматором и двумя триггерами Open/Close
        Openable,
        //воспроизвести звук
        SoundEffect,
        //подобрать предмет
        Item,
        //целевой объект, для использования необходим некоторый item в инвентаре
        ItemTarget,
        //кодовый замок (5 цифр), не используется из-за того, что повторяет функционал Openable, упразднено,
        //но для сохранения рабочих id для enum пришлось оставить как заглушку
        CodeInterface,
        //компьютер (ввести пароль -> посмотреть рабочий стол / вставить флешку / сыграть в мини-игру)
        Computer,
        //блокирование возможности взять или активировать предмет через стену
        RaycastBlocker,
        //показать на экране субтитры
        Subtitles,
        //включить/выключить звук
        DynamicSoundSource,
        //предметы для коллекции: монеты
        Collectable,
        //записка
        Note,
        //активирует объект + субтитры
        SpeakingActivator,
        //часть кода от главной двери, не используется, так как повторяет функционал Note.
        //не удалено по той же причине, что и CodeInterface
        PartOfCode,
        //объект, который можно физически переместить
        Pickable,
        //запускает последовательность концовки
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

        //получаем метод из класса t (текущий класс) с именем, равным имени enum типа
        //InteractableObject. Если не определен - exception
        MethodInfo method = t.GetMethod(objectType.ToString());
        if (method == null) throw new
           Exception(string.Format("Метод {0} не определён в типе {1}!",
               objectType.ToString(), t.Name));
        //вызываем метод для текущего объекта (this) без параметров (null)
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
        //не можем интерактивничать с другими объектами, пока несём некоторый объект в руке!
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
