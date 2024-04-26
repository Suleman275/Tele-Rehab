using UnityEngine.UIElements;
using MiniUI;
using UnityEngine;

public class OfflineGameUI : MiniPage {
    [SerializeField] StyleSheet styles;

    private Label ballCounterLabel;
    protected override void RenderPage() {
        AddStyleSheet(styles);

        ballCounterLabel = CreateAndAddElement<Label>("ballCounterLabel");
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

        var main = CreateAndAddElement<MiniElement>("main");

        var retryBtn = main.CreateAndAddElement<Button>("btn");
        retryBtn.text = "Play Again";
        retryBtn.clicked += () => {
            enabled = false;
            OfflineGameManager.Instance.RestartGame();
        };

        var exitBtn = main.CreateAndAddElement<Button>("btn");
        exitBtn.text = "Exit to Dashboard";
        exitBtn.clicked += () => {
            enabled = false;
            OfflineGameManager.Instance.ExitGame();
        };
    }
}