using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameView : MonoBehaviour
{
    public Image timeBar;
    void Awake()
    {
        timeBar.transform.localScale = Vector3.one;
    }
    void Update()
    {
        SetTimeBar();
    }
    void SetTimeBar()
    {
        timeBar.transform.localScale = Vector3.one;
    }
    void OnEnd(float gameResult)
    {

    }
}
