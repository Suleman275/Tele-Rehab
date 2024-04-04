using System;
using UnityEngine;

public class OfflineBall : MonoBehaviour {
    private TempHandOffline holdingHand;

    private void Update() {
        if (holdingHand != null) {
            transform.position = holdingHand.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.TryGetComponent<TempHandOffline>(out TempHandOffline hand) && !hand.hasBall) {
            hand.hasBall = true;
            holdingHand = hand;
        }
    }
}
