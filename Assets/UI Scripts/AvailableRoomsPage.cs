using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniUI;
using UnityEngine.UIElements;
using Unity.Services.Lobbies.Models;

public class AvailableRoomsPage : MiniPage {
    [SerializeField] StyleSheet styles;

    MiniComponentRouter router;

    MiniElement mainSection;
    List<Lobby> lobbies;

    protected override async void RenderPage() {
        AddStyleSheet(styles);

        router = GetComponent<MiniComponentRouter>();

        lobbies = await UnityServicesManager.Instance.QueryLobbies();

        var container = CreateAndAddElement<MiniElement>("container");

        var topSection = container.CreateAndAddElement<MiniElement>("top");

        topSection.CreateAndAddElement<Label>().text = "Available Rooms";

        var refreshBtn = topSection.CreateAndAddElement<Button>();
        refreshBtn.text = "Refresh List";
        refreshBtn.clicked += async () => {
            lobbies = await UnityServicesManager.Instance.QueryLobbies();
            RenderAvailableRooms();
        };

        mainSection = container.CreateAndAddElement<MiniElement>("main");

        RenderAvailableRooms();
    }

    private void RenderAvailableRooms() {
        mainSection.hierarchy.Clear();

        foreach (Lobby lobby in lobbies) {
            var card = mainSection.CreateAndAddElement<LobbyCard>("lobbyCard");
            card.lobbyName.text = lobby.Name;
            card.joinBtn.clicked += () => {
                UnityServicesManager.Instance.JoinLobbyByID(lobby.Id);
                router.Navigate(this, "OnlineDoctorPreGamePage");

            };
        }
    }
}

public class LobbyCard : MiniElement {
    public Label lobbyName;
    public Button joinBtn;

    public LobbyCard() {
        lobbyName = CreateAndAddElement<Label>();
        joinBtn = CreateAndAddElement<Button>();
        joinBtn.text = "Join";
    }
}