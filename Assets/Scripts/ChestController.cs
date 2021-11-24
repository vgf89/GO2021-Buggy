using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public Animator chestAnimator;

    public bool isDiscovered;
    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        //For prefab instantiation
        chestAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //chestAnimator.SetBool("Open" , false);
    }

    void OpenChest()
    {
        if (!isOpen)
        {
            chestAnimator.SetBool("Open", true);
            isOpen = true;
            Debug.Log(chestAnimator.runtimeAnimatorController);
        }
    }

    void CloseChest()
    {
        if (isOpen)
        {
            chestAnimator.SetBool("Open", false);
            isOpen = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.CompareTag("Adventurer"))
        {
            if (collision.GetComponent<AdventurerController>().keysCount > 0)
            {
                OpenChest();
                collision.GetComponent<AdventurerController>().keysCount -= 1;
            }
        }
    }
}
