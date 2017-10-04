using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        string wallName = transform.name;

        Transform chocon = collision.transform.parent.gameObject.transform;
        string nombre = chocon.name;

        if (wallName == "FenceB"){
            if (nombre != "Background" && nombre != "Fence" && nombre != "Foreground"){
                chocon.position = new Vector3(chocon.position.x, 8f, chocon.position.z);
            } else if (nombre == "Foreground"){
                chocon = collision.transform;
                chocon.position = new Vector3(chocon.position.x, 8f, chocon.position.z);
            }
        } else if (wallName == "FenceT"){
            if (nombre != "Background" && nombre != "Fence" && nombre != "Foreground"){
                chocon.position = new Vector3(chocon.position.x, -5.2f, chocon.position.z);
            } else if (nombre == "Foreground"){
                chocon = collision.transform;
                chocon.position = new Vector3(chocon.position.x, -5.2f, chocon.position.z);
            }
        } else if (wallName == "FenceR"){
            if (nombre != "Background" && nombre != "Fence" && nombre != "Foreground"){
                chocon.position = new Vector3(-9f, chocon.position.y, chocon.position.z);
            } else if (nombre == "Foreground"){
                chocon = collision.transform;
                chocon.position = new Vector3(-9f, chocon.position.y, chocon.position.z);
            }
        } else if (wallName == "FenceL"){
            
            if (nombre != "Background" && nombre != "Fence" && nombre != "Foreground"){
                chocon.position = new Vector3(8.7f, chocon.position.y, chocon.position.z);
            } else if (nombre == "Foreground"){
                chocon = collision.transform;
                chocon.position = new Vector3(8.7f, chocon.position.y, chocon.position.z);
            }
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
