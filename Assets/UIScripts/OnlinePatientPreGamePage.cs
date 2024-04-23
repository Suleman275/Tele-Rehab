using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class OnlinePatientPreGamePage : MiniPage {
    protected override void RenderPage() {
        OnlineGameManager.Instance.OnGameStarted += () => enabled = false;
        
        CreateAndAddElement<Label>().text = "Patient Pre Game Page";

        var readyBtn = CreateAndAddElement<Button>();
        readyBtn.text = "Ready?";
        readyBtn.clicked += () => {
            if (OnlinePlayer.LocalInstance != null) {
                OnlineGameManager.Instance.TogglePatientReadyServerRPC();
            }
        };
    }
}