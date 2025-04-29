using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Run Settings")]
    public float speed = 5f;                // Horizontal run speed
    public float jumpForce = 10f;           // Upward velocity for jump

    [Header("Ground Check")]
    public Transform groundCheck;           // Empty child used to detect ground
    public float groundCheckRadius = 0.1f;  // Radius for ground overlap check
    public LayerMask groundLayer;           // LayerMask for ground objects

    [Header("Fall Detection")]
    public float fallThresholdY = -5f;      // Y below this triggers game over

    [Header("References")]
    public GameManager gameManager;         // Drag your GameManager here in Inspector

    private Rigidbody2D rb;
    private bool isGrounded;

    void Awake()
    {
        // Cache Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Maintain constant horizontal velocity
        Vector2 velocity = rb.linearVelocity;
        velocity.x = speed;
        rb.linearVelocity = velocity;

        // Check if we're touching ground
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // Jump when pressing Space and if grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity = rb.linearVelocity;
            velocity.y = jumpForce;
            rb.linearVelocity = velocity;
        }

        // Fall detection: if below threshold, trigger Game Over
        if (transform.position.y < fallThresholdY)
        {
            gameManager.GameOver();
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize ground check radius in Editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Trigger Game Over on obstacle collision
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameManager.GameOver();
        }
    }
}
