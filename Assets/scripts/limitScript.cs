using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limitScript : MonoBehaviour {

    private List<string> nonCol = new List<string>();
	// Use this for initialization
	void Start () {
        // nonCol: List of variables we do not want to detect collisions
        nonCol.Add("Background");
        nonCol.Add("Foreground");
        nonCol.Add("Fence");
        nonCol.Add("ObstacleCube");
        nonCol.Add("shotPrefab");
        nonCol.Add("shotPrefab(Clone)");
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Avoid double trigger condition
        if (nonCol.Contains(collision.transform.name)) {
            return;
        }
        
        // var to get the actual "limit" in the map
        string wallName = transform.name;
        // var to get the collider object
        Transform chocon = collision.transform.parent.gameObject.transform;
        string nombre = chocon.name;
        
        // what limit we hit?
        if (wallName == "FenceB"){
            // we want to detect collision? If True, we do
            if (!nonCol.Contains(nombre)){
                // go to the opposite site
                chocon.position = new Vector3(chocon.position.x, 8f, chocon.position.z);
            // special case for all limits                
            } else if (nombre == "Foreground"){
                // collider object is not the same
                chocon = collision.transform;
                // go to the opposite site
                chocon.position = new Vector3(chocon.position.x, 8f, chocon.position.z);
            }
        // same for the rest
        } else if (wallName == "FenceT"){
            if (!nonCol.Contains(nombre)){
                chocon.position = new Vector3(chocon.position.x, -5.2f, chocon.position.z);
            } else if (nombre == "Foreground"){
                chocon = collision.transform;
                chocon.position = new Vector3(chocon.position.x, -5.2f, chocon.position.z);
            }
        } else if (wallName == "FenceR"){
            if (!nonCol.Contains(nombre)){
                chocon.position = new Vector3(-9f, chocon.position.y, chocon.position.z);
            } else if (nombre == "Foreground"){
                chocon = collision.transform;
                chocon.position = new Vector3(-9f, chocon.position.y, chocon.position.z);
            }
        } else if (wallName == "FenceL"){
            if (!nonCol.Contains(nombre)){
                chocon.position = new Vector3(8.7f, chocon.position.y, chocon.position.z);
            } else if (nombre == "Foreground"){
                chocon = collision.transform;

                chocon.position = new Vector3(8.7f, chocon.position.y, chocon.position.z);
            }
        }

    }

}
