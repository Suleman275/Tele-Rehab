using System.Collections.Generic;
using MiniUI;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class DoctorDashboard : MiniPage {
    [SerializeField] private StyleSheet styles;

    private MiniElement lobbiesSection;
    protected override void RenderPage() {
        AddStyleSheet(styles);
        
        var container = CreateAndAddElement<MiniElement>("overlay");
        var topRow = container.CreateAndAddElement<MiniElement>("topRow");

        topRow.CreateAndAddElement<Label>().text = "Available Rooms";
        
        var refreshBtn = topRow.CreateAndAddElement<Button>();
        refreshBtn.text = "Refresh List";
        refreshBtn.clicked += () => {
            print("refresh clicked");
            ListRooms();
        };

        lobbiesSection = container.CreateAndAddElement<MiniElement>("lobbiesSection");

        ListRooms();
    }

    private async void ListRooms() {
        lobbiesSection.hierarchy.Clear();
        
        List<Lobby> lobbies = await UnityServicesManager.Instance.QueryLobbies();

        foreach (Lobby lobby in lobbies) {
            var lobbyRow = lobbiesSection.CreateAndAddElement<MiniElement>("lobbyRow");
            lobbyRow.CreateAndAddElement<Label>().text = lobby.Name;
            var joinBtn = lobbyRow.CreateAndAddElement<Button>();
            joinBtn.text = "Join Room";
            joinBtn.clicked += async () => {
                await UnityServicesManager.Instance.JoinLobby(lobby.Id);
                GetComponent<MiniComponentRouter>().Navigate(this, "DoctorLobbyPage");
            };
        }
    }
}