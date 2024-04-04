using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class OnlineGameUI : MiniPage {
    private Label codeLabel;
    protected override void RenderPage() {
        codeLabel = CreateAndAddElement<Label>();
    }

    public void setCodeLabel(string text) {
        codeLabel.text = text;
    }
}
