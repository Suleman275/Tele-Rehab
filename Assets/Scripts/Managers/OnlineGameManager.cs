using System;
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

    public async void StartClient(string joinCode) {
        onlineGameEnv.SetActive(true);
        onlineGameUI.setCodeLabel(joinCode);
        UnityServicesManager.Instance.StartClientWithRelay(joinCode);
    }
}
