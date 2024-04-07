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
using UnityEngine;

public class UnityServicesManager : MonoBehaviour {
    public static UnityServicesManager Instance;

    public Lobby currentLobby;
    private bool isLobbyHost;
    
    private float lobbyPollTimer;
    
    

    public Action OnPlayerJoinedLobby;
    public Action OnPlayerDataUpdated;
    public Action OnBothPlayersReady;
    public Action OnHostShouldStart;
    public Action OnClientShouldStart;

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

    public async Task CreateLobbyAndStartHost() {
        if (!AuthenticationService.Instance.IsSignedIn) {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        Allocation a = await RelayService.Instance.CreateAllocationAsync(2);
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
        

        var options = new CreateLobbyOptions() {
            IsPrivate = false,
            Data = new Dictionary<string, DataObject>() {
                {
                    "RelayCode", new DataObject(DataObject.VisibilityOptions.Member, joinCode)
                }
            },
            Player = new Player() {
                Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        "UserName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, UserDataManager.Instance.userEmail) //using email for now
                    },
                    {
                        "isReady", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "No")
                    }
                }
            }
        };

        var lobby = await Lobbies.Instance.CreateLobbyAsync(UserDataManager.Instance.userEmail, 2, options);

        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));
        
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(a.RelayServer.IpV4, (ushort) a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);
        NetworkManager.Singleton.StartHost();

        currentLobby = lobby;
        isLobbyHost = true;
    }

    private IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds) {
        var delay = new WaitForSeconds(waitTimeSeconds);

        while (true) {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    public async Task JoinLobby(string lobbyId) {
        if (!AuthenticationService.Instance.IsSignedIn) {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        var joinLobbyOptions = new JoinLobbyByIdOptions() {
            Player = new Player() {
                Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        "UserName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, UserDataManager.Instance.userEmail) //using email for now
                    },
                    {
                        "isReady", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "No")
                    }
                }
            }
        };
        var lobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId, joinLobbyOptions);

        currentLobby = lobby;
    }
    
    public async Task JoinLobbyAndStartClient(string lobbyId) {
        if (!AuthenticationService.Instance.IsSignedIn) {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        var joinLobbyOptions = new JoinLobbyByIdOptions() {
            Player = new Player() {
                Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        "UserName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, UserDataManager.Instance.userEmail) //using email for now
                    },
                    {
                        "isReady", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "No")
                    }
                }
            }
        };
        
        var lobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId, joinLobbyOptions);

        currentLobby = lobby;
        
        var a = await RelayService.Instance.JoinAllocationAsync(lobby.Data["RelayCode"].Value);
        // Configure transport
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(a.RelayServer.IpV4, (ushort) a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
        // Start client
        NetworkManager.Singleton.StartClient();
    }

    public async Task<List<Lobby>> QueryLobbies() {
        if (!AuthenticationService.Instance.IsSignedIn) {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync();
        return lobbies.Results;
    }
}
