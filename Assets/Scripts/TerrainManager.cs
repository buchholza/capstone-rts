using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public class TerrainManager : MonoBehaviour {

	// Use this for initialization
    public GameObject treeObject;
    public GameObject rockObject;

    public int halfWidth = 10;
    public int halfHeight = 10;

	void Start () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;

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

        // mesh.Clear();

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

        GetComponent<MeshCollider>().sharedMesh = mesh;

        for (int x = 0; x < halfWidth * 2; x++) {
            for (int z = 0; z < halfHeight * 2; z++) {
                int xx = x - halfWidth;
                int zz = z - halfHeight;
                
                NavMeshHit hit;
                if (NavMesh.SamplePosition(new Vector3(xx, 0.0f, zz), out hit, 0.1f, NavMesh.AllAreas)) {
                    if (Random.Range(0, 20) < 1) {
                        addTree(xx, zz);
                    }
                    else if (Random.Range(0, 25) < 1) {
                        addRock(xx, zz);
                    }
                }
            }
        }

	}

    void addTree (int x, int z) {
        Instantiate(treeObject, new Vector3(x, 0, z), Quaternion.identity);
    }

    void addRock (int x, int z) {
        if (rockObject != null)
            Instantiate(rockObject, new Vector3(x + 0.5f, 0.2f, z + 0.5f), Quaternion.identity);
    }
}
