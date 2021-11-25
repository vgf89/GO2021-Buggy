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

    [SerializeField]
    private GameTiles gameTiles;

    [SerializeField]
    private List<GameObject> allChestsList;
    [SerializeField]
    private List<GameObject> allKeysList;

    [SerializeField]
    private int openChestCount;

    public enum conditionsOfTheWoeld
    { 
        AllChestsHaveBeenOpened,
        AllChestsHaveBeenDiscovered,
        AllTilesHaveBeenExplored,
        AdventurerIsExploring
    }

    [Header("Current condition of the world.")]
    public conditionsOfTheWoeld condition;


    void Awake()
    {
        if (gameTiles == null)
            Debug.LogError("Please assign Game Tiles Script to " + this.GetType().Name);
        GameObject[] chestGameObjects, keyGameObjects;
        chestGameObjects = GameObject.FindGameObjectsWithTag("Chest");
        keyGameObjects = GameObject.FindGameObjectsWithTag("Key");

        foreach(GameObject go in chestGameObjects)
        {
            allChestsList.Add(go);
        }

        foreach (GameObject go in keyGameObjects)
        {
            allKeysList.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MouseHandling();

        CheckChestOpen();
        CheckConditionOfTheWorld();
    }

    private void MouseHandling()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DisplayTileInfo();
        }
                        
    }

    //Displays TileData of the right-clicked tile on the console
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

    void CheckChestOpen()
    {
        int count = 0;
        foreach (GameObject go in allChestsList)
        {
            if (go.GetComponent<ChestController>().isOpen)
                count++;
        }
        openChestCount = count;
    }
    
    void CheckConditionOfTheWorld()
    {
        if (openChestCount == allChestsList.Count)
            condition = conditionsOfTheWoeld.AllChestsHaveBeenOpened;
        else if (ExploredChestCount() == allChestsList.Count)
            condition = conditionsOfTheWoeld.AllChestsHaveBeenDiscovered;
        else if (gameTiles.AllTilesExplored())
            condition = conditionsOfTheWoeld.AllTilesHaveBeenExplored;
        else
            condition = conditionsOfTheWoeld.AdventurerIsExploring;
    }

    int ExploredChestCount()
    {
        int count = 0;
        foreach(GameObject go in allChestsList)
        {
            if(go.GetComponent<ChestController>().isDiscovered)
            {
                count++;
            }
        }
        return count;
    }
}
