using System;
using System.Collections;
using System.Collections.Generic;
using MiniUI;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class OnlineGameUI : MiniPage {
    [SerializeField] TestDocDash testDocDash;
    [SerializeField] TestPatDash testPatDash;
    [SerializeField] private GameObject gameEnv;
    
    
    private Label ballCounterLabel;
    protected override void RenderPage() {
        SetupEvents();
        
        ballCounterLabel = CreateAndAddElement<Label>();
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
        
        var retryBtn = CreateAndAddElement<Button>();
        retryBtn.text = "Play Again?";
        retryBtn.clicked += () => {
            if (UserDataManager.Instance.userRole == "Patient") {
                OnlineGameManager.Instance.TogglePatientReadyServerRPC();
            }
            else {
                OnlineGameManager.Instance.ToggleDoctorReadyServerRPC();
            }
        };

        var exitBtn = CreateAndAddElement<Button>();
        exitBtn.text = "Exit to Dashboard";
        exitBtn.clicked += () => {
            _root.Clear();
            enabled = false;
            
            NetworkManager.Singleton.Shutdown();
            gameEnv.SetActive(false);
            
            if (UserDataManager.Instance.userRole == "Patient") {
                testPatDash.enabled = true;
            }
            else {
                testDocDash.enabled = true;
            }
        };
    }

    private void SetupEvents() {
        OnlineGameManager.Instance.OnGameStarted += () => {
            _root.Clear();
            ballCounterLabel = CreateAndAddElement<Label>();
        };
    }
}