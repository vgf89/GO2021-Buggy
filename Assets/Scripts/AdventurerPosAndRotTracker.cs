//GO2021-Buggy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/* This script stores The Adventurer's position and rotation within timeSaved seconds
 * 
 * This will be used in the future for The Bug's [Send The Adventure Back In Iime] ability
 * */
public class AdventurerPosAndRotTracker : MonoBehaviour
{
    //A class that stores a Vector2 position and float rotation
    class PosAndRot
    {
        private Vector3 position;
        private float rotation;

        public PosAndRot(Vector3 pos, float rot)
        {
            position = pos;
            rotation = rot;
        }

        public Vector3 Position {
            get { return position; }
            set { position = value; }
            }

        public float Rotation {
            get { return rotation;}
            set { rotation = value;}
        }
    }

    [Tooltip("What game object you want to use to track the Adventurer")]
    public GameObject trackingGameObject;

    [Tooltip("How many seconds in the past that wants to be saved.\n MUST BE GREATER THAN 0")]
    [MinAttribute(0)]
    public int secondsSaved;

    
    [Tooltip("Place the Tilemap for Ground here.")]
    [SerializeField]
    private Tilemap groundTilemap;
    Queue<PosAndRot> savePosAndRotQueue;
    bool isTracking;
    

    [Tooltip("How much time that must pass before saving a Pos and Rot.\nUSUALLY SET TO 0.017 TO REPRESENT 1 FRAME MUST PASS BEFORE STORING.")]
    public float waitTimer = 0.017f;
    private float timer;

    [SerializeField]
    private bool drawGizmos;

    [Tooltip("Will display relevant console information from this script.")]
    [SerializeField]
    private bool isDebugging;

    // Start is called before the first frame update
    void Awake()
    {
        isTracking = false;
        //Instantiates Queue of PosAndRot objects
        savePosAndRotQueue = new Queue<PosAndRot>();
        isTracking = true;
    }

    // Update is called once per frame
    void Update()
    {
        PosAndRotQueueHandling();
    }

    void PosAndRotQueueHandling()
    {
        timer += Time.deltaTime;
        //If the secondsSaved is greater than 0, find The Adventurer's position and rotation and add it to the savePoseAndRotQueue
        if (timer > waitTimer)
        {
            Vector3Int currentGridPosition = groundTilemap.WorldToCell(trackingGameObject.transform.position);
            float currentRotation = trackingGameObject.transform.rotation.x;
            SavePositionToQueue(currentGridPosition, currentRotation);
            CheckPosAndRotQueue();
            timer = 0f;
        }
        
        
    }

    //Receives a Vector3 position relative to the grid and float rotation and that to savePosAndRotQueue
    void SavePositionToQueue(Vector3 position, float rotation)
    {
        PosAndRot temp = new PosAndRot(position, rotation);
        //Adds to queue
        savePosAndRotQueue.Enqueue(temp);
        if(isDebugging)
            Debug.Log(string.Format("Enqueing: {0} at {1}‹.", temp.Position, temp.Rotation));
        if(isDebugging)
            Debug.Log(string.Format("SavePosAndRot Queue Count: {0}", savePosAndRotQueue.Count));

    }

    //Double checks if at any moment there are too many PosAndRot in the queue 
    void CheckPosAndRotQueue()
    {
        if (savePosAndRotQueue.Count >= secondsSaved * 60)
        {
            savePosAndRotQueue.Dequeue();
        }
    }

    void OnDrawGizmos()
    {
        if (isTracking && drawGizmos)
        {
            Gizmos.color = Color.blue;
            Vector3 trailPosition = new Vector3 (savePosAndRotQueue.Peek().Position.x + 0.5f, savePosAndRotQueue.Peek().Position.y + 0.5f, -0.5f);
            //Vector3 trailPosition = Vector3.zero;
            Gizmos.DrawCube(trailPosition, new Vector3(1, 1, 0));
        }
    }

    public PosAndRot GetPastPosAndRot()
    {
        return savePosAndRotQueue.Dequeue();
    }
}
