using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Shared => Instance ?? (Instance = GameObject.FindObjectOfType<T>() ?? new GameObject(typeof(T).ToString()).AddComponent<T>());
    private static T Instance;
    protected virtual void Awake() {
        if(Instance) {
            Destroy(this);
        }
        else{
            Instance = this as T;
        }
    }
}
