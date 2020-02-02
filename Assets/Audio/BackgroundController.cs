using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();

        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameEnded += OnGameEnded;
    }

    private void OnGameEnded(float obj) {
        audioSource.Stop();
    }

    private void OnGameStarted() {
        audioSource.Stop();
        audioSource.Play();
    }
}