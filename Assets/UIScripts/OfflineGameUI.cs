using UnityEngine.UIElements;
using MiniUI;
using UnityEngine;

public class OfflineGameUI : MiniPage {
    private Label ballCounterLabel;
    protected override void RenderPage() {
        ballCounterLabel = CreateAndAddElement<Label>();
        ballCounterLabel.text = "Game Stared";
    }

    public void SetCounterLabelText(string text) {
        ballCounterLabel.text = text;
    }
}