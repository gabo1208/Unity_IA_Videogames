using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limitScript : MonoBehaviour {

    private List<string> nonCol = new List<string>();
	// Use this for initialization
	void Start () {
        nonCol.Add("Background");
        nonCol.Add("Foreground");
        nonCol.Add("Fence");
        nonCol.Add("ObstacleCube");
        nonCol.Add("shotPrefab");
        nonCol.Add("shotPrefab(Clone)");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (nonCol.Contains(collision.transform.name)) {
            return;
        }
        
        string wallName = transform.name;
        Transform chocon = collision.transform.parent.gameObject.transform;
        string nombre = chocon.name;
        
        if (wallName == "FenceB"){
            if (!nonCol.Contains(nombre)){
                chocon.position = new Vector3(chocon.position.x, 8f, chocon.position.z);
            } else if (nombre == "Foreground"){
                chocon = collision.transform;
                chocon.position = new Vector3(chocon.position.x, 8f, chocon.position.z);
            }
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

    // Update is called once per frame
    void Update () {
		
	}
}
