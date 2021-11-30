using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBugController : MonoBehaviour
{
    [Tooltip("Place the Adventurer's transform here.")]
    [SerializeField]
    private Transform adventurerTransform;

    

    [Tooltip("The speed which the camera target transform moves.")]
    [MinAttribute(0f)]
    public float scrollSpeed;

    [Header("Cooldown Timers")]
    [Tooltip("The cooldown(in seconds) it takes to send the adventurer in the past.")]
    [MinAttribute(0.1f)]
    public float sendToPastCDTime;
    [Tooltip("How many seconds in the past the Adventurer will be sent.")]
    [MinAttribute(1)]
    public int secondsInPast;
    public float sendToPastTimer;

    [Tooltip("The cooldown(in seconds) it takes to manipulate the world speed.")]
    [MinAttribute(0.1f)]
    public float manipulateTimeCDTime;
    [Tooltip("The duration(in seconds) of manipulate the world speed glitch.")]
    [MinAttribute(0.1f)]
    public float manipulateTimeDuration;
    public float manipulateWorldTimer;

    public bool isChoosingSpawner;
    [Tooltip("The cooldown(in seconds) it takes to affect a spawner.")]
    [MinAttribute(0.1f)]
    public float affectSpawnerCDTime;
    public float affectSpawnerTimer;
    public int affectedSpawnCountRandomMax;

    [Header("Frustration Values")]
    public int sentToPastFrustrationFlat;
    [Tooltip("This rate is relative to the duration of the Manipulate World Speed Glitch")]
    public float timeManipulationFrustrationRate;

    public enum cameraTargetMode
    {
        TrackingAdventuerer,
        Unlocked,
        DoNothing
    }

    [Header("")]
    public cameraTargetMode targetMode;

    [SerializeField]
    private Tilemap wallTileMap;

    private Vector3 trackingObjectTransform;
    private AdventurerPosAndRotTracker advPosAndRotTracker;

    private GameWorldSpeedController gWorldController;

    private AdventurerFrustrationTracker frustrationTracker;

    [Header("Inspector Debugging")]
    [SerializeField]
    private bool isDebugging;

    // Start is called before the first frame update
    void Start()
    {
        if (adventurerTransform == null)
            adventurerTransform = GameObject.Find("Adventurer").transform;

        advPosAndRotTracker = GetComponent<AdventurerPosAndRotTracker>();
        gWorldController = Object.FindObjectOfType<GameWorldSpeedController>();
        frustrationTracker = Object.FindObjectOfType<AdventurerFrustrationTracker>();

        if (isDebugging)
        {
            Debug.Log("Adventurer Transform has been set: " + (adventurerTransform != null) + " to " + this.name);
            Debug.Log("Wall Tile Map has been set: " + (wallTileMap != null) + " to " + this.name);
        }

        if (manipulateTimeDuration > manipulateTimeCDTime)
            Debug.LogError("Please make the manipulateTimeCDTime longer than the manipulateTimeDuration");

    }

    // Update is called once per frame
    void Update()
    {
        SetTimers();
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
        GlitchHandling();
        if (isDebugging)
            DebugGlitchAbilityTimers();
    }

    private void SetTimers()
    {
        sendToPastTimer = Mathf.Clamp(sendToPastTimer, 0, sendToPastCDTime);
        manipulateWorldTimer = Mathf.Clamp(manipulateWorldTimer, 0, manipulateTimeCDTime);
        affectSpawnerTimer = Mathf.Clamp(affectSpawnerTimer, 0, affectSpawnerCDTime); 

        sendToPastTimer += Time.deltaTime;
        manipulateWorldTimer += Time.deltaTime;
        affectSpawnerTimer += Time.deltaTime;
    }


    private void TrackingAdventurerHandling()
    {
        transform.position = adventurerTransform.position;
        trackingObjectTransform = transform.position;

        if (Input.GetKeyDown(KeyCode.Space) || 
            Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
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

        UnlockedMovementHandlingInput(cameraTargetGridPos);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetMode = cameraTargetMode.TrackingAdventuerer;
            if (isDebugging)
                Debug.Log(KeyCode.Space.ToString() + " was pressed. Switching to " + targetMode.ToString());
        }

        transform.position = trackingObjectTransform;
    }

    void UnlockedMovementHandlingInput(Vector3Int gridPos)
    {
        //Horizontal Input
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            trackingObjectTransform.x -= scrollSpeed;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            trackingObjectTransform.x += scrollSpeed;

        trackingObjectTransform.x = Mathf.Clamp(trackingObjectTransform.x, wallTileMap.cellBounds.xMin, wallTileMap.cellBounds.xMax);

        //Vertical Input
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            trackingObjectTransform.y -= scrollSpeed;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            trackingObjectTransform.y += scrollSpeed;

        trackingObjectTransform.y = Mathf.Clamp(trackingObjectTransform.y, wallTileMap.cellBounds.yMin, wallTileMap.cellBounds.yMax);
    }

    void GlitchHandling()
    {
        if (!PauseMenu.isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SendToPast();
            if (Input.GetKeyDown(KeyCode.Alpha2))
                ManipulateTime();
            if (Input.GetKeyDown(KeyCode.Alpha3) && affectSpawnerTimer >= affectSpawnerCDTime)
                isChoosingSpawner = true;
                
            if (isChoosingSpawner)
            {
                GetComponent<BugAbilityButtonController>().ability3.SetButtonText("Select a Spawner");
                AffectSpawner();
            }
            else
                GetComponent<BugAbilityButtonController>().ability3.SetButtonText("Spawn More Enemies");
        }
    }
    #region GlitchPowers

    public bool SendToPast()
    {
        if (sendToPastTimer >= sendToPastCDTime)
        {
            AdventurerPosAndRotTracker.PosAndRot tempPosAndRot = SetAdventurerPosAndRot();
            Vector3 sendToPastPosition = tempPosAndRot.Position;
            Vector3 sendToPastRotation = new Vector3(tempPosAndRot.Rotation, 0, 0);
            adventurerTransform.position = sendToPastPosition;
            adventurerTransform.eulerAngles = sendToPastRotation;
            if (isDebugging)
                Debug.Log("Sending Adventurer into the past to " + sendToPastPosition.ToString());
            adventurerTransform.GetComponent<AdventurerAIBehavior>().RestartThinking();
            //Add frustrationi
            frustrationTracker.AddFrustrationFlat(sentToPastFrustrationFlat);
            sendToPastTimer = 0;
            return true;
        }
        else
            return false;
    }

    public bool ManipulateTime()
    {
        if (manipulateWorldTimer >= manipulateTimeCDTime)
        {
            gWorldController.ChangeWorldSpeed();
            frustrationTracker.AddFrustrationRate(timeManipulationFrustrationRate, manipulateTimeDuration);
            manipulateWorldTimer = 0;
            return true;
        }
        return false;
    } 

    public bool AffectSpawner()
    {
        if (affectSpawnerTimer >= affectSpawnerCDTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    //If spawner was clicked
                    if (hit.transform.name.Contains("Spawner"))
                    {
                        int randomSpawnCount = Random.Range(2, affectedSpawnCountRandomMax + 1);
                        hit.transform.GetComponent<Spawner>().AffectedSpawnGlitch(randomSpawnCount);
                        affectSpawnerTimer = 0;
                        return true;
                    }
                }
                
                isChoosingSpawner = false;
            }
        }
        else
        {
            isChoosingSpawner = false;
        }
        return false;
    }


    #endregion

    AdventurerPosAndRotTracker.PosAndRot SetAdventurerPosAndRot()
    {
        return advPosAndRotTracker.GetPastPosAndRot();
    }

    void DebugGlitchAbilityTimers()
    {
        if (sendToPastTimer >= sendToPastCDTime)
            Debug.Log("Send Adventurer into the past is ready");
        if (manipulateWorldTimer >= manipulateTimeCDTime)
            Debug.Log("Lag Glitch is ready");
        if (affectSpawnerTimer >= affectSpawnerCDTime)
            Debug.Log("Affect Spawner is ready");
    }
}
