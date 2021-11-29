using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugAbilityButtonController : MonoBehaviour
{

    private PlayerBugController playerBugController;

    public AbilityButton ability1, ability2, ability3, ability4, ability5;

    void Awake()
    {
        playerBugController = GetComponent<PlayerBugController>();
        SetButtonCDTimers();
    }

    // Update is called once per frame
    void Update()
    {
        SetButtonFullBars();
    }

    public void SetButtonCDTimers()
    {
        ability1.abiltyCooldownTimer = playerBugController.sendToPastCDTime;
        ability2.abiltyCooldownTimer = playerBugController.manipulateTimeCDTime;
        ability3.abiltyCooldownTimer = playerBugController.affectSpawnerCDTime;
        ability4.abiltyCooldownTimer = 1;
        ability5.abiltyCooldownTimer = 1;
    }

    public void SetButtonFullBars()
    {
        ability1.currentFillBar = playerBugController.sendToPastTimer;
        ability2.currentFillBar = playerBugController.manipulateWorldTimer;
        ability3.currentFillBar = playerBugController.affectSpawnerTimer;
    }

    public void UseAbility1()
    {
        if (playerBugController.SendToPast())
            ability1.UseAbility();
    }

    public void UseAbility2()
    {
        if (playerBugController.ManipulateTime())
            ability2.UseAbility();
    }

    public void UseAbility3()
    {
        if (ability3.currentFillBar >= ability3.abiltyCooldownTimer)
        {
            //ability3.SetButtonText("Select Spawner");
            playerBugController.isChoosingSpawner = true;
        }
        /*if (playerBugController.AffectSpawner())
            ability3.UseAbility();*/
    }
}
