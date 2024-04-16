using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class OnlinePreGamePage : MiniPage {
    [SerializeField] private TempPlayerOnline player;
    protected override void RenderPage() {
        CreateAndAddElement<Label>().text = "Pre game page";

        var readyBtn = CreateAndAddElement<Button>();
        readyBtn.text = "Ready?";
        readyBtn.clicked += () => {
            player.TogglePlayerReadyServerRPC();
        };
    }
}