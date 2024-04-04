using UnityEngine.UIElements;
using MiniUI;
using UnityEngine;

public class OfflineInGameUI : MiniPage {
    private Label ballCounterLabel;
    protected override void RenderPage() {
        ballCounterLabel = CreateAndAddElement<Label>();
        ballCounterLabel.text = "Game Started";
    }

    public void SetCounterLabelText(string text) {
        if (ballCounterLabel == null) {
            ballCounterLabel = CreateAndAddElement<Label>();
        }
        ballCounterLabel.text = text;
    }

    public void GameCompleted() {
        _root.Clear();
        var retryBtn = CreateAndAddElement<Button>();
        retryBtn.text = "Play Again";
        retryBtn.clicked += () => {
            enabled = false;
            OfflineGameManager.Instance.RestartGame();
        };

        var exitBtn = CreateAndAddElement<Button>();
        exitBtn.text = "Exit to Dashboard";
        exitBtn.clicked += () => { };
    }
}