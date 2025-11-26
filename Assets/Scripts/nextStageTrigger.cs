using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class nextStageTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched next stage!");

            // Load next scene by index (make sure it's added in Build Settings)
            SceneManager.LoadScene(2);
        }
    }
}
