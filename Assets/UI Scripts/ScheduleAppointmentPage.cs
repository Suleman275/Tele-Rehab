using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniUI;
using UnityEngine.UIElements;

public class ScheduleAppointmentPage : MiniPage {
    protected override void RenderPage() {

        APIManager.Instance.AppointmentCreated += () => {
            GetComponent<MiniComponentRouter>().Navigate(this, "PatientDashboard");
        };
        
        var nameTF = CreateAndAddElement<TextField>();
        var timeTF = CreateAndAddElement<TextField>();

        var btn = CreateAndAddElement<Button>();
        btn.text = "Schedule Meeting";
        btn.clicked += () => {
            APIManager.Instance.TryCreateAppointment(nameTF.value, timeTF.value);
        };
    }
}