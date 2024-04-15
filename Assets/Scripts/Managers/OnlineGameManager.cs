using System;
using System.Collections;
using System.Collections.Generic;using Unity.Netcode;
using UnityEngine;

public class OnlineGameManager : NetworkBehaviour {
    public static OnlineGameManager Instance;

    public Dictionary<ulong, bool> playerReadyDictionary;
    
    private void Awake() {
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void printClientIDServerRPC(ServerRpcParams serverRpcParams = default) {
        print(serverRpcParams.Receive.SenderClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void readyPlayerServerRPC(ServerRpcParams serverRpcParams = default) {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
        
        print("client " + serverRpcParams.Receive.SenderClientId + " is ready");
        print("both players ready: " + CheckBothPlayersReady());
    }

    private bool CheckBothPlayersReady() {
        if (playerReadyDictionary.Keys.Count > 1) {
            return playerReadyDictionary[0] == true && playerReadyDictionary[1] == true;
        }
        else {
            return false;
        }
    }
}