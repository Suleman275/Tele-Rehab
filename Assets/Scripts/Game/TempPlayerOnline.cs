using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class TempPlayerOnline : NetworkBehaviour {
    [SerializeField] private GameObject hands;

    public NetworkVariable<bool> isReady = new NetworkVariable<bool>(false);
    
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            Destroy(this);
            return;
        }
        
        print("Player spawned: " + UserDataManager.Instance.userRole);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TogglePlayerReadyServerRPC() {
        isReady.Value = !isReady.Value;
        OnlineGameManager.Instance.SetPatientReadyStatusServerRPC(isReady.Value);
    }
}