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
    private List<string> lista = new List<string>();
    public float initx;
    public float inity;
    public float initz;
    public Vector3 speed = new Vector3(1, 1, 0);
    public Vector3 direccion = new Vector3(-1, 0, 0);
    private Vector3 gravity;
    private Vector3 movement;
    private Rigidbody rigidbodyComponent;

    // Use this for initialization
    void Start(){
        initx = transform.localScale.x;
        inity = transform.localScale.y;
        initz = transform.localScale.z;
        // objects to ignore on trigger
        lista.Add("Background");
        lista.Add("Foreground");
        lista.Add("Enemies");
        lista.Add("shots");

        // 2 - Limited time to live to avoid any leak
        Destroy(gameObject, 5); // 5 sec
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) {
        GameObject choque = other.transform.gameObject;
        // if not in list, we can destroy the shot
        if (!lista.Contains(choque.transform.tag)){
            Destroy(transform.gameObject, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 2 - Movement
        movement = new Vector3(
            speed.x * direccion.x,
            speed.y * direccion.y,
            speed.z * direccion.z);
    }

    private void FixedUpdate()
    {
        // Apply movement to the rigidbody
        rigidbodyComponent.velocity = movement;

        // reduce shot size by time
        rigidbodyComponent.transform.localScale = new Vector3(
           initx * 3 * Mathf.Abs(rigidbodyComponent.transform.position.z),
           inity * 3 * Mathf.Abs(rigidbodyComponent.transform.position.z),
           initz);

    }
}