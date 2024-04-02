using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking; // Import the necessary namespace

public class APIManager : MonoBehaviour {
    public static APIManager Instance;
    private string baseUrl = "http://localhost:3000"; // Your Express API URL
    
    //Events
    public Action<UserDataModel> UserSignedIn;
    public Action<String> UserSignInFailed;

    private void Awake() {
        Instance = this;
    }

    public void TrySignUp(string email, string password, string role) {
        var jsonBody = new UserDataModel();
        jsonBody.email = email;
        jsonBody.password = password;
        jsonBody.role = role;
        
        string json = JsonUtility.ToJson(jsonBody);
        
        StartCoroutine(SendSignUpRequest(json));
    }

    private IEnumerator SendSignUpRequest(string json) {
        string url = $"{baseUrl}/signup";

        using (UnityWebRequest request = UnityWebRequest.Post(url, json, "application/json")) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success) {
                Debug.Log("User created successfully");
            }
            else {
                Debug.LogError("Error creating user: " + request.error);
            }
        }
    }
    
    public void TrySignIn(string email, string password) {
        var jsonBody = new UserDataModel();
        jsonBody.email = email;
        jsonBody.password = password;
    
        // Convert the JSON object to a string
        string json = JsonUtility.ToJson(jsonBody);

        StartCoroutine(SendSignInRequest(json));
    }

    private IEnumerator SendSignInRequest(string json) {
        string url = $"{baseUrl}/signin";

        using (UnityWebRequest request = UnityWebRequest.Post(url, json, "application/json")) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success) {
                string responseJson = request.downloadHandler.text;

                if (responseJson == "Invalid Credentials") {
                    //Debug.LogError("Invalid Credentials");
                    UserSignInFailed?.Invoke("Invalid Credentials");
                }
                else {
                    var userData = JsonUtility.FromJson<UserDataModel>(responseJson);
                    UserSignedIn?.Invoke(userData);
                }
            }
            else {
                //Debug.LogError("Error signing in: " + request.error);
                UserSignInFailed?.Invoke(request.error);
            }
        }
    }
}