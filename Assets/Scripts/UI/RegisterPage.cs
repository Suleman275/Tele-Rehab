using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class RegisterPage : MiniPage {
    [SerializeField] private StyleSheet _styles;

    private Label errorText;
    private TextField emailTF;
    private TextField passwordTF;
    private TextField passwordTF2;
    private DropdownField roleDropdown;
    private Button registerBtn;
    private Button loginBtn;

    private MiniComponentRouter router;
    protected override void RenderPage() {
        SetupEvents();
        
        router = GetComponent<MiniComponentRouter>();
        
        AddStyleSheet(_styles);
        
        var div = CreateAndAddElement<MiniElement>("main");

        errorText = div.CreateAndAddElement<Label>("errorText");

        div.CreateAndAddElement<Label>("white").text = "Enter Your email";
        emailTF = div.CreateAndAddElement<TextField>();
        
        div.CreateAndAddElement<Label>("white").text = "Enter Your Password";
        passwordTF = div.CreateAndAddElement<TextField>();
        passwordTF.isPasswordField = true;
        
        div.CreateAndAddElement<Label>("white").text = "Re-Enter Your Password";
        passwordTF2 = div.CreateAndAddElement<TextField>();
        passwordTF2.isPasswordField = true;
        
        div.CreateAndAddElement<Label>("white").text = "Enter Your Role";
        roleDropdown = div.CreateAndAddElement<DropdownField>();
        roleDropdown.choices.Add("Doctor");
        roleDropdown.choices.Add("Patient");

        registerBtn = div.CreateAndAddElement<Button>("btn");
        registerBtn.text = "Register";
        registerBtn.clicked += () => {
            errorText.text = "";
            
            if (passwordTF.value != passwordTF2.value) {
                errorText.text = "Passwords Must Match";
                return;
            }

            if (emailTF.value == null) {
                errorText.text = "Email should not be left blank";
                return;
            }

            if (roleDropdown.value == null) {
                errorText.text = "You must select a role";
                return;
            }
            
            APIManager.Instance.TryRegisterUser(emailTF.value, passwordTF.value, roleDropdown.value);
        };

        loginBtn = div.CreateAndAddElement<Button>("link");
        loginBtn.text = "Already have an account? Login one here";
        loginBtn.clicked += () => {
            router.Navigate(this, "LoginPage");
        };
    }

    private void SetupEvents() {
        APIManager.Instance.OnUserRegisterError += s => errorText.text = s;
        APIManager.Instance.OnUserRegistered += async () => {
            UserDataManager.Instance.email = emailTF.value;
            UserDataManager.Instance.role = roleDropdown.value;

            await UnityServicesManager.Instance.SignInAnonymously();
            
            //redirect to dashboard
            if (UserDataManager.Instance.role == "Patient") {
                router.Navigate(this, "PatientDashboard");
            }
            else {
                router.Navigate(this, "DoctorDashboard");
            }
        };
    }
}
