using MiniUI;
using UnityEngine.UIElements;

public class LobbyPage : MiniPage {
    private Label doctorNameLabel;
    protected override async  void RenderPage() {
        //SetupEvents();
    }

    private void SetupEvents() {
        // UnityServicesManager.Instance.OnPlayerJoinedLobby += () => {
        //     doctorNameLabel.text = UnityServicesManager.Instance.currentLobby.Players[1].Data["UserName"].Value;
        // };
    }
}
