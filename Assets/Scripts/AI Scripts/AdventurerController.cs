using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class AdventurerController : MonoBehaviour
{
    

    [SerializeField] 
    private Camera mainCamera;
    [SerializeField]
    private NavMeshAgent adventurerNavMeshAgent;
    [SerializeField]
    private Tilemap map;

    private Vector3 adventurerDestination;

    // Start is called before the first frame update
    void Start()
    {
        //If NavMeshAgent has no reference from the inspector
        if (adventurerDestination == null)
            adventurerNavMeshAgent = GetComponent<NavMeshAgent>();

        //Prevents Agent from rotating away from the Camera
        adventurerNavMeshAgent.updateRotation = false;
        adventurerNavMeshAgent.updateUpAxis = false;

        if (adventurerNavMeshAgent != null)
            Debug.Log("Successfully set NaveMeshAgent from: " + transform.name);

    }

    // Update is called once per frame
    void Update()
    {
        MouseHandling();
        
    }



    //To test if navMeshAgent works, click an area in the game and it sets the agents destination
    void MouseHandling()
    {
        Vector3 mouseWorldPos = new Vector3();
        if (Input.GetMouseButtonDown(0))
        {
            mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Vector3Int mouseGridPos = map.WorldToCell(mouseWorldPos);
            adventurerDestination = mouseGridPos;
            Debug.Log("Setting " + transform.name + "'s destination to: " + mouseGridPos.ToString());
            adventurerNavMeshAgent.SetDestination(adventurerDestination);
        }
    }

    private void OnDrawGizmos()
    {
        if (adventurerNavMeshAgent.hasPath)
        {
            //Draws a blue wire cube at the Adventurer's destination
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(adventurerDestination, new Vector3(1, 1, 0));
        }
        else
            Gizmos.color = Color.clear;
    }

}