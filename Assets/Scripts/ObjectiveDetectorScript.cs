using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDetectorScript : MonoBehaviour
{
    public AdventurerController controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(name + " has found " + collision.name);
        if (collision.CompareTag("Chest"))
        {
            if (!collision.GetComponent<ChestController>().isDiscovered)
            {
                controller.SaveItemPosition(collision.transform, collision.tag);
                collision.GetComponent<ChestController>().isDiscovered = true;
            }
        }

        if (collision.CompareTag("Key"))
        {
            if (!collision.GetComponent<KeyController>().isDiscovered)
            {
                controller.SaveItemPosition(collision.transform, collision.tag);
                collision.GetComponent<KeyController>().isDiscovered = true;
            }
        }

    }
}
