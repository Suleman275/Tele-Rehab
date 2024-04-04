using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class OfflineGameManager : MonoBehaviour {
    [SerializeField] private GameObject offlineGameEnv;
    [SerializeField] private OfflineBallSpawner offlineBallSpawner;
    [SerializeField] private OfflineMiddleWall offlineMiddleWall;
    [SerializeField] private OfflineInGameUI offlineInGameUI;
    
    public static OfflineGameManager Instance;

    private int numCompletedBalls;
    private int totalBalls;
    private int wallHeight;
    private void Awake() {
        Instance = this;
        numCompletedBalls = 0;
    }

    public void StartGame(int numOfBalls, int givenWallHeight) {
        totalBalls = numOfBalls;
        wallHeight = givenWallHeight;
        
        offlineGameEnv.SetActive(true);
        offlineBallSpawner.SpawnBalls(numOfBalls);
        offlineMiddleWall.SetWallHeight(wallHeight);
        offlineInGameUI.SetCounterLabelText("Balls Completed 0 / " + numOfBalls);
    }

    public void BallCompleted() {
        numCompletedBalls++;
        offlineInGameUI.SetCounterLabelText($"Balls Completed {numCompletedBalls} / {totalBalls}");

        if (numCompletedBalls == totalBalls) {
            print("Game Completed");
            offlineInGameUI.GameCompleted();
        }
    }

    public void RestartGame() {
        offlineBallSpawner.ClearChildren();
        numCompletedBalls = 0;
        
        offlineBallSpawner.SpawnBalls(totalBalls);
        offlineMiddleWall.SetWallHeight(wallHeight);
        offlineInGameUI.enabled = true;
        offlineInGameUI.SetCounterLabelText("Balls Completed 0 / " + totalBalls);
    }
}
