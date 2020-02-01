using UnityEngine;

public class GameManager : Singleton<GameManager> {

    [SerializeField]
    private PropSpawner propSpawner;

    private float gameDuration = 20f;

    public GameState GameState { get; private set; }

    protected override void Awake() {
        base.Awake();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.End)) {
            EndGame();
        }
    }

    private void EndGame() {
        GameState = GameState.Ended;

        propSpawner.ClearProps();
    }

    public void StartGame() {
        if (GameState == GameState.Playing) {
            return;
        }

        GameState = GameState.Playing;
        propSpawner.SetProps();
    }

}

public enum GameState {
    NotStarted,
    Playing,
    Paused,
    Ended
}