using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;

public class UnityServicesManager : MonoBehaviour {
    public static UnityServicesManager Instance;

    public Lobby currentLobby;
    private bool isLobbyHost;
    
    private float lobbyPollTimer;

    public Action OnPlayerJoinedLobby;

    private async void Awake() {
        Instance = this;
        await UnityServices.InitializeAsync();
        
#if UNITY_EDITOR
        if (ParrelSync.ClonesManager.IsClone()) {
            // When using a ParrelSync clone, switch to a different authentication profile to force the clone
            // to sign in as a different anonymous user account.
            print("this is psync");
            string customArgument = ParrelSync.ClonesManager.GetArgument();
            AuthenticationService.Instance.SwitchProfile($"Clone_{customArgument}_Profile");
        }
#endif
        
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update() {
        HandleLobbyPollForUpdates();
    }

    //test code
    public async Task<string> StartHostWithRelay() {
        print("starting host");
        //Initialize the Unity Services engine
        //await UnityServices.InitializeAsync();
        
        //Always authenticate your users beforehand
        if (!AuthenticationService.Instance.IsSignedIn) {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        Allocation a = await RelayService.Instance.CreateAllocationAsync(2);
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(a.RelayServer.IpV4, (ushort) a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);
        NetworkManager.Singleton.StartHost();
        
        return joinCode;
    }
    
    //test code
    public async void StartClientWithRelay(string joinCode) {
        print("starting client");
        //Initialize the Unity Services engine
        //await UnityServices.InitializeAsync();
        
        //Always authenticate your users beforehand
        if (!AuthenticationService.Instance.IsSignedIn) {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        // Join allocation
        var a = await RelayService.Instance.JoinAllocationAsync(joinCode);
        // Configure transport
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(a.RelayServer.IpV4, (ushort) a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
        // Start client
        NetworkManager.Singleton.StartClient();
    }
    
    private async void HandleLobbyPollForUpdates() {
        if (currentLobby != null) {
            lobbyPollTimer -= Time.deltaTime;

            if (lobbyPollTimer < 0) {
                lobbyPollTimer = 2;

                var updatedLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);

                if (updatedLobby.Players.Count > currentLobby.Players.Count) {
                    currentLobby = updatedLobby;
                    OnPlayerJoinedLobby?.Invoke();
                }
                else {
                    currentLobby = updatedLobby;
                }
            }
        }
    }

    public async Task CreateLobby() {
        if (!AuthenticationService.Instance.IsSignedIn) {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        int maxPlayers = 2;
        CreateLobbyOptions options = new CreateLobbyOptions() {
            IsPrivate = false,
            Player = new Player(
                data: new Dictionary<string, PlayerDataObject>() { 
                    {
                        "Username", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, UserDataManager.Instance.userEmail) 
                    },
                    {
                        "PlayerId", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, UserDataManager.Instance.userId)
                    }
                }
            ) 
        };

        currentLobby = await LobbyService.Instance.CreateLobbyAsync(UserDataManager.Instance.userEmail, maxPlayers, options);
        
        print("Lobby created " + currentLobby.LobbyCode);
        
        StartCoroutine(HeartbeatLobbyCoroutine(currentLobby.Id, 15));
    }
    
    private IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds) {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);

        while (true) {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    public async Task<List<Lobby>> QueryLobbies() {
        QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync();
        return lobbies.Results;
    }
}
