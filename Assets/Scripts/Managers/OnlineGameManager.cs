using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.Netcode;
using UnityEngine;

public class OnlineGameManager : NetworkBehaviour {
    public static OnlineGameManager Instance;

    public NetworkVariable<bool> isPatientReady = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> isDoctorReady = new NetworkVariable<bool>(false);
    
    private void Awake() {
        Instance = this;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPatientReadyStatusServerRPC(bool value) {
        isPatientReady.Value = value;
        CheckBothPlayersReady();
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SetDoctorReadyStatusServerRPC(bool value) {
        isDoctorReady.Value = value;
        CheckBothPlayersReady();
    }

    private void CheckBothPlayersReady() {
        if (isPatientReady.Value && isDoctorReady.Value) {
            print("both Players ready");
        }
        else {
            print("both not ready");
        }
    }
}