using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExpand : MonoBehaviour {

    public GameObject building;
    public float innerRadius = 3.0f;
    public float outerRadius = 10.0f;
    public float spawnTimer = 10.0f;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Build", 0.0f, spawnTimer);
	}

    void Build () {
        Vector3 center = new Vector3(
            transform.position.x,
            transform.position.y + 0.5f,
            transform.position.z
        );

        float theta = Random.Range(0.0f, Mathf.PI * 2.0f);
        float radius = Random.Range(innerRadius, outerRadius);
        Vector3 offset = new Vector3(
            radius * Mathf.Cos(theta),
            0.0f,
            radius * Mathf.Sin(theta)
        );

        Vector3 location = center + offset;
        
        Instantiate(building, location, Quaternion.identity);

    }
	
}
