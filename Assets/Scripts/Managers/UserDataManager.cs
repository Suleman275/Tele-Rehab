using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;

public class UserDataManager : MonoBehaviour {
    public static UserDataManager Instance;
    
    public string role;
    public string email;
    //public string playerId;

    //public Lobby currentLobby;
    public bool isLobbyHost;
    
    public Allocation relayAllocation;
    
    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(this);
    }
}