using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour {
	
	[SerializeField]
    private BreakablePropController[] props;
    private List<BreakablePropController> instanced = new List<BreakablePropController>();
    void Start () => SetProps();
    
    [ContextMenu("Set Props")]
    private void SetProps() {
        instanced.ForEach(item => DestroyImmediate(item));
        instanced.Clear();
        for (var c=0; c<20; c++) {
            var pos = new Vector2Int();
            var newProp = Instantiate(props[Random.Range(0, props.Length)]);
            var success = GridManager.Shared.PlaceProp(newProp.Size, out pos);

            if (success) {
                newProp.transform.position = new Vector3(pos.x, 0f, pos.y);
                instanced.Add(newProp);
            }
            else {
                Destroy(newProp);
            }
        }
    }
}