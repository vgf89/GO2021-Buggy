using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawn;
    [SerializeField] private GameObject entitiesParent;
    [SerializeField] private int maxSpawns =  10;
    [ReadOnly][SerializeField]private int spawnCount = 0;
    [SerializeField] float spawnDelay = 5f; // How long to wait between spawns
    [ReadOnly][SerializeField]private float lastSpawnTime = 0f;
    [SerializeField] float maximumAdventurerDistance = 10f; // Only spawn when adventurer is within this radius

    // Update is called once per frame
    void Update()
    {
        if (spawnCount >= maxSpawns) {
            return;
        }
        float proximity = Vector2.Distance(GameObject.FindGameObjectWithTag("Adventurer").transform.position, transform.position);
        if (Time.time >= lastSpawnTime + spawnDelay && proximity < maximumAdventurerDistance) {
            GameObject newObject = Instantiate(spawn, transform.position, transform.rotation);
            newObject.transform.SetParent(entitiesParent.transform);
            lastSpawnTime = Time.time;
            spawnCount++;
        }
    }

    //Referenced from the PlayerBugController as a glitch
    public void AffectedSpawnGlitch(int _spawnCount)
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            GameObject newObject = Instantiate(spawn, entitiesParent.transform);
            newObject.transform.position = transform.position;
        }
    }
}
