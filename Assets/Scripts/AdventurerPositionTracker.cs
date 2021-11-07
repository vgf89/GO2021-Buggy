using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerPositionTrackerScript : MonoBehaviour
{
    class PosAndRot
    {
        Vector2 position;
        float rotation;

        public PosAndRot(Vector2 pos, float rot)
        {
            position = pos;
            rotation = rot;
        }

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }
    }


    public GameObject trackingGameObject;

    [Tooltip("How many seconds in the past that wants to be saved")]
    public int secondsSaved;
    Queue<PosAndRot> savePosAndRotQueue;

    // Start is called before the first frame update
    void Start()
    {
        //Search for Adventurer GameObjects???

        //Instantiates an array of PosAndRot objects
        savePosAndRotQueue = new Queue<PosAndRot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SavePositionToQueue(Vector2 position, float rotation)
    {
        PosAndRot temp = new PosAndRot(position, rotation);

        //Adds to queue
        savePosAndRotQueue.Enqueue(temp);
        Debug.Log(string.Format("Enqueing: {0} : {1}‹.", temp.Position, temp.Rotation));
        savePosAndRotQueue.Dequeue();
    }

    void CheckPosAndRotQueue()
    {
        if(savePosAndRotQueue.Count > secondsSaved)
        {
            savePosAndRotQueue.Dequeue();
        }
    }

}
