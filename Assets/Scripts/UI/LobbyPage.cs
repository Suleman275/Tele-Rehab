using System;
using System.Collections;
using System.Collections.Generic;
using MiniUI;
using Unity.Services.Lobbies;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyPage : MiniPage {
    [SerializeField] private StyleSheet styleSheet;

    private Label doctorStatusLabel;
    private Label lobbyCodeLabel;
    protected override void RenderPage() {
        AddStyleSheet(styleSheet);

        var container = CreateAndAddElement<MiniElement>("overlay");

        var topRow = container.CreateAndAddElement<MiniElement>("topRow");

        doctorStatusLabel = topRow.CreateAndAddElement<Label>();
        doctorStatusLabel.text = "Waiting for Doctor...";
        
        lobbyCodeLabel = topRow.CreateAndAddElement<Label>();
        lobbyCodeLabel.text = "XXXXXX";

        var optionsSection = container.CreateAndAddElement<MiniElement>("optionsSection");

        var inputDeviceDropDown = optionsSection.CreateAndAddElement<DropdownField>();
        inputDeviceDropDown.value = "Select Audio Input Device";
        // inputDeviceDropDown.choices = new List<string>() { };

        var bottomRow = container.CreateAndAddElement<MiniElement>("bottomRow");

        var readyBtn = bottomRow.CreateAndAddElement<Button>();
        readyBtn.text = "Ready?";
        readyBtn.clicked += async () => {
            await UnityServicesManager.Instance.TogglePlayerReadyStatus();
            UnityServicesManager.Instance.PrintPlayersInLobby();
        };
        
        SetupPageBackend();
    }

    private async void SetupPageBackend() {
        UnityServicesManager.Instance.OnPlayerJoinedLobby += () => {

            StartCoroutine(UpdateLabel());
        };
        
        
        
        await UnityServicesManager.Instance.CreateLobby();
        lobbyCodeLabel.text = UnityServicesManager.Instance.currentLobby.LobbyCode;
    }

    IEnumerator UpdateLabel() {
        yield return new WaitForSecondsRealtime(2f);
        
        doctorStatusLabel.text = UnityServicesManager.Instance.currentLobby.Players[1].Data["PlayerName"].Value + " has joined";
    }
}