using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndView : MonoBehaviour {
    public PlayerController playerOne, playerTwo;
    public Transform cameraTransform, P1Transform, P2Transform;
    void OnEnable() {
        Camera.main.transform.SetPositionAndRotation(cameraTransform.position, cameraTransform.rotation);
        playerOne.transform.SetPositionAndRotation(P1Transform.position, P1Transform.rotation);
        playerTwo.transform.SetPositionAndRotation(P2Transform.position, P2Transform.rotation);
    }
}
