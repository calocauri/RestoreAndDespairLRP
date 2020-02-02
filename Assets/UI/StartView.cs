using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartView : MonoBehaviour {
    // Transform cameraTransform;
    public GameObject nextView;
    public PlayerController player1, player2;
    bool readyPlayer_1;
    bool readyPlayer_2;
    public Image tick_1;
    public Image tick_2;
    void OnEnable() {
        player1.canMove = player2.canMove = false;
        Camera.main.orthographicSize = 4.6f;
        tick_1.enabled = tick_2.enabled = readyPlayer_1 = readyPlayer_2 = false;
        // Camera.main.transform.SetPositionAndRotation(cameraTransform.position, cameraTransform.rotation);
    }
    void Update() {
        InputListener();
    }

    void InputListener() {
        if (Input.GetButtonUp("Interact_1")) {
            tick_2.enabled = readyPlayer_1 = !readyPlayer_1;
        }
        if (Input.GetButtonUp("Interact_2")) {
            tick_1.enabled = readyPlayer_2 = !readyPlayer_2;
        }
        if (readyPlayer_1 && readyPlayer_2) {
            readyPlayer_1 = readyPlayer_2 = false;
            player1.canMove = player2.canMove = true;
            nextView.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
