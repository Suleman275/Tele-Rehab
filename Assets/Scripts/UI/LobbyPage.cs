using System.Collections.Generic;
using MiniUI;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyPage : MiniPage {
    [SerializeField] private StyleSheet styles;
    
    MiniElement playersSection;
    protected override async void RenderPage() {
        AddStyleSheet(styles);
        
        UserDataManager.Instance.relayAllocation = await UnityServicesManager.Instance.CreateRelayAllocation();
        UnityServicesManager.Instance.SetTransportToUseAllocation(UserDataManager.Instance.relayAllocation);

        var joinCode = await UnityServicesManager.Instance.GetRelayJoinCodeFromAllocation(UserDataManager.Instance.relayAllocation);
        
        //using email for now, change to name later
        await UnityServicesManager.Instance.CreateLobby(UserDataManager.Instance.email, 2, joinCode);

        var lobbyEventsCallbacks = new LobbyEventCallbacks();
        lobbyEventsCallbacks.PlayerJoined += LobbyEventsCallbacksOnPlayerJoined;
        lobbyEventsCallbacks.PlayerDataChanged += LobbyEventsCallbacksOnPlayerDataChanged;
        
        await Lobbies.Instance.SubscribeToLobbyEventsAsync(UserDataManager.Instance.currentLobby.Id, lobbyEventsCallbacks);
        
        var div = CreateAndAddElement<MiniElement>("main");

        var container = div.CreateAndAddElement<MiniElement>("overlay");

        var row1 = container.CreateAndAddElement<MiniElement>("row");
        row1.CreateAndAddElement<Label>("white").text = UserDataManager.Instance.currentLobby.LobbyCode;
        row1.CreateAndAddElement<Label>("white").text = joinCode;
        row1.CreateAndAddElement<Label>("white").text = "Waiting For Doctor to join...";

        playersSection = container.CreateAndAddElement<MiniElement>("container");
        var patentRow = playersSection.CreateAndAddElement<MiniElement>("row");
        patentRow.CreateAndAddElement<Label>().text = UserDataManager.Instance.email; // using email temp
    }

    private void LobbyEventsCallbacksOnPlayerDataChanged(Dictionary<int, Dictionary<string, ChangedOrRemovedLobbyValue<PlayerDataObject>>> obj) {
        print("player data changed");
    }

    private void LobbyEventsCallbacksOnPlayerJoined(List<LobbyPlayerJoined> obj) {
        print(UserDataManager.Instance.currentLobby.Players.Count);
        foreach (var lobbyPlayerJoined in obj) {
            var doctorRow = playersSection.CreateAndAddElement<MiniElement>("row");
            doctorRow.CreateAndAddElement<Label>().text = lobbyPlayerJoined.Player.Id; // using email temp
        }
    }
}