using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameView : MonoBehaviour {
    public EndView endView;
    //public Transform cameraTransform;
    public Image timeBar;
    void Awake() {
        timeBar.fillAmount = 1;
        GameManager.OnGameEnded += OnEnd;
        GameManager.OnGameEnded += endView.OnEnd;
    }
    void OnEnable() {
        Camera.main.orthographicSize = 9.2f;
        GameManager.Shared.StartGame();
    }
    void Update() {
        SetTimeBar();
    }
    void SetTimeBar() {
        timeBar.fillAmount = 1 - GameManager.Shared.GameProgress;
    }
    void OnEnd(float gameResult) {
        endView.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
