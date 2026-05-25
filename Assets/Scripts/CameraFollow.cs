using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // Drag your Player here
    public Vector3 offset = new Vector3(0f, 6f, -9f); // Position relative to player
    public float smoothSpeed = 5f; // How smoothly it glides (higher is faster)

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate where the camera wants to go
        Vector3 desiredPosition = target.position + offset;
        
        // Smoothly slide from current position to desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // Apply the position
        transform.position = smoothedPosition;

        // Keep looking at the player
        transform.LookAt(target.position + Vector3.up * 1f);
    }
}
