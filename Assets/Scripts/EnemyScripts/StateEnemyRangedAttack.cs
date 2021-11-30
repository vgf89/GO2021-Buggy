using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnemyRangedAttack : State
{
    [SerializeField] private TriggerCheck adventurerDetector;
    new private Rigidbody2D rigidbody2D;

    [SerializeField] private GameObject projectile; // Projectile to instantiate on state enter
    private GameObject entityContainer;
    [SerializeField] private Transform projectile_spawn_position; // A Transform for where to spawn the projectile
    [SerializeField] private float projectileVelocity = 1;

    override public void Awake() {
        base.Awake();

        rigidbody2D = adventurerDetector.GetComponent<Collider2D>().attachedRigidbody;
        entityContainer = GameObject.FindGameObjectWithTag("EntityContainer");
    }
    
    override public void enter()
    {    
        base.enter();
        rigidbody2D.velocity = Vector2.zero;

        // Check existence of adventurer and projectile prefab
        GameObject player = adventurerDetector.getOther();
        if (player == null || projectile == null || entityContainer == null || projectile_spawn_position == null) {
            return;
        }
        // Figure out direction and velocity
        Vector2 vel2 = (player.transform.position - projectile_spawn_position.position);
        vel2 = vel2.normalized * projectileVelocity;
        GameObject newProjectile = Instantiate(projectile, entityContainer.transform);
        newProjectile.transform.position = projectile_spawn_position.transform.position;
        Projectile newProjectileComponent = newProjectile.GetComponent<Projectile>();
        if (newProjectileComponent == null) {
            Debug.Log("WARNING: Projectile prefab " + newProjectile.name + " is missing Projectile script");
        }
        newProjectileComponent.velocity = vel2;

    }

    override public bool stateUpdate() {
        if (base.stateUpdate()) {
            return true;
        }

        if (animationFinished) {
            stateSystem.popState();
            return true;
        }

        return false;
    }
}
