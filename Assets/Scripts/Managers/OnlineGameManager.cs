using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnlineGameManager : MonoBehaviour {
    [SerializeField] private GameObject onlineGameEnv;
    [SerializeField] private OnlineGameUI onlineGameUI;
    
    public static OnlineGameManager Instance;

    private static List<TempPlayerOnline> playersList;

    private void Awake() {
        playersList = new List<TempPlayerOnline>();
        Instance = this;
    }

    private void Start() {
        SetupEvents();
    }

    private void SetupEvents() {
        // TempPlayerOnline.OnAnyPlayerSpawned += player => {
        //     print(player.IsHost);
        //     playersList.Add(player);
        //     print("player joined");
        // };
    }

    public async void StartHost() {
        onlineGameEnv.SetActive(true);
        string joinCode = await UnityServicesManager.Instance.StartHostWithRelay();
        onlineGameUI.setCodeLabel(joinCode);
    }
    
    public async void StartClient(string joinCode) {
        onlineGameEnv.SetActive(true);
        onlineGameUI.setCodeLabel(joinCode);
        UnityServicesManager.Instance.StartClientWithRelay(joinCode);
    }
}
