using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public bool isDiscovered;

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
        //If this object collides with the Adventurer, pick up the key for a chest.
        if (collision.gameObject.CompareTag("Adventurer"))
        {
            collision.gameObject.GetComponent<AdventurerController>().keysCount++;
            Debug.Log(collision.gameObject.name + " has picked up" + name + ".");
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
