using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.Netcode;
using UnityEngine;

public class OnlineGameManager : NetworkBehaviour {
    [SerializeField] private GameObject onlineGameEnv;
    [SerializeField] private OnlineMiddleWall onlineMiddleWall;
    [SerializeField] private OnlineBallSpawner onlineBallSpawner;
    [SerializeField] private OnlineGameUI onlineGameUI;
    
    public static OnlineGameManager Instance;

    public NetworkVariable<bool> isPatientReady = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> isDoctorReady = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> hasGameStarted = new NetworkVariable<bool>(false);
    public NetworkVariable<int> numOfBalls = new NetworkVariable<int>(0);
    public NetworkVariable<int> wallHeight = new NetworkVariable<int>(0);

    private int numOfCompletedBalls;

    public Action OnGameStarted;
    
    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (!hasGameStarted.Value) {
            HandleGameStart();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TogglePatientReadyServerRPC() {
        isPatientReady.Value = !isPatientReady.Value;
        print("patient ready: " + isPatientReady.Value);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void ToggleDoctorReadyServerRPC() {
        isDoctorReady.Value = !isDoctorReady.Value;
        print("doctor ready: " + isDoctorReady.Value);
    }

    private void HandleGameStart() {
        if (IsServer) {
            if (isPatientReady.Value && isDoctorReady.Value) {
                hasGameStarted.Value = true;
                StartGameClientRPC(wallHeight.Value, numOfCompletedBalls, numOfBalls.Value);
                
                onlineBallSpawner.SpawnBalls(numOfBalls.Value);
            }
        }
    }

    [ClientRpc]
    public void StartGameClientRPC(int wallHeight, int completedBalls, int totalBalls) {
        print("Game starting");
        OnGameStarted?.Invoke();
        onlineGameEnv.SetActive(true);
        onlineMiddleWall.SetWallHeight(wallHeight);
        onlineGameUI.enabled = true;
        onlineGameUI.SetBallCounterText($"Balls Completed {completedBalls} / {totalBalls}");
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetGameDataServerRPC(int numOfBalls, int wallHeight) {
        this.numOfBalls.Value = numOfBalls;
        this.wallHeight.Value = wallHeight;
        print("game data set");
    }

    [ServerRpc(RequireOwnership = false)]
    public void BallCompletedServerRPC() {
        print("Ball Completed");
        this.numOfCompletedBalls++;
        NotifyClientsClientRPC(numOfCompletedBalls, numOfBalls.Value);

        if (numOfCompletedBalls == numOfBalls.Value) {
            
            isDoctorReady.Value = false;
            isPatientReady.Value = false;
            hasGameStarted.Value = false;
            numOfCompletedBalls = 0;
            
            ShowGameCompletedUIClientRPC();
        }
    }

    [ClientRpc]
    private void NotifyClientsClientRPC(int newCount, int oldCount) {
        onlineGameUI.SetBallCounterText($"Balls Completed {newCount} / {oldCount}");
    }

    [ClientRpc]
    private void ShowGameCompletedUIClientRPC() {
        onlineGameUI.ShowGameCompletedUI();
    }
}