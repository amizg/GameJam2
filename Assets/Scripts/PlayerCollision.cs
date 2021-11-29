
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerController controller;
    public Rigidbody2D rb;

    private AudioManager sounds;

    private void Awake()
    {
        sounds = FindObjectOfType<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Checkpoint") {
            controller.respawnPoint = other.transform.position;
        }
        else if (other.tag == "Spikes") {
            sounds.Play("SpikeDeath");
            controller.Die();
        }
        else if (other.tag == "BossArea") {
            sounds.Stop("MainTheme");
            sounds.Play("BossAreaEnter");
            sounds.Play("BossTheme");
        }
        else if (other.tag == "ParkourArea") {
            sounds.Stop("MainTheme");
            sounds.Play("ParkourTheme");
        }
        else if (other.tag == "MainTheme") {
            sounds.Stop("ParkourTheme");
            sounds.Play("MainTheme");
        }
    }
}
