using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Place the Adventurer GameObject here")]
    public Transform adventurerTransform;

    [Tooltip("If following the target, how many seconds until it arrives.")]
    [MinAttribute(0f)]
    public float cameraArrivalTime = 0.25f;

    private Vector3 cameraVelocity = Vector3.zero;

    [Tooltip("Camera offset value from Adventurer about the z-axis")]
    public float cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destinationPosition = adventurerTransform.position + new Vector3(0,0,cameraOffset);
        transform.position = Vector3.SmoothDamp(transform.position, destinationPosition, ref cameraVelocity, cameraArrivalTime);
    }
}
