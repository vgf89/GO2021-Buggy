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


    public bool displayTileDataToConsole;

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
        
        CheckPickedUpKeys();
        GetChestOpenCount();
        CheckConditionOfTheWorld();
        if (displayTileDataToConsole)
            MouseHandling();
    }

    //If any keys were picked up and destroyed, remove them from the allKeysList
    private void CheckPickedUpKeys()
    {
        for (int i = 0; i < allKeysList.Count; i++)
            if (allKeysList[i] == null)
                allKeysList.RemoveAt(i);
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

    void GetChestOpenCount()
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

    //TO DO: Create failsafe once All tiles have been explored
    public List<Vector3> GetAllNonDiscoveredObjectives (ref List<Vector3> _keyLocations)
    {
        List<Vector3> chestLocations = new List<Vector3>();

        foreach (GameObject chest in allChestsList)
        {
            if (!chest.GetComponent<ChestController>().isOpen)
                chestLocations.Add(Vector3Int.FloorToInt(chest.transform.position));
        }

        foreach(GameObject key in allKeysList)
        {
            if (!key.GetComponent<KeyController>().isDiscovered)
                _keyLocations.Add(Vector3Int.FloorToInt(key.transform.position));
        }
        return chestLocations;
    }

}
