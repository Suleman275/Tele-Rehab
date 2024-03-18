using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PatientDashboard : MiniPage {
    [SerializeField] private StyleSheet styles;
    protected override void RenderPage() {
        AddStyleSheet(styles);
        
        CreateAndAddElement<Label>("title").text = "Patient Dashboard";
        var div = CreateAndAddElement<MiniElement>("main");

        var playAloneBtn = div.CreateAndAddElement<Button>("btn");
        playAloneBtn.text = "Play alone";
        playAloneBtn.clicked += () => {
            //SceneManager.LoadScene("OfflineGameScene");
        };
        
        var playWithDoctorBtn = div.CreateAndAddElement<Button>("btn");
        playWithDoctorBtn.text = "Play with Doctor";
        playWithDoctorBtn.clicked += () => {
            GetComponent<MiniComponentRouter>().Navigate(this, "LobbyPage");
        };
    }
}