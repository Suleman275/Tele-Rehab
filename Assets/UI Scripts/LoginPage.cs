using UnityEngine;
using MiniUI;
using UnityEngine.UIElements;

public class LoginPage : MiniPage {
    [SerializeField] StyleSheet styles;


    private Label errorText;
    //private TextField emailTf;
    //private TextField passwordTf;
    //private Button loginBtn;
    //private Button registerBtn;
    private MiniComponentRouter router;
    
    protected override void RenderPage() {
        AddStyleSheet(styles);

        router = GetComponent<MiniComponentRouter>();

        SetupEvents();
        

        //errorText = CreateAndAddElement<Label>();
            
        //CreateAndAddElement<Label>().text = "Enter Email";
        //emailTf = CreateAndAddElement<TextField>();
        
        //CreateAndAddElement<Label>().text = "Enter Password";
        //passwordTf = CreateAndAddElement<TextField>();
        //passwordTf.isPasswordField = true;

        //loginBtn = CreateAndAddElement<Button>();
        //loginBtn.text = "Login";
        //loginBtn.clicked += () => {
        //    errorText.text = "";
        //    APIManager.Instance.TrySignIn(emailTf.value, passwordTf.value);
        //};
        
        //registerBtn = CreateAndAddElement<Button>();
        //registerBtn.text = "Register";
        //registerBtn.clicked += () => {
        //    router.Navigate(this, "RegisterPage");
        //};


        var container = CreateAndAddElement<MiniElement>("container");

        var welcomeLabel = container.CreateAndAddElement<Label>("welcome");
        welcomeLabel.text = "Welcome Back";

        errorText = container.CreateAndAddElement<Label>("errorText");

        var emailLabel = container.CreateAndAddElement<Label>();
        emailLabel.text = "Enter Your Email";

        var emailTf = container.CreateAndAddElement<TextField>();

        var passwordLabel = container.CreateAndAddElement<Label>();
        passwordLabel.text = "Enter Your Password";

        var passwordTf = container.CreateAndAddElement<TextField>();
        passwordTf.isPasswordField = true;

        var loginBtn = container.CreateAndAddElement<Button>("btn");
        loginBtn.text = "Login";
        loginBtn.clicked += () => {
            errorText.text = "";

            if (emailTf.value == "") {
                errorText.text = "Email must not be left blank";
                return;
            }
            else if (passwordTf.value == "") {
                errorText.text = "Password must not be left blank";
            }
            else {
                APIManager.Instance.TrySignIn(emailTf.value, passwordTf.value);
            }
        };

        var registerBtn = container.CreateAndAddElement<Button>("link");
        registerBtn.text = "Already have an account? Login here";
        registerBtn.clicked += () => {
            router.Navigate(this, "RegisterPage");
        };
    }

    private void SetupEvents() {
        APIManager.Instance.UserSignedIn += model => {
            //login to vivox as well
            UnityServicesManager.Instance.LoginVivox();

            if (model.role == "Patient") {
                router.Navigate(this, "PatientDashboard");
                AstraManager.Instance.StartBodyStream();
            }
            else {
                router.Navigate(this, "DoctorDashboard");
            }
        };

        APIManager.Instance.UserSignInFailed += errorMsg => errorText.text = errorMsg;
    }
}