using UnityEngine;

public class PropSpawner : MonoBehaviour {
	
	[SerializeField]
    private BreakablePropController[] props;

    private void Start() {
        for (var c=0; c<100; c++) {
            var pos = new Vector2Int();
            var newProp = Instantiate(props[Random.Range(0, props.Length)]);
            var success = GridManager.Shared.PlaceProp(newProp.Size, out pos);

            if (success) {
                newProp.transform.position = new Vector3(pos.x, 0f, pos.y);
            }
            else {
                Destroy(newProp);
            }
        }
    }

}