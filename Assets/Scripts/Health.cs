using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int startingHealth = 5;
    [ReadOnly] [SerializeField] private int health;
    [SerializeField] private string soundEffect;
    [SerializeField] private Animator animator;
    [SerializeField] private string takeDamageTrigger;
    [SerializeField] private bool respawnOnDeath = false;
    private Vector3 startingPosition;

    void Start() {
        health = startingHealth;
        startingPosition = GetComponentInParent<Rigidbody2D>().transform.position;
    }    

    public void takeDamage(int damage) {
        health -= damage;
        AudioManager.PlaySFX(soundEffect);
        if (health < 1) {
            // TODO: Die/respawn, increase frustration stat
            Debug.Log(name + " has 0 HP");
            if (respawnOnDeath) {
                // TODO: Play a death animation before doing this
                GetComponentInParent<Rigidbody2D>().transform.position = startingPosition;
                health = startingHealth;
            }
            else {
                Destroy(gameObject);
            }
            return;
        }

        if (animator) {
            animator.SetTrigger(takeDamageTrigger);
        }
    }
}
