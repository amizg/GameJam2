using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpell : MonoBehaviour
{
    public PlayerController player;
    
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Destroy(gameObject, 1f);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            player.TakeDamage(damage);
        }
    }

}
