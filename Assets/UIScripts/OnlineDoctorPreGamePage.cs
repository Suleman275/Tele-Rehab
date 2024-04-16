using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class OnlineDoctorPreGamePage : MiniPage {
    protected override void RenderPage() {
        OnlineGameManager.Instance.OnGameStarted += () => enabled = false;
        
        CreateAndAddElement<Label>().text = "Doctor Pre Game Page";
        
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
        
        var readyBtn = CreateAndAddElement<Button>();
        readyBtn.text = "Ready?";
        readyBtn.clicked += () => {
            if (TempPlayerOnline.LocalInstance != null) {
                OnlineGameManager.Instance.SetGameDataServerRPC(int.Parse(ballsDD.value), int.Parse(wallDD.value));
                OnlineGameManager.Instance.ToggleDoctorReadyServerRPC();
            }
        };
    }
}