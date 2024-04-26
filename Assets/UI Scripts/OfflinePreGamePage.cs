using MiniUI;
using UnityEngine.UIElements;
using UnityEngine;

public class OfflinePreGamePage : MiniPage {
    [SerializeField] StyleSheet styles;
    protected override void RenderPage() {
        AddStyleSheet(styles);

        var main = CreateAndAddElement<MiniElement>("main");

        var ballsDD = main.CreateAndAddElement<DropdownField>();
        ballsDD.value = "Select Number Of Balls";
        ballsDD.choices.Add("3");
        ballsDD.choices.Add("5");
        ballsDD.choices.Add("7");
        ballsDD.choices.Add("9");

        var wallDD = main.CreateAndAddElement<DropdownField>();
        wallDD.value = "Select Wall Height";
        wallDD.choices.Add("5");
        wallDD.choices.Add("7");
        wallDD.choices.Add("9");
        wallDD.choices.Add("11");

        var btn = main.CreateAndAddElement<Button>("btn");
        btn.text = "Start Game";
        btn.clicked += () => {
            enabled = false;
            OfflineGameManager.Instance.StartGame(int.Parse(ballsDD.value), int.Parse(wallDD.value));
        };
    }
}