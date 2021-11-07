//GO2021-Buggy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script stores The Adventurer's position and rotation within timeSaved seconds
 * 
 * This will be used in the future for The Bug's [Send The Adventure Back In Iime] ability
 * */
public class AdventurerPosAndRotTracker : MonoBehaviour
{
    //A class that stores a Vector2 position and float rotation
    class PosAndRot
    {
        private Vector2 position;
        private float rotation;

        public PosAndRot(Vector2 pos, float rot)
        {
            position = pos;
            rotation = rot;
        }

        public Vector2 Position {
            get { return position; }
            set { position = value; }
            }

        public float Rotation {
            get { return rotation;}
            set { rotation = value;}
        }
    }


    public GameObject trackingGameObject;

    [Tooltip("How many seconds in the past that wants to be saved.\n MUST BE GREATER THAN 0")]
    [MinAttribute(0)]
    public int secondsSaved;
    public bool isDebugging;
    Queue<PosAndRot> savePosAndRotQueue;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiates Queue of PosAndRot objects
        savePosAndRotQueue = new Queue<PosAndRot>();
        if (secondsSaved <= 0)
        {
            secondsSaved = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PosAndRotQueueHandling();
    }

    void PosAndRotQueueHandling()
    {
        //If the secondsSaved is greater than 0, find The Adventurer's position and rotation and add it to the savePoseAndRotQueue
        if (secondsSaved > 0)
        {
            Vector2 currentVector2 = new Vector2(trackingGameObject.transform.position.x,
              trackingGameObject.transform.position.z);
            float currentRotation = trackingGameObject.transform.rotation.x;
            SavePositionToQueue(currentVector2, currentRotation);
            CheckPosAndRotQueue();
        }
    }

    //Receives a Vector2 position and float rotation and that to savePosAndRotQueue
    void SavePositionToQueue(Vector2 position, float rotation)
    {
        PosAndRot temp = new PosAndRot(position, rotation);
        Debug.Log("temp Position: " + temp.Position);
        Debug.Log("temp Rotation: " + temp.Rotation);
        //Adds to queue
        savePosAndRotQueue.Enqueue(temp);
        if(isDebugging)
            Debug.Log(string.Format("Enqueing: {0} at {1}‹.", temp.Position, temp.Rotation));
        if (savePosAndRotQueue.Count > secondsSaved)
            savePosAndRotQueue.Dequeue();
        if(isDebugging)
            Debug.Log(string.Format("SavePosAndRot Queue Count: {0}", savePosAndRotQueue.Count));
        
    }

    //Double checks if at any moment there are too many PosAndRot in the queue 
    void CheckPosAndRotQueue()
    {
        if (savePosAndRotQueue.Count > secondsSaved)
        {
            savePosAndRotQueue.Dequeue();
        }
    }

}
