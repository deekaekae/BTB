using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;  // The target the camera follows
    public float distance = 10f;  // Distance from the target
    public float height = 10f;  // Height above the target
    public float angle = 45f;  // Angle of the camera
    public float followSpeed = 10f;  // Speed at which the camera follows the target

    void Start()
    {
        // Set the camera to orthographic
        Camera.main.orthographic = true;
        
        // Set the initial position and rotation
        UpdateCameraPosition(true);
    }

    void LateUpdate()
    {
        // Update the camera position every frame
        UpdateCameraPosition();
    }

    void UpdateCameraPosition(bool instant = false)
    {
        // Calculate the new position
        Vector3 newPosition = target.position;
        newPosition -= Vector3.forward * distance;
        newPosition += Vector3.up * height;

        if (instant)
        {
            transform.position = newPosition;
        }
        else
        {
            // Smoothly interpolate to the new position
            transform.position = Vector3.Lerp(transform.position, newPosition, followSpeed * Time.deltaTime);
        }

        // Rotate the camera to look at the target
        transform.rotation = Quaternion.Euler(angle, 45f, 0f);
    }
}