using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartButton : MonoBehaviour
{
    public Button yourButton;
    public Animator transitionAnimator; // Assign your transition prefab's Animator here
    public float transitionTime = 1f;   // duration of the fade

    void Start()
    {
        yourButton.onClick.AddListener(OnClickStart);
    }

    void OnClickStart()
    {
        // Start fade-out
        transitionAnimator.SetTrigger("End");

        // Load the next scene after the fade duration
        StartCoroutine(LoadSceneAfterFade(1));
    }

    IEnumerator LoadSceneAfterFade(int sceneIndex)
    {
        // Wait for the fade-out animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Load scene
        SceneManager.LoadScene(sceneIndex);

        // Optional: trigger fade-in after scene loads
        // If your transition prefab is still active across scenes
        transitionAnimator.SetTrigger("Start");
    }
}
