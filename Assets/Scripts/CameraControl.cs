using UnityEngine;

/// <summary>
/// Controls the camera movement and rotation based on player input.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5.0f; // Speed of camera movement based on keyboard input.
    public float verticalSpeed = 5.0f; // Speed of camera vertical movement (up/down).

    [Header("Rotation Settings")]
    public float sensitivity = 2.0f; // Sensitivity of camera rotation based on mouse movement.

    private float yaw = 0.0f; // Yaw is the rotation around the y-axis (used for left and right rotation).
    private float pitch = 0.0f; // Pitch is the rotation around the x-axis (used for up and down rotation).

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    /// <summary>
    /// Handles the camera's movement based on AWSD keys and vertical movement with Space and Shift.
    /// </summary>
    private void HandleMovement()
    {
        // Calculate movement based on horizontal (A/D) and vertical (W/S) input.
        float xMovement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float zMovement = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        // Apply movement.
        transform.Translate(xMovement, 0, zMovement, Space.Self);

        // Handle vertical movement: up with Space, down with Shift.
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            transform.Translate(Vector3.down * verticalSpeed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Handles the camera's rotation based on mouse movement.
    /// </summary>
    private void HandleRotation()
    {
        // Adjust yaw and pitch based on mouse input.
        yaw += sensitivity * Input.GetAxis("Mouse X");
        pitch -= sensitivity * Input.GetAxis("Mouse Y");

        // Clamp the pitch to prevent flipping the camera over the top.
        pitch = Mathf.Clamp(pitch, -89f, 89f);

        // Apply rotation.
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
