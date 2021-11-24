using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator animator;

    public int maxHealth;
    private int currHealth;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Skeleton_Attack")) {
            currHealth -= damage;
            animator.SetTrigger("Hit");
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
        GetComponent<SkeletonBehavior>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }
}
