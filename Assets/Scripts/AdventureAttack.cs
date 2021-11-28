using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureAttack : MonoBehaviour
{
    public AdventurerController adventurerController;
    private float attackTimer;
    [SerializeField]List<Transform> inRangeEnemies;

    public GameObject meleeAttackBurst;

    public bool isDebugging;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;   
        
        if (attackTimer > adventurerController.attackCooldown && inRangeEnemies.Count != 0)
        {
            FindClosestEnemy();
            attackTimer = 0;
        }
    }
    void FindClosestEnemy()
    {
        Transform closestEnemy = inRangeEnemies[0];
        float closestDistance = Vector3.Distance(adventurerController.transform.position, closestEnemy.position);

        foreach (Transform t in inRangeEnemies)
        {
            float tempDistance = Vector3.Distance(adventurerController.transform.position, t.position);
            if (tempDistance < closestDistance)
            {
                closestDistance = tempDistance;
                closestEnemy = t;
            }
        }
        Attack(closestEnemy);
    }

    void Attack(Transform _go)
    {
        //_go.GetComponent<Health>().takeDamage(adventurerController.damageValue);
        //Spawn the attack point on the enemies position
        if (meleeAttackBurst == null)
        {
            Debug.LogError("meleeAttackBurst prefab has not been sent");
        }
        else
        {
            Instantiate(meleeAttackBurst, _go.transform.position, Quaternion.identity);
        }
        if (isDebugging)
            Debug.Log("Attacking " + _go.name );
    }


    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            inRangeEnemies.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            inRangeEnemies.Remove(collision.transform);
        }
    }
}
