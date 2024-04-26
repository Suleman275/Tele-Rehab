using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AstraManager : MonoBehaviour {
    public static AstraManager Instance;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        // Ensure that the AstraSDKManager exists
        if (AstraSDKManager.Instance == null) {
            Debug.LogError("AstraSDKManager is not assigned!");
            return;
        }

        // Subscribe to the OnInitializeSuccess event
        AstraSDKManager.Instance.OnInitializeSuccess.AddListener(OnAstraSDKInitializeSuccess);
    }

    void OnAstraSDKInitializeSuccess() {
        Debug.Log("Astra SDK initialized successfully!");
    }

    public void StartBodyStream() {
        AstraSDKManager.Instance.IsBodyOn = true;
    }
    
    public void StopBodyStream() {
        AstraSDKManager.Instance.IsBodyOn = false;
    }
}