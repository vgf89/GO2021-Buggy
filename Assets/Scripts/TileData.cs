using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData : ScriptableObject
{
    //Position to the grid
    public Vector3Int localPosition { get; set; }

    //Position in the world
    public Vector3 worldPosition { get; set; }

    public TileBase tileBase { get; set; }

    public Tilemap tilemapMember { get; set; }

    public string tileName { get; set; }

    //Important Data for Searching

    public bool isExplored { get; set; }
    //For Queue visiting
    public bool isVisited { get; set; }

    //Neighboring Tiles
    public Dictionary<Vector3, TileData> tileNeighbors;

    public float tileValue { get; set; }
    public float efficiencyScore { get; set; }

    public const string GROUNDTILENAMESTRING = "Ground Tile";
    public const string WALLTILENAMESTRING = "Wall Tile";

    public const float GroundTileNeighborValue = 1f;
    public const float WallTileNeighborValue = 1.4f;


    public string printData()
    {
        string print = string.Format("TileData: " +
            "\ntileName {0}" +
            "\nlocalPosition {1}" +
            "\nworldPosition {2}" +
            "\nisExplored [{3}]" +
            "\nhas [{4}] neighbors" +
            "\ntileValue = {5}" +
            "\nisVisited [{6}]", 
            tileName, localPosition, worldPosition, isExplored, tileNeighbors.Count, tileValue, isVisited);
        return print;
    }

    public bool CheckisExplored()
    {
        if (isExplored || tileName.Equals(WALLTILENAMESTRING))
        {
            tileValue = 0;
            efficiencyScore = 0f;
        }
        return isExplored;
    }

    //Tile Value is based on its neighbors
    public void GetTileValue()
    {
        if (tileName.Equals(WALLTILENAMESTRING))
        {
            tileValue = 0;
            return;
        }
        else
        {
            float temp = 0f;
            foreach (KeyValuePair<Vector3, TileData> neighborTileData in tileNeighbors)
            {
                
                if (!isExplored)
                {
                    if (neighborTileData.Value.tileName.Equals(GROUNDTILENAMESTRING))
                        temp += GroundTileNeighborValue;
                    else if (neighborTileData.Value.tileName.Equals(WALLTILENAMESTRING))
                        temp += WallTileNeighborValue;
                }
                
            }
            tileValue = temp;
        }
    }

    public void SetTileToExplored()
    {
        isExplored = true;

        //If this ground tile is explored, reduce the neighbor's tileValue.
        foreach (KeyValuePair<Vector3, TileData> tileData in tileNeighbors)
        {
            if (!isExplored && tileName.Equals(GROUNDTILENAMESTRING))
                tileData.Value.tileValue -= GroundTileNeighborValue;
        }
    }
}
