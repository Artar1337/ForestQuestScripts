using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private RectTransform target;
    private Canvas canvas;

    private void Awake()
    {
        target = transform.parent.GetComponent<RectTransform>();
        canvas = GameObject.Find("Main Canvas").GetComponent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        target.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        target.SetAsLastSibling();
    }
}
