using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnemyIdle : State
{
    [SerializeField] private TriggerCheck adventurerDetector;
    new private Rigidbody2D rigidbody2D;

    override public void Awake() {
        base.Awake();

        rigidbody2D = adventurerDetector.GetComponent<Collider2D>().attachedRigidbody;
        
    }
    
    override public void enter()
    {    
        base.enter();
        rigidbody2D.velocity = Vector2.zero;
    }

    override public bool stateUpdate() {
        if (base.stateUpdate()) {
            return true;
        }
        
        if (adventurerDetector.isColliding()) {
            stateSystem.pushState(stateSystem.GetComponentInChildren<StateEnemyChase>());
            //Debug.Log("I see player");
            return true;
        }

        return false;
    }
}
