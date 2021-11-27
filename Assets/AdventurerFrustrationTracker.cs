using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AdventurerFrustrationTracker : MonoBehaviour
{
    [Header("Frustation Timer")]
    public float maxFrustration;
    public float currentFrustation;
    [MinAttribute(1)]
    public float decreasingFillRate;

    public FrustrationBarUI frustrationUI;

    private float decreasingTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentFrustation = Mathf.Clamp(currentFrustation, 0, maxFrustration);


        currentFrustation -= Time.deltaTime * decreasingFillRate;
        frustrationUI.SetFrustration(currentFrustation);

        if (Input.GetKeyDown(KeyCode.T))
            AddFrustration(10);
    }

    public void AddFrustration (float value)
    {
        currentFrustation += value;
        frustrationUI.SetFrustration(currentFrustation);
    }
}
