using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundMap;


    [SerializeField]
    private TileData groundTileData;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        MouseHandling();
    }

    private void MouseHandling()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DisplayTileInfo();
        }

        if (Input.GetMouseButtonDown(2))
        {
            
        }

        
    }

    //Displays TileData in console
    void DisplayTileInfo()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var worldPoint = new Vector3(Mathf.FloorToInt(mousePosition.x), Mathf.FloorToInt(mousePosition.y), 0);

        //calls the dictionary of tiles
        var tilesDictionary = GameTiles.instance.tiles;

        /*
        //Find the tilebase from the world position that has been right clicked
        if (tilesDictionary.TryGetValue(worldPoint, out groundTileData))
        {
            groundTileData.isExplored = !groundTileData.isExplored;
            Debug.Log("Tile at " + groundTileData.worldPosition.ToString() + " set isExplored = " + groundTileData.isExplored);
        }*/

        if (tilesDictionary.TryGetValue(worldPoint, out groundTileData))
        {
            //Debug.Log("Tile at " + groundTileData.worldPosition.ToString() + " has [" + groundTileData.tileNeighbors.Count + "] neighbors and isExplored = " + groundTileData.isExplored);
            Debug.Log(groundTileData.printData());
        }
    }
}
