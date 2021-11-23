using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraTargetContoller : MonoBehaviour
{
    [Tooltip("Place the Adventurer's transform here.")]
    [SerializeField]
    private Transform adventurerTransform;

    [SerializeField]
    private bool isDebugging;

    [Tooltip("The speed which the camera target transform moves.")]
    [MinAttribute(0f)]
    public float scrollSpeed;
    public enum cameraTargetMode {
        TrackingAdventuerer,
        Unlocked,
        DoNothing
    }

    public cameraTargetMode targetMode;

    [SerializeField]
    private Tilemap wallTileMap;

    private Vector3 trackingObjectTransform;
    // Start is called before the first frame update
    void Start()
    {
        if (adventurerTransform == null)
            adventurerTransform = GameObject.Find("Adventurer").transform;


        if (isDebugging)
        {
            Debug.Log("Adventurer Transform has been set: " + (adventurerTransform != null) + " to " + this.name);
            Debug.Log("Wall Tile Map has been set: " + (wallTileMap != null) + " to " + this.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (targetMode)
        {
            case cameraTargetMode.DoNothing:
                break;

            case cameraTargetMode.TrackingAdventuerer:
                TrackingAdventurerHandling();
                break;

            //Use arrow keys or mouse to have the CameraTarget transform move
            case cameraTargetMode.Unlocked:
                UnlockedHandling();
                break;
        }

    }

    private void TrackingAdventurerHandling()
    {
        transform.position = adventurerTransform.position;
        trackingObjectTransform = transform.position;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            targetMode = cameraTargetMode.Unlocked;
            if (isDebugging)
                Debug.Log("Switching to " + targetMode.ToString());
        }
        
    }

    private void UnlockedHandling()
    {
        Vector3Int cameraTargetGridPos = Vector3Int.FloorToInt(transform.position);
        cameraTargetGridPos.z = 0;

        ArrowKeyInput(cameraTargetGridPos);

        if (Input.GetKeyDown("space"))
        {
            targetMode = cameraTargetMode.TrackingAdventuerer;
            if (isDebugging)
                Debug.Log(KeyCode.Space.ToString() + " was pressed. Switching to " + targetMode.ToString());
        }

        transform.position = trackingObjectTransform;
    }

    void ArrowKeyInput(Vector3Int gridPos)
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (gridPos.x > wallTileMap.cellBounds.xMin)
            {
                trackingObjectTransform.x -= scrollSpeed;
                if (isDebugging)
                    Debug.Log(KeyCode.LeftArrow.ToString() + " is being pressed.");
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (gridPos.x < wallTileMap.cellBounds.xMax)
            {
                trackingObjectTransform.x += scrollSpeed;
                if (isDebugging)
                    Debug.Log(KeyCode.RightArrow.ToString() + " is being pressed.");
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (gridPos.y > wallTileMap.cellBounds.yMin)
            {
                trackingObjectTransform.y -= scrollSpeed;
                if (isDebugging)
                    Debug.Log(KeyCode.DownArrow.ToString() + " is being pressed.");
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (gridPos.y < wallTileMap.cellBounds.yMax)
            {
                trackingObjectTransform.y += scrollSpeed;
                if (isDebugging)
                    Debug.Log(KeyCode.UpArrow.ToString() + " is being pressed.");
            }
        }
    }

}
