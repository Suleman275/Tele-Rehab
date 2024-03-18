using MiniUI;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class DoctorDashboard : MiniPage {
    [SerializeField] private StyleSheet styles;
    private TextField tf;
    private Button btn;
    private MiniElement container;
    private QueryResponse lobbiesQuery;
    
    protected override async void RenderPage() {
        AddStyleSheet(styles);

        lobbiesQuery = await UnityServicesManager.Instance.QueryLobbies();
        
        CreateAndAddElement<Label>("title").text = "Doctor Dashboard";
        tf = CreateAndAddElement<TextField>();
        btn = CreateAndAddElement<Button>("btn");
        btn.text = "join lobby";
        btn.clicked += async () => {
            await UnityServicesManager.Instance.JoinLobbyByCode(tf.value);
            print("joined lobby");
        };
        container = CreateAndAddElement<MiniElement>();

        foreach (var lobby in lobbiesQuery.Results) {
            container.CreateAndAddElement<Label>("white").text = lobby.Name;
            container.CreateAndAddElement<Label>("white").text = lobby.LobbyCode;
            container.CreateAndAddElement<Label>("white").text = lobby.Id;
            
            print(lobby.Name + lobby.LobbyCode + lobby.Id);
        }
    }
}