using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(NavMeshAgent))]

public class AdventurerAIBehavior : MonoBehaviour
{
    private const float NAVAGENTOFFPOSITIONOFFSET = 0.5f;

    public AdventurerController adventurerController;
    [SerializeField]
    private Tilemap groundTileMap;
    [Tooltip("Get the GameTiles Component from Grid")]
    [SerializeField]
    private GameTiles gameTiles;
    [SerializeField]
    [Tooltip("Movement cost to move a tile.")]
    

    private NavMeshAgent navAgent;

    [SerializeField]
    [Tooltip("The depth/tiles away from the current position to consider for exploreation.\nThe original value for this is 2.")]
    [MinAttribute(1)]
    private int DEPTHMAX = 5;

    public DetectorScript detectorScript;

    public int tooManyEnemyThreshold;

    [Header("Inspector Debugging")]
    [SerializeField]
    private bool isDebugging;
    public enum behaviors
    {
        DoNothing,
        Thinking,
        Exploring, 
        InCombat,
        SeekKey,
        SeekChest,
        AllTilesExplored
    }
    public behaviors behavior;

    private Dictionary<Vector3, TileData> tiles;

    [SerializeField]
    private Queue<TileData> tileDataQueue;

    //This gives the AI time to determine a Destination for the NavAgent before Exploring
    private bool isThinking;
    private bool isFleeing;

    private List<Vector3> mostEfficientTilePositions;
    private float mostEfficientScore;

    // Start is called before the first frame update
    void Start()
    {
        isThinking = false;
        
        navAgent = GetComponent<NavMeshAgent>();
        tiles = gameTiles.tiles;
        tileDataQueue = new Queue<TileData>();
        mostEfficientTilePositions = new List<Vector3>();
        detectorScript = GetComponentInChildren<DetectorScript>();
        if (navAgent != null && isDebugging)
            Debug.Log("navAgent initialized");
        if (adventurerController == null)
            Debug.LogError("Adventurer Controller has not be set in " + this.GetType().Name);
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(behavior)
        {
            case behaviors.Thinking:
                if (!isThinking || isFleeing)
                {
                    isThinking = true;
                    if (gameTiles.AllTilesExplored())
                    {
                        tileDataQueue.Clear();
                        adventurerController.discoveredChestPositionList = Object.FindObjectOfType<MapManager>().GetAllNonDiscoveredObjectives(ref adventurerController.discoveredKeyPositionList);
                        behavior = behaviors.AllTilesExplored;
                        break;
                    }
                    StartThinking();
                    BFSRecursionExploration(tileDataQueue, 0);
                    FindDestination();
                    behavior = CheckToSwitchBehaviors(behavior);
                    break;
                }
                break;

            case behaviors.Exploring:
                if (!navAgent.hasPath)
                {
                    mostEfficientScore = 0;
                    isThinking = false;
                    behavior = CheckToSwitchBehaviors(behavior);
                    if (detectorScript.enemyTransforms.Count > tooManyEnemyThreshold)
                        isFleeing = true;
                    else
                        isFleeing = false;
                    break;
                }
                break;

            case behaviors.AllTilesExplored:
                //navAgent.autoBraking = true;
                behavior = CheckToSwitchBehaviors(behavior);
                break;

            case behaviors.SeekKey:
                isThinking = false;
                GoToClosestObjective(adventurerController.discoveredKeyPositionList);
                behavior = CheckToSwitchBehaviors(behavior);
                break;

            case behaviors.SeekChest:
                isThinking = false;
                GoToClosestObjective(adventurerController.discoveredChestPositionList);
                behavior = CheckToSwitchBehaviors(behavior);
                break;

            case behaviors.InCombat:
                FindClosestEnemy(detectorScript.enemyTransforms);
                isThinking = false;

                behavior = CheckToSwitchBehaviors(behavior);
                break;
        }

        
    }


    //Sets the first steps of adding the Adventurer's current tile to the queue before Doing BFSRecusionExploration
    void StartThinking()
    {
        var tempTileData = FindCurrentTile();
        tempTileData.isVisited = true;
        gameTiles.CheckAllTilesIsExplored();
        gameTiles.GetAllTileValues();
        tileDataQueue.Enqueue(tempTileData);
    }

    //This is mostly for when the Adventurer is sent into the past, it resets its train of thought and will think of its route from the position it was reset to.
    public void RestartThinking()
    {
        tileDataQueue.Clear();
        isThinking = false;
        behavior = behaviors.Thinking;
        var tempTileData = FindCurrentTile();
        tempTileData.isVisited = true;
        gameTiles.CheckAllTilesIsExplored();
        gameTiles.GetAllTileValues();
        tileDataQueue.Enqueue(tempTileData);
    }

    #region DestinationHandling

    
    TileData FindCurrentTile()
    {
        Vector3Int currentGridPos = groundTileMap.WorldToCell(transform.position);

        return tiles[currentGridPos];
    }

    void FindDestination()
    {
        Vector3 destination = transform.position;
        int tilePositionsCount = mostEfficientTilePositions.Count;
        //If there are no tile positions in the list
        if (tilePositionsCount != 0)
        {
            int randomIndex = Random.Range(0, tilePositionsCount);
            destination = mostEfficientTilePositions[randomIndex];
            if (isDebugging)
                Debug.Log("Most efficient score is " + mostEfficientScore + " at " + destination.ToString());
        }
        else
        {
            destination = FindClosestUnexploredTile(transform.position);
        }

        SetAdvNavAgentDestination(destination);
    }

    void SetAdvNavAgentDestination(Vector3 destinationPosition)
    {
        navAgent.SetDestination(destinationPosition + new Vector3(NAVAGENTOFFPOSITIONOFFSET, NAVAGENTOFFPOSITIONOFFSET, 0));
        mostEfficientTilePositions.Clear();
        tileDataQueue.Clear();
    }

    void GoToClosestObjective(List<Vector3> objectiveList)
    {
        List<Vector3> tempObjectiveList = objectiveList;
        if (tempObjectiveList.Count != 0)
        {
            Vector3 closestObjectivePosition = tempObjectiveList[0];
            float closestDistanceToObjective = Vector3.Distance(transform.position, closestObjectivePosition);
            

            foreach (Vector3 chestPosition in tempObjectiveList)
            {
                float tempDistance = Vector3.Distance(transform.position, chestPosition);
                if (tempDistance < closestDistanceToObjective)
                {
                    closestDistanceToObjective = tempDistance;
                    closestObjectivePosition = chestPosition;
                }
            }
            SetAdvNavAgentDestination(closestObjectivePosition);
        }
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
        if (isDebugging)
            Debug.Log("Finding Closest Unexplored Tile from" + _currentPosition.ToString() + " to " + closestPosition.ToString() + " at a distance of " + tempDistance);
        return closestPosition;
    }

    //Finds the closest enemy in the detection range and goes to that enemy
    void FindClosestEnemy(List<Transform> transforms)
    {
        List<Transform> tempEnemyTransforms = transforms;
        if(transforms.Count != 0)
        {
            Vector3 closestEnemyPosition = tempEnemyTransforms[0].position;
            float closestDistanceToEnemy = Vector3.Distance(transform.position, closestEnemyPosition);

            foreach(Transform t in tempEnemyTransforms)
            {
                float tempDistance = Vector3.Distance(transform.position, t.position);
                if (tempDistance < closestDistanceToEnemy)
                {
                    closestDistanceToEnemy = tempDistance;
                    closestEnemyPosition = t.position;
                }
            }
            SetAdvNavAgentDestination(closestEnemyPosition);
        }
    }

    #endregion

    //Make a copy of the Dictionary of tiles
    //What needs to be passed: the <Vector3> current position (hypothetical or real), the depthCounter to check if they have reached the maximum amount of moves
    //What needs to be return: the <Vector3> position with the best score. That positions needs to be set as the destination
    //Check the current state of the board and determine its efficiency
    //If the absolute value difference between the currentTile's position and tile it is checking(which should be one of its neighbor) is greater than 1, the diagonal movecost is Mathf.Sqrt(2 * (Mathf.Pow(movementCost,2))


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

    //Find the tileData based on the current Adventurer's position


    /*void DepthSearching(TileData _tileData, int depthCounter)
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
    }*/

    /*    void BFS(TileData src)
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
    }*/

    #region ExplorationAlgorithm

    void BFSRecursionExploration(Queue<TileData> q, int depthCounter)
    {
        //Condition to break the recursive process
        if (q.Count == 0)
        {
            //isThinking = false;
            //Debug.Log("Done");
            return;
        }

        Vector3 efficientDestination = transform.position;

        //Dequeue 
        var groundTileData = q.Dequeue();
        depthCounter++;

        //Another condition to break the recursive process
        if (depthCounter > DEPTHMAX)
        {
            //isThinking = false;
            //Debug.Log("Done");
            return;
        }

        //Processes for non explored tiles
        //if (!groundTileData.isExplored)
        EfficiencyHandling(groundTileData);

        if (isDebugging)
            Debug.Log("Tile at " + groundTileData.worldPosition + "has been dequeued.");

        //Queue its neighbors
        foreach (KeyValuePair<Vector3, TileData> neighborTileData in groundTileData.tileNeighbors)
        {
            if (!neighborTileData.Value.isVisited && neighborTileData.Value.tileName.Equals(GameTiles.GROUNDTILENAMESTRING))
            {
                neighborTileData.Value.isVisited = true;
                q.Enqueue(neighborTileData.Value);
                if (isDebugging)
                    Debug.Log("Tile at " + neighborTileData.Value.worldPosition + "has been enqueued at a depth of [" + depthCounter + "].");
            }
        }


        BFSRecursionExploration(q, depthCounter);
    }

    //Determines efficiency by the tileValue divided by the movement cost [Depends whether the movement is a diagnoal or straight]]
    //Finds the most efficient score and adds the position to the mostEfficientTilePositions List
    void EfficiencyHandling(TileData _tileData)
    {
        float moveCost = Vector3.Distance(_tileData.worldPosition, transform.position);
        float effScore = _tileData.tileValue / (moveCost * 10);

        //if tileValue is 0, do nothing. This is so if all tiles within the DEPTHMAX range have been explored find the closest unexplored tile to the Adventurer.
        if (effScore == 0)
        {
            return;
        }
        if (effScore >= mostEfficientScore)
        {
            mostEfficientScore = effScore;
            mostEfficientTilePositions.Clear();
            mostEfficientTilePositions.Add(_tileData.worldPosition);
        }
        else if (effScore == mostEfficientScore)
        {
            mostEfficientTilePositions.Add(_tileData.worldPosition);
        }

    } 
    #endregion
    
    
    behaviors CheckToSwitchBehaviors(behaviors currentBehavior)
    {
        int currentKeyCount = adventurerController.keysCount;
        int currentDiscoveredChestCount = adventurerController.discoveredChestPositionList.Count;
        int currentDiscoveredKeyCount = adventurerController.discoveredKeyPositionList.Count;

        if (currentBehavior == behaviors.Thinking)
        {
            //Prioritizes unlocking chests than seeking keys
            if (currentKeyCount > 0 && currentDiscoveredChestCount > 0)
                return behaviors.SeekChest;
            if (detectorScript.enemyTransforms.Count > 0 && !isFleeing)
                return behaviors.InCombat;
            //If the adventurer knows where the keys are, but doesn't have them Å® pick up a key
            if (currentKeyCount < currentDiscoveredKeyCount && currentDiscoveredKeyCount > 0)
                return behaviors.SeekKey;
            else
                return behaviors.Exploring;
        }

        if (currentBehavior == behaviors.Exploring)
        {
            if (!navAgent.hasPath)
                return behaviors.Thinking;
        }

        if (currentBehavior == behaviors.SeekKey)
        {
            //The moment it picks up a key and no longer knows where the other keys are
            if (currentDiscoveredKeyCount == 0)
            {
                return behaviors.Thinking;
            }
            else
                return behaviors.SeekKey;
        }

        if (currentBehavior == behaviors.SeekChest)
        {
            //If the Adventurer knows where one chest is and has at least one key Å® unlock chest
            if (currentKeyCount > 0 && currentDiscoveredChestCount > 0)
            {
                return behaviors.SeekChest;
            }
            else
                return behaviors.Thinking;

        }

        if (currentBehavior == behaviors.InCombat)
        {
            if (isFleeing)
                return behaviors.Thinking;
            if (detectorScript.enemyTransforms.Count == 0)
                return behaviors.Thinking;
        }

        //If all tiles have been explored, if The Adventurer has a key, open a chest. Else pick up a key.
        if (currentBehavior == behaviors.AllTilesExplored)
        {
            if (currentKeyCount > 0)
                return behaviors.SeekChest;
            else
                return behaviors.SeekKey;
        }
        return currentBehavior;
    }
}
