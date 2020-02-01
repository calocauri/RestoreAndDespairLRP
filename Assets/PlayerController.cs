using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        InputListener();
    }

    void InputListener()
    {
        var delta = new Vector3( Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical"));
        transform.Translate(delta);
    }
}
