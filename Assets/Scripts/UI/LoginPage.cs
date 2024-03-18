using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class LoginPage : MiniPage {
    [SerializeField] private StyleSheet _styles;
    
    private Label errorText;
    private TextField emailTF;
    private TextField passwordTF;
    
    private MiniComponentRouter router;
    protected override void RenderPage() {
        SetupEvents();
        
        AddStyleSheet(_styles);
        
        router = GetComponent<MiniComponentRouter>();
        
        var div = CreateAndAddElement<MiniElement>("main");

        errorText = div.CreateAndAddElement<Label>("errorText");

        div.CreateAndAddElement<Label>("white").text = "Enter your email";
        emailTF = div.CreateAndAddElement<TextField>();
        
        div.CreateAndAddElement<Label>("white").text = "Enter your Password";
        passwordTF = div.CreateAndAddElement<TextField>();
        passwordTF.isPasswordField = true;

        var loginBtn = div.CreateAndAddElement<Button>("btn");
        loginBtn.text = "Login";
        loginBtn.clicked += () => {
            //login
            errorText.text = "";
            APIManager.Instance.TryLoginUser(emailTF.value, passwordTF.value);
        };

        var registerBtn = div.CreateAndAddElement<Button>("link");
        registerBtn.text = "Don't have an account? Create one here";
        registerBtn.clicked += () => {
            router.Navigate(this, "RegisterPage");
        };
    }
    
    private void SetupEvents() {
        APIManager.Instance.OnUserLoggedInError += s => errorText.text = s;
        APIManager.Instance.OnUserLoggedIn += async userData => {
            UserDataManager.Instance.email = userData.email;
            UserDataManager.Instance.role = userData.role;
            
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
