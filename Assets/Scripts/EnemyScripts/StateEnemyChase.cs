using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateEnemyChase : State
{
    [Header("Chase State Settings")]
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] private TriggerCheck adventurerDetector;
    [SerializeField] private TriggerCheck adventurerDetectorClose;
    [SerializeField] private float speed = 2f;
    private float originalSpeed;

    private GameObject adventurer;
    new private Rigidbody2D rigidbody2D;

    override public void Awake() {
        base.Awake();

        adventurer = GameObject.FindGameObjectWithTag("Adventurer"); // Alternatively: FindObjectOfType<AdventurerController>();
        rigidbody2D = adventurerDetector.GetComponent<Collider2D>().attachedRigidbody;
        
    }

    public override void enter()
    {
        base.enter();
        originalSpeed = navMeshAgent.speed;

    }

    public override void exit()
    {
        navMeshAgent.speed = originalSpeed;
        navMeshAgent.SetDestination(navMeshAgent.transform.position);
        base.exit();
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

        //rigidbody2D.velocity = (adventurer.transform.position - transform.position).normalized * speed;
        navMeshAgent.SetDestination(adventurer.transform.position);
        navMeshAgent.speed = speed;

        return false;
    }
}
