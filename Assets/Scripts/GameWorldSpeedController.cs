using UnityEngine;

public class GameWorldSpeedController : MonoBehaviour
{
    [SerializeField]
    [MinAttribute(0f)]
    public static float worldSpeedMultiplier;
    public static bool worldSpeedIsChanged;

    public float originalWorldSpeed = 1;
    [MinAttribute(0f)]
    public float manipulatedWorldSpeed = 0.5f;

    [SerializeField] Animator[] worldAnimations;

    // Start is called before the first frame update
    void Start()
    {
        worldAnimations = Object.FindObjectsOfType<Animator>();
        worldSpeedIsChanged = false;
        worldSpeedMultiplier = originalWorldSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        worldAnimations = Object.FindObjectsOfType<Animator>();
    }

    public void ChangeWorldSpeed()
    {
        worldSpeedIsChanged = !worldSpeedIsChanged;

        if (worldSpeedIsChanged)
        {
            worldSpeedMultiplier = manipulatedWorldSpeed;
        }
        else
            worldSpeedMultiplier = originalWorldSpeed;

        ChangeWorldAnimations();
    }

    void ChangeWorldAnimations()
    {
        foreach (Animator a in worldAnimations)
        {
            a.speed = 1 * worldSpeedMultiplier;
        }
    }
}
