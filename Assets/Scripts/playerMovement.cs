using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private float horizontal; 
    private float speed = 8f;
    private float jumpHeight = 16f;
    public bool isFacingRight = true;

    public int doubleJump;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private bool canDash = true;
    public bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = .25f;
    public float dashingCooldown = 1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    void Start()
    {
        References.thePlayer = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }


        horizontal = Input.GetAxisRaw("Horizontal");

        if(IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = 0;
        }

        if(IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
            if ((jumpBufferCounter > 0f && coyoteTimeCounter > 0f) || doubleJump < 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                if(!IsGrounded() && coyoteTimeCounter <= 0f)
                {
                    doubleJump += 1;
                }

                jumpBufferCounter = 0f;
            } 
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }


        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
