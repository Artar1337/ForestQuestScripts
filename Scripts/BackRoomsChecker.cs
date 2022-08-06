using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackRoomsChecker : MonoBehaviour
{

    private void FixedUpdate()
    {
        if (transform.position.y < -100)
            SceneManager.LoadScene("Backrooms", LoadSceneMode.Single);
    }
}
