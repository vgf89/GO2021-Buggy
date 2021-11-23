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
    }

    //Displays TileData in console
    void DisplayTileInfo()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var worldPoint = new Vector3(Mathf.FloorToInt(mousePosition.x), Mathf.FloorToInt(mousePosition.y), 0);

        //calls the dictionary of tiles
        var tilesDictionary = GameTiles.instance.tiles;


        if (tilesDictionary.TryGetValue(worldPoint, out groundTileData))
        {
            Debug.Log(groundTileData.printData());
        }
    }
}
