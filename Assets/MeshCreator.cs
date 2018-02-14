using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class MeshCreator : MonoBehaviour {

	// Use this for initialization
    List<Vector3> newVerts = new List<Vector3>();
    List<int> newTris = new List<int>();

	void Start () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;

        newVerts.Add(new Vector3(0, 0, 0));
        newVerts.Add(new Vector3(0, 1, 0));
        newVerts.Add(new Vector3(1, 1, 0));

        pushQuad(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0));

        for (int x = 0; x < 20; x++) {
            for (int y = 0; y < 20; y++) {
                if (Random.Range(0, 20) < 1) {
                    pushWall(x, y);
                } else {
                    pushFloor(x, y);
                }
            }
        }

        Vector3[] vertArray = newVerts.ToArray();
        int[] triArray = newTris.ToArray();

        mesh.Clear();

        mesh.vertices = vertArray;
        mesh.triangles = triArray;

        mesh.RecalculateNormals();
	}

    /*
    p1 -----> p2
    ^ \  ^     |
    |   \  \   |
    |     V  \ V
    p0 <----- p3
    */

    void pushFloor(int x, int z) {
        pushQuad(new Vector3(x, 0, z), new Vector3(x+1, 0, z), new Vector3(x+1, 0, z+1), new Vector3(x, 0, z+1));
    }
    void pushWall(int x, int z) {
        // top
        pushQuad(new Vector3(x, 1, z), new Vector3(x+1, 1, z), new Vector3(x+1, 1, z+1), new Vector3(x, 1, z+1));

        pushQuadBackwards(new Vector3(x, 0, z), new Vector3(x, 1, z), new Vector3(x+1, 1, z), new Vector3(x+1, 0, z));
        pushQuad(new Vector3(x, 0, z+1), new Vector3(x, 1, z+1), new Vector3(x+1, 1, z+1), new Vector3(x+1, 0, z+1));
        pushQuadBackwards(new Vector3(x, 0, z), new Vector3(x, 0, z+1), new Vector3(x, 1, z+1), new Vector3(x, 1, z));
        pushQuad(new Vector3(x+1, 0, z), new Vector3(x+1, 0, z+1), new Vector3(x+1, 1, z+1), new Vector3(x+1, 1, z));
    }

    void pushQuadBackwards(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        pushQuad(p3, p2, p1, p0);
    }

    void pushQuad(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        int vertStart = newVerts.Count;

        newVerts.Add(p0);
        newVerts.Add(p1);
        newVerts.Add(p2);
        newVerts.Add(p3);

        newTris.Add(vertStart + 3);
        newTris.Add(vertStart + 1);
        newTris.Add(vertStart + 0);

        newTris.Add(vertStart + 3);
        newTris.Add(vertStart + 2);
        newTris.Add(vertStart + 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
