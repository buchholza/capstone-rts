using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMesh : MonoBehaviour {

    List<Vector3> newVerts = new List<Vector3>();
    List<int> newTris = new List<int>();

    float treeRadius = 0.8f;

	void Start () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
        
        pushTri(
            new Vector3(0.5f, 1.0f, 0.5f),
            new Vector3(treeRadius, 0.0f, treeRadius),
            new Vector3(1.0f - treeRadius, 0.0f, treeRadius)
        );
        pushTri(
            new Vector3(0.5f, 1.0f, 0.5f),
            new Vector3(treeRadius, 0.0f, 1.0f - treeRadius),
            new Vector3(treeRadius, 0.0f, treeRadius)
        );
        pushTri(
            new Vector3(0.5f, 1.0f, 0.5f),
            new Vector3(1.0f - treeRadius, 0.0f, 1.0f - treeRadius),
            new Vector3(treeRadius, 0.0f, 1.0f - treeRadius)
        );
        pushTri(
            new Vector3(0.5f, 1.0f, 0.5f),
            new Vector3(1.0f - treeRadius, 0.0f, treeRadius),
            new Vector3(1.0f - treeRadius, 0.0f, 1.0f - treeRadius)
        );

        Vector3[] vertArray = newVerts.ToArray();
        int[] triArray = newTris.ToArray();

        mesh.Clear();

        mesh.vertices = vertArray;
        mesh.triangles = triArray;

        mesh.RecalculateNormals();
	}

    void pushTriBackwards (Vector3 p0, Vector3 p1, Vector3 p2) {
        pushTri(p2, p1, p0);
    }

    void pushTri (Vector3 p0, Vector3 p1, Vector3 p2) {
        int vertStart = newVerts.Count;

        newVerts.Add(p0);
        newVerts.Add(p1);
        newVerts.Add(p2);

        newTris.Add(vertStart + 2);
        newTris.Add(vertStart + 1);
        newTris.Add(vertStart + 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
