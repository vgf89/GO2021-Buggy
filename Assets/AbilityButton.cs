using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Slider fillBar;
    public float currentFillBar;


    public float timer;
    public float abiltyCooldownTimer;

    public PlayerBugController playerContoller;

    // Start is called before the first frame update
    void Start()
    {
        fillBar.maxValue = abiltyCooldownTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        currentFillBar = timer;
        timer = Mathf.Clamp(timer, 0, abiltyCooldownTimer);
        SetFillButton(currentFillBar);

    } 
    

    public void SetFillButton(float currentFill)
    {
        fillBar.value = currentFill;
    }

    public void UseAbility()
    {
        if (currentFillBar >= abiltyCooldownTimer)
        {
            Debug.Log("Ability Used");
            timer = 0;
        }
    }

    public void UseSendBackInTimeAbility ()
    {
        playerContoller.SendToPast();
    }
}
