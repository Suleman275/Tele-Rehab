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
    
    public async Task CreateLobby(string lobbyName, int maxPlayers, string relayJoinCode) {
        CreateLobbyOptions options = new CreateLobbyOptions();
        
        options.IsPrivate = false;
        options.Data = new Dictionary<string, DataObject>() {
            {
                "RelayJoinCode", new DataObject(DataObject.VisibilityOptions.Public, relayJoinCode)
            }
        };
		
        var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));
        UserDataManager.Instance.currentLobby = lobby;
    }
    
    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds) {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);

        while (true) {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
    
    public Task<QueryResponse> QueryLobbies() {
        return Lobbies.Instance.QueryLobbiesAsync();
    }

    public async Task JoinLobbyByCode(string lobbyCode) {
        var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
        UserDataManager.Instance.currentLobby = lobby;
    }

    public Task<Allocation> CreateRelayAllocation() {
        return RelayService.Instance.CreateAllocationAsync(2);
    }

    public void SetTransportToUseAllocation(Allocation a) {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(a, "dtls"));
    }

    public Task<string> GetRelayJoinCodeFromAllocation(Allocation allocation) {
        return RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    }

    public async Task UpdatePlayerEmail() {
        UpdatePlayerOptions options = new UpdatePlayerOptions();

        options.Data = new Dictionary<string, PlayerDataObject>() {
            {
                "PlayerEmail", new PlayerDataObject(
                    visibility: PlayerDataObject.VisibilityOptions.Public,
                    value: UserDataManager.Instance.email)
            }
        };
            
        string playerId = AuthenticationService.Instance.PlayerId;
            
        UserDataManager.Instance.currentLobby = await LobbyService.Instance.UpdatePlayerAsync(UserDataManager.Instance.currentLobby.Id, playerId, options);
        return;
    }
}
