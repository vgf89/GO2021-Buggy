using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBurstScript : MonoBehaviour
{
    //This area will be temporarily spawned to damage enemies in the area and then disappear
    public int damageValue;
    public float lifeTime;
    float startTime;
    float timer;
    void Awake()
    {
        startTime = Time.time;
    }

    private void FixedUpdate()
    {
        //Lives for about 1 frame and then destroys itself
        if (Time.time > startTime + Time.deltaTime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
            collision.gameObject.GetComponent<Health>().takeDamage(damageValue);
    }
}
