using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnlineMiddleWall : NetworkBehaviour {
    // public static OnlineMiddleWall Instance;
    //
    // private void Awake() {
    //     Instance = this;
    // }

    public void SetWallHeight(int height) {
        transform.localScale = new Vector3(1f, height, 1f);
    }
}