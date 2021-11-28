using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AdventurerFrustrationTracker : MonoBehaviour
{
    [Header("Frustation Timer")]
    public float maxFrustration;
    public static float currentFrustation;
    [MinAttribute(1)]
    public float decreasingFillRate;

    public FrustrationBarUI frustrationUI;

    private float decreasingTimer;
    private float increasingFillRate;
    private float increasingFillRateTimer;
    private float increasingFillRateDuration;
    private bool isFilling;
    //private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FillBarHandling();

    }

    private void FillBarHandling()
    {
        currentFrustation = Mathf.Clamp(currentFrustation, 0, maxFrustration);

        if (isFilling)
        {
            increasingFillRateTimer += Time.deltaTime;
            if (increasingFillRateTimer > increasingFillRateDuration)
            {
                increasingFillRate = 0;
                increasingFillRateTimer = 0;
                isFilling = false;
            }
        }

        currentFrustation += Time.deltaTime * ((-decreasingFillRate) + increasingFillRate);
        frustrationUI.SetFrustration(currentFrustation);
    }

    public void AddFrustrationFlat (float value)
    {
        currentFrustation += value;
        frustrationUI.SetFrustration(currentFrustation);
    }

    public void AddFrustrationRate (float rate, float seconds)
    {
        increasingFillRate = rate;
        increasingFillRateDuration = seconds;
        increasingFillRateTimer = 0;
        isFilling = true;
    }

}
