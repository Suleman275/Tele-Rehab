using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class OnlinePreGamePage : MiniPage {
    protected override void RenderPage() {
        CreateAndAddElement<Label>().text = "Online Pre Game Page";
        var printIdBtn = CreateAndAddElement<Button>();
        printIdBtn.text = "Print Client ID on Server";
        printIdBtn.clicked += () => {
            OnlineGameManager.Instance.printClientIDServerRPC();
        };

        var readyBtn = CreateAndAddElement<Button>();
        readyBtn.text = "Ready?";
        readyBtn.clicked += () => {
            OnlineGameManager.Instance.readyPlayerServerRPC();
        };
    }
    
    
}