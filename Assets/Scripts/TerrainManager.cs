﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public class TerrainManager : MonoBehaviour {

	// Use this for initialization
    public GameObject[] treeObject;
    public GameObject[] rockObject;

    public int halfWidth = 10;
    public int halfHeight = 10;

	void Start () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;

        // this nav mesh stuff is mostly copy pasted from some place

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

        var UVs = UnityEditor.Unwrapping.GeneratePerTriangleUV(mesh);

        Vector2[] uvs = new Vector2[mesh.vertices.Length];

        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = new Vector2(mesh.vertices[i].x / 4 + mesh.vertices[i].y / 4, mesh.vertices[i].z / 4 + mesh.vertices[i].y / 4);
        }

        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

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
        UnityEngine.AI.NavMesh.AddNavMeshData(data);

        UnityEngine.AI.NavMesh.CalculateTriangulation();//no areas, vertices or triangles

        GetComponent<MeshCollider>().sharedMesh = mesh;

        // decorate terrain
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
        Instantiate(treeObject[Random.Range(0,2)], new Vector3(x, 0.2f, z), Quaternion.identity);
    }

    void addRock (int x, int z) {
        if (rockObject != null)
            Instantiate(rockObject[Random.Range(0,rockObject.Length)], new Vector3(x + 0.5f, 0.0f, z + 0.5f), Quaternion.identity);
    }
}
