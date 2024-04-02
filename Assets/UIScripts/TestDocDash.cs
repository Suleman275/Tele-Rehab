using System.Collections;
using System.Collections.Generic;
using MiniUI;
using UnityEngine;
using UnityEngine.UIElements;

public class TestDocDash : MiniPage {
    protected override void RenderPage() {
        CreateAndAddElement<Label>().text = "Testing Dashboard Doctor";
    }
}
