using UnityEngine;

public class TempHandOffline : MonoBehaviour {
    public float moveSpeed = 5f;
    public bool hasBall;
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