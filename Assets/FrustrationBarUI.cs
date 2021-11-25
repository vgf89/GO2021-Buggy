using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrustrationBarUI : MonoBehaviour
{
    public float timer;
    public float waitTime;
    public float fillRate = 5f;


    public int maxFrustration = 100;
    public int currentFrustration;

    [SerializeField]
    private Slider fillBar;



    // Start is called before the first frame update
    void Start()
    {
        fillBar.maxValue = maxFrustration;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (timer > waitTime)
            {
                SetMinFrustration();
                timer = 0f;
            }
        }
        SetFrustration(timer * fillRate);
    }

    public void SetFrustration(float frustration)
    {
        fillBar.value = frustration;
    }

    public void SetMaxFrustration(int maxFrustration)
    {
        fillBar.maxValue = maxFrustration;
    }

    public void SetMinFrustration()
    {
        fillBar.value = 0;
    }

}
