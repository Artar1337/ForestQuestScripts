using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotAutoDisable : MonoBehaviour
{
    private float timeToDisable = 2f, currentTime = 2f;

    private void OnEnable()
    {
        currentTime = timeToDisable;
    }

    void FixedUpdate()
    {
        currentTime -= Time.fixedDeltaTime;
        if (currentTime < 0f)
        {
            gameObject.SetActive(false);
            currentTime = timeToDisable;
        }
    }
}
