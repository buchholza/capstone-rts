using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = UnityEngine.Random;
public class TerrainManager : MonoBehaviour {

    public int width = 5;
    public int height = 5;

    public int walls_max = 5;
    public int walls_min = 3;

    public GameObject floorTile;
    public GameObject wallTile;

    private Transform terrainHolder;
    private List <Vector3> gridPositions = new List<Vector3>();

    void MakeGridPos () {
        gridPositions.Clear();
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                gridPositions.Add(new Vector3(transform.position.x + x, 0.0f, transform.position.z + z));
            }
        }
    }

    void TerrainSetup () {
        terrainHolder = new GameObject("Board").transform;

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < width; z++) {
                GameObject toInstantiate = floorTile;
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, -1.0f, z), Quaternion.identity) as GameObject;

                instance.transform.SetParent(terrainHolder);
            }
        }
    }

    Vector3 RandomPosition () {
        int index = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[index];
        gridPositions.RemoveAt(index);
        return randomPosition;
    }

    void DecorateTerrain (GameObject tile, int min, int max) {
        int count = Random.Range(min, max + 1);

        for (int i = 0; i < count; i++) {
            Vector3 randomPos = RandomPosition();
            Instantiate(tile, randomPos, Quaternion.identity);
        }
    }

	// Use this for initialization
	void Start () {
		MakeGridPos();
        // TerrainSetup();
        DecorateTerrain(wallTile, walls_min, walls_max);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
