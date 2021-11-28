using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorScript : MonoBehaviour
{
    public AdventurerController controller;

    public bool isDebugging;

    public List<Transform> enemyTransforms;

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
        
        if (collision.CompareTag("Chest"))
        {
            if (!collision.GetComponent<ChestController>().isDiscovered)
            {
                if (isDebugging)
                    Debug.Log(name + " has newly discovered " + collision.name + " at " + Vector3Int.FloorToInt(collision.transform.position));
                controller.SaveItemPosition(collision.transform, collision.tag);
                collision.GetComponent<ChestController>().isDiscovered = true;
            }
        }

        if (collision.CompareTag("Key"))
        {
            if (!collision.GetComponent<KeyController>().isDiscovered)
            {
                if (isDebugging)
                    Debug.Log(name + " has newly discovered " + collision.name + " at " + Vector3Int.FloorToInt(collision.transform.position));
                controller.SaveItemPosition(collision.transform, collision.tag);
                collision.GetComponent<KeyController>().isDiscovered = true;
            }
        }

        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            enemyTransforms.Add(collision.transform);
        }
        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            enemyTransforms.Remove(collision.transform);
        }
    }
}
