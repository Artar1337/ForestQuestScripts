using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyLampLightChanger : MonoBehaviour
{
    private Renderer[] renderers;
    private Light[] lights;
    public Color startColor, endColor;
    private Color currentColor;
    public float lerpSpeed = 5f;
    public float waitTime = 1f;
    private float currentTime;
    private bool start = true;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        lights = GetComponentsInChildren<Light>();
        currentTime = lerpSpeed + waitTime;
    }

    void FixedUpdate()
    {
        if (currentTime > waitTime) 
        {
            //значение от 0 до 1 - смешивание первого и второго цвета
            float t = (currentTime - waitTime) / lerpSpeed;
            if (start)
                currentColor = Color.Lerp(startColor, endColor, t);
            else
                currentColor = Color.Lerp(endColor, startColor, t);

            currentTime -= Time.fixedDeltaTime;
            foreach (Renderer r in renderers)
            {
                r.material.SetColor("_EmissionColor", currentColor);
            }
            foreach (Light l in lights)
            {
                l.color = currentColor;
            }
            return;
        }
        else if(currentTime > 0f)
        {
            currentTime -= Time.fixedDeltaTime;
            return;
        }
        start = !start;
        currentTime = lerpSpeed + waitTime;
    }
}
