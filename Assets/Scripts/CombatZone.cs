using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    private SkeletonBehavior enemyParent;
    private bool inRange;
    private Animator animator;

    private void Awake()
    {
        enemyParent = GetComponentInParent<SkeletonBehavior>();
        animator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Skeleton_Attack") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Dead")) {
            enemyParent.Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            inRange = false;
            gameObject.SetActive(false);
            enemyParent.triggerArea.SetActive(true);
            enemyParent.inRange = false;
            enemyParent.SelectTarget();
        }
    }
}
