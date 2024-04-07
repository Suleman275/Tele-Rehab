using MiniUI;
using UnityEngine.UIElements;

public class LobbyPage : MiniPage {
    private Label doctorNameLabel;
    protected override async  void RenderPage() {
        SetupEvents();
        
        await UnityServicesManager.Instance.CreateLobbyAndStartHost();
        
        doctorNameLabel = CreateAndAddElement<Label>();
        doctorNameLabel.text = "Waiting For Doctor...";

        var lobbyCodeLabel = CreateAndAddElement<Label>();
        lobbyCodeLabel.text = UnityServicesManager.Instance.currentLobby.LobbyCode;

        // var relayCode = CreateAndAddElement<Label>();
        // relayCode.text = UnityServicesManager.Instance.currentLobby.Data["RelayCode"].Value;
        
        var btn = CreateAndAddElement<Button>();
        btn.text = "Ready?";
        btn.clicked += () => {
            // UnityServicesManager.Instance.TogglePlayerReadyOwn();
        };
    }

    private void SetupEvents() {
        UnityServicesManager.Instance.OnPlayerJoinedLobby += () => {
            doctorNameLabel.text = UnityServicesManager.Instance.currentLobby.Players[1].Data["UserName"].Value;
        };
    }
}
