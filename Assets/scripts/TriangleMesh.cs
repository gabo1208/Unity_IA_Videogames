using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleMesh : MonoBehaviour {
    public float width = 1;
    public float height = 1;
    public Material material;

    // Use this for initialization
    void Start () {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        
        mesh.Clear();

        // make changes to the Mesh by creating arrays which contain the new values
        mesh.vertices = new Vector3[] {
            new Vector3(height/2, -width/2, 0),
            new Vector3(0, width/2, 0),
            new Vector3(-height/2, -width/2, 0)
        };
        mesh.uv = new Vector2[] {
            new Vector2(height / 2, -width / 2),
            new Vector2(0, width/2),
            new Vector2(-height/2, -width/2)
        };

        // assing triangle values
        mesh.triangles = new int[] { 0, 1, 2 };
        // making enemy triangles with "material"
        gameObject.GetComponent<Renderer>().material = material;
    }

}
