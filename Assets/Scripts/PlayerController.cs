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

        originalColliderSize = boxCol.size;
        originalColliderOffset = boxCol.offset;
        originalScale = transform.localScale;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (InputManager.JumpPressed && isGrounded && !isSliding)
            jumpRequest = true;

        if (InputManager.SlidePressed && isGrounded && !isSliding)
            StartSlide();

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
                EndSlide();
        }

        if (transform.position.y < fallThresholdY)
            GameManager.Instance.TriggerGameOver();
    }

    void FixedUpdate()
    {
        Vector2 vel = rb.linearVelocity;
        vel.x = speed;
        rb.linearVelocity = vel;

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
        boxCol.size = slideColliderSize;
        boxCol.offset = slideColliderOffset;
        transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
    }

    private void EndSlide()
    {
        isSliding = false;
        boxCol.size = originalColliderSize;
        boxCol.offset = originalColliderOffset;
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
            GameManager.Instance.TriggerGameOver();
        }
    }
}
