using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniUI;
using UnityEngine.UIElements;

public class DoctorDashboard : MiniPage {
    [SerializeField] StyleSheet styles;
    protected override void RenderPage() {
        AddStyleSheet(styles);

        var router = GetComponent<MiniComponentRouter>();

        var navbar = CreateAndAddElement<MiniElement>("navbar");

        var usernameLabel = navbar.CreateAndAddElement<Label>();
        usernameLabel.text = "Welcome " + UserDataManager.Instance.userEmail; //using email for now

        var upcomingSessionsLabel = CreateAndAddElement<Label>("big");
        upcomingSessionsLabel.text = "Upcoming Sessions";

        var sessionsgroup = CreateAndAddElement<MiniElement>("vgroup");

        var sessionCard1 = sessionsgroup.CreateAndAddElement<SessionCard>("sessionCard");
        var sessionCard2 = sessionsgroup.CreateAndAddElement<SessionCard>("sessionCard");
        var sessionCard3 = sessionsgroup.CreateAndAddElement<SessionCard>("sessionCard");

        var additionalActionsLabel = CreateAndAddElement<Label>("big");
        additionalActionsLabel.text = "Additional Actions";

        var btnGroup = CreateAndAddElement<MiniElement>("vgroup");

        var pastSessionsBtn = btnGroup.CreateAndAddElement<Button>("actionBtn");
        pastSessionsBtn.text = "View Past Sessions";
        pastSessionsBtn.clicked += () => {
            print("To be implemented");
        };

        var searchRoomsBtn = btnGroup.CreateAndAddElement<Button>("actionBtn");
        searchRoomsBtn.text = "Search for rooms";
        searchRoomsBtn.clicked += () => {
            router.Navigate(this, "AvailableRoomsPage");
        };

        var scheduleMeetingBtn = btnGroup.CreateAndAddElement<Button>("actionBtn");
        scheduleMeetingBtn.text = "Schedule A Meeting";
        scheduleMeetingBtn.clicked += () => {
            print("to be implemented");
        };
    }
}