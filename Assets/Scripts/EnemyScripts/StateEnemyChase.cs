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
    [SerializeField] private State attackState;
    [SerializeField] private float attackDelay = 0f; // Minimum time between attacks
    [SerializeField] private TriggerCheck adventurerDetectorTooClose;
    private float lastAttackTime = 0f;
    [SerializeField] private State runAwayState;
    [SerializeField] private float speed = 2f;
    private float originalSpeed;

    private GameObject adventurer;
    new private Rigidbody2D rigidbody2D;

    override public void Awake() {
        base.Awake();

        adventurer = GameObject.FindGameObjectWithTag("Adventurer"); // Alternatively: FindObjectOfType<AdventurerController>();, or use a serializedfield
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
        
        if (adventurerDetector && !adventurerDetector.isColliding()) {
            //Debug.Log("Lost player");
            stateSystem.popState();
            return true;
        }

        if (adventurerDetectorTooClose && runAwayState && adventurerDetectorTooClose.isColliding()) {
            //Debug.Log("Adventurer too close, run away");
            stateSystem.pushState(runAwayState);
            return true;
        }

        if (adventurerDetectorClose && attackState && Time.time >= lastAttackTime + attackDelay && adventurerDetectorClose.isColliding()) {
            lastAttackTime = Time.time;
            //Debug.Log("Player close enough to attack");
            stateSystem.pushState(attackState);
            return true;
        }

        Vector3 destination;
        if (speed < 0) {
            // negative speed means to move away from the adventurer, so we want to put the destination in the opposite direction
            destination = (adventurer.transform.position - navMeshAgent.transform.position).normalized * -10f;
        } else {
            destination = adventurer.transform.position;
        }
        
        // If where we are trying to navigate isn't on the navmesh, we need to find the closest point on the navmesh to navigate to
        NavMeshHit hit;
        NavMesh.SamplePosition(destination, out hit, 10f,NavMesh.AllAreas);
        navMeshAgent.SetDestination(hit.position);        

        //navMeshAgent.SetDestination(destination);
        navMeshAgent.speed = Mathf.Abs(speed);

        return false;
    }
}
