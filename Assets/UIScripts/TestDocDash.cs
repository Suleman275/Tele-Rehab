using System.Collections;
using System.Collections.Generic;
using MiniUI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class TestDocDash : MiniPage {
    protected override void RenderPage() {
        var router = GetComponent<MiniComponentRouter>();
        
        CreateAndAddElement<Label>().text = "Testing Dashboard Doctor";
        
        var playClientBtn = CreateAndAddElement<Button>();
        playClientBtn.text = "NetCode Test Client";
        playClientBtn.clicked += () => {
            NetworkManager.Singleton.StartClient();
            router.Navigate(this, "OnlineDoctorPreGamePage");
        };
    }
}
