using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnlineMiddleWall : MonoBehaviour {
    public void SetWallHeight(int height) {
        transform.localScale = new Vector3(1f, height, 1f);
    }
}