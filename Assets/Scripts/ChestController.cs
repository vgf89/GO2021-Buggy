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
            AudioManager.PlaySFX("chest");
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

        if(collision.gameObject.CompareTag("Adventurer"))
        {
            AdventurerController tempController = collision.gameObject.GetComponent<AdventurerController>();
            if (tempController.keysCount > 0)
            {
                for (int i = 0 ; i < tempController.discoveredChestPositionList.Count; i++)
                {
                    Vector3Int currentChestGridPos = Vector3Int.FloorToInt(gameObject.transform.position);
                    currentChestGridPos.z = 0;
                    if (currentChestGridPos == tempController.discoveredChestPositionList[i])
                    {
                        tempController.discoveredChestPositionList.RemoveAt(i);
                        Debug.Log("Chest sucessfully removed from list.");
                    }
                }
                OpenChest();
                tempController.keysCount -= 1;
            }
        }
    }
}
