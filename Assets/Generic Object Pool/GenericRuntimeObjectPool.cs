using System.Collections.Generic;
using UnityEngine;

public class GenericRuntimeObjectPool<T> where T : PoolableObject {

    private T prefab;
    private int count;

    private Queue<T> pool = new Queue<T>();

    public bool IsInitialized => prefab != null;

    public void Initialize(T prefab) {
        this.prefab = prefab;
    }

    public T Get() {
        if (pool.Count == 0) {
            Add(1);
        }
        pool.Peek().gameObject.SetActive(true);
        pool.Peek().OnGetFromPool();
        return pool.Dequeue();
    }

    public void Return(T pooledObject) {
        pooledObject.OnReturnToPool();
        pooledObject.gameObject.SetActive(false);
        pool.Enqueue(pooledObject);
    }

    private void Add(int count) {
        this.count += count;
        for (int c = 0; c < count; c++) {
            var newObject = GameObject.Instantiate(prefab);
            newObject.name = $"{prefab.name} {this.count}";
            pool.Enqueue(newObject);
        }
    }

}