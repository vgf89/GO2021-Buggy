using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateEnemyPatrol : State
{
    [Header("Patrol State Settings")]
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] private TriggerCheck adventurerDetector;
    [SerializeField] float patrolRadius = 5;
    private Vector3 startingPos;

    private Vector3 destination;
    

    // Start is called before the first frame update
    override public void enter() {
        base.enter();
        startingPos = navMeshAgent.transform.position;
    }

    override public bool stateUpdate()
    {
        if (base.stateUpdate()) {
            return true;
        }
        
        if (adventurerDetector.isColliding()) {
            stateSystem.switchState(stateSystem.GetComponentInChildren<StateEnemyChase>());
            return true;
        }

        if (navMeshAgent.remainingDistance < 0.1f) {
            float angle = Random.Range(0f, 360f);
            float magnitude = Random.Range(0f, patrolRadius);
            Vector2 delta = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * magnitude;
            destination = startingPos + new Vector3(delta.x, delta.y, startingPos.z);
            NavMeshHit hit;
            NavMesh.SamplePosition(destination, out hit, patrolRadius*3f,NavMesh.AllAreas);
            navMeshAgent.SetDestination(hit.position);           
        }

        return false;
    }

    // Update is called once per frame
    override public void exit()
    {
        navMeshAgent.SetDestination(navMeshAgent.transform.position);
        base.exit();
    }
}
