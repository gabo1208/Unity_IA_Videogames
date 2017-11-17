using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshScript : MonoBehaviour {
    public GameObject startNode;
    public Color pathColor = Color.red;
    public float pathThreshold = 2f;

    private GameObject[] nodes;
    private List<GameObject> nodes_rays;
    private LinkedList<GameObject> path = new LinkedList<GameObject>();
    private GameObject target, endNode;
    private EnemyScript movementScript;
    private int n_nodes;
    private float[] gCost;
    private Dictionary<GameObject, float> fScore;

    // Use this for initialization
    private void Start () {
        endNode = startNode;
        nodes = GameObject.FindGameObjectsWithTag("Node");
        nodes_rays = new List<GameObject>(nodes);
        movementScript = GetComponent<EnemyScript>();
        target = GameObject.Find("Player");
        n_nodes = nodes.Length;
        gCost = new float[n_nodes];
        fScore = new Dictionary<GameObject, float>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (startNode != null && endNode != null && heuristic(startNode) > pathThreshold){
            // This one follows the exact optimum path to the target, every point that it has been along the way
            //Astar(endNode);

            // This modifies the path each time the target position change closer to other node
            Astar(startNode);

            movementScript.path = new List<GameObject>(path);

            movementScript.switcher = 9;
        }

        // Draw Graph conections
        foreach (var node in nodes_rays){
            foreach (var obj in node.GetComponent<NodeScript>().conections){
                if (path.Contains(obj) && path.Contains(node)){
                    // Deactivate a node if it is in any path
                    Debug.DrawRay(node.transform.position, obj.transform.position - node.transform.position, pathColor);
                }
                // Draw a Green line for every active conection between active nodes
                else if(node.GetComponent<NodeScript>().active || path.Contains(node)){
                    Debug.DrawRay(node.transform.position, obj.transform.position - node.transform.position, Color.green);
                }
            }
        }


    }

    // A* heuristic function
    private float heuristic(GameObject node){
        // distance between target and actual position
        return (target.transform.position - node.transform.position).magnitude;
    }

    // A* algorithm
    public void Astar(GameObject start){
        List<GameObject> closedSet = new List<GameObject>();
        List<GameObject>  openSet = new List<GameObject>();
        float tentative_cost = 0f;
        GameObject current;
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();

        openSet.Add(start);
        foreach (var node in nodes){
            fScore[node] = float.MaxValue;
        }
        fScore[startNode] = heuristic(startNode);

        for (int i = 0; i < gCost.Length; i++){
            gCost[i] = float.MaxValue;
        }
        gCost[int.Parse(start.name.Remove(0, 4))] = 0;

        while (openSet.Count > 0){
            current = openSet[0];
            if (heuristic(current) < pathThreshold){
                // First path found, probably the best or a close one
                path = get_path(cameFrom, current);
                endNode = current;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in current.GetComponent<NodeScript>().conections){
                if (closedSet.Contains(neighbor)){
                    continue;
                }

                if(!openSet.Contains(neighbor)){
                    openSet.Add(neighbor);
                }

                tentative_cost = gCost[int.Parse(current.name.Remove(0, 4))] + 
                    (current.transform.position - neighbor.transform.position).magnitude;

                if (tentative_cost >= gCost[int.Parse(neighbor.name.Remove(0, 4))]){
                    continue;
                }

                cameFrom[neighbor] = current;
                gCost[int.Parse(neighbor.name.Remove(0, 4))] = tentative_cost;
                fScore[neighbor] = gCost[int.Parse(current.name.Remove(0, 4))] + heuristic(neighbor);
            }
            openSet = nodesToSort(openSet, fScore);
        }
    }

    // sort the nodes based in the heuristic
    private List<GameObject> nodesToSort(List<GameObject> nodes, Dictionary<GameObject, float> fScore){
        // list to array, so we can sort
        GameObject[] arrayNodes = nodes.ToArray();
        int n_nodes = nodes.Count;
        // create new array so we can sort
        float[] h_nodes = new float[nodes.Count];
        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i < nodes.Count; i++){
            // get heuristic values to h_nodes
            h_nodes[i] = fScore[nodes[i]];
        }

        // sort in base to heuristic 
        Array.Sort(h_nodes, arrayNodes);
        list.AddRange(arrayNodes);
        return list;
    }

    // build the path from the 'current' target position to enemy actual position 
    private LinkedList<GameObject> get_path(Dictionary<GameObject, GameObject> cameFrom, GameObject current){
        LinkedList<GameObject> path = new LinkedList<GameObject>();
        // current node is the first in the path
        path.AddFirst(current);
        // if current is in the cameFrom Dictionary
        while (cameFrom.ContainsKey(current)){
            // now first is the node closer to enemy position who is connected to the last current
            current = cameFrom[current];
            path.AddFirst(current);
        }
        return path;
    }

    // remove the 1st node of the path
    public void removeFirstInPath(){
        // if path list is not empty, remove 1st
        if (path.Count > 0){
            path.First.Value.GetComponent<NodeScript>().active = true;
            path.RemoveFirst();
        }

        if(path.Count > 0){
            // if path still not empty, re assing startnode to first in path
            startNode = path.First.Value;
        }
    }

}
