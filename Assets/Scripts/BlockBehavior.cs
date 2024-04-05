using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the behavior of individual blocks, including scoring on significant movement
/// and visual feedback on collision.
/// </summary>
public class BlockBehavior : MonoBehaviour
{
    [Header("Block Configuration")]
    public float significantMovementThreshold = 1.5f; // Distance to consider as significant movement for scoring.

    [Header("References")]
    public GameManager gameManager; // Reference to the GameManager to update the score.

    private Renderer blockRenderer; // Renderer component for changing block color.
    private Rigidbody rb; // Rigidbody component for physics calculations.
    private Vector3 initialPosition; // The initial position of the block to measure movement.
    private bool hasScored = false; // Flag to ensure scoring occurs only once per block.

    private void Start()
    {
        blockRenderer = GetComponent<Renderer>(); // Initialize the renderer component.
        rb = GetComponent<Rigidbody>(); // Initialize the Rigidbody component.
        initialPosition = transform.position; // Store the initial position for later comparison.
    }

    void Update()
    {
        CheckForSignificantMovement(); // Check if the block has moved significantly.
    }

    /// <summary>
    /// Checks if the block has moved beyond a specified threshold from its original position
    /// and updates the score if it has not already been done.
    /// </summary>
    private void CheckForSignificantMovement()
    {
        // Check for significant movement based on the predefined threshold.
        if (!hasScored && Vector3.Distance(transform.position, initialPosition) > significantMovementThreshold)
        {
            hasScored = true; // Mark as scored to prevent duplicate scoring.
            Debug.Log($"Block {gameObject.name} is moving significantly.");
            gameManager.IncreaseScore(10); // Increase the game score.
        }
    }

    /// <summary>
    /// Responds to collision events with objects tagged as "Ball" by initiating a color change.
    /// </summary>
    /// <param name="collision">The Collision data associated with this collision event.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball")) // Check for collision with the ball.
        {
            StartCoroutine(ChangeColorTemporarily()); // Start the color change coroutine.
        }
    }

    /// <summary>
    /// Temporarily changes the block's color to red before reverting back to its original color.
    /// </summary>
    /// <returns>IEnumerator for coroutine sequencing.</returns>
    IEnumerator ChangeColorTemporarily()
    {
        Color originalColor = blockRenderer.material.color; // Store the original color.
        blockRenderer.material.color = Color.red; // Change to red to indicate collision.
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds.
        blockRenderer.material.color = originalColor; // Revert to the original color.
    }
}
