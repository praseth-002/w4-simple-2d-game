using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startButton : MonoBehaviour
{
    public Button yourButton; // Reference to the button in the Inspector

    void Start()
    {
        // Add a listener to the onClick event
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        Debug.Log("You have clicked the button!"); // Your custom logic here
        // e.g., Load a new scene using SceneManager.LoadScene(1);
        SceneManager.LoadScene(1);
    }
}
