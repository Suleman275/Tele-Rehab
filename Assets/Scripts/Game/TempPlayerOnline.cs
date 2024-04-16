using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class TempPlayerOnline : NetworkBehaviour {
    public static TempPlayerOnline LocalInstance;

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            LocalInstance = this;
        }
        else {
            Destroy(this);
        }
    }
}