using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTiles : MonoBehaviour
{
    //Function currently unknown
    public static GameTiles instance;

    [Tooltip("Insert the Walkable Ground Tilemap Here")]
    public Tilemap tilemap;

    public Dictionary<Vector3, GroundTileData> tiles;

    public GroundTileData[,] tilesArray;

    [Tooltip("Will display relevant console information from this script.")]
    [SerializeField]
    private bool isDebugging;

   

    void Awake()
    {
        if (instance == null)
            instance = this;
        /*else if (instance != this)
        {
            Destroy(gameObject);
        }*/

        //GetGridTilesInArray();
        GetGridTilesInDictionary();
        SetGridTileNeighbors();
        GetAllTileValues();
    }

    //Adds all tiles from the Ground Tilemap and stores it into a 2D array. CURRENTLY NOT IN USE
    private void GetGridTilesInArray()
    {
        tilesArray = new GroundTileData[tilemap.size.x, tilemap.size.y];

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            var localPos = new Vector3Int(pos.x, pos.y, pos.z);

            if (!tilemap.HasTile(pos))
                continue;

            GroundTileData tileData = ScriptableObject.CreateInstance<GroundTileData>();

            tileData.localPosition = localPos;
            tileData.worldPosition = tilemap.CellToWorld(localPos);
            tileData.tilemapMember = tilemap;
            tileData.isExplored = false;
            tileData.tileValue = 0;

            tilesArray[pos.x, pos.y] = tileData;
        }
    }

    //Finds all from the Ground Tilemap and stores in a Dictionary. The key is the tile's world position.
    private void GetGridTilesInDictionary()
    {
        tiles = new Dictionary<Vector3, GroundTileData>();

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            var localPos = new Vector3Int(pos.x, pos.y, pos.z);

            if (!tilemap.HasTile(localPos))
                continue;

            var tile = ScriptableObject.CreateInstance<GroundTileData>();
            tile.localPosition = localPos;
            tile.worldPosition = tilemap.CellToWorld(localPos);
            tile.tilemapMember = tilemap;
            tile.isExplored = false;
            tile.tileNeighbors = new Dictionary<Vector3, GroundTileData>();
            tile.tileValue = 0;


            //Adds the tile with the key being the tile's position in the world
            tiles.Add(tile.worldPosition, tile);
            //Debug.Log("Tile from " + tile.worldPosition + " has been added to the tiles Dictionary.");
        }
    }

    //After all ground tiles have their GroundTileData, find their neighbor and add to the tileNeighbors dictionary
    private void SetGridTileNeighbors()
    {
        var groundTileData = ScriptableObject.CreateInstance<GroundTileData>();
        Vector3 checkingPosition = new Vector3();

        foreach (KeyValuePair<Vector3, GroundTileData> groundTile in tiles)
        {
            for (int x = -1; x <= 1; x++)
            {

                for (int y = -1; y <= 1; y++)
                {
                    checkingPosition = groundTile.Value.worldPosition + new Vector3(x, y, 0);
                    //If the value is itself, DO NOT ADD AS NEIGHBOR
                    if (groundTile.Value.worldPosition == checkingPosition)
                    {
                        //continue;
                    }
                    else if (tiles.TryGetValue(checkingPosition, out groundTileData))
                    {
                        groundTile.Value.tileNeighbors.Add(checkingPosition, tiles[checkingPosition]);
                        //Debug.Log("Tile " + groundTile.Value.worldPosition.ToString() + " found a neighbor at " + groundTile.Value.tileNeighbors[checkingPosition].worldPosition.ToString());
                    }
                }
                ;
            }
            //Debug.Log("Tile " + groundTile.Value.worldPosition.ToString() + " has " + groundTile.Value.tileNeighbors.Count + " neighbors.");
        }
    }

    //Iterates through every tile in the dictionary and sets their tileValue relative to their tileNeighbors.
    public void GetAllTileValues()
    {

        foreach (KeyValuePair<Vector3, GroundTileData> groundTile in tiles)
        {
            int tempCounter = 0;
            foreach (KeyValuePair<Vector3, GroundTileData> neighborTile in groundTile.Value.tileNeighbors)
            {
                //if the neighboring tiles have not been explored
                if (!neighborTile.Value.isExplored)
                {
                    tempCounter++;
                }
            }
            groundTile.Value.tileValue = tempCounter;
            if (isDebugging)
                Debug.Log(groundTile.Value.printData());
        }
    }

    //Mostly for tiles that have been explored. Find the tileData and adjust the tileNeighbor's tileValue.
    public void GetTileValueOfNeighbors(Vector3 tilePosition)
    {
        var groundTileData = tiles[tilePosition];
        foreach (KeyValuePair<Vector3, GroundTileData> neighborTile in groundTileData.tileNeighbors)
        {
            int tempCounter = 0;
            foreach (KeyValuePair<Vector3, GroundTileData> tile in neighborTile.Value.tileNeighbors)
            {
                //if the neighboring tiles have not been explored
                if (!tile.Value.isExplored)
                {
                    tempCounter++;
                }
            }
            neighborTile.Value.tileValue = tempCounter;
            if (isDebugging)
                Debug.Log("The tile at " + neighborTile.Value.worldPosition + " has a new value of [" + neighborTile.Value.tileValue + "].");
        }
        CheckAllTilesIsExplored();
    }

    //Check if all tiles from the GroundTileMap isExplored
    public void CheckAllTilesIsExplored()
    {
        foreach (KeyValuePair<Vector3, GroundTileData> groundTile in tiles)
        {
            groundTile.Value.CheckisExplored();
        }
    }
  

    // Update is called once per frame
    void Update()
    {
        
    }
}
