using MiniUI;
using UnityEngine.UIElements;

public class WelcomePage : MiniPage {
    protected override void RenderPage() {
        var router = GetComponent<MiniComponentRouter>();

        var btn = CreateAndAddElement<Button>();
        btn.text = "Get Started";
        btn.clicked += () => {
            router.Navigate(this, "LoginPage");
        };
    }
}