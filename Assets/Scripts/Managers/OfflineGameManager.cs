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
    [SerializeField] private OfflineGameUI offlineGameUI;
    [SerializeField] private TestPatDash patientDashboard;
    
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
        offlineGameUI.SetCounterLabelText("Balls Completed 0 / " + numOfBalls);
    }

    public void BallCompleted() {
        numCompletedBalls++;
        offlineGameUI.SetCounterLabelText($"Balls Completed {numCompletedBalls} / {totalBalls}");

        if (numCompletedBalls == totalBalls) {
            print("Game Completed");
            offlineGameUI.GameCompleted();
        }
    }

    public void RestartGame() {
        offlineBallSpawner.ClearChildren();
        numCompletedBalls = 0;
        offlineGameUI.enabled = true;
        
        StartGame(totalBalls, wallHeight);
    }

    public void ExitGame() {
        numCompletedBalls = 0;
        offlineBallSpawner.ClearChildren();
        offlineGameEnv.SetActive(false);
        
        patientDashboard.enabled = true;
    }
}
