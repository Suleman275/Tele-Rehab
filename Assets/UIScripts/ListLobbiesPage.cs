using System.Collections;
using System.Collections.Generic;
using MiniUI;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UIElements;

public class ListLobbiesPage : MiniPage {
    protected override async void RenderPage() {
        var router = GetComponent<MiniComponentRouter>();
        
        var lobbies = await UnityServicesManager.Instance.QueryLobbies();
        
        print("Lobbies Found: " + lobbies.Count);

        foreach (var lobby in lobbies) {
            CreateAndAddElement<Label>().text = lobby.Name;
            
            var btn = CreateAndAddElement<Button>();
            btn.text = "Join Lobby";
            btn.clicked += async () => {
                await UnityServicesManager.Instance.JoinLobbyAndStartClient(lobby.Id);
                router.Navigate(this, "DoctorLobbyPage");
            };
        }
    }
}
