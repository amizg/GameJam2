
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerController controller;
    public Rigidbody2D rb;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Checkpoint") {
            controller.respawnPoint = other.transform.position;
        }
    }
}
