using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class OnlineGameUI : MiniPage {
    private Label ballCounterLabel;
    protected override void RenderPage() {
        ballCounterLabel = CreateAndAddElement<Label>();
    }

    public void SetBallCounterText(string text) {
        print("setting text: " + text);
        ballCounterLabel.text = text;
    }

    public void ShowGameCompletedUI() {
        _root.Clear();
        
        var retryBtn = CreateAndAddElement<Button>();
        retryBtn.text = "Play Again?";
        retryBtn.clicked += () => {
            if (UserDataManager.Instance.userRole == "Patient") {
                OnlineGameManager.Instance.TogglePatientReadyServerRPC();
            }
            else {
                OnlineGameManager.Instance.ToggleDoctorReadyServerRPC();
            }
        };

        var exitBtn = CreateAndAddElement<Button>();
        exitBtn.text = "Exit to Dashboard";
        exitBtn.clicked += () => {
            // enabled = false;
            // OfflineGameManager.Instance.ExitGame();
        };
    }
}