using System.Collections;
using System.Collections.Generic;
using MiniUI;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class ListLobbiesPage : MiniPage {
    protected override async void RenderPage() {
        var router = GetComponent<MiniComponentRouter>();
        
        var lobbies = await UnityServicesManager.Instance.QueryLobbies();

        foreach (Lobby lobby in lobbies) {
            CreateAndAddElement<Label>().text = "Lobby Name: " + lobby.Name;
            var btn = CreateAndAddElement<Button>();
            btn.text = "Join";
            btn.clicked += async () => {
                await UnityServicesManager.Instance.JoinLobbyById(lobby.Id);
                router.Navigate(this, "DoctorLobbyPage");
            };
        }
    }
}
