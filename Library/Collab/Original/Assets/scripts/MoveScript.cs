using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simply moves for objects
/// </summary>
public class MoveScript : MonoBehaviour
{
    // 1 - Designer variables

    /// <summary>
    /// Object speed
    /// </summary>
    public Vector3 speed = new Vector3(10, 10,10);

    /// <summary>
    /// Moving direction
    /// </summary>
    public Vector3 direccion = new Vector3(-1, 0,0);

    private Vector2 movement;
    private Rigidbody rigidbodyComponent;

    // Use this for initialization
    void Start()
    {

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
        if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody>();

        // Apply movement to the rigidbody
        rigidbodyComponent.velocity = movement;
    }
}
