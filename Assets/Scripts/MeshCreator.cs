using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public class MeshCreator : MonoBehaviour {

	// Use this for initialization
    List<Vector3> newVerts = new List<Vector3>();
    List<int> newTris = new List<int>();

    public GameObject treeObject;
    public GameObject unitObject;

    public int halfWidth = 10;
    public int halfHeight = 10;

	void Start () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;

        for (int x = 0; x < halfWidth * 2; x++) {
            for (int z = 0; z < halfHeight * 2; z++) {
                int xx = x - halfWidth;
                int zz = z - halfHeight;
                if (Random.Range(0, 20) < 1) {
                    pushWall(xx, zz);
                } else {
                    pushFloor(xx, zz);
                    if (Random.Range(0, 20) < 1) {
                        addTree(xx, zz);
                    }
                }
            }
        }

        Vector3[] vertArray = newVerts.ToArray();
        int[] triArray = newTris.ToArray();

        // this NavMeshBuildSettings is copy pasted from some place

        NavMeshBuildSettings bs = new NavMeshBuildSettings() {
            agentClimb = 10f,
            agentHeight = 1.0f,
            agentRadius = 0.3f,
            agentSlope = 25.0f,
            agentTypeID = 0,
            minRegionArea = 0.1f,
            tileSize = 5,
            voxelSize = 0.005f /*whatever, doesnt matter for quad source*/
        };

        mesh.Clear();

        mesh.vertices = vertArray;
        mesh.triangles = triArray;

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

        // this next part is also copy pasted from some place

        var sources = new List<NavMeshBuildSource>() {
            new NavMeshBuildSource() {
                area = 0,
                component = null /*a sprite?*/,
                shape = NavMeshBuildSourceShape.Mesh,
                size = new Vector3(7.2f,4.8f),
                sourceObject = mesh /*mesh*/,
                transform = Matrix4x4.identity /*already in world coordinates*/
            }
        };

        var data = NavMeshBuilder.BuildNavMeshData(bs, sources, mesh.bounds, Vector3.zero, Quaternion.identity);
        var res = UnityEngine.AI.NavMesh.AddNavMeshData(data);

        var triangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();//no areas, vertices or triangles

        if (unitObject != null)
            Instantiate(unitObject, new Vector3(0, 0, 0), Quaternion.identity);

        GetComponent<MeshCollider>().sharedMesh = mesh;
	}

    void addTree (int x, int z) {
        Instantiate(treeObject, new Vector3(x, 0, z), Quaternion.identity);
    }

    /*
    p1 -----> p2
    ^ \  ^     |
    |   \  \   |
    |     V  \ V
    p0 <----- p3
    */

    void pushFloor (int x, int z) {
        pushQuad(new Vector3(x, 0, z), new Vector3(x+1, 0, z), new Vector3(x+1, 0, z+1), new Vector3(x, 0, z+1));
    }
    void pushWall (int x, int z) {
        // top
        pushQuad(new Vector3(x, 1, z), new Vector3(x+1, 1, z), new Vector3(x+1, 1, z+1), new Vector3(x, 1, z+1));

        pushQuadBackwards(new Vector3(x, 0, z), new Vector3(x, 1, z), new Vector3(x+1, 1, z), new Vector3(x+1, 0, z)); // dir 0
        pushQuad(new Vector3(x, 0, z+1), new Vector3(x, 1, z+1), new Vector3(x+1, 1, z+1), new Vector3(x+1, 0, z+1));  // dir 2
        pushQuadBackwards(new Vector3(x, 0, z), new Vector3(x, 0, z+1), new Vector3(x, 1, z+1), new Vector3(x, 1, z)); // dir 3
        pushQuad(new Vector3(x+1, 0, z), new Vector3(x+1, 0, z+1), new Vector3(x+1, 1, z+1), new Vector3(x+1, 1, z));  // dir 1
    }

    /*
    p1   1   p2
               
    0         2
               
    p0   3   p3

    dirs:
        0 - p0, p1
        1 - p1, p2
        2 - p2, p3
        3 - p3, p0
    */

    void pushRamp (int x, int z, int dir) {
        switch (dir) {
            case 0:
                pushQuad(new Vector3(x, 1, z), new Vector3(x+1, 1, z), new Vector3(x+1, 0, z+1), new Vector3(x, 0, z+1));
                pushQuadBackwards(new Vector3(x, 0, z), new Vector3(x, 1, z), new Vector3(x+1, 1, z), new Vector3(x+1, 0, z)); // dir 0

                pushTriBackwards(new Vector3(x, 0, z), new Vector3(x, 0, z+1), new Vector3(x, 1, z)); // dir 3 right
                pushTri(new Vector3(x+1, 0, z), new Vector3(x+1, 0, z+1), new Vector3(x+1, 1, z));  // dir 1 left

                break;

            case 1:
                pushQuad(new Vector3(x, 0, z), new Vector3(x+1, 1, z), new Vector3(x+1, 1, z+1), new Vector3(x, 0, z+1));
                pushQuad(new Vector3(x+1, 0, z), new Vector3(x+1, 0, z+1), new Vector3(x+1, 1, z+1), new Vector3(x+1, 1, z));  // dir 3

                pushTriBackwards(new Vector3(x, 0, z), new Vector3(x+1, 1, z), new Vector3(x+1, 0, z)); // dir 0
                pushTri(new Vector3(x, 0, z+1), new Vector3(x+1, 1, z+1), new Vector3(x+1, 0, z+1));  // dir 2

                break;

            case 2:
                pushQuad(new Vector3(x, 0, z), new Vector3(x+1, 0, z), new Vector3(x+1, 1, z+1), new Vector3(x, 1, z+1));
                pushQuad(new Vector3(x, 0, z+1), new Vector3(x, 1, z+1), new Vector3(x+1, 1, z+1), new Vector3(x+1, 0, z+1));  // dir 2

                pushTriBackwards(new Vector3(x, 0, z), new Vector3(x, 0, z+1), new Vector3(x, 1, z+1)); // dir 3
                pushTri(new Vector3(x+1, 0, z), new Vector3(x+1, 0, z+1), new Vector3(x+1, 1, z+1));  // dir 1

                break;

            case 3:
                pushQuad(new Vector3(x, 1, z), new Vector3(x+1, 0, z), new Vector3(x+1, 0, z+1), new Vector3(x, 1, z+1));
                pushQuadBackwards(new Vector3(x, 0, z), new Vector3(x, 0, z+1), new Vector3(x, 1, z+1), new Vector3(x, 1, z)); // dir 1

                pushTriBackwards(new Vector3(x, 0, z), new Vector3(x, 1, z), new Vector3(x+1, 0, z)); // dir 0
                pushTri(new Vector3(x, 0, z+1), new Vector3(x, 1, z+1), new Vector3(x+1, 0, z+1));  // dir 2

                break;
        }
    }

    void pushQuadBackwards (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        pushQuad(p3, p2, p1, p0);
    }

    void pushQuad (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
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
