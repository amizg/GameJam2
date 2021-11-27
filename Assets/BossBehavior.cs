using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour {
    public Transform leftLimit;
    public Transform rightLimit;
    public float attackDistance; // min attack distance
    
    public float castDistance; //distance for cast
    public Transform castArea;

    public float moveSpeed;
    public float timer; // cooldown tiemr for attacks
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject combatZone;
    public GameObject triggerArea;

    public int damage;

    private Animator animator;
    private float distance; // distance from player
    private bool attacking;
    private bool cooldown;
    private float intTimer;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayers;


    private void Awake()
    {
        intTimer = timer;
        animator = GetComponent<Animator>();
        SelectTarget();
    }

    void Update()
    {
        if (!attacking) {
            Move();
        }

        if (!InsideofLimits() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            SelectTarget();
        }

        if (inRange == true) {
            EnemyBehavior();
        }
    }

    void EnemyBehavior()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance) {
            StopAttack();
        }
        else if (attackDistance >= distance && !cooldown) {
            Attack();
        }

        if (cooldown) {
            Cooldown();
            animator.SetBool("Attack", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            target = other.transform;
            inRange = true;
            Flip();
        }
    }

    void Move()
    {
        animator.SetBool("canWalk", true);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {

            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

    }

    void Attack()
    {
        timer = intTimer;
        attacking = true;

        animator.SetBool("canWalk", false);
        animator.SetBool("Attack", true);
    }

    void StopAttack()
    {
        cooldown = false;
        attacking = false;

        animator.SetBool("Attack", false);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooldown && attacking) {
            cooldown = false;
            timer = intTimer;
        }

    }

    public void TriggerCooldown()
    {
        cooldown = true;
    }

    public void DealDamage()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        foreach (Collider2D player in hitPlayers) {
            player.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight) {
            target = leftLimit;
        }
        else {
            target = rightLimit;
        }

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;

        if (transform.position.x > target.position.x) {
            rotation.y = 180f;
        }
        else {
            rotation.y = 0f;
        }

        transform.eulerAngles = rotation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
