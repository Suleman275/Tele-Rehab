using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class UnityServicesManager : MonoBehaviour {
    public static UnityServicesManager Instance;

    private void Awake() {
        Instance = this;
    }

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
}
