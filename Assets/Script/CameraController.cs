using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target; // Player's transform
    [SerializeField] private float smoothSpeed = 5f; // Adjust this to control the smoothness of the camera movement

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position for the camera
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            // Smoothly move the camera towards the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
