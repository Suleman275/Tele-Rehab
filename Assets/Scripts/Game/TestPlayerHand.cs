using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerHand : MonoBehaviour {
    public float moveSpeed = 5f; // Adjust the movement speed
    public float maxY = 5f; // Maximum height
    public float minY = 1f; // Minimum height

    private void Update() {
        // Get arrow key input
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate new position
        Vector3 newPosition = transform.position + Vector3.up * verticalInput * moveSpeed * Time.deltaTime;

        // Clamp the position within the specified range
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Apply the new position using Translate
        transform.Translate(Vector3.up * verticalInput * moveSpeed * Time.deltaTime);

        // Alternatively, you can use:
        // transform.position = newPosition;
    }
}