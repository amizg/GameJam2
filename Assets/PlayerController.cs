using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public CharacterController2D controller;
    //public DeathMenu deathMenuUI;

    public float runSpeed;
    public float jumpForce;
    public Transform isGroundedChecker;
    public Collider2D isOnWallChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float rememberGroundedFor;
    float lastTimeGrounded;
    bool isGrounded = false;
    bool isBlocking = false;
    public bool wallSlide = false;
    public bool isAlive = true;

    private float move;
    private float yVelocity;

    private Rigidbody2D rb;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get directional input
        move = Input.GetAxis("Horizontal") * runSpeed * Time.fixedDeltaTime;
        yVelocity = rb.velocity.y;

        animator.SetFloat("AirSpeedY", yVelocity);

        if (Mathf.Abs(move) >= 0.1 && isGrounded ) animator.SetInteger("AnimState", 1);
        if (Mathf.Abs(move) < 0.1) animator.SetInteger("AnimState", 0);

        if (isGrounded) {
            animator.SetBool("Grounded", true);
        }
        else if (!isGrounded) {
            animator.SetBool("Grounded", false);
        }

        CheckIfGrounded();
        CheckIfOnWall();

        if (!isBlocking) {
            Move(move);
            Jump();
            BetterJump();
        }
        Block();
    }

    void FixedUpdate()
    {

    }

    void Move(float move)
    {
        //Move Player
        controller.Move(move, false, false);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor)) {
            animator.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void CheckIfGrounded()
    {
        Collider2D colliders = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (colliders != null) {
            isGrounded = true;
        }
        else {
            if (isGrounded) {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }
    }

    void CheckIfOnWall()
    {
        
    }

    void BetterJump()
    {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;

        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    void Block()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && isGrounded) {
            animator.SetTrigger("Block");
        }
        else if (Input.GetKey(KeyCode.Mouse1) && isGrounded) {
            animator.SetBool("IdleBlock", true);
            isBlocking = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && isGrounded) {
            animator.SetBool("IdleBlock", false);
            isBlocking = false;
        }
    }
}
