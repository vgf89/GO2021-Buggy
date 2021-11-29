using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityButton : MonoBehaviour
{
    public Slider fillBar;
    public float currentFillBar;
    public float abiltyCooldownTimer;
    public PlayerBugController playerContoller;
    [SerializeField] private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        //fillBar = GetComponentInChildren<Slider>();
        fillBar.maxValue = abiltyCooldownTimer;
        text = GetComponentInChildren<TextMeshProUGUI>();
        if (text == null)
            Debug.LogError(name + " could find text.");
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

    public void SetButtonText(string s)
    {
        text.text = s;
    }

}
