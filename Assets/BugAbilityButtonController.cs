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
        
    }

    public void SetButtonTimers()
    {
        ability1.abiltyCooldownTimer = playerBugController.sendToPastCDTime;
    }

    public void UseAbility1()
    {
        playerBugController.SendToPast();
    }

}
