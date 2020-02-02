using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour {

    [SerializeField]
    private BreakablePropController[] props;

    private List<GenericObjectPool> pools = new List<GenericObjectPool>();
    public List<BreakablePropController> InstancedProps { get; private set; }

    private void Awake() {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameEnded += OnGameEnded;

        foreach (var prop in props) {
            var propPool = new GenericObjectPool();
            propPool.Init(prop);
            pools.Add(propPool);
        }
    }

    private void OnGameStarted() {

    }

    private void OnGameEnded(float percentage) {
        ClearProps();
    }

    public void SetProps() {
        InstancedProps = new List<BreakablePropController>();

        for (var c = 0; c < 20; c++) {
            var pos = new Vector2Int();
            var pooledObject = pools[Random.Range(0, pools.Count)].Get();
            var newProp = (BreakablePropController)pooledObject;

            var success = GridManager.Shared.PlaceProp(newProp.Size, out pos);

            if (success) {
                newProp.transform.position = new Vector3(pos.x, 0f, pos.y);
                InstancedProps.Add(newProp);
            }
            else {
                newProp.ReturnToPool();
            }
        }
    }

    public void ClearProps() {
        GridManager.Shared.Reset();
        foreach (var prop in InstancedProps) {
            prop.ReturnToPool();
        }
    }
}