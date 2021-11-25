using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Slider fillBar;
    public float fillBarMax = 100;
    public float currentFillBar;


    public float timer;
    public float fillRate = 5f;


    // Start is called before the first frame update
    void Start()
    {
        fillBar.maxValue = fillBarMax;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        currentFillBar = timer * fillRate;
        currentFillBar = Mathf.Clamp(currentFillBar, 0, fillBarMax);
        SetFillButton(currentFillBar);

    }

    public void SetFillButton(float currentFill)
    {
        fillBar.value = currentFill;
    }

    public void UseAbility()
    {
        if (currentFillBar >= fillBarMax)
        {
            Debug.Log("Ability Used");
            timer = 0;
        }
    }
}
