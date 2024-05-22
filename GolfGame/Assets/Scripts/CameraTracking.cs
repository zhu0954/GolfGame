using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public Transform player;  // Reference to the player
    public Transform ball;    // Reference to the ball
    public GameObject arrow;  // Reference to the arrow UI element
    public Camera mainCamera; // Reference to the main camera

    public float smoothSpeed = 0.125f; // Smooth speed for camera movement
    public Vector3 offset; // Offset for the camera position
    public float minZoom = 5f;  // Minimum zoom level
    public float maxZoom = 15f; // Maximum zoom level
    public float zoomLimiter = 50f; // Zoom limiter to adjust the zoom level

    void Update()
    {
        // Check if the ball is within the camera's view
        Vector3 viewPos = mainCamera.WorldToViewportPoint(ball.position);

        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            // Ball is in view, hide the arrow
            arrow.SetActive(false);
        }
        else
        {
            // Ball is out of view, show and update the arrow
            arrow.SetActive(true);
            UpdateArrowPositionAndRotation();
        }
    }

    void FixedUpdate()
    {
        // Update the camera position to track both the player and the ball
        Vector3 midpoint = (player.position + ball.position) / 2;
        Vector3 desiredPosition = midpoint + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Adjust the camera zoom
        float distance = Vector3.Distance(player.position, ball.position);
        float zoom = Mathf.Lerp(maxZoom, minZoom, distance / zoomLimiter);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoom, Time.deltaTime);
    }

    void UpdateArrowPositionAndRotation()
    {
        // Calculate the direction from the camera to the ball
        Vector3 direction = ball.position - mainCamera.transform.position;
        direction.z = 0;

        // Set the position of the arrow at the edge of the screen
        Vector3 screenPos = mainCamera.WorldToScreenPoint(ball.position);
        screenPos.x = Mathf.Clamp(screenPos.x, 50, Screen.width - 50);
        screenPos.y = Mathf.Clamp(screenPos.y, 50, Screen.height - 50);

        arrow.transform.position = screenPos;

        // Rotate the arrow to point towards the ball
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
