using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int startingHealth = 5;
    [ReadOnly] [SerializeField] private int health;

    void Start() {
        health = startingHealth;
    }    

    public void takeDamage(int damage) {
        health -= damage;
        if (health < 1) {
            // TODO: Die/respawn, increase frustration stat
            Debug.Log(name + " has 0 HP");
            Destroy(gameObject);
        }
    }
}
