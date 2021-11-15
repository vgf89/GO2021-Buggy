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
    private const int DEPTHMAX = 2;

    //PLEASE TREAT AS A CONSTANT
    private float DIAGONALDISTANCE = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(1, 1, 0));
    private float NONDIAGONALDISTANCE = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(1, 0, 0));

    public bool isDebugging;
    public enum behaviors
    {
        DoNothing,
        Exploring, 
        InCombat,
        CompletingObjective
    }
    public behaviors behavior;

    public Vector3 currentKey, neighborKey;

    private Dictionary<Vector3, TileData> tiles;

    private const float NAVAGENTOFFPOSITIONOFFSET = 0.5f; 

    // Start is called before the first frame update
    void Start()
    {
        diagMovementCost = Mathf.Sqrt(Mathf.Pow(movementCost, 2));
        navAgent = GetComponent<NavMeshAgent>();
        //behavior = behaviors.DoNothing;
        tiles = gameTiles.tiles;

        if (navAgent != null && isDebugging)
            Debug.Log("navAgent initialized");

    }

    // Update is called once per frame
    void Update()
    {
        switch(behavior)
        {
            case behaviors.DoNothing:
                CalculateEfficiency(currentKey, neighborKey);
                break;

            case behaviors.Exploring:
                if (!navAgent.hasPath)
                {
                    FindPath();
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
        if ((tempScore == 0f) && (adventurerDestination.Equals(currentIntPos)))
            adventurerDestination = FindClosestUnexploredTile(adventurerDestination) + new Vector3(NAVAGENTOFFPOSITIONOFFSET, NAVAGENTOFFPOSITIONOFFSET, 0);
        navAgent.SetDestination(adventurerDestination);
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

        return tileData;
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

    //Determines efficiency by the tileValue divided by the movement cost [Depends whether the movement is a diagnoal or straight]]
    float CalculateEfficiency(Vector3 _currentTileKey, Vector3 _neighboryKey)
    {
        float distance = Vector3.Distance(_currentTileKey, _neighboryKey);
        
        //Determines if the distance is diagonal
        if(distance == DIAGONALDISTANCE)
        {
            Debug.Log("diagonal");
            //return (tileDictionary[_neighborKey].tileValue / diagMovementCost);
        }
        //Determines if the distance is nondiagonal
        else if(distance == NONDIAGONALDISTANCE)
            {
                Debug.Log("Nondiagonal");
                //return (tileDictionary[_neighborKey].tileValue / movementCost);
            }


        return 0;
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
        //Debug.Log("Finding Closest Unexplored Tile from" + _currentPosition.ToString() + " to " + closestPosition.ToString() + " at a distance of " + tempDistance);
        return closestPosition;
    }
}
