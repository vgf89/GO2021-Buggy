using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeathWarp : MonoBehaviour
{
    private Vector3 startingPosition;
    private NavMeshAgent navMeshAgent;
    void Awake()
    {
        startingPosition = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void Warp()
    {
        navMeshAgent.Warp(startingPosition);
        Object.FindObjectOfType<AdventurerAIBehavior>().RestartThinking();
    }
}
