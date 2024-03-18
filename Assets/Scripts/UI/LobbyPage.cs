using System.Collections.Generic;
using MiniUI;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyPage : MiniPage {
    [SerializeField] private StyleSheet styles;

    private Lobby lobby;
    
    MiniElement playersSection;
    protected override async void RenderPage() {
        AddStyleSheet(styles);
        
        //using email for now, change to name later
        lobby = await UnityServicesManager.Instance.CreateLobby(UserDataManager.Instance.email, 2);
        
        var lobbyEventsCallbacks = new LobbyEventCallbacks();
        // lobbyEventsCallbacks.OnPlayerJoined += OnPlayerJoined;
        lobbyEventsCallbacks.PlayerJoined += LobbyEventsCallbacksOnPlayerJoined;
        await Lobbies.Instance.SubscribeToLobbyEventsAsync(lobby.Id, lobbyEventsCallbacks);
        
        var div = CreateAndAddElement<MiniElement>("main");

        var container = div.CreateAndAddElement<MiniElement>("overlay");

        var row1 = container.CreateAndAddElement<MiniElement>("row");
        //row1.CreateAndAddElement<Label>("white").text = lobby.Name;
        row1.CreateAndAddElement<Label>("white").text = lobby.LobbyCode;
        row1.CreateAndAddElement<Label>("white").text = "Waiting For Doctor to join...";

        playersSection = container.CreateAndAddElement<MiniElement>("container");
        var patentRow = playersSection.CreateAndAddElement<MiniElement>("row");
        patentRow.CreateAndAddElement<Label>().text = UserDataManager.Instance.email; // using email temp
    }

    private void LobbyEventsCallbacksOnPlayerJoined(List<LobbyPlayerJoined> obj) {
        print(lobby.Players.Count);
        foreach (var lobbyPlayerJoined in obj) {
            var doctorRow = playersSection.CreateAndAddElement<MiniElement>("row");
            doctorRow.CreateAndAddElement<Label>().text = lobbyPlayerJoined.Player.Id; // using email temp
        }
    }
}