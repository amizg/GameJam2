using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBuff : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null) {
            controller.CollectBuff("jump");
            Destroy(gameObject);
        }
    }
}
