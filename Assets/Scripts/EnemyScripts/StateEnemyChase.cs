using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnemyChase : State
{
    [SerializeField] private TriggerCheck adventurerDetector;
    [SerializeField] private TriggerCheck adventurerDetectorClose;
    [SerializeField] private float speed = 0.5f;

    private GameObject adventurer;
    new private Rigidbody2D rigidbody2D;

    override public void Awake() {
        base.Awake();

        adventurer = GameObject.FindGameObjectWithTag("Adventurer"); // Alternatively: FindObjectOfType<AdventurerController>();
        rigidbody2D = adventurerDetector.GetComponent<Collider2D>().attachedRigidbody;
        
    }

    override public bool stateUpdate() {
        if (base.stateUpdate()) {
            return true;
        }
        
        if (!adventurerDetector.isColliding()) {
            stateSystem.popState();
            //Debug.Log("Lost player");
            return true;
        }

        if (adventurerDetectorClose.isColliding()) {
            //Debug.Log("Player close enough to attack");
            stateSystem.pushState(stateSystem.GetComponentInChildren<StateEnemyMeleeAttack>());
            return true;
        }

        rigidbody2D.velocity = (adventurer.transform.position - transform.position).normalized * speed;

        return false;
    }
}
