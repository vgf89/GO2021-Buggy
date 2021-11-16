using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(NavMeshAgent))]

public class AdventurerAIBehavior : MonoBehaviour
{

    [SerializeField]
    private Tilemap groundTileMap;
    [SerializeField]
    private Tilemap wallTileMap;
    [Tooltip("Get the GameTiles Component from Grid")]
    [SerializeField]
    private GameTiles gameTiles;
    [SerializeField]
    [Tooltip("Movement cost to move a tile.")]
    [MinAttribute(0)]
    private float movementCost;
    private float diagMovementCost;

    private NavMeshAgent navAgent;

    [SerializeField]
    [Tooltip("The depth/tiles away from the current position to consider for exploreation.\nThe original value for this is 2.")]
    [MinAttribute(1)]
    private int DEPTHMAX = 5;

    //PLEASE TREAT AS A CONSTANT
    private float DIAGONALDISTANCE = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(1, 1, 0));
    private float NONDIAGONALDISTANCE = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(1, 0, 0));

    public bool isDebugging;
    public enum behaviors
    {
        DoNothing,
        Thinking,
        Exploring, 
        InCombat,
        CompletingObjective
    }
    public behaviors behavior;

    public Vector3 currentKey, neighborKey;

    private Dictionary<Vector3, TileData> tiles;

    private const float NAVAGENTOFFPOSITIONOFFSET = 0.5f;

    [SerializeField]
    private Queue<TileData> tileDataQueue;

    private bool isThinking;

    private Vector3 mostEfficientTilePosition;
    private float mostEfficientScore;

    // Start is called before the first frame update
    void Start()
    {
        isThinking = false;
        diagMovementCost = Mathf.Sqrt(Mathf.Pow(movementCost, 2));
        navAgent = GetComponent<NavMeshAgent>();
        //behavior = behaviors.DoNothing;
        tiles = gameTiles.tiles;
        tileDataQueue = new Queue<TileData>();
        if (navAgent != null && isDebugging)
            Debug.Log("navAgent initialized");

        
    }

    // Update is called once per frame
    void Update()
    {
        switch(behavior)
        {
            case behaviors.Thinking:
                if (!isThinking)
                {
                    isThinking = true;
                    //BFS(FindCurrentTile());
                    var tempTileData = FindCurrentTile();
                    tempTileData.isVisited = true;
                    tileDataQueue.Enqueue(tempTileData);
                    BFSRecursion(tileDataQueue, 0);
                    if (isDebugging)
                    Debug.Log("Most efficient score is " + mostEfficientScore + " at " + mostEfficientTilePosition.ToString());
                    navAgent.SetDestination(mostEfficientTilePosition + new Vector3(0.5f,0.5f,0));
                    behavior = behaviors.Exploring;
                    //isThinking = false;
                    //tileDataQueue.Clear();
                }
                break;

            case behaviors.Exploring:
                if (!navAgent.hasPath)
                {
                    isThinking = false;
                    mostEfficientScore = -1;
                    mostEfficientTilePosition = transform.position;
                    behavior = behaviors.Thinking;
                }
                break;
        }

    }

    //Make a copy of the Dictionary of tiles
    //What needs to be passed: the <Vector3> current position (hypothetical or real), the depthCounter to check if they have reached the maximum amount of moves
    //What needs to be return: the <Vector3> position with the best score. That positions needs to be set as the destination
    //Check the current state of the board and determine its efficiency
    //If the absolute value difference between the currentTile's position and tile it is checking(which should be one of its neighbor) is greater than 1, the diagonal movecost is Mathf.Sqrt(2 * (Mathf.Pow(movementCost,2))
    void FindPath()
    {
        //Set the temp position to itself starting out, then compare efficiency score
        
        Vector3Int currentGridPos = groundTileMap.WorldToCell(transform.position);
        Vector3 adventurerDestination = currentGridPos;
        var currentTileData = tiles[currentGridPos];
        //This tempScore will mostly likely start out a zero.
        float tempScore = currentTileData.efficiencyScore;
        foreach (KeyValuePair<Vector3, TileData> neighborData in currentTileData.tileNeighbors)
        {
            if (neighborData.Value.efficiencyScore > tempScore && !neighborData.Value.isExplored)
            {
                adventurerDestination = neighborData.Value.worldPosition + new Vector3(NAVAGENTOFFPOSITIONOFFSET, NAVAGENTOFFPOSITIONOFFSET, 0);
                adventurerDestination.z = 0f;
                tempScore = neighborData.Value.efficiencyScore;
                Debug.Log(adventurerDestination.ToString());
            }
        }
        Vector3Int currentIntPos = Vector3Int.FloorToInt(transform.position);
        currentIntPos.z = 0;
        //If a tile can't be decided, SetDestination to the closes unexplored GroundTile
        /*if ((tempScore == 0f) && (adventurerDestination.Equals(currentIntPos)))
            adventurerDestination = FindClosestUnexploredTile(adventurerDestination) + new Vector3(NAVAGENTOFFPOSITIONOFFSET, NAVAGENTOFFPOSITIONOFFSET, 0);
        navAgent.SetDestination(adventurerDestination);*/
    }

    //Hardcopying a tile's TileData
    TileData HardCopyTileData(TileData _tileData)
    {
        var tileData = ScriptableObject.CreateInstance<TileData>();
        tileData.tileName = _tileData.tileName;
        tileData.localPosition = _tileData.localPosition;
        tileData.worldPosition = _tileData.worldPosition;
        tileData.tilemapMember = _tileData.tilemapMember;
        tileData.isExplored = _tileData.isExplored;
        tileData.tileNeighbors = _tileData.tileNeighbors;
        tileData.tileValue = _tileData.tileValue;
        tileData.efficiencyScore = _tileData.efficiencyScore;

        return tileData;
    }

    TileData FindCurrentTile()
    {
        Vector3Int currentGridPos = groundTileMap.WorldToCell(transform.position);

        return tiles[currentGridPos];
    }

    void DepthSearching(TileData _tileData, int depthCounter)
    {
        //The ending condition of a recursive function
        if (depthCounter >= DEPTHMAX)
        {

            return;
        }
        else
        {
            TileData tempTileData = HardCopyTileData(_tileData);
            DepthSearching(tempTileData, depthCounter + 1);
        }
    }

    void BFS(TileData src)
    {
        int depthCounter = 0;
        float tempEfficiencyScore = -1;
        Vector3 efficientDestination = src.worldPosition;

        tileDataQueue.Enqueue(src);
        src.isVisited = true;

        while (tileDataQueue.Count != 0)
        {
           
            var groundTileData = HardCopyTileData(tileDataQueue.Dequeue());
            Debug.Log("Tile at " + groundTileData.worldPosition + "has been dequeued.");
            //visit the Dequeued neighbors tiles and add them to the queue
            foreach (KeyValuePair<Vector3, TileData> neighborTileData in groundTileData.tileNeighbors)
            {
                //If already in the queue or TileData is for a wall tile
                if (!neighborTileData.Value.isVisited || neighborTileData.Value.tileValue.Equals(GameTiles.GROUNDTILENAMESTRING))
                {
                    //Enqueues the neighbors to be visited later
                    tileDataQueue.Enqueue(neighborTileData.Value);
                    Debug.Log("Tile at " + neighborTileData.Value.worldPosition + "has been enqueued.");
                    neighborTileData.Value.isVisited = true;
                }
            }

            //Determine the efficiency and set it to the TileData's efficiency score
            groundTileData.efficiencyScore = CalculateEfficiency(groundTileData);

            if (groundTileData.efficiencyScore > tempEfficiencyScore)
            {
                tempEfficiencyScore = groundTileData.efficiencyScore;
                efficientDestination = groundTileData.worldPosition;
            }
            
        }
        tileDataQueue.Clear();
        Debug.Log("The most efficient route is going to " + efficientDestination + " with an efficiency score of " + tempEfficiencyScore);
    }

    void BFSRecursion (Queue<TileData> q, int depthCounter)
    {
        if (depthCounter >= DEPTHMAX || q.Count == 0)
        {
            //isThinking = false;
            //Debug.Log("Done");
            return;
        }

        
        Vector3 efficientDestination = transform.position;

        //Dequeue 
        var groundTileData = q.Dequeue();
        depthCounter++;
        float tempEfficiencyScore = CalculateEfficiency(groundTileData);
        if (tempEfficiencyScore > mostEfficientScore)
        {
            mostEfficientScore = tempEfficiencyScore;
            mostEfficientTilePosition = groundTileData.worldPosition;
        }
        if(isDebugging)
            Debug.Log("Tile at " + groundTileData.worldPosition + "has been dequeued.");

        //Queue its neighbors
        foreach (KeyValuePair<Vector3, TileData> neighborTileData in groundTileData.tileNeighbors)
        {
            if (!neighborTileData.Value.isVisited && neighborTileData.Value.tileName.Equals(GameTiles.GROUNDTILENAMESTRING))
            {
                neighborTileData.Value.isVisited = true;
                q.Enqueue(neighborTileData.Value);
                if(isDebugging)
                    Debug.Log("Tile at " + neighborTileData.Value.worldPosition + "has been enqueued at a depth of [" + depthCounter + "].");
            }
        }
        
        
        BFSRecursion(q, depthCounter);
    }

    //Determines efficiency by the tileValue divided by the movement cost [Depends whether the movement is a diagnoal or straight]]
    float CalculateEfficiency(TileData _tileData)
    {
        float moveCost = Vector3.Distance(_tileData.worldPosition, transform.position);
        float effScore = _tileData.tileValue / (moveCost * 10);

        return effScore;
    }

    //Finds the closest unexplored groundTile
    Vector3 FindClosestUnexploredTile(Vector3 _currentPosition)
    {
        float tempDistance = float.MaxValue;
        Vector3 closestPosition = _currentPosition;

        foreach (KeyValuePair<Vector3, TileData> tileData in tiles)
        {
            Vector3 checkingPosition = tileData.Value.worldPosition;
            if (tiles[checkingPosition].tileName.Equals(GameTiles.GROUNDTILENAMESTRING) && !tiles[checkingPosition].isExplored 
                && (Vector3.Distance(_currentPosition, checkingPosition) < tempDistance))
            {
                tempDistance = Vector3.Distance(_currentPosition, checkingPosition);
                closestPosition = tileData.Value.worldPosition;
            }
        }
        closestPosition.z = 0f;
        Debug.Log("Finding Closest Unexplored Tile from" + _currentPosition.ToString() + " to " + closestPosition.ToString() + " at a distance of " + tempDistance);
        return closestPosition;
    }
    
}
