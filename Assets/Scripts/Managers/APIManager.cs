using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour {
    public string serverUrl = "http://localhost:3000";
    public static APIManager Instance;

    public Action OnUserRegistered;
    public Action<string> OnUserRegisterError;
    public Action<UserDataModel> OnUserLoggedIn;
    public Action<string> OnUserLoggedInError;
    
    private void Awake() {
        Instance = this;
    }

    public void TryRegisterUser(string email, string password, string role) {
        var user = new UserDataModel() {email = email, password = password, role = role };
        string jsonData = JsonUtility.ToJson(user);

        StartCoroutine(TryRegisterUserCoroutine(jsonData));
    }

    private IEnumerator TryRegisterUserCoroutine(string jsonData) {
        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl + "/signup", jsonData, "application/json")) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                OnUserRegisterError?.Invoke(www.error);
            }
            else if (www.downloadHandler.text == "Email already exists") {
                OnUserRegisterError?.Invoke("Email already in use");
            }
            else {
                OnUserRegistered?.Invoke();
            }
        }
    }

    public void TryLoginUser(string email, string password) {
        var user = new UserDataModel() {email = email, password = password};
        string jsonData = JsonUtility.ToJson(user);

        StartCoroutine(TryLoginUserCoroutine(jsonData));
    }

    private IEnumerator TryLoginUserCoroutine(string jsonData) {
        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl + "/signin", jsonData, "application/json")) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                OnUserLoggedInError?.Invoke(www.error);
            }
            else if (www.downloadHandler.text == "Invalid credentials") {
                OnUserLoggedInError?.Invoke("Invalid credentials");
            }
            else {
                var loadedUser = JsonUtility.FromJson<UserDataModel>(www.downloadHandler.text);
                
                OnUserLoggedIn?.Invoke(loadedUser);
            }
        }
    }
}
