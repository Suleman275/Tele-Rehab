using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OfflineGameManager : MonoBehaviour {
    [SerializeField] private GameObject offlineGameEnv;
    [SerializeField] private OfflineBallSpawner offlineBallSpawner;
    [SerializeField] private OfflineMiddleWall offlineMiddleWall;
    
    public static OfflineGameManager Instance;

    private void Awake() {
        Instance = this;
    }

    public void StartGame(int numOfBalls, int wallHeight) {
        //print($"starting game with {numOfBalls} and {wallHeight}");
        offlineGameEnv.SetActive(true);
        offlineBallSpawner.SpawnBalls(numOfBalls);
        offlineMiddleWall.SetWallHeight(wallHeight);
    }
}
