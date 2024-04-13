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
}
