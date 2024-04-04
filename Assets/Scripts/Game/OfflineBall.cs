using System;
using UnityEngine;

public class OfflineBall : MonoBehaviour {
    public bool isCompleted;
    private TempHandOffline holdingHand;

    private void Update() {
        if (holdingHand) {
            transform.position = holdingHand.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!isCompleted) {
            if (other.gameObject.TryGetComponent<TempHandOffline>(out TempHandOffline hand) && !hand.hasBall) { //if collided with hand and hand does not have a ball
                hand.hasBall = true;
                holdingHand = hand;
            }
            else if (other.gameObject.TryGetComponent<OfflineMiddleWall>(out OfflineMiddleWall wall) && holdingHand != null) { //if in hand and collides with wall
                holdingHand.hasBall = false;
                holdingHand = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        isCompleted = true;
        holdingHand.hasBall = false;
        holdingHand = null;
        
        OfflineGameManager.Instance.BallCompleted();
    }
}
