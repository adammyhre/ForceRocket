using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust =  50f;

    [SerializeField] ParticleSystem leftBoosters;
    [SerializeField] ParticleSystem rightBoosters;
    [SerializeField] ParticleSystem mainBooster;

    Rigidbody rb;
    AudioSource audioSource;
    [SerializeField] AudioClip rocketSound;

    void Awake() {
        rb =  GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update() {
    }

    void FixedUpdate() {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust() {
        if (Input.GetKey(KeyCode.Space)) {
            StartThrust();
        }
        else {
            StopThrust();
        }
    }

    void StartThrust() {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(rocketSound);
        }
        if (!mainBooster.isPlaying) {
            mainBooster.Play();
        }
    }

    void StopThrust() {
        audioSource.Stop();
        mainBooster.Stop();
    }

    void ProcessRotation() {
        if (Input.GetKey(KeyCode.A)) {
            RotateRight();
        }
        else if (Input.GetKey(KeyCode.D)) {
            RotateLeft();
        }
        else {
            RotateNone();
        }
    }

    void RotateRight() {
        if (!rightBoosters.isPlaying) {
            rightBoosters.Play();
        }
        ApplyRotation(rotationThrust);
    }

    void RotateLeft() {
        if (!leftBoosters.isPlaying) {
            leftBoosters.Play();
        }
        ApplyRotation(-rotationThrust);
    }

    void RotateNone() {
        rightBoosters.Stop();
        leftBoosters.Stop();
    }

    void ApplyRotation(float rotation) {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
