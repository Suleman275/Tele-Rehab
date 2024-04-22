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
            //NetworkManager.Singleton.StartHost();
            UnityServicesManager.Instance.StartHost();
            router.Navigate(this, "OnlinePatientPreGamePage");
        };
    }
}
