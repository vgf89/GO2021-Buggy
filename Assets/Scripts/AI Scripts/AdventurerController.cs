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

    [SerializeField]
    [Tooltip("Vision radius based on the tile the Adventurer is located.")]
    [MinAttribute(0)]
    private int visionRadius;

    [SerializeField]
    private GameTiles gameTiles;

    private Vector3 adventurerDestination;

    [SerializeField]
    public List<Vector3> discoveredChestPositionList;
    public int keysCount;
    [SerializeField]
    public List<Vector3> discoveredKeyPositionList;

    [Header("Inspector Debugging")]
    [SerializeField]
    [Tooltip("Will display relevant console information from this script.")]
    private bool isDebugging;
    [SerializeField]
    private bool drawGizmo;
    [SerializeField]
    [Tooltip("Mostly used for Adventurer movement debugging purposes. PLEASE SET THE ADVENTURER AI BEHAVIOR TO 'DO NOTHING'.")]
    private bool useMouse;

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        //If NavMeshAgent has no reference from the inspector
        if (adventurerDestination == null)
            adventurerNavMeshAgent = GetComponent<NavMeshAgent>();

        if (adventurerNavMeshAgent != null && isDebugging)
            Debug.Log("Successfully set NaveMeshAgent from: " + transform.name);

        speed = adventurerNavMeshAgent.speed;
    }

    // Update is called once per frame
    void Update()
    {

        if (useMouse)
            MouseHandling();

        SetExploring();

        adventurerNavMeshAgent.speed = speed * GameWorldSpeedController.worldSpeedMultiplier;
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
            adventurerDestination = new Vector3 (0.5f, 0.5f, 0) + mouseGridPos;
            if(isDebugging)
                Debug.Log("Setting " + transform.name + "'s destination to: " + mouseGridPos.ToString());
            adventurerNavMeshAgent.SetDestination(adventurerDestination);
        }
        
    }

    //Once a tile has been explored, set the neighboring tiles' new value
    void SetExploring()
    {
        Vector3Int currentGridPos = map.WorldToCell(transform.position);
        var gTileData = ScriptableObject.CreateInstance<TileData>();

        //Displays vision radius USE THIS ALGORITHM FOR SETTING GROUNDDATATILES TO isExplored
        for (int x = -visionRadius; x <= visionRadius; x++)
        {
            for (int y = -visionRadius; y <= visionRadius; y++)
            {
                int absValue = Mathf.Abs(x) + Mathf.Abs(y);
                if (absValue <= visionRadius)
                {
                    if (gameTiles.tiles.TryGetValue(currentGridPos + new Vector3Int(x, y, 0), out gTileData))
                        if (!gTileData.isExplored)
                        {
                            gTileData.isExplored = true;

                            // Color the tile to show it's been explored
                            gameTiles.ColorExploredTile(gTileData);

                            if (isDebugging)
                                Debug.Log(transform.name + " has explored the ground tile at " + gTileData.worldPosition.ToString());
                            gameTiles.GetTileValueOfNeighbors(gTileData.worldPosition);
                        }
                }
            }
        }
    }

    public void SaveItemPosition(Transform itemTransform, string tagName)
    {
        Vector3Int itemPosition = Vector3Int.FloorToInt(itemTransform.position);
        itemPosition.z = 0;
        if (tagName.Equals("Chest"))
            discoveredChestPositionList.Add(itemPosition);
        else if (tagName.Equals("Key"))
            discoveredKeyPositionList.Add(itemPosition);
        if (isDebugging)
        {
            Debug.Log(tagName + " position saved at: " + itemPosition.ToString());
        }
    }

    
    

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Vector3Int intCurrentPos = map.WorldToCell(transform.position);
            //Displays the Adventurer's destination in Grid
            if (adventurerNavMeshAgent.hasPath)
            {
                //Draws a blue wire cube at the Adventurer's destination
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(adventurerDestination, new Vector3(1, 1, 0));
            }

            //Displays vision radius USE THIS ALGORITHM FOR SETTING GROUNDDATATILES TO isExplored
            for (int x = -visionRadius; x <= visionRadius; x++)
            {
                for (int y = -visionRadius; y <= visionRadius; y++)
                {
                    int absValue = Mathf.Abs(x) + Mathf.Abs(y);
                    if (absValue <= visionRadius)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(intCurrentPos + new Vector3Int(x, y, 0) + new Vector3 (0.5f, 0.5f, 0), new Vector3(1, 1, 0));
                    }
                }
            }
        }
    }

    

}
