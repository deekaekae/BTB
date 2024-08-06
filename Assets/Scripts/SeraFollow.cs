// SeraFollow.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public float followSpeed = 5f;  // Speed at which the object follows the player
    public float followDistance = 2f;  // Distance to maintain behind the player
    public float followHeight = 1f;  // Height offset to maintain
    public float rotationSpeed = 5f;  // Speed at which Sera rotates to face the player
    public float mouseFollowSpeed = 1f;  // Speed at which Sera rotates to face the mouse

    void LateUpdate()
    {
        // Calculate the desired position directly behind the player
        Vector3 desiredPosition = player.position - player.forward * followDistance + Vector3.up * followHeight;

        // Smoothly interpolate to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Smoothly rotate to face the mouse
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        // Get the mouse position in world space
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(ray, out rayLength))
        {
            Vector3 pointToLook = ray.GetPoint(rayLength);
            Vector3 direction = (pointToLook - transform.position).normalized;

            // Rotate the Sera to face the direction of the mouse slowly
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, mouseFollowSpeed * Time.deltaTime);
        }
    }
}
