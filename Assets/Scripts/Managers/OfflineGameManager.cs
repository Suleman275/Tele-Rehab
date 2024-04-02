using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OfflineGameManager : MonoBehaviour {
    [SerializeField] private GameObject OfflineGameEnv;
    [SerializeField] private OfflineBallSpawner OfflineBallSpawner;
    
    public static OfflineGameManager Instance;

    private void Awake() {
        Instance = this;
    }

    public void StartGame(int numOfBalls, int wallHeight) {
        //print($"starting game with {numOfBalls} and {wallHeight}");
        OfflineGameEnv.SetActive(true);
        OfflineBallSpawner.SpawnBalls(numOfBalls);
    }
}
