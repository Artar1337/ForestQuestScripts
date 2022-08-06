using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrollgeScreamer : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        animator.SetTrigger("Go");
    }

    public void SetCameraToPosition()
    {
        GameObject.Find("EndCam").gameObject.SetActive(false);
        transform.Find("Cam").gameObject.SetActive(true);
        StartCoroutine(LoadMenuAfterTime(5f));
    }

    private IEnumerator LoadMenuAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
