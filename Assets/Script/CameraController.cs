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

    public Camera Cam
    {
        get { return cam; }
    }

    void Start()
{
    z = transform.position.z;
    cam = GetComponent<Camera>();

    // Initialize target zoom to the camera's current zoom level (field of view or orthographic size)
    targetZoom = cam.orthographic ? cam.orthographicSize : cam.fieldOfView;

    // Clamp the target zoom to a minimum of 15
    targetZoom = Mathf.Max(targetZoom, 15f);

    // Apply the clamped target zoom to the camera
    if (cam.orthographic == false)
    {
        cam.fieldOfView = targetZoom;
    }
    else
    {
        cam.orthographicSize = targetZoom;
    }
}

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, z);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }

    void Update()
    {
        // Check if the game is paused
        if (Time.timeScale == 0)
        {
            return; // Exit the Update method if the game is paused
        }

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
