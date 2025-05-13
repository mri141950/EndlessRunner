using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Run & Jump Settings")]
    public float speed = 5f;
    public float jumpForce = 10f;

    [Header("Ground & Slide Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Slide Settings")]
    public float slideDuration = 0.7f;
    public Vector2 slideColliderSize = new Vector2(1f, 0.5f);
    public Vector2 slideColliderOffset = new Vector2(0f, -0.25f);

    [Header("Fall Detection")]
    public float fallThresholdY = -5f;

    [Header("References")]
    public GameManager gameManager;

    private Rigidbody2D rb;
    private BoxCollider2D boxCol;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    private Vector3 originalScale;

    private bool isGrounded;
    private bool jumpRequest = false;
    private bool isSliding = false;
    private float slideTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();

        // Cache original collider and scale
        originalColliderSize = boxCol.size;
        originalColliderOffset = boxCol.offset;
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // Handle jump input (only if not sliding)
        if (Input.GetButtonDown("Jump") && isGrounded && !isSliding)
        {
            jumpRequest = true;
        }

        // Handle slide input
        if (Input.GetButtonDown("Slide") && isGrounded && !isSliding)
        {
            StartSlide();
        }

        // Update slide timer
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                EndSlide();
            }
        }

        // Fall detection
        if (transform.position.y < fallThresholdY)
        {
            gameManager.GameOver();
        }
    }

    void FixedUpdate()
    {
        // Maintain horizontal run speed
        Vector2 vel = rb.linearVelocity;
        vel.x = speed;
        rb.linearVelocity = vel;

        // Apply jump impulse
        if (jumpRequest && !isSliding)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequest = false;
        }
    }

    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;

        // Shrink collider
        boxCol.size = slideColliderSize;
        boxCol.offset = slideColliderOffset;

        // Shrink sprite visually
        transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
    }

    private void EndSlide()
    {
        isSliding = false;

        // Restore collider
        boxCol.size = originalColliderSize;
        boxCol.offset = originalColliderOffset;

        // Restore sprite scale
        transform.localScale = originalScale;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameManager.GameOver();
        }
    }
}
