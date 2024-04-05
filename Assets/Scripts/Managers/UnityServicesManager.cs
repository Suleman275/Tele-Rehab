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

    private void Awake() {
        Instance = this;
    }

    //test code
    public async Task<string> StartHostWithRelay() {
        print("starting host");
        //Initialize the Unity Services engine
        await UnityServices.InitializeAsync();
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
        await UnityServices.InitializeAsync();
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

    public async void CreateLobby() {
        if (!AuthenticationService.Instance.IsSignedIn) {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        Allocation a = await RelayService.Instance.CreateAllocationAsync(2);
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        var options = new CreateLobbyOptions() {
            Data = new Dictionary<string, DataObject>() {
                {
                    "RelayCode", new DataObject(DataObject.VisibilityOptions.Member, joinCode)
                }
            }
        };

        var lobby = await Lobbies.Instance.CreateLobbyAsync(UserDataManager.Instance.userEmail, 2, options);

        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));
        
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(a.RelayServer.IpV4, (ushort) a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        currentLobby = lobby;
    }

    private IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds) {
        var delay = new WaitForSeconds(waitTimeSeconds);

        while (true) {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    public async void JoinLobby(string lobbyId) {
        var lobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId);

        currentLobby = lobby;
        
        var a = await RelayService.Instance.JoinAllocationAsync(lobby.Data["RelayCode"].Value);
        // Configure transport
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(a.RelayServer.IpV4, (ushort) a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
    }
}
