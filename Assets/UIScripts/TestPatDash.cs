using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine.UIElements;

public class TestPatDash : MiniPage {
    protected override void RenderPage() {
        CreateAndAddElement<Label>().text = "Testing Dashboard Patient";
    }
}
