using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public CharacterController2D controller;
    public float runSpeed;
    public float jumpForce;
    public float dodgeForce;
    private int damageMul = 1;

    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float rememberGroundedFor;
    float lastTimeGrounded;

    bool isGrounded = false;
    bool isBlocking = false;
    bool isRolling = false;
    public bool isAlive = true;

    private float move;
    private float yVelocity;
    private int attackCounter = 0;
    private float attackTime = 0.0f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int maxHealth;
    private int currHealth;

    private Rigidbody2D rb;
    public Animator animator;

    public Vector3 respawnPoint;

    private bool invul = false;

    private AudioManager sounds;

    public HealthBar healthBar;
    public GameObject bossHealth;

    // Start is called before the first frame update
    void Start()
    {
        sounds = FindObjectOfType<AudioManager>();

        respawnPoint = transform.position;

        currHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("AttackTimer", 0.1f, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        //Get directional input
        move = Input.GetAxisRaw("Horizontal") * runSpeed * Time.fixedDeltaTime;
        yVelocity = rb.velocity.y;

        animator.SetFloat("AirSpeedY", yVelocity);

        if (Mathf.Abs(move) >= 0.1 && isGrounded) animator.SetInteger("AnimState", 1);
        if (Mathf.Abs(move) < 0.1) animator.SetInteger("AnimState", 0);

        if (isGrounded) {
            animator.SetBool("Grounded", true);
        }
        else if (!isGrounded) {
            animator.SetBool("Grounded", false);
        }

        CheckIfGrounded();

        if (isGrounded) {
            Block();
        }
        if (!isBlocking) {
            Move(move);
            Jump();
            BetterJump();


            if (!isRolling) {
                Roll();
                Attack();
            }
        }

        if (isBlocking) {
            Move(0);
        }

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
            sounds.Play("PlayerJump");

            CameraShake.Instance.ShakeCamera(1, .1f);

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
            invul = true;
        }
        if (isRolling) Invoke("resetRoll", 1f);
    }

    void resetRoll()
    {
        isRolling = false;
        invul = false;
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {

            if (attackCounter == 0) { 
                animator.SetTrigger("Attack1");
                attackCounter++;
                attackTime = 0;

                sounds.Play("PlayerAttack1");
                CameraShake.Instance.ShakeCamera(1, .1f);
            }
            else if (attackCounter == 1 && attackTime >= 1) {
                animator.SetTrigger("Attack2");
                attackCounter++;

                sounds.Play("PlayerAttack2");
                CameraShake.Instance.ShakeCamera(1, .1f);
            }
            else if (attackCounter == 2 && attackTime >= 2) {
                animator.SetTrigger("Attack3");
                attackCounter++;

                sounds.Play("PlayerAttack3");
                CameraShake.Instance.ShakeCamera(2, .1f);
            }
            else if (attackCounter == 3 && attackTime >= 3) {
                attackCounter++;
            }

            if (attackCounter == 4) {
                attackCounter = 0;
            }
        }
    }

    public void DealDamage(int damage)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies) {
    
            enemy.GetComponent<Enemy>().TakeDamage(damage * damageMul);
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
            isBlocking = true;
            invul = true;
        }
        else if (Input.GetKey(KeyCode.Mouse1) && isGrounded) {
            animator.SetBool("IdleBlock", true);
            isBlocking = true;
            invul = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && isGrounded) {
            animator.SetBool("IdleBlock", false);
            isBlocking = false;
            invul = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!invul) {
            CameraShake.Instance.ShakeCamera(damage * 2, .2f);
            sounds.Play("PlayerHit");
            currHealth -= damage;
            animator.SetTrigger("Hurt");

            healthBar.SetHealth(currHealth);
        }
        else if (invul && isBlocking) {
            sounds.Play("PlayerBlock");
            CameraShake.Instance.ShakeCamera(damage, .2f);
        }

        if (currHealth <= 0 && !invul) {
            Die();
            invul = true;
        }
    }

    public void Die()
    {
        sounds.Stop("MainTheme");
        sounds.Play("PlayerDie");

        CameraShake.Instance.ShakeCamera(5f, 1f);
        animator.SetTrigger("Death");
        Invoke("Respawn", 2f);
        
        rb.velocity = new Vector2 (0, 0);
        this.enabled = false;

        bossHealth.SetActive(false);
    }

    public void CollectBuff(string buff)
    {
        sounds.Play("BuffAcquired");

        if(buff == "damage") {
            FindObjectOfType<BuffMenu>().DamageBuff();
            damageMul = 2;
        }
        else if (buff == "speed") {
            FindObjectOfType<BuffMenu>().SpeedBuff();
            runSpeed += 10;
        }
        else if (buff == "jump") {
            FindObjectOfType<BuffMenu>().JumpBuff();
            jumpForce += 3;
        }
        else if(buff == "health") {
            FindObjectOfType<BuffMenu>().HealthBuff();
            maxHealth += 30;
            currHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetMaxHealth(currHealth);
        }
    }

    public void Heal(int amount)
    {
        sounds.Play("Health");

        if (currHealth < maxHealth) {
            currHealth += amount;

            if (currHealth > maxHealth) {
                currHealth = maxHealth;
            }

            healthBar.SetHealth(currHealth);
        }
    }

    void Respawn()
    {
        invul = false;
        this.enabled = true;
        sounds.Stop("ParkourTheme");
        sounds.Stop("BossTheme");
        sounds.Play("MainTheme");
        animator.SetTrigger("Respawn");

        transform.position = respawnPoint;
        currHealth = maxHealth;
        healthBar.SetHealth(currHealth);
    }

    public void PlayRollSound()
    {
        sounds.Play("PlayerRoll");
    }
}
