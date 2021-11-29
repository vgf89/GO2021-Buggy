using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugAbilityButtonController : MonoBehaviour
{

    private PlayerBugController playerBugController;

    public AbilityButton ability1, ability2, ability3, ability4, ability5;

    // Start is called before the first frame update
    void Start()
    {
        playerBugController = GetComponent<PlayerBugController>();
        SetButtonTimers();
    }

    // Update is called once per frame
    void Update()
    {
        ability1.currentFillBar = playerBugController.sendToPastTimer;
        ability2.currentFillBar = playerBugController.manipulateWorldTimer;
    }

    public void SetButtonTimers()
    {
        ability1.abiltyCooldownTimer = playerBugController.sendToPastCDTime;
        ability2.abiltyCooldownTimer = playerBugController.manipulateTimeCDTime;
    }

    public void UseAbility1()
    {
        if (playerBugController.SendToPast()) {
            ability1.UseAbility();
        }
    }

    public void UseAbility2()
    {
        if (playerBugController.ManipulateTime()) {
            ability2.UseAbility();
        }
    }

}
