using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class DoctorLobbyPage : MiniPage {
    protected override void RenderPage() {
        var ballsDD = CreateAndAddElement<DropdownField>();
        ballsDD.value = "Select Number Of Balls";
        ballsDD.choices.Add("3");
        ballsDD.choices.Add("5");
        ballsDD.choices.Add("7");
        ballsDD.choices.Add("9");

        var wallDD = CreateAndAddElement<DropdownField>();
        wallDD.value = "Select Wall Height";
        wallDD.choices.Add("5");
        wallDD.choices.Add("7");
        wallDD.choices.Add("9");
        wallDD.choices.Add("11");

        var btn = CreateAndAddElement<Button>();
        btn.text = "Start Game";
        btn.clicked += () => {
            // OnlineGameManager.Instance.SetGameData(ballsDD.value, wallDD.value);
            // UnityServicesManager.Instance.TogglePlayerReadyOwn();
            // OnlineGameManager.Instance.TempSetDataAndStartHost(ballsDD.value, wallDD.value);
            UnityServicesManager.Instance.SetPlayerData(ballsDD.value, wallDD.value);
        };
    }
}