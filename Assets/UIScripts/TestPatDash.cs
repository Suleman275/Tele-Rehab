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
        playHostBtn.text = "NetCode Test Host with relay";
        playHostBtn.clicked += () => {
            enabled = false;
            OnlineGameManager.Instance.StartHost();
        };

        var codeTf = CreateAndAddElement<TextField>();
        codeTf.value = "Enter Join Code";

        var playClientBtn = CreateAndAddElement<Button>();
        playClientBtn.text = "NetCode Test Client with relay";
        playClientBtn.clicked += () => {
            enabled = false;
            OnlineGameManager.Instance.StartClient(codeTf.value);
        };

        // var createRelayBtn = CreateAndAddElement<Button>();
        // createRelayBtn.text = "Create relay";
        // createRelayBtn.clicked += () => { };
        
        
    }
}
