using System;
using System.Collections;
using UnityEngine;

public class CrosshairDisabler : MonoBehaviour
{

    //Скрипт нужен для фотокомнаты. Также отвечает за скриншоты (F2)
    private GameObject screenshot, canvas;

    private void Awake()
    {
        canvas = GameObject.Find("Main Canvas");
        screenshot = GameObject.Find("Main Canvas").transform.Find("Screenshot").gameObject;
    }

    //COORDS: -550.82; 0.065; -299.804
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;
        canvas.transform.Find("Crosshair").gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
            ScreenShot();
    }

    private void ScreenShot()
    {
        string s = "screenshot_at_" + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".png";
        Debug.Log("Screenshot taken with name: " + s);
        canvas.SetActive(false);
        ScreenCapture.CaptureScreenshot(s);
        StartCoroutine(TimedObjectActivator());
    }

    private IEnumerator TimedObjectActivator()
    {
        yield return null;
        screenshot.SetActive(true);
        canvas.SetActive(true);
    }
}
