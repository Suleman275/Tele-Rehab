using Unity.Services.Lobbies.Models;
using UnityEngine;

public class UserDataManager : MonoBehaviour {
    public static UserDataManager Instance;
    
    public string role;
    public string email;
    //public string playerId;

    public Lobby currentLobby;
    
    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(this);
    }
}