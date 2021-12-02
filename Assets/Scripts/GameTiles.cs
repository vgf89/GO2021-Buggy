using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTiles : MonoBehaviour
{
    public const string GROUNDTILENAMESTRING = "Ground Tile";
    public const string WALLTILENAMESTRING = "Wall Tile";

    /*
    [Header("Exploration Algorithm Values")]
    [MinAttribute(0)]
    [Tooltip("The value to add to tileValue if the neighboring tile is a Ground Tile")]
    public float groundTileNeighborValue;
    [Tooltip("The value to add to tileValue if the neighboring tile is a Wall Tile")]
    public float wallTileNeighborValue;
    */

    //Function currently unknown
    public static GameTiles instance;

    [Header("")]
    [Tooltip("Insert the Walkable Ground Tilemap Here")]
    public Tilemap groundTileMap;
    [Tooltip("Insert the Not Walkable Wall Tilemap Here")]
    public Tilemap wallTileMap;

    //Dictionary of tileData with their Vector3 positions as the key
    public Dictionary<Vector3, TileData> tiles;

    public TileData[,] tilesArray;

    [Header("Inspector Debugging")]
    [Tooltip("Will display relevant console information from this script.")]
    [SerializeField]
    private bool isDebugging;

    [SerializeField]
    private Color unexploredTileColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    [SerializeField]
    private Color exploredTileColor = Color.white;
    
   

    void Awake()
    {
        if (instance == null)
            instance = this;
        /*else if (instance != this)
        {
            Destroy(gameObject);
        }*/

        //GetGridTilesInArray();
        tiles = new Dictionary<Vector3, TileData>();


        GetGridTilesInDictionary(groundTileMap, GROUNDTILENAMESTRING);
        SetAllUnexploredTileColors(groundTileMap);
        GetGridTilesInDictionary(wallTileMap, WALLTILENAMESTRING);
        SetGridTileNeighbors();
        GetAllTileValues();
    }

    //Adds all tiles from the Ground Tilemap and stores it into a 2D array. CURRENTLY NOT IN USE
    private void GetGridTilesInArray()
    {
        tilesArray = new TileData[groundTileMap.size.x, groundTileMap.size.y];

        foreach (Vector3Int pos in groundTileMap.cellBounds.allPositionsWithin)
        {
            var localPos = new Vector3Int(pos.x, pos.y, pos.z);

            if (!groundTileMap.HasTile(pos))
                continue;

            TileData tileData = ScriptableObject.CreateInstance<TileData>();

            tileData.localPosition = localPos;
            tileData.worldPosition = groundTileMap.CellToWorld(localPos);
            tileData.tilemapMember = groundTileMap;
            tileData.isExplored = false;
            tileData.tileValue = 0;

            tilesArray[pos.x, pos.y] = tileData;
        }
    }

    //Finds all tile from _tilemap and stores in the tile Dictionary. The key is the tile's world position. BE SURE TO SET THE NAME TO DISINGUISH BETWEEN WALL AND GROUND TILES
    private void GetGridTilesInDictionary(Tilemap _tileMap, string _tileName)
    {
        //Gets all tiles and adds them to the tiles Dictionary
        foreach (Vector3Int pos in _tileMap.cellBounds.allPositionsWithin)
        {
            var localPos = new Vector3Int(pos.x, pos.y, pos.z);

            if (!_tileMap.HasTile(localPos))
                continue;

            var tile = ScriptableObject.CreateInstance<TileData>();
            tile.tileName = _tileName;
            tile.localPosition = localPos;
            tile.worldPosition = _tileMap.CellToWorld(localPos);
            tile.tilemapMember = _tileMap;
            tile.isExplored = false;
            tile.tileNeighbors = new Dictionary<Vector3, TileData>();
            tile.tileValue = 0f;
            tile.efficiencyScore = 0f;

            //Adds the tile with the key being the tile's position in the world
            tiles.Add(tile.worldPosition, tile);
            //Debug.Log("Tile from " + tile.worldPosition + " has been added to the tiles Dictionary.");
        }
    }

    private void SetAllUnexploredTileColors(Tilemap _tileMap) {
        foreach (Vector3Int pos in _tileMap.cellBounds.allPositionsWithin) {
            if (_tileMap.HasTile(pos)) {
                ColorUnexploredTile(pos);
            }
        }            
    }


    

    //After all tiles have their TileData, find their neighbor and add to the tileNeighbors dictionary
    private void SetGridTileNeighbors()
    {
        var tileData = ScriptableObject.CreateInstance<TileData>();
        Vector3 checkingPosition = new Vector3();

        foreach (KeyValuePair<Vector3, TileData> tile in tiles)
        {
            for (int x = -1; x <= 1; x++)
            {

                for (int y = -1; y <= 1; y++)
                {
                    checkingPosition = tile.Value.worldPosition + new Vector3(x, y, 0);
                    //If the value is itself, DO NOT ADD AS NEIGHBOR
                    if (tile.Value.worldPosition == checkingPosition)
                    {
                        //continue;
                    }
                    else if (tiles.TryGetValue(checkingPosition, out tileData))
                    {
                        tile.Value.tileNeighbors.Add(checkingPosition, tiles[checkingPosition]);
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

        foreach (KeyValuePair<Vector3, TileData> _tileData in tiles)
        {
            _tileData.Value.GetTileValue();
            
            if (isDebugging)
                Debug.Log(_tileData.Value.printData());
        }
    }

    //Mostly for tiles that have been explored. Find the tileData and adjust the tileNeighbor's tileValue.
    public void GetTileValueOfNeighbors(Vector3 tilePosition)
    {
        var tileData = tiles[tilePosition];
        //Look at the this tile's neighbors
        foreach (KeyValuePair<Vector3, TileData> neighborTile in tileData.tileNeighbors)
        {
            //Go through all the neigboring tiles of this neighbor to determine the tileValue if it is a groundTile
            if (neighborTile.Value.tileName.Equals(GROUNDTILENAMESTRING))
            {
                neighborTile.Value.GetTileValue();
                /*
                float tempCounter = 0;
                foreach (KeyValuePair<Vector3, TileData> tile in neighborTile.Value.tileNeighbors)
                {
                    //if the neighboring ground tiles have not been explored
                    if ((!tile.Value.isExplored) && tile.Value.tileName.Equals(GROUNDTILENAMESTRING))
                        tempCounter += TileData.GroundTileNeighborValue;
                    else if (tile.Value.tileName.Equals(WALLTILENAMESTRING)) //If the neighborTile is a Wall
                        tempCounter += TileData.WallTileNeighborValue;
                }
                neighborTile.Value.tileValue = tempCounter;
                */
            }
                
            if (isDebugging)
                Debug.Log("The tile at " + neighborTile.Value.worldPosition + " has a new value of [" + neighborTile.Value.tileValue + "].");
        }
        CheckAllTilesIsExplored();
    }

    //Check if all tiles from the GroundTileMap isExplored, if so, set the tileValue to 0
    public void CheckAllTilesIsExplored()
    {
        foreach (KeyValuePair<Vector3, TileData> groundTile in tiles)
        {
            groundTile.Value.CheckisExplored();
        }
    }

    //Check if all ground tiles in the dictionary have been explored
    public bool AllTilesExplored()
    {
        foreach (KeyValuePair<Vector3, TileData> groundTile in tiles)
        {
            if (groundTile.Value.tileName.Equals(GROUNDTILENAMESTRING))
                if (!groundTile.Value.isExplored)
                    return false;
        }
        return true;
    }

    public void ResetVisiting()
    {
        foreach (KeyValuePair<Vector3, TileData> groundTile in tiles)
        {
            groundTile.Value.isVisited = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void ColorExploredTile(TileData tile) {
        ColorTile(tile, exploredTileColor);
    }

    public void ColorUnexploredTile(TileData tile) {
        ColorTile(tile, unexploredTileColor);
    }

    public void ColorExploredTile(Vector3Int pos) {
        ColorTile(pos, exploredTileColor);
    }

    public void ColorUnexploredTile(Vector3Int pos) {
        ColorTile(pos, unexploredTileColor);
    }

    public void ColorTile(TileData tile, Color color)
    {
        Vector3Int tilePos = Vector3Int.FloorToInt(tile.worldPosition);
        ColorTile(tilePos, color);

    }

    public void ColorTile(Vector3Int tilePos, Color color) {
        if (groundTileMap.HasTile(tilePos)) {
            groundTileMap.SetTileFlags(tilePos, TileFlags.None);
            groundTileMap.SetColor(tilePos, color);
        }
    }
}
