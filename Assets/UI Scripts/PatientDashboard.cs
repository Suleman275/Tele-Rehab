using MiniUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PatientDashboard : MiniPage {
    [SerializeField] StyleSheet styles;
    protected override void RenderPage() {
        AddStyleSheet(styles);

        var router = GetComponent<MiniComponentRouter>();

        var navbar = CreateAndAddElement<MiniElement>("navbar");

        var usernameLabel = navbar.CreateAndAddElement<Label>();
        usernameLabel.text = UserDataManager.Instance.userEmail; //using email for now

        var upcomingSessionsLabel = CreateAndAddElement<Label>("big");
        upcomingSessionsLabel.text = "Upcoming Sessions";

        var sessionsgroup = CreateAndAddElement<MiniElement>("vgroup");

        var sessionCard1 = sessionsgroup.CreateAndAddElement<SessionCard>("sessionCard");
        var sessionCard2 = sessionsgroup.CreateAndAddElement<SessionCard>("sessionCard");
        var sessionCard3 = sessionsgroup.CreateAndAddElement<SessionCard>("sessionCard");

        var additionalActionsLabel = CreateAndAddElement<Label>("big");
        additionalActionsLabel.text = "Additional Actions";

        var btnGroup = CreateAndAddElement<MiniElement>("vgroup");

        var practiceBtn = btnGroup.CreateAndAddElement<Button>("actionBtn");
        practiceBtn.text = "Practice";
        practiceBtn.clicked += () => {
            router.Navigate(this, "OfflinePreGamePage");
        };

        var playOnlineBtn = btnGroup.CreateAndAddElement<Button>("actionBtn");
        playOnlineBtn.text = "Play with Doctor";
        playOnlineBtn.clicked += () => {
            UnityServicesManager.Instance.CreateLobby();
            router.Navigate(this, "OnlinePatientPreGamePage");
        };

        var scheduleMeetingBtn = btnGroup.CreateAndAddElement<Button>("actionBtn");
        scheduleMeetingBtn.text = "Schedule A Meeting";
        scheduleMeetingBtn.clicked += () => {
            print("to be implemented");
        };
    }
}

class SessionCard : MiniElement {
    Label dateLabel;
    Label typeLabel;
    Label doctorLabel;

    public SessionCard() {
        dateLabel = CreateAndAddElement<Label>();
        dateLabel.text = "12th March";

        typeLabel = CreateAndAddElement<Label>();
        typeLabel.text = "Left Arm";

        doctorLabel = CreateAndAddElement<Label>();
        doctorLabel.text = "testdoc";
    }
}