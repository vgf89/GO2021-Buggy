using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundTileData : ScriptableObject
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

    //Neighboring Tiles
    public Dictionary<Vector3, GroundTileData> tileNeighbors;

    public int tileValue { get; set; }

    /*
    public void GetTileValue()
    {
        int tempCounter = 0;
        foreach (KeyValuePair<Vector3, GroundTileData> groundTileData in tileNeighbors)
        {
            if (groundTileData.Value.isExplored)
                tempCounter++;
        }

        tileValue = tempCounter;
    }
    */

    public string printData()
    {
        string print = string.Format("TileData: " +
            "\nlocalPosition {0}" +
            "\nworldPosition {1}" +
            "\nisExplored [{2}]" +
            "\nhas [{3}] neighbors" +
            "\ntileValue = {4}", 
            localPosition, worldPosition, isExplored, tileNeighbors.Count, tileValue);
        return print;
    }

    public void CheckisExplored()
    {
        if (isExplored)
            tileValue = 0;
    }
}
