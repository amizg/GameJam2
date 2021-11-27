using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    private SkeletonBehavior enemyParent;

    private void Awake()
    {
        enemyParent = GetComponentInParent<SkeletonBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            gameObject.SetActive(false);
            enemyParent.target = other.transform;
            enemyParent.inRange = true;
            enemyParent.combatZone.SetActive(true);
        }
    }
}
