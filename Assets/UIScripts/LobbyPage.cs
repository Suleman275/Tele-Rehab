using MiniUI;
using UnityEngine.UIElements;

public class LobbyPage : MiniPage {
    private Label doctorNameLabel;
    protected override async  void RenderPage() {
        SetupEvents();
        
        await UnityServicesManager.Instance.CreateLobby();

        CreateAndAddElement<Label>().text = "Lobby Code: " + UnityServicesManager.Instance.currentLobby.LobbyCode;
        CreateAndAddElement<Label>().text = "Lobby Name: " + UnityServicesManager.Instance.currentLobby.Name;
        
        doctorNameLabel = CreateAndAddElement<Label>();
        doctorNameLabel.text = "Waiting for Doctor...";
    }

    private void SetupEvents() {
        UnityServicesManager.Instance.OnPlayerJoinedLobby += () => {
            doctorNameLabel.text = UnityServicesManager.Instance.currentLobby.Players[1].Data["UserName"].Value + " has joined";
        };
    }
}
