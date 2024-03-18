using System;
using System.Threading.Tasks;
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

    private void Awake() {
        Instance = this;
        InitialiseUnityServices();
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
    
    public Task<Lobby> CreateLobby(string lobbyName, int maxPlayers) {
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = false;
		
        return LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
    }
    
    public Task<QueryResponse> QueryLobbies() {
        return Lobbies.Instance.QueryLobbiesAsync();
    }

    public Task<Lobby> JoinLobbyByCode(string lobbyCode) {
        return LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
    }

    public Task<Allocation> CreateRelayAllocation() {
        return RelayService.Instance.CreateAllocationAsync(2);
/*
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
*/
    }

    public Task<string> GetRelayJoinCodeFromAllocation(Allocation allocation) {
        return RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    }
}
