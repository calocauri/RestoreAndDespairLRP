using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndView : MonoBehaviour {

    public Camera cam;
    public PlayerController playerOne, playerTwo;
    // public Transform cameraTransform, 
    public Transform P1Transform, P2Transform;
    void OnEnable() {
        // cam.transform.SetPositionAndRotation(cameraTransform.position, cameraTransform.rotation);
        cam.orthographicSize /= 2;
        playerOne.transform.SetPositionAndRotation(P1Transform.position, P1Transform.rotation);
        playerTwo.transform.SetPositionAndRotation(P2Transform.position, P2Transform.rotation);
        playerOne.canMove = playerTwo.canMove = false;
    }
    void OnDisable() {
        cam.orthographicSize *= 2;
        playerOne.canMove = playerTwo.canMove = false;
    }
}
