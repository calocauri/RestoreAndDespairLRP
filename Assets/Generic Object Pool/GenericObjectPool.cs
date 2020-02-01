using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool {

    private PoolableObject prefab;

    private Queue<PoolableObject> objects = new Queue<PoolableObject>();

    public void Init(PoolableObject prefab) {
        this.prefab = prefab;
    }

    private void AddObjects(int quantity) {
        var newElement = GameObject.Instantiate(prefab);
        newElement.OnCreatedFromPool(this);
        newElement.gameObject.SetActive(false);
        objects.Enqueue(newElement);
    }

    public PoolableObject Get() {
        if (objects.Count == 0) {
            AddObjects(1);
        }
        var element = objects.Dequeue();
        element.gameObject.SetActive(true);
        element.OnGetFromPool();
        return element;
    }

    public void Return(PoolableObject element) {
        element.OnReturnToPool();
        element.gameObject.SetActive(false);
        objects.Enqueue(element);
    }

}