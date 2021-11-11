using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {

    [SerializeField] float levelLoadDelay = 2f;

    void OnCollisionEnter(Collision other) {
        switch(other.gameObject.tag) {
            case "Finish":
                StartSuccessSequence();
                break;
            case "Friendly":
                print("Friendly");
                break;
            default: 
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence() {
        GetComponent<Movement>().enabled = false;
        StartCoroutine(ReloadScene());
    }

    void StartSuccessSequence() {
        GetComponent<Movement>().enabled = false;
        StartCoroutine(LoadNextLevel());

    }

    IEnumerator LoadNextLevel() {
        yield return new WaitForSeconds(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = 
            (currentSceneIndex == SceneManager.sceneCountInBuildSettings)
            ? 0
            : currentSceneIndex + 1;
        
        SceneManager.LoadScene(nextSceneIndex);
    }

    IEnumerator ReloadScene() {
        yield return new WaitForSeconds(levelLoadDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
