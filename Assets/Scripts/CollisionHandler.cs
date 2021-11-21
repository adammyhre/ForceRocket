using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {

    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip victorySound;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem victoryParticles;

    AudioSource audioSource;
    bool isTransitioning = false;
    bool collisionsDisabled = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys() {
        if (Input.GetKeyDown(KeyCode.L)) {
            StartCoroutine(LoadNextLevel());
        } else if (Input.GetKeyDown(KeyCode.C)) {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    void OnCollisionEnter(Collision other) {
        if(isTransitioning || collisionsDisabled) { return; }

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
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        StartCoroutine(ReloadScene());
    }

    void StartSuccessSequence() {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel() {
        audioSource.Stop();
        audioSource.PlayOneShot(victorySound);
        victoryParticles.Play();
        isTransitioning = true;

        yield return new WaitForSeconds(levelLoadDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = 
            (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
            ? 0
            : currentSceneIndex + 1;
        
        SceneManager.LoadScene(nextSceneIndex);
    }

    IEnumerator ReloadScene() {
        audioSource.Stop();
        audioSource.PlayOneShot(explosionSound);
        explosionParticles.Play();

        yield return new WaitForSeconds(levelLoadDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
