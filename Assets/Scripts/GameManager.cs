using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages game state, including score, ball spawning, and launch force.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Game Configuration")]
    public int score = 0; // Current game score.
    public float minForce = 50f; // Minimum force for launching the ball.
    public float maxForce = 300f; // Maximum force for launching the ball.
    public float forceChangeSpeed = 100f; // Speed at which the launch force oscillates.

    [Header("Scene References")]
    public Text scoreText; // UI Text component displaying the current score.
    public Text forceText; // UI Text component displaying the current launch force.
    public GameObject ballPrefab; // Prefab for the ball to spawn.
    public Transform cameraTransform; // Camera's Transform for spawning balls in front of it.

    private GameObject currentBall; // Currently active ball in the scene.
    private bool isPressing = false; // Whether the left mouse button is being pressed.
    private float launchForce = 0f; // Current launch force based on player input.
    private float direction = 1f; // Direction of the force change, toggles between 1 and -1.

    private void Start()
    {
        // Initialize UI elements.
        forceText.text = "Force: 0";
        UpdateScoreText();
    }

    void Update()
    {
        // Handle scene reset.
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }

        HandleBallSpawnAndLaunch();

        // Update UI.
        if (isPressing || currentBall != null) // Update the force text dynamically.
        {
            forceText.text = "Force: " + Mathf.RoundToInt(launchForce).ToString();
        }
    }

    /// <summary>
    /// Resets the current scene, effectively restarting the game.
    /// </summary>
    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        score = 0;
        UpdateScoreText();
    }

    /// <summary>
    /// Handles the spawning and launching of the ball based on mouse input.
    /// </summary>
    void HandleBallSpawnAndLaunch()
    {
        // Spawn ball on left mouse button down if there isn't already a ball.
        if (Input.GetMouseButtonDown(0) && currentBall == null)
        {
            SpawnBallAtCursor();
            isPressing = true;
            launchForce = minForce;
        }

        // Oscillate launch force while pressing.
        if (isPressing)
        {
            OscillateLaunchForce();
        }

        // Launch ball on left mouse button up.
        if (Input.GetMouseButtonUp(0) && currentBall != null)
        {
            LaunchBall();
            isPressing = false;
        }
    }

    /// <summary>
    /// Spawns a ball at a position in front of the camera based on cursor position.
    /// </summary>
    void SpawnBallAtCursor()
    {
        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 2;
        currentBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    /// <summary>
    /// Launches the currently active ball with the calculated force.
    /// </summary>
    void LaunchBall()
    {
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.useGravity = true;
        Vector3 launchDirection = cameraTransform.forward;
        rb.AddForce(launchDirection * launchForce * 0.2f, ForceMode.Impulse); // Apply scaled force.
        currentBall = null; // Allow for a new ball to be spawned.
    }

    /// <summary>
    /// Oscillates the launch force within the specified min and max range.
    /// </summary>
    void OscillateLaunchForce()
    {
        launchForce += direction * forceChangeSpeed * Time.deltaTime;
        if (launchForce > maxForce || launchForce < minForce)
        {
            direction *= -1; // Toggle direction.
            launchForce = Mathf.Clamp(launchForce, minForce, maxForce);
        }
    }

    /// <summary>
    /// Increases the game score by a specified amount.
    /// </summary>
    /// <param name="amount">The amount to increase the score by.</param>
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    /// <summary>
    /// Updates the score text UI.
    /// </summary>
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString() + " /40";
    }
}
