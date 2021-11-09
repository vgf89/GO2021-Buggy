using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundMap;

    [SerializeField]
    private List<WalkableTileData> walkableTileDatas;

    private Dictionary<TileBase, WalkableTileData> dataFromTiles;

    // Start is called before the first frame update
    void Start()
    {
        dataFromTiles = new Dictionary<TileBase, WalkableTileData>();
        foreach (var walkableTileData in walkableTileDatas)
        {
            foreach (var tile in walkableTileData.tiles)
            {
                dataFromTiles.Add(tile, walkableTileData);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mapGridPosition = groundMap.WorldToCell(mousePosition);

            //Find the tilebase from the area rightclicked
            TileBase rightClickedTile = groundMap.GetTile(mapGridPosition);
            if (rightClickedTile != null)
            {
                bool beenExplored = dataFromTiles[rightClickedTile].explored;
                Debug.Log(rightClickedTile.ToString() + " at " + mapGridPosition.ToString() + " has been explored: " + beenExplored);
            }
            else if (rightClickedTile == null)
                Debug.Log("Did not right click a walkable tile.");
        }

        if (Input.GetMouseButtonDown(2))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mapGridPosition = groundMap.WorldToCell(mousePosition);

            //Find the tilebase from the area rightclicked
            TileBase rightClickedTile = groundMap.GetTile(mapGridPosition);
            if (rightClickedTile != null)
            {
                dataFromTiles[rightClickedTile].explored = true;
                Debug.Log(rightClickedTile.ToString() + " at " + mapGridPosition.ToString() + " has been now been explored!");
            }
            else if (rightClickedTile == null)
                Debug.Log("Did not middle click a walkable tile.");
        }
    }
}
