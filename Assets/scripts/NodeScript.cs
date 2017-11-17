using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour {
    public GameObject[] conections;
    public bool active = true;

	// Use this for initialization
	void Start () {
		// Se le debe asignar una posicion especial para el eje z
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.5490916f);
    }

}
