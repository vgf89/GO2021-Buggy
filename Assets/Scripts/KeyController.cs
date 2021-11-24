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
            AdventurerController tempController = collision.gameObject.GetComponent<AdventurerController>();
            tempController.keysCount++;
            Debug.Log(collision.gameObject.name + " has picked up " + name + ".");
            for (int i = 0; i < tempController.discoveredKeyPositionList.Count; i++)
            {
                Vector3Int currentKeyGridPos = Vector3Int.FloorToInt(gameObject.transform.position);
                currentKeyGridPos.z = 0;
                if (currentKeyGridPos == tempController.discoveredKeyPositionList[i])
                {
                    tempController.discoveredKeyPositionList.RemoveAt(i);
                    Debug.Log("Key successfully removed from list.");
                }
            }
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
