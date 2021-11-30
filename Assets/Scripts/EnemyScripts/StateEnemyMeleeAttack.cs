using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnemyMeleeAttack : State
{
    [SerializeField] private TriggerCheck adventurerDetector;
    new private Rigidbody2D rigidbody2D;
    [SerializeField] private int damage = 1;

    override public void Awake() {
        base.Awake();

        rigidbody2D = adventurerDetector.GetComponent<Collider2D>().attachedRigidbody;
        
    }
    
    override public void enter()
    {    
        base.enter();
        rigidbody2D.velocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Health otherHealth = other.GetComponent<Health>();
        if (otherHealth) {
            otherHealth.takeDamage(damage);
        }
    }

    override public bool stateUpdate() {
        if (base.stateUpdate()) {
            return true;
        }

        if (animationFinished) {
            stateSystem.popState();
            return true;
        }

        return false;
    }
}
