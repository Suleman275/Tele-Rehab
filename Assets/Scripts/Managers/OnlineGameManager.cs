using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnlineGameManager : MonoBehaviour {
    [SerializeField] private GameObject onlineGameEnv;
    [SerializeField] private OnlineGameUI onlineGameUI;
    
    public static OnlineGameManager Instance;

    private void Awake() {
        Instance = this;
    }

    public async void StartHost() {
        onlineGameEnv.SetActive(true);
        string joinCode = await UnityServicesManager.Instance.StartHostWithRelay();
        onlineGameUI.setCodeLabel(joinCode);
    }
    
    public void StartClient(string joinCode) {
        onlineGameEnv.SetActive(true);
        onlineGameUI.setCodeLabel(joinCode);
        UnityServicesManager.Instance.StartClientWithRelay(joinCode);
    }

    // public void TempSetDataAndStartHost(string numOfBalls, string wallHeight) {
    //     UnityServicesManager.Instance.SetLobbyData(numOfBalls, wallHeight);
    // }
}
