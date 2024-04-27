using System;
using System.Collections;
using System.Collections.Generic;
using MiniUI;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class OnlineGameUI : MiniPage {
    [SerializeField] StyleSheet styles;
    [SerializeField] DoctorDashboard doctorDashboard;
    [SerializeField] PatientDashboard patientDashboard;
    [SerializeField] private GameObject gameEnv;
    
    
    private Label ballCounterLabel;
    protected override void RenderPage() {
        AddStyleSheet(styles);

        SetupEvents();
        
        ballCounterLabel = CreateAndAddElement<Label>("ballCounterLabel");
    }

    // private void Start() {
    //     RenderPage();
    // }

    public void SetBallCounterText(string text) {
        print("setting text: " + text);
        ballCounterLabel.text = text;
    }

    public void ShowGameCompletedUI() {
        _root.Clear();

        var main = CreateAndAddElement<MiniElement>("main");

        var retryBtn = main.CreateAndAddElement<Button>("btn");
        retryBtn.text = "Play Again?";
        retryBtn.clicked += () => {
            if (UserDataManager.Instance.userRole == "Patient") {
                OnlineGameManager.Instance.TogglePatientReadyServerRPC();
            }
            else {
                OnlineGameManager.Instance.ToggleDoctorReadyServerRPC();
            }
        };

        var exitBtn = main.CreateAndAddElement<Button>("btn");
        exitBtn.text = "Exit to Dashboard";
        exitBtn.clicked += () => {
            _root.Clear();
            enabled = false;
            
            NetworkManager.Singleton.Shutdown();
            gameEnv.SetActive(false);
            
            if (UserDataManager.Instance.userRole == "Patient") {
                patientDashboard.enabled = true;
            }
            else {
                doctorDashboard.enabled = true;
            }
        };
    }

    private void SetupEvents() {
        OnlineGameManager.Instance.OnGameStarted += () => {
            _root.Clear();
            ballCounterLabel = CreateAndAddElement<Label>("ballCounterLabel");
        };
    }
}