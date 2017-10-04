using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Launch projectiles
/// </summary>
public class WeaponScript : MonoBehaviour
{

    //-----------------
    // 1 - Designer Variables
    //-----------------

    /// <summary>
    /// Projectile prefab for shooting
    /// </summary>
    public Transform shotPrefab;

    /// <summary>
    /// Cooldown in seconds between shots
    /// </summary>
    public float shootingRate = 0.75f;

    //-----------------
    // 2 - Cooldown
    //-----------------
    private float shootCooldown;

    void Start()
    {
        shootCooldown = 0f;
    }

    void Update()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    //-----------------
    // 3 - Shooting from another Script
    //-----------------

    /// <summary>
    /// Create a new projectile if possible
    /// </summary>
    public void Attack(bool isEnemy)
    {
        if (canAttack)
        {
            shootCooldown = shootingRate;

            // Create a new shot
            var shotTransform = Instantiate(shotPrefab) as Transform;

            // Assign position
            shotTransform.position = transform.position;
            shotTransform.rotation = transform.rotation;

            // This is enemy Property
            ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
            if (shot != null)
            {
                shot.isEnemyShot = isEnemy;
            }

            // Make the weapon shot always towards it
            MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
            if (move != null)
            {
                move.direccion = this.transform.up;
                // Towards in 2D space is the right of the sprite
            }
        }
    }

    /// <summary>
    /// Is the weapon ready to create a new projectile?
    /// </summary>
    public bool canAttack
    {
        get
        {
            return shootCooldown <= 0f;
        }
    }
}
