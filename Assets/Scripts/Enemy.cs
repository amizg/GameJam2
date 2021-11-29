using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator animator;

    public int maxHealth;
    private int currHealth;
    private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        isAttacking = checkAttack();
    }

    public void TakeDamage(int damage)
    {

        if (!isAttacking) {
            currHealth -= damage;
            animator.SetTrigger("Hit");
            return;
        }
        else if (isAttacking) {
            animator.ResetTrigger("Hit");
            return;
        }

        if (currHealth <= 0) {
            Die();
        }
    }

    void Die()
    {
        //Die animation and disable
        animator.SetTrigger("Die");

        Destroy(gameObject, 1f);
        this.enabled = false;
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
