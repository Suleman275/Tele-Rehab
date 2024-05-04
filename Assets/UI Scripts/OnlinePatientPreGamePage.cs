using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class OnlinePatientPreGamePage : MiniPage {
    [SerializeField] StyleSheet styles;

    Label doctorNameLabel;
    Label lobbyCodeLabel;
    protected override void RenderPage() {
        AddStyleSheet(styles);

        SetupEvents();

        var main = CreateAndAddElement<MiniElement>("main");

        var topSection = main.CreateAndAddElement<MiniElement>("top");

        doctorNameLabel = topSection.CreateAndAddElement<Label>();
        doctorNameLabel.text = "Waiting for Doctor...";
        
        lobbyCodeLabel = topSection.CreateAndAddElement<Label>();
        lobbyCodeLabel.text = "XXXX";

        var middleSection = main.CreateAndAddElement<MiniElement>("middle");

        var bottomSection = main.CreateAndAddElement<MiniElement>("bottom");

        var readyBtn = bottomSection.CreateAndAddElement<Button>("btn");
        readyBtn.text = "Ready?";
        readyBtn.clicked += () => {
            if (OnlinePlayer.LocalInstance != null) {
                OnlineGameManager.Instance.TogglePatientReadyServerRPC();
            }
        };
    }

    private void SetupEvents() {
        OnlineGameManager.Instance.OnGameStarted += () => enabled = false;

        UnityServicesManager.Instance.OnDoctorJoined += () => {
            doctorNameLabel.text = UnityServicesManager.Instance.currentLobby.Players[1].Data["PlayerName"].Value + " has joined";
            UserDataManager.Instance.joinedDoctorName = UnityServicesManager.Instance.currentLobby.Players[1].Data["PlayerName"].Value;
        };

        UnityServicesManager.Instance.OnLobbyCreated += () => {
            lobbyCodeLabel.text = UnityServicesManager.Instance.currentLobby.LobbyCode;
        };
    }
}