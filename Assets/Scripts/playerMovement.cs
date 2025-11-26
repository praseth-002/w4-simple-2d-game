using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    float horizontalMovement;

    public float jumpPower = 5.5f;
    public int maxJumps = 2;
    int remainingJumps;

    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;

    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMult = 2f;

    bool isFacingRight = true;

    public float wallSlideSpeed = 2f;
    bool isWallSliding;

    public float wallJumpForceX = 6f;
    public float wallJumpForceY = 8f;

    private float wallSlideDelay = 0.05f;
    private float wallSlideTimer = 0f;

    void Update()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        GroundCheck();
        Gravity();
        Flip();
        WallSlide();
    }

    // Movement input
    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    // Jump input
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Wall Jump
            if (isWallSliding && !isGrounded)
            {
                remainingJumps = maxJumps; // reset jumps

                float jumpDir = isFacingRight ? -1 : 1; // push opposite of wall
                rb.velocity = new Vector2(jumpDir * wallJumpForceX, wallJumpForceY);

                // Flip if needed
                if ((isFacingRight && jumpDir == -1) || (!isFacingRight && jumpDir == 1))
                {
                    FlipDirection();
                }

                isWallSliding = false;
                wallSlideTimer = 0;
            }
            // Normal Jump
            else if (remainingJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                remainingJumps--;
            }
        }
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            remainingJumps = maxJumps;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }

    private void Gravity()
    {
        if (isWallSliding)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMult;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            FlipDirection();
        }
    }

    private void FlipDirection()
    {
        isFacingRight = !isFacingRight;
        Vector3 ls = transform.localScale;
        ls.x *= -1f;
        transform.localScale = ls;
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    private void WallSlide()
    {
        if (!isGrounded && WallCheck() && rb.velocity.y < -0.1f)
        {
            wallSlideTimer += Time.deltaTime;

            if (wallSlideTimer >= wallSlideDelay)
            {
                isWallSliding = true;
                rb.gravityScale = 0;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
            }
        }
        else
        {
            isWallSliding = false;
            wallSlideTimer = 0f;
        }
    }
}
