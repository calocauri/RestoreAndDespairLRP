using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameView : MonoBehaviour {
    public EndView endView;
    public Transform cameraTransform;
    public Image timeBar;
    void Awake() {
        timeBar.fillAmount = 1;
        GameManager.OnGameEnded += OnEnd;
    }
    void OnEnable() {
        Camera.main.transform.SetPositionAndRotation(cameraTransform.position, cameraTransform.rotation);
    }
    void Update() {
        SetTimeBar();
    }
    void SetTimeBar() {
        timeBar.fillAmount = 1 - GameManager.Shared.GameProgress;
    }
    void OnEnd(float gameResult) {
        print(gameResult);
        endView.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
