using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(NavMeshAgent))]
public class AdventurerAIBehavior : MonoBehaviour
{

    [SerializeField]
    private Tilemap map;
    [SerializeField]
    [Tooltip("Movement cost to move a tile.")]
    [MinAttribute(0)]
    private int movementCost;

    private NavMeshAgent navAgent;

    public bool isDebugging;
    public enum behaviors
    {
        DoNothing,
        Exploring, 
        InCombat,
        CompletingObjective
    }
    public behaviors behavior;
    

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        behavior = behaviors.Exploring;

        if (navAgent != null && isDebugging)
            Debug.Log("navAgent initialized");

    }

    // Update is called once per frame
    void Update()
    {
        switch(behavior)
        {
            case behaviors.DoNothing:
                break;

            case behaviors.Exploring:
                if (!navAgent.hasPath)
                {
                    FindPath();
                }
                break;
        }

    }

    void FindPath()
    {

    }
}
