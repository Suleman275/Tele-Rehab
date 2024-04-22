using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class UnityServicesManager : MonoBehaviour {
    [SerializeField] UnityTransport transport;

    public static UnityServicesManager Instance;

    public string relayJoinCode;

    private async void Awake() {
        Instance = this;

        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }


    public async void StartHost() {
        if (AuthenticationService.Instance.IsSignedIn) {
            Allocation a = await RelayService.Instance.CreateAllocationAsync(2);
            relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            transport.SetHostRelayData(a.RelayServer.IpV4, (ushort) a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

            NetworkManager.Singleton.StartHost();
        }
    }

    public async void StartClient(string joinCode) {
        if (AuthenticationService.Instance.IsSignedIn) {
            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(joinCode);

            transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
            
            NetworkManager.Singleton.StartClient();
        }
    }
}