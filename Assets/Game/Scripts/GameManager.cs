using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    [SerializeField]
    private PropSpawner propSpawner;

    private float gameDuration = 20f;
    private float gameElapsed = 0f;

    public GameState GameState { get; private set; }
    public static Action OnGameStarted = delegate { };
    public static Action<float> OnGameEnded = delegate { };

    public float GameProgress => gameElapsed / gameDuration;

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

        var fixedPercentage = CalculateFixedPercentage();
        propSpawner.ClearProps();

        OnGameEnded(fixedPercentage);
        print($"Game ended");
    }

    private float CalculateFixedPercentage() {
        var propCount = (float)propSpawner.InstancedProps.Count;
        var fixedPropCount = 0;
        foreach (var prop in propSpawner.InstancedProps) {
            if (prop.State == PropState.Fixed || prop.State == PropState.Fixing) {
                fixedPropCount++;
            }
        }

        return fixedPropCount / propCount;
    }

    private IEnumerator CGameloop() {
        while (gameElapsed < gameDuration) {
            gameElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        EndGame();
    }

    public void StartGame() {
        if (GameState == GameState.Playing) {
            return;
        }

        GameState = GameState.Playing;
        propSpawner.SetProps();

        gameElapsed = 0f;
        StartCoroutine(CGameloop());
        OnGameStarted();
    }

}