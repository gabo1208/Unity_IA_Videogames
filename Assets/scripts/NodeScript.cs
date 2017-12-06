using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour {
    public GameObject[] conections;
    public bool active = true;
    public float smellLevel = 0f;
    public string smellType = "";
    public float smellThreshold = 8f;
    public float maxSmell = 10f;
    public GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        // Special position for z axis
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.5490916f);
    }

    private void Update()
    {
        float smellDistance = (player.transform.position - transform.position).magnitude;
        if (smellDistance <= smellThreshold)
        {
            if(smellLevel <= maxSmell * (1/smellDistance))
            {
                smellLevel += (1 / smellDistance) * 2 * Time.deltaTime;
            }
            else
            {
                smellLevel -= 2f * Time.deltaTime;
            }
        }
        else if(smellLevel > 0)
        {
            smellLevel -= 0.5f * Time.deltaTime;
        }
        else
        {
            smellLevel = 0f;
        }
    }

}
