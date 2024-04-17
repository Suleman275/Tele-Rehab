using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnlineBall : NetworkBehaviour {
    private bool isCompleted;
    private TempHandOffline holdingHand;

    private void Update() {
        if (IsServer) {
            if (holdingHand) {
                transform.position = holdingHand.transform.position;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (IsServer) {
            if (!isCompleted) {
                if (other.gameObject.TryGetComponent<TempHandOffline>(out TempHandOffline hand) && !hand.hasBall) { //if collided with hand and hand does not have a ball
                    hand.hasBall = true;
                    holdingHand = hand;
                }
                else if (other.gameObject.TryGetComponent<OnlineMiddleWall>(out OnlineMiddleWall wall) && holdingHand != null) { //if in hand and collides with wall
                    holdingHand.hasBall = false;
                    holdingHand = null;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (IsServer) {
            isCompleted = true;
            holdingHand.hasBall = false;
            holdingHand = null;

            OnlineGameManager.Instance.BallCompletedServerRPC();
        }
    }
}