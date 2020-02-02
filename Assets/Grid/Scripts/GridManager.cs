using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager> {

    [SerializeField]
    private GameObject FloorTilePrefab;

    private int maxAttempts = 3;
    private int width = 16;
    private int height = 16;
    [SerializeField]
    private Dictionary<Vector2Int, bool> grid;

    protected override void Awake() {
        base.Awake();
        InitializeGrid();
    }

    private void InitializeGrid() {
        grid = new Dictionary<Vector2Int, bool>();

        for (var x = 0; x < width; x++) {
            for (var y = 0; y < height; y++) {
                var pos = new Vector2Int(x, y);
                grid[pos] = false;
                // InstantiateFloorTile(pos);
            }
        }
    }

    private void InstantiateFloorTile(Vector2Int pos) {
        var posWithOffset = new Vector2Int(pos.x - width / 2, pos.y - height / 2);
        var newFloorTile = Instantiate(FloorTilePrefab);
        newFloorTile.transform.position = new Vector3(posWithOffset.x, 0f, posWithOffset.y);
    }

    private void OnDrawGizmos() {
        if (grid == null) return;

        foreach (var kvp in grid) {
            if (kvp.Value) {
                Gizmos.color = Color.blue;
            }
            else {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawSphere(
                new Vector3(kvp.Key.x - (width / 2), 0.1f, kvp.Key.y - (height / 2)),
                0.25f
            );
        }
    }

    public bool PlaceProp(Vector2Int size, out Vector2Int pos) {
        var attemps = 0;
        while (attemps < maxAttempts) {
            var trypos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
            if (trypos.x + size.x > width) {
                continue;
            }
            if (trypos.y + size.x > height) {
                continue;
            }

            if (grid[trypos] == true) {
                continue;
            }

            for (var x = trypos.x; x < trypos.x + size.x; x++) {
                for (var y = trypos.y; y < trypos.y + size.y; y++) {
                    var relativePos = new Vector2Int(x, y);
                    if (grid.ContainsKey(relativePos)) {
                        if (grid[relativePos] == true) {
                            continue;
                        }
                    }
                }
            }

            pos = new Vector2Int
            {
                x = trypos.x - (width / 2),
                y = trypos.y - (width / 2)
            };
            for (var x = trypos.x; x < trypos.x + size.x; x++) {
                for (var y = trypos.y; y < trypos.y + size.y; y++) {
                    grid[new Vector2Int(x, y)] = true;
                }
            }
            return true;
        }

        pos = new Vector2Int();
        return false;
    }

    public void Reset() {
        for (var x = 0; x < width; x++) {
            for (var y = 0; y < height; y++) {
                grid[new Vector2Int(x, y)] = false;
            }
        }
    }

}