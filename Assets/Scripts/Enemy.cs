using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator animator;

    private AudioManager sounds;

    public int maxHealth;
    private int currHealth;
    private bool isAttacking;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        sounds = FindObjectOfType<AudioManager>();
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        isAttacking = checkAttack();
    }

    public void TakeDamage(int damage)
    {
        if (currHealth <= 0 && isAlive) {
            isAlive = false;
            Die();
            return;
        }

        if (!isAttacking && isAlive) {
            sounds.Play("EnemyHit");
            currHealth -= damage;
            animator.SetTrigger("Hit");
        }
        else if (isAttacking && isAlive) {
            animator.ResetTrigger("Hit");
        }
    }

    void Die()
    {
        this.enabled = false;
        
        animator.SetTrigger("Die");
        sounds.Play("EnemyKilled");
        
        Destroy(gameObject, 1.5f);
    }

    bool checkAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            return true;
        }
        else
            return false;
    }
}
