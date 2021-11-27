using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameWorldSpeedController : MonoBehaviour
{
    [MinAttribute(0f)]
    public float worldSpeedMultiplier = 0.33f;
    public bool changedSpeed;

    [SerializeField] NavMeshAgent[] worldNavMeshAgents;
    [SerializeField] Animator[] worldAnimations;

    // Start is called before the first frame update
    void Start()
    {
        worldNavMeshAgents = Object.FindObjectsOfType<NavMeshAgent>();
        worldAnimations = Object.FindObjectsOfType<Animator>();
        changedSpeed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        worldNavMeshAgents = Object.FindObjectsOfType<NavMeshAgent>();
        worldAnimations = Object.FindObjectsOfType<Animator>();

    }

    public void changeWorldSpeed()
    {
        changedSpeed = !changedSpeed;
        foreach (Animator a in worldAnimations)
        {
            if (changedSpeed)
                a.speed *= worldSpeedMultiplier;
            else
                a.speed /= worldSpeedMultiplier;
        }
        foreach (NavMeshAgent agent in worldNavMeshAgents)
        {
            if (changedSpeed)
            {
                agent.speed *= worldSpeedMultiplier;
                agent.acceleration *= worldSpeedMultiplier;
            }
            else
            {
                agent.speed /= worldSpeedMultiplier;
                agent.acceleration /= worldSpeedMultiplier;
            }
        }
    }
}
