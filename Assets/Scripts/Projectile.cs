using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int damage = 1;
    [SerializeField] private float life = 1f;
    private float startTime;
    public Vector2 velocity;

    void Start() {
        startTime = Time.time;
    }

    void OnTriggerEnter2D(Collider2D other) {
        Health health = other.GetComponent<Health>();
        if (health != null) {
            health.takeDamage(damage);
        }
        //Debug.Log("Projectile hit other: " + other.name + ". Destroying this projectile: " + gameObject.name);
        Destroy(gameObject);
    }

    void Update() {
        if (Time.time > startTime + life) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
