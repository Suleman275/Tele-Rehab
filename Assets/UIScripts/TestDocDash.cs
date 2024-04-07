using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class TestDocDash : MiniPage {
    protected override void RenderPage() {
        var router = GetComponent<MiniComponentRouter>();
        
        CreateAndAddElement<Label>().text = "Testing Dashboard Doctor";
        
        var listLobbiesPage = CreateAndAddElement<Button>();
        listLobbiesPage.text = "List Lobbies";
        listLobbiesPage.clicked += () => {
            router.Navigate(this, "ListLobbiesPage");
        };
        
        var codeTf = CreateAndAddElement<TextField>();
        codeTf.value = "Enter Join Code";

        var playClientBtn = CreateAndAddElement<Button>();
        playClientBtn.text = "NetCode Test Client with relay";
        playClientBtn.clicked += () => {
            enabled = false;
            OnlineGameManager.Instance.StartClient(codeTf.value);
        };
    }
}
