using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;

    [Tooltip("Place the Adventurer GameObject here")]
    public Transform adventurerTransform;
    

    private Vector3 cameraVelocity = Vector3.zero;

    [Tooltip("Camera offset value from Adventurer about the z-axis")]
    public float cameraOffset;
    [Tooltip("If following the target, how many seconds until it arrives.")]
    [MinAttribute(0f)]
    public float cameraArrivalTime = 0.25f;

    [Header("Camera Zoom")]
    [SerializeField]
    [Tooltip("The rate at which the camera zooms relative to deltaTime [HIGHER VALUES RECOMMENDED]")]
    [MinAttribute(0f)]
    private float cameraZoomSpeed = 75f;
    [SerializeField]
    [MinAttribute(1.0f)]
    private float cameraMinZoom = 3f;
    [SerializeField]
    [MinAttribute(1.0f)]
    private float cameraMaxZoom = 6f;
    private float currentZoom;

    // Start is called before the first frame update
    void Start()
    {
        currentZoom = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destinationPosition = adventurerTransform.position + new Vector3(0,0,cameraOffset);
        transform.position = Vector3.SmoothDamp(transform.position, destinationPosition, ref cameraVelocity, cameraArrivalTime);
        HandleCameraZoom();
    }

    void HandleCameraZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
            currentZoom -= cameraZoomSpeed * Time.deltaTime;

        if (Input.mouseScrollDelta.y < 0)
            currentZoom += cameraZoomSpeed * Time.deltaTime;

        currentZoom = Mathf.Clamp(currentZoom, cameraMinZoom, cameraMaxZoom);
        camera.orthographicSize = Mathf.Pow(currentZoom, 2);
    }
}
