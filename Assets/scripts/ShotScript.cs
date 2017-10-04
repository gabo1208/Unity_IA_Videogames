using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projectile behavior
/// </summary>
public class ShotScript : MonoBehaviour
{
    // 1 - Designer Variables

    /// <summary>
    /// Damage inflicted
    /// </summary>
    public int damage = 1;

    /// <summary>
    /// Projectile damage player or enemies?
    /// </summary>
    public bool isEnemyShot = false;


    // Use this for initialization
    void Start()
    {
        // 2 - Limited time to live to avoid any leak
        Destroy(gameObject, 5); // 20 sec
    }

    // Update is called once per frame
    void Update()
    {
    }
}

