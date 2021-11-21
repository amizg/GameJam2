using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public CharacterController2D controller;
    //public DeathMenu deathMenuUI;

    public float runSpeed;
    public float jumpForce;
    public float dodgeForce;
    public Transform isGroundedChecker;
    //public Transform isOnWallChecker;
    public float checkGroundRadius;
    //public float checkWallRadius;
    public LayerMask groundLayer;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float rememberGroundedFor;
    float lastTimeGrounded;
    bool isGrounded = false;
    bool isBlocking = false;
    bool isRolling = false;
    //bool isOnWall = false;
    public bool isAlive = true;

    private float move;
    private float yVelocity;
    private int attackCounter = 0;
    private float attackTime = 0.0f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    private Rigidbody2D rb;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("AttackTimer", 0.1f, 0.4f);
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

        if (!isBlocking) {
            Move(move);
            Jump();
            BetterJump();


            if (!isRolling) {
                Roll();
                Attack();
            }
        }
        Block();



        //CheckIfOnWall();

        //if (!isOnWall) {
        //    animator.SetBool("WallSlide", false);
        //}

        //if (isOnWall && !isGrounded) {
        //    animator.SetInteger("AnimState", 0);
        //    animator.SetBool("WallSlide", true);
        //    animator.SetBool("Grounded", false);
        //    Debug.Log("WallSlide");
        //}
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

    void Roll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Mathf.Abs(move) > 0 && isGrounded) {
            if (move > 0) rb.AddForce(new Vector2(dodgeForce, 0));
            if (move < 0) rb.AddForce(new Vector2(-dodgeForce, 0));
            animator.SetTrigger("Roll");
            isRolling = true;

        }
        if (isRolling) Invoke("resetRoll", 1.0f);
    }

    void resetRoll()
    {
        isRolling = false;
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {

            if (attackCounter == 0) {
                animator.SetTrigger("Attack1");
                attackCounter++;
                attackTime = 0;
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

                foreach (Collider2D enemy in hitEnemies) {
                    enemy.GetComponent<Enemy>().TakeDamage(1);
                }
            }
            else if (attackCounter == 1 && attackTime >= 1) {
                animator.SetTrigger("Attack2");
                attackCounter++;
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

                foreach (Collider2D enemy in hitEnemies) {
                    enemy.GetComponent<Enemy>().TakeDamage(2);
                }
            }
            else if (attackCounter == 2 && attackTime >= 2) {
                animator.SetTrigger("Attack3");
                attackCounter++;
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

                foreach (Collider2D enemy in hitEnemies) {
                    enemy.GetComponent<Enemy>().TakeDamage(4);
                }
            }
            else if (attackCounter == 3 && attackTime >= 3) {
                attackCounter++;
            }

            if (attackCounter == 4) {
                attackCounter = 0;
            }
        }
    }

    void AttackTimer()
    {
        attackTime++;

        if (attackTime == 5) {
            attackTime = 0;
            attackCounter = 0;
        }
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

    //void CheckIfOnWall()
    //{
    //    Collider2D colliders = Physics2D.OverlapCircle(isOnWallChecker.position, checkWallRadius, groundLayer);
    //    if (colliders != null) {
    //        isOnWall = true;
    //    }
    //    else {
    //        isOnWall = false;
    //    }
    //}
}