using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Slider fillBar;
    public float currentFillBar;
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
        currentFillBar = Mathf.Clamp(currentFillBar, 0, abiltyCooldownTimer);
        SetFillButton(currentFillBar);
    } 
    

    public void SetFillButton(float currentFill)
    {
        fillBar.value = currentFill;
        currentFillBar = currentFill;
    }

    public void UseAbility()
    {
        if (currentFillBar >= abiltyCooldownTimer)
        {
            fillBar.value = 0;
            currentFillBar = 0;
        }
    }

}
