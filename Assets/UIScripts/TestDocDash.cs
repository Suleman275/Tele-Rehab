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

        var codeTf = CreateAndAddElement<TextField>();
        codeTf.value = "Join Code";
        
        var playClientBtn = CreateAndAddElement<Button>();
        playClientBtn.text = "NetCode Test Client";
        playClientBtn.clicked += () => {
            //NetworkManager.Singleton.StartClient();
            UnityServicesManager.Instance.StartClient(codeTf.value);
            router.Navigate(this, "OnlineDoctorPreGamePage");
        };
    }
}
