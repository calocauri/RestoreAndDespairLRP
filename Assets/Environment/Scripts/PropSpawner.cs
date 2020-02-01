using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour {

    [SerializeField]
    private BreakablePropController[] props;

    private List<GenericObjectPool> pools = new List<GenericObjectPool>();
    private List<BreakablePropController> instancedProps = new List<BreakablePropController>();

    private void Awake() {
        foreach (var prop in props) {
            var propPool = new GenericObjectPool();
            propPool.Init(prop);
            pools.Add(propPool);
        }
    }

    public void SetProps() {
        for (var c = 0; c < 20; c++) {
            var pos = new Vector2Int();
            var pooledObject = pools[Random.Range(0, pools.Count)].Get();
            var newProp = (BreakablePropController)pooledObject;

            var success = GridManager.Shared.PlaceProp(newProp.Size, out pos);

            if (success) {
                newProp.transform.position = new Vector3(pos.x, 0f, pos.y);
                instancedProps.Add(newProp);
            }
            else {
                newProp.ReturnToPool();
            }
        }
    }

    public void ClearProps() {
        GridManager.Shared.Reset();
        foreach (var prop in instancedProps) {
            prop.ReturnToPool();
        }
    }
}