using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float y;
    private float z;
    public float FollowSpeed = 2f;
    public Transform target;

    // Zoom variables
    public float zoomSpeed = 10f; // Speed of zooming in and out
    public float minZoom = 5f; // Minimum zoom value (field of view or orthographic size)
    public float maxZoom = 60f; // Maximum zoom value
    public float zoomSmoothTime = 0.2f; // Smoothing time for zooming

    private Camera cam;
    private float targetZoom; // Target zoom value
    private float zoomVelocity = 0f; // Reference velocity for SmoothDamp

    void Start()
    {
        z = transform.position.z;
        cam = GetComponent<Camera>();
        targetZoom = cam.orthographic ? cam.orthographicSize : cam.fieldOfView; // Initialize target zoom
    }

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, z);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }

    void Update()
    {
        // Handle zooming
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollInput * zoomSpeed; // Adjust the target zoom based on scroll input

        // Clamp the target zoom value within the min and max limits
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        if (cam.orthographic == false)
        {
            // Smoothly interpolate the field of view to the target zoom value
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetZoom, ref zoomVelocity, zoomSmoothTime);
        }
        else
        {
            // Smoothly interpolate the orthographic size to the target zoom value
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, zoomSmoothTime);
        }
    }
}
