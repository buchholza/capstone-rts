using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PeriodicSpawn : MonoBehaviour {
    public GameObject unit;

    public float spawnTimer = 10f;
    public  Vector3 spawnOffset = new Vector3(5.0f, 0.0f, 5.0f);

	// Use this for initialization
	void Start () {
        InvokeRepeating("Spawn", 2.0f, spawnTimer);
    }
	
	void Spawn() {
        Vector3 center = transform.position;
        Vector3 location = center + spawnOffset;

        var newUnit = GameObject.Instantiate(unit, location, transform.rotation);
        var gather = newUnit.GetComponent<GatherResource>();
        if (gather) {
            gather.homeBase = gameObject.transform;
        }
    }
}
