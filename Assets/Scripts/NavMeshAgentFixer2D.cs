// Prevents NavMeshAgent from rotating its gameobject. Useful for utilizing NavMesh in 2D

using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentFixer2D : MonoBehaviour
{
    void Awake()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent == null) {
            Debug.Log("NavMeshAgent not found in " + gameObject.name);
            return;
        }
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
}
