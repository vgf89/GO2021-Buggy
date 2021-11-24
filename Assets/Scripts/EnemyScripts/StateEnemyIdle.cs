using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnemyIdle : State
{
    [Header("Idle State Settings")]
    [SerializeField] private TriggerCheck adventurerDetector;
    new private Rigidbody2D rigidbody2D;

    [SerializeField] private float idlePatrollingTimeout = 5f;
    private float patrollingStartTime;

    override public void Awake() {
        base.Awake();

        rigidbody2D = adventurerDetector.GetComponent<Collider2D>().attachedRigidbody;
        
    }
    
    override public void enter()
    {    
        base.enter();
        rigidbody2D.velocity = Vector2.zero;
        patrollingStartTime = Time.time + idlePatrollingTimeout;
    }

    override public bool stateUpdate() {
        if (base.stateUpdate()) {
            return true;
        }

        if (Time.time > patrollingStartTime) {
            stateSystem.pushState(stateSystem.GetComponentInChildren<StateEnemyPatrol>());
            return true;
        }
        
        if (adventurerDetector.isColliding()) {
            stateSystem.pushState(stateSystem.GetComponentInChildren<StateEnemyChase>());
            return true;
        }

        return false;
    }
}
