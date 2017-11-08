using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshScript : MonoBehaviour {
    public GameObject startNode, endNode;
    public Color pathColor = Color.red;

    private GameObject[] nodes;
    private List<GameObject> nodes_rays;
    private LinkedList<GameObject> path = new LinkedList<GameObject>();

    // Use this for initialization
    private void Start () {
        nodes = GameObject.FindGameObjectsWithTag("Node");
        nodes_rays = new List<GameObject>(nodes);
        EnemyScript movementScript = GetComponent<EnemyScript>();
        if (startNode != null && endNode != null)
        {
            nodes_rays.Add(endNode);
            Astar(startNode, endNode);
            GameObject[] path_array = new GameObject[path.Count];
            int count = 0;
            foreach (var node in path)
            {
                path_array[count] = node;
                count++;
            }
            movementScript.path = path_array;
            movementScript.switcher = 9;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Draw Graph conections
        foreach (var node in nodes_rays)
        {
            foreach (var obj in node.GetComponent<NodeScript>().conections)
            {
                if (path.Contains(obj) && path.Contains(node))
                {
                    // Deactivate a node if it is in any path
                    node.GetComponent<NodeScript>().active = false;
                    Debug.DrawRay(node.transform.position, obj.transform.position - node.transform.position, pathColor);
                }
                // Draw a Green line for every active conection between active nodes
                else if(node.GetComponent<NodeScript>().active)
                {
                    Debug.DrawRay(node.transform.position, obj.transform.position - node.transform.position, Color.green);
                }
            }
        }


    }

    // A* heuristic function
    private float heuristic(GameObject node)
    {
        return (endNode.transform.position - node.transform.position).magnitude;
    }

    // A* algorith
    public void Astar(GameObject start, GameObject end)
    {
        int n_nodes = nodes.Length;
        List<GameObject> closedSet = new List<GameObject>();
        List<GameObject> openSet = new List<GameObject>() { start };
        GameObject current;
        float tentative_cost = 0f;
        float[] gCost = new float[n_nodes];
        Dictionary<GameObject, float> fScore = new Dictionary<GameObject, float>();
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();

        foreach (var node in nodes){
            fScore[node] = float.MaxValue;
        }
        fScore[startNode] = heuristic(startNode);

        for (int i = 0; i < gCost.Length; i++)
        {
            gCost[i] = float.MaxValue;
        }
        gCost[int.Parse(start.name.Remove(0, 4))] = 0;

        while (openSet.Count > 0)
        {
            current = openSet[0];
            if (current == end)
            {
                // First path found, probably the best or a close one
                path = get_path(cameFrom, current);
                // Print the path
                foreach (var item in path)
                {
                    print(item.name + ", ");
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in current.GetComponent<NodeScript>().conections)
            {
                if (closedSet.Contains(neighbor)){
                    continue;
                }

                if(!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }

                tentative_cost = gCost[int.Parse(current.name.Remove(0, 4))] + 
                    (current.transform.position - neighbor.transform.position).magnitude;
                if (tentative_cost >= gCost[int.Parse(neighbor.name.Remove(0, 4))])
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gCost[int.Parse(neighbor.name.Remove(0, 4))] = tentative_cost;
                fScore[neighbor] = gCost[int.Parse(current.name.Remove(0, 4))] + heuristic(neighbor);
            }
            openSet = nodesToSort(openSet, fScore);
        }
    }

    private List<GameObject> nodesToSort(List<GameObject> nodes, Dictionary<GameObject, float> fScore)
    {
        GameObject[] arrayNodes = nodes.ToArray();
        int n_nodes = nodes.Count;
        float[] h_nodes = new float[nodes.Count];
        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i < nodes.Count; i++)
        {
            h_nodes[i] = fScore[nodes[i]];
        }

        Array.Sort(h_nodes, arrayNodes);
        list.AddRange(arrayNodes);
        return list;
    }

    private LinkedList<GameObject> get_path(Dictionary<GameObject, GameObject> cameFrom, GameObject current)
    {
        LinkedList<GameObject> path = new LinkedList<GameObject>();
        path.AddFirst(current);
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.AddFirst(current);
        }
        return path;
    }

}
