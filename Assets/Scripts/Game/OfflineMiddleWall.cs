using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineMiddleWall : MonoBehaviour {
    private void Start() {
        
    }

    public void SetWallHeight(int wallHeight) {
        transform.localScale = new Vector3(transform.localScale.x, wallHeight, transform.localScale.z);
    }
}