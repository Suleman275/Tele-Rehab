using MiniUI;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PastSessionsPage : MiniPage {
    List<SessionDataModel> pastSessionsList;

    protected override void RenderPage() { //pls refactor this to use events
        APIManager.Instance.TryGetPastSessions((data) => {
            pastSessionsList = JsonConvert.DeserializeObject<List<SessionDataModel>>(data);
            
            foreach (SessionDataModel pastSession in pastSessionsList) {
                CreateAndAddElement<Label>().text = pastSession._id;
                CreateAndAddElement<Label>().text = pastSession.patientName;
                var btn = CreateAndAddElement<Button>();
                btn.text = "Replay this session";
                btn.clicked += () => {
                    APIManager.Instance.TryGetPastSession(pastSession._id, (jsonData) => {
                        DataRecorder.Instance.ImportDataFromJson(jsonData);
                        enabled = false;
                        ReplayController.Instance.StartReplay();
                    });
                };
            }
        });
    }
}