using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{

    public float speed;
    public float lineOfSight;
    public float combatRange;
    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;

    private Transform player;
    public LayerMask playerLayers;

    public float timer;
    private bool attacking;
    private bool cooldown;
    private float intTimer;


    // Start is called before the first frame update
    void Start()
    {
        intTimer = timer;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if(distanceFromPlayer < lineOfSight && distanceFromPlayer > combatRange) {
            cooldown = false;
            attacking = false;

            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
            animator.SetBool("canAttack", false);
            Flip();
        }
        else if (distanceFromPlayer <= combatRange && !cooldown) {
            timer = intTimer;
            attacking = true;

            animator.SetBool("canAttack", true);
        }
        // maybe just if?
        else if (cooldown) {
            Cooldown();
            animator.SetBool("canAttack", false);
        }

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
            player.GetComponent<PlayerController>().TakeDamage(1);
        }
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;

        if (transform.position.x > player.position.x) {
            rotation.y = 180f;
        }
        else {
            rotation.y = 0f;
        }

        transform.eulerAngles = rotation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireSphere(transform.position, combatRange);
    }
}
