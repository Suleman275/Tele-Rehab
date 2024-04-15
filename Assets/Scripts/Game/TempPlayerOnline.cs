using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class TempPlayerOnline : NetworkBehaviour {
    [SerializeField] private GameObject hands;
    
    public NetworkVariable<bool> canMove = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> hasHands = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> isReady = new NetworkVariable<bool>(false);
    
    public override void OnNetworkSpawn() {
        print("Player spawned");
        
        if (!IsOwner) {
            Destroy(this);
            return;
        }

        isReady.OnValueChanged += (previousValue, newValue) => {
            print("old was " + previousValue + " new is " + newValue);
        };
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            TogglePlayerReadyServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TogglePlayerReadyServerRPC() {
        isReady.Value = !isReady.Value;
    }
}