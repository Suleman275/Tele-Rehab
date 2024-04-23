using ParrelSync;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] UnityTransport transport;

    public static UnityServicesManager Instance;

    public string relayJoinCode;

    public Lobby currentLobby;

    public string lobbyId;

    private async void Awake() {
        Instance = this;

        Authenticate();
    }

    private async void Authenticate() {
        var options = new InitializationOptions();

#if UNITY_EDITOR
        options.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "Primary");
#endif

        await UnityServices.InitializeAsync(options);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateLobby() {
        if (AuthenticationService.Instance.IsSignedIn) {
            Allocation a = await RelayService.Instance.CreateAllocationAsync(2);
            relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            var options = new CreateLobbyOptions() {
                Data = new Dictionary<string, Unity.Services.Lobbies.Models.DataObject> {
                    {
                        "RelayCode", new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode)
                    }

                }
            };

            currentLobby = await Lobbies.Instance.CreateLobbyAsync(UserDataManager.Instance.userEmail, 2, options);

            lobbyId = currentLobby.Id;

            StartCoroutine(HeartbeatLobbyCoroutine(currentLobby.Id, 15));

            transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

            NetworkManager.Singleton.StartHost();
        }
    }

    public static IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds) { 
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);

        while (true) {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    public async void JoinLobbyByID(string lobbyId) { 
        currentLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId);

        JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(currentLobby.Data["RelayCode"].Value);

        transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }
}