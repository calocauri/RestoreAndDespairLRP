using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameView : MonoBehaviour {
    public Transform cameraTransform;
    public Image timeBar;
    void Awake() {
        timeBar.transform.localScale = Vector3.one;
        GameManager.OnGameEnded += OnEnd;
    }
    void OnEnable() {
        Camera.main.transform.SetPositionAndRotation(cameraTransform.position, cameraTransform.rotation);
    }
    void Update() {
        SetTimeBar();
    }
    void SetTimeBar() {
        timeBar.transform.localScale = Vector3.one * (1 - GameManager.Shared.GameProgress);
    }
    void OnEnd(float gameResult) {
        gameObject.SetActive(false);
    }
}
