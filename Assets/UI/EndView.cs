using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndView : MonoBehaviour {
    public GameObject contButtons;
    public GameObject startView;

    public Camera cam;
    public PlayerController playerOne, playerTwo;
    // public Transform cameraTransform, 
    public Transform P1Transform, P2Transform;
    private bool continueP1, continueP2;
    public GameObject readyP1, readyP2;
    public Text endFixedResult;
    public Text endDestroyedResult;

    public Image fixedPercentage;
    public Image destroyedPercentage;

    public Image timerImage;
    private float timerElapsed;

    void OnEnable() {
        // cam.transform.SetPositionAndRotation(cameraTransform.position, cameraTransform.rotation);
        cam.orthographicSize = 4.6f;
        playerOne.transform.SetPositionAndRotation(P1Transform.position, P1Transform.rotation);
        playerTwo.transform.SetPositionAndRotation(P2Transform.position, P2Transform.rotation);
        playerOne.canMove = playerTwo.canMove = false;
        readyP1.SetActive(false);
        readyP2.SetActive(false);
        // StartCoroutine(Exit());

        timerElapsed = 0f;
    }

    private void Update() {
        timerElapsed += Time.deltaTime;
        timerImage.fillAmount = 1 - (timerElapsed / 5f);

        if (timerElapsed > 5f) {
            // got to start view
            startView.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    IEnumerator Exit() {
        contButtons.SetActive(false);
        continueP1 = continueP2 = false;
        yield return new WaitForSeconds(3f);
        contButtons.SetActive(true);
        while (!continueP2 || !continueP1) {
            if (Input.GetButtonUp("Interact_1")) continueP1 = !continueP1;
            if (Input.GetButtonUp("Interact_2")) continueP2 = !continueP2;
            readyP1.SetActive(continueP1);
            readyP2.SetActive(continueP2);
            yield return null;
        }
        playerOne.canMove = playerTwo.canMove = true;
        startView.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnEnd(float gameResult) {
        print(gameResult);
        endFixedResult.text = $"{(gameResult) * 100}";
        endDestroyedResult.text = $"{(1 - gameResult) * 100}";

        fixedPercentage.fillAmount = gameResult;
        destroyedPercentage.fillAmount = 1 - gameResult;
    }

}