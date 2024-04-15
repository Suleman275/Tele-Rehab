using MiniUI;
using Unity.Netcode;
using UnityEngine.UIElements;

public class TestPatDash : MiniPage {
    protected override void RenderPage() {
        var router = GetComponent<MiniComponentRouter>();
        
        CreateAndAddElement<Label>().text = "Testing Dashboard Patient";

        var playOfflineBtn = CreateAndAddElement<Button>();
        playOfflineBtn.text = "Offline game";
        playOfflineBtn.clicked += () => {
            router.Navigate(this, "OfflinePreGamePage");
        };

        var playHostBtn = CreateAndAddElement<Button>();
        playHostBtn.text = "NetCode Test Host";
        playHostBtn.clicked += () => {
            router.Navigate(this, "OnlinePreGamePage");
            NetworkManager.Singleton.StartHost();
        };

        var playClientBtn = CreateAndAddElement<Button>();
        playClientBtn.text = "NetCode Test Client";
        playClientBtn.clicked += () => {
            router.Navigate(this, "OnlinePreGamePage");
            NetworkManager.Singleton.StartClient();
        };
    }
}
