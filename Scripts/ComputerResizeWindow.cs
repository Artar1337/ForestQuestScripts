using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerResizeWindow : MonoBehaviour
{
    private RectTransform rect;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void RevertSize()
    {
        if (System.Math.Abs(rect.offsetMin.x) < 0.001f)
            Resize(false);
        else
            Resize(true);
    }

    public void Resize(bool maximize)
    {
        if (maximize)
        {
            SetLeft(rect, 0);
            SetRight(rect, 0);
            SetTop(rect, 20);
            SetBottom(rect, 50);
            return;
        }
        SetLeft(rect, 400);
        SetRight(rect, 400);
        SetTop(rect, 200);
        SetBottom(rect, 200);
    }

    public void SetLeft(RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public void SetRight(RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public void SetTop(RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public void SetBottom(RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}
