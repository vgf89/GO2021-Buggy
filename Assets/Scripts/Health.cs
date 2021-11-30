using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int startingHealth = 5;
    [ReadOnly] [SerializeField] private int health;
    [SerializeField] private string soundEffect;
    [SerializeField] private Animator animator;
    [SerializeField] private string takeDamageTrigger;
    [SerializeField] private bool dontDestroyOnDeath = false;
    //[ReadOnly][SerializeField]private Vector3 startingPosition;
    public UnityEvent damaged;
    public UnityEvent killed;

    void Awake() {
        health = startingHealth;
        //startingPosition = transform.position;
    }    

    public void takeDamage(int damage) {
        health -= damage;
        AudioManager.PlaySFX(soundEffect);
        if (health < 1) {
            // TODO: Die/respawn, increase frustration stat
            Debug.Log(name + " has 0 HP");
            killed.Invoke();
            if (dontDestroyOnDeath) {
                // TODO: Play a death animation before doing this
                //transform.position = startingPosition;
                health = startingHealth;
            }
            else {
                Destroy(gameObject);
            }
            return;
        }

        damaged.Invoke(); // QUESTION: should both this and killed be invoked on death?

        if (animator) {
            animator.SetTrigger(takeDamageTrigger);
        }
    }
}
