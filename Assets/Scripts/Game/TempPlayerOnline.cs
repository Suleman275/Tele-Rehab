using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class TempPlayerOnline : NetworkBehaviour {
    [SerializeField] private GameObject hands;
    
    public static TempPlayerOnline LocalInstance;

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            LocalInstance = this;
            
            if (UserDataManager.Instance.userRole == "Doctor") {
                DeactivateHandsServerRPC();
            } 
        }
    }

    [ServerRpc]
    private void DeactivateHandsServerRPC() {
        DeactivateHandsClientRPC();
    }

    [ClientRpc]
    private void DeactivateHandsClientRPC() {
        hands.SetActive(false);
    }
}