using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class UnityServicesManager : MonoBehaviour {
    public static UnityServicesManager Instance;

    public Lobby currentLobby;
    public bool isLobbyHost;

    private float lobbyPollTimer;

    public Action OnPlayerJoinedLobby;
    public Action OnDataAddedToLobby;
    public Action OnPlayerDataChanged;

    private void Awake() {
        Instance = this;
        InitialiseUnityServices();
    }

    private void Update() {
        HandleLobbyPollForUpdates();
    }
    
    private async void HandleLobbyPollForUpdates() {
        if (currentLobby != null) {
            lobbyPollTimer -= Time.deltaTime;

            if (lobbyPollTimer < 0) {
                lobbyPollTimer = 2;

                currentLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);
            }
        }
    }

    public async void InitialiseUnityServices() {
        try {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }
    
    public async Task SignInAnonymously() {
        
#if UNITY_EDITOR
        if (ParrelSync.ClonesManager.IsClone()) {
            // When using a ParrelSync clone, switch to a different authentication profile to force the clone
            // to sign in as a different anonymous user account.
            print("this is psync");
            string customArgument = ParrelSync.ClonesManager.GetArgument();
            AuthenticationService.Instance.SwitchProfile($"Clone_{customArgument}_Profile");
        }
#endif
        
        try {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");
        
            // Shows how to get the playerID
        }
        catch (AuthenticationException ex) {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex) {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); 

    }

    public async Task CreateLobby() {
        try {
            Allocation a = await RelayService.Instance.CreateAllocationAsync(2);
            string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(a, "dtls"));

            string lobbyName = UserDataManager.Instance.email; // using email for now
            int maxPlayers = 2;

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions() {
                Data = new Dictionary<string, DataObject>() {
                    {
                        "RelayJoinCode", new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode)
                    }
                },
                Player = new Player() {
                    Data = new Dictionary<string, PlayerDataObject>() {
                        {
                            "PlayerName",
                            new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, UserDataManager.Instance.email)
                        }, 
                        {
                            "ReadyStatus", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "No")
                        }
                    }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            currentLobby = lobby;
            isLobbyHost = true;
            
            await SetLobbyEventsCallbacks();
            
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 20));

            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.LobbyCode + " " + lobby.Id);
        }
        catch (LobbyServiceException e) {
            print(e);
        }
        catch (Exception e) {
            print(e);
        }
    }
    
    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);

        while (true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
    
    public async Task<List<Lobby>> QueryLobbies() {
        try {
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
            
            print("Lobbies Found: " + queryResponse.Results.Count);
            
            return queryResponse.Results;
        }
        catch (LobbyServiceException e) {
            print(e);
            return null;
        }
    }

    public async Task JoinLobby(string lobbyId) {
        try {
            JoinLobbyByIdOptions joinLobbyByIdOptions = new JoinLobbyByIdOptions() {
                Player = new Player() {
                    Data = new Dictionary<string, PlayerDataObject>() {
                        {
                            "PlayerName",
                            new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, UserDataManager.Instance.email)
                        }, 
                        {
                            "ReadyStatus", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "No")
                        }
                    }
                }
            };
            
            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, joinLobbyByIdOptions);

            currentLobby = lobby;
            isLobbyHost = false;

            await SetLobbyEventsCallbacks();
            
            print("Joined Lobby with code " + lobbyId);
        }
        catch (LobbyServiceException e) {
            print(e);
        }
    }

    public async Task UpdateCurrentLobby() {
        currentLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);
        print("lobby updated");
    }
    
    public void PrintPlayersInLobby() {
        if (currentLobby == null) {
            print("You are not in a lobby");
            return;
        }
        print(currentLobby.Name);
        print(currentLobby.Id);
        foreach (Player player in currentLobby.Players) {
            print(player.Id + " " + player.Data["PlayerName"].Value);
            print(player.Id + " " + player.Data["ReadyStatus"].Value);
        }
    }
    
    private async void OnApplicationQuit() {
        if (isLobbyHost) {
            await LobbyService.Instance.DeleteLobbyAsync(currentLobby.Id);
        }
    }

    public async Task SetLobbyEventsCallbacks() {
        LobbyEventCallbacks lobbyEventCallbacks = new LobbyEventCallbacks();
        lobbyEventCallbacks.PlayerJoined += list => {
            print("Player joined");
            //await UpdateCurrentLobby();
            OnPlayerJoinedLobby?.Invoke();
        };
        
        // lobbyEventCallbacks.DataAdded += async values => {
        //     print("data added");
        //     await UpdateCurrentLobby();
        //     OnDataAddedToLobby?.Invoke();
        // };
        // lobbyEventCallbacks.PlayerDataChanged += async dictionary => {
        //     print("Player Data Changed");
        //     await UpdateCurrentLobby();
        //     OnPlayerDataChanged?.Invoke();
        // };
        
        lobbyEventCallbacks.LobbyChanged += changes => {
            print("LOBBY CHANGED");
            //await UpdateCurrentLobby();
        };
        
        try {
            var lobbyEvents =
                await LobbyService.Instance.SubscribeToLobbyEventsAsync(currentLobby.Id,
                    lobbyEventCallbacks);
            
            print("callbacks set");
        }
        catch (LobbyServiceException e) {
            print(e);
        }
    }
    
    public async Task TogglePlayerReadyStatus() {
        print("updating ready status");
        Dictionary<string,PlayerDataObject> playerData;
        if (isLobbyHost) {
            playerData = currentLobby.Players[0].Data;
        }
        else {
            playerData = currentLobby.Players[1].Data;
        }

        if (playerData["ReadyStatus"].Value == "Yes") {
            playerData["ReadyStatus"].Value = "No";
        }
        else {
            playerData["ReadyStatus"].Value = "Yes";
        }
        try {
            currentLobby = await LobbyService.Instance.UpdatePlayerAsync(currentLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions() {
                Data = playerData
            });
            print("done updating");
        }
        catch (LobbyServiceException e) {
            print(e);
        }
    }
}
