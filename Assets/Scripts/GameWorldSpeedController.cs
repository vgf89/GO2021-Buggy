using System.Collections;
using System.Collections.Generic;
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

    private float abilityDurationTimer;

    [SerializeField] List<Animator> worldAnimators;

    PlayerBugController playerBugController;

    // Start is called before the first frame update
    void Awake()
    {
        playerBugController = FindObjectOfType<PlayerBugController>();
        worldSpeedIsChanged = false;
        worldSpeedMultiplier = originalWorldSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //worldAnimations = Object.FindObjectsOfType<Animator>();
        ChangeWorldAnimations();

        if (worldSpeedIsChanged)
        {
            abilityDurationTimer += Time.deltaTime;
        }

        if (abilityDurationTimer > playerBugController.manipulateTimeDuration)
            ResetWorldSpeed();

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
        foreach (Animator a in worldAnimators)
        {
            a.speed = 1 * worldSpeedMultiplier;
        }
    }

    public void ResetWorldSpeed()
    {
        worldSpeedMultiplier = originalWorldSpeed;
        worldSpeedIsChanged = false;
        abilityDurationTimer = 0;
    }

    public void AddAnimatorToWorldAnimatorList (Animator animator)
    {
        worldAnimators.Add(animator);
    }
    public void RemoveAnimatorToWorldAnimatorList(Animator animator)
    {
        worldAnimators.Remove(animator);
    }
}

