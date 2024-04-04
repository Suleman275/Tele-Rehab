using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerOffline : MonoBehaviour  {
    public float moveSpeed = 5f; // Adjust the movement speed as needed

    private void Update()  {
        // Get input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement vector
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime;

        // Apply movement using transform.Translate
        transform.Translate(movement);
    }
}