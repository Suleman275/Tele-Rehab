using MiniUI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class LoginPage : MiniPage {
    private Label errorText;
    private TextField emailTf;
    private TextField passwordTf;
    private Button loginBtn;
    private Button registerBtn;
    private MiniComponentRouter router;
    
    protected override void RenderPage() {
        SetupEvents();
        
        router = GetComponent<MiniComponentRouter>();

        errorText = CreateAndAddElement<Label>();
            
        CreateAndAddElement<Label>().text = "Enter Email";
        emailTf = CreateAndAddElement<TextField>();
        
        CreateAndAddElement<Label>().text = "Enter Password";
        passwordTf = CreateAndAddElement<TextField>();
        passwordTf.isPasswordField = true;

        loginBtn = CreateAndAddElement<Button>();
        loginBtn.text = "Login";
        loginBtn.clicked += () => {
            errorText.text = "";
            APIManager.Instance.TrySignIn(emailTf.value, passwordTf.value);
        };
        
        registerBtn = CreateAndAddElement<Button>();
        registerBtn.text = "Register";
        registerBtn.clicked += () => {
            router.Navigate(this, "RegisterPage");
        };
    }

    private void SetupEvents() {
        APIManager.Instance.UserSignedIn += model => {
            if (model.role == "Patient") {
                router.Navigate(this, "TestPatDash");    
            }
            else {
                router.Navigate(this, "TestDocDash");
            }
            
        };

        APIManager.Instance.UserSignInFailed += errorMsg => errorText.text = errorMsg;
    }
}
