using System.Collections;
using System.Collections.Generic;
using MiniUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DoctorLobbyPage : MiniPage {
    [SerializeField] private StyleSheet styles;

    protected override void RenderPage() {
        AddStyleSheet(styles);
        
        var container = CreateAndAddElement<MiniElement>("overlay");

        var topRow = container.CreateAndAddElement<MiniElement>("topRow");
        topRow.CreateAndAddElement<Label>().text = UnityServicesManager.Instance.currentLobby.Players[0].Data["PlayerName"].Value;
        
        var optionsSection = container.CreateAndAddElement<MiniElement>("optionsSection");
        
        var numberOfBallsDropDown = optionsSection.CreateAndAddElement<DropdownField>();
        numberOfBallsDropDown.value = "Set Number Of Balls";
        
        var wallHeightDropDown = optionsSection.CreateAndAddElement<DropdownField>();
        wallHeightDropDown.value = "Set Wall Height";
        
        var bottomRow = container.CreateAndAddElement<MiniElement>("bottomRow");

        var readyBtn = bottomRow.CreateAndAddElement<Button>();
        readyBtn.text = "Ready?";
        readyBtn.clicked += async () => {
            await UnityServicesManager.Instance.TogglePlayerReadyStatus();
            UnityServicesManager.Instance.PrintPlayersInLobby();
        };
    }
}
