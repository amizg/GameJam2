using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehavior : MonoBehaviour
{    
    public Transform rayCast;
    public LayerMask rayCastMask;
    public float rayCastLength;
    public float attackDistance; // min attack distance
    public float moveSpeed;
    public float timer; // cooldown tiemr for attacks

    private RaycastHit2D hit;
    private GameObject target;
    private Animator animator;
    private float distance; // distance from player
    private bool attacking;
    private bool inRange;
    private bool cooldown;
    private float intTimer;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayers;


    private void Awake()
    {
        intTimer = timer;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (inRange) {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, rayCastMask);
            RayCastDebugger();
        }

        if(hit.collider != null) {
            EnemyBehavior();
        }
        else if (hit.collider == null) {
            inRange = false;
        }
        
        if (inRange == false) {
            animator.SetBool("canWalk", false);
            StopAttack();
        }
    }

    void EnemyBehavior()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > attackDistance) {
            Move();
            StopAttack();
        }
        else if(attackDistance >= distance && !cooldown){
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
            target = other.gameObject;
            inRange = true;
        }
    }

    void RayCastDebugger()
    {
        if(distance > attackDistance) {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);
        }
        else if(attackDistance > distance) {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.green);
        }
    }

    void Move()
    {
        animator.SetBool("canWalk", true);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Skeleton_Attack")) {
            
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

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
            player.GetComponent<PlayerController>().TakeDamage(2);
        }
    }


}
