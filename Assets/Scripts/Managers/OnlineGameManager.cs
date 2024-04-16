using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.Netcode;
using UnityEngine;

public class OnlineGameManager : NetworkBehaviour {
    [SerializeField] private GameObject onlineGameEnv;
    
    public static OnlineGameManager Instance;

    public NetworkVariable<bool> isPatientReady = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> isDoctorReady = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> hasGameStarted = new NetworkVariable<bool>(false);
    public NetworkVariable<int> numOfBalls = new NetworkVariable<int>(0);
    public NetworkVariable<int> wallHeight = new NetworkVariable<int>(0);
    
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
                StartGameClientRPC();
            }
        }
    }

    [ClientRpc]
    public void StartGameClientRPC() {
        print("Game starting");
        onlineGameEnv.SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetGameDataServerRPC(int numOfBalls, int wallHeight) {
        this.numOfBalls.Value = numOfBalls;
        this.wallHeight.Value = wallHeight;
    }
}