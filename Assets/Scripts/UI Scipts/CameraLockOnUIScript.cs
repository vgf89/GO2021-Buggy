using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLockOnUIScript : MonoBehaviour
{
    public RawImage lockOnImage;

    public PlayerBugController bugController;

    // Start is called before the first frame update
    void Start()
    {
        if (lockOnImage == null)
            Debug.LogError("Please assign lock on image to " + this.name);

        bugController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBugController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(bugController.targetMode)
        {
            case PlayerBugController.cameraTargetMode.TrackingAdventuerer:
                lockOnImage.enabled = true;
                break;
            case PlayerBugController.cameraTargetMode.Unlocked:
                lockOnImage.enabled = false;
                break;
        }
    }

}
