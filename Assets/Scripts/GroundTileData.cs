using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundTileData : ScriptableObject
{
    public Vector3Int localPosition { get; set; }

    public Vector3 worldPosition { get; set; }

    public TileBase tileBase { get; set; }

    public Tilemap tilemapMember { get; set; }

    public string tileName { get; set; }

    //Important Data for Searching

    public bool isExplored { get; set; }

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

}
