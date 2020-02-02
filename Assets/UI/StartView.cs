using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartView : MonoBehaviour
{
    Transform cameraTransform;
    public UnityAction OnReady;
    bool readyPlayer_1;
    bool readyPlayer_2;
    public Image tick_1;
    public Image tick_2;
    void OnEnable()
    {
        tick_1.enabled = tick_2.enabled = readyPlayer_1 = readyPlayer_2 = false;
        Camera.main.transform.SetPositionAndRotation(cameraTransform.position, cameraTransform.rotation);
    }
    void Update()
    {
        InputListener();
    }

    void InputListener()
    {
        if(Input.GetButton("Select_1"))
        {
            tick_1.enabled = readyPlayer_1 = !readyPlayer_1;
        }
        if(Input.GetButton("Select_2"))
        {
            tick_2.enabled = readyPlayer_2 = !readyPlayer_2;
        }
        if(readyPlayer_1 && readyPlayer_2)
        {
            readyPlayer_1 = readyPlayer_2 = false;
            OnReady?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
