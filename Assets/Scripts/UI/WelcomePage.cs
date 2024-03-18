using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class WelcomePage : MiniPage {
    [SerializeField] private StyleSheet _styles;
    protected override void RenderPage() {
        AddStyleSheet(_styles);
        
        //Navigation Bar
        var navBar = CreateAndAddElement<MiniElement>("nav");

        navBar.CreateAndAddElement<Label>("title").text = "TeleRehabilitation";
        
        //Main Section
        var mainSection = CreateAndAddElement<MiniElement>("main");
       
        var btn = mainSection.CreateAndAddElement<Button>("btn");
        btn.text = "Get Started";
        btn.clicked += () => {
            GetComponent<MiniComponentRouter>().Navigate(this, "LoginPage");
        };
    }
}