using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExpand : MonoBehaviour {

    public GameObject building;
    public float innerRadius = 3.0f;
    public float outerRadius = 10.0f;
    public float spawnTimer = 10.0f;
    public float resourceGoal = 200;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Build", 0.0f, spawnTimer);
        InvokeRepeating("CheckResources", 0.0f, 0.5f);
	}

    void CheckResources () {
        var team = RtsManager.current.teams[1];
        var units = team.units;

        if (team.stone < resourceGoal || team.wood < resourceGoal) {
            int unitsToSwitch = (int) ((resourceGoal - team.stone) / 2);
            int switchedUnits = 0;

            int index = 0;
            while (true) {
                if (index >= units.Count) break;
                if (switchedUnits >= unitsToSwitch) break;
                if (units[index] == null) continue;

                var gather = units[index].GetComponent<GatherResource>();
                var wander = units[index].GetComponent<WanderNPC>();
                
                if (gather) {
                    gather.enabled = true;
                    wander.enabled = false;
                    switchedUnits++;
                }
                index++;
            }
        } else {
            Debug.Log("Something is seriously messed up");
            int index = 0;
            while (true) {
                if (index >= units.Count) break;
                if (units[index] == null) continue;

                var gather = units[index].GetComponent<GatherResource>();
                var wander = units[index].GetComponent<WanderNPC>();
                
                if (gather) {
                    gather.enabled = false;
                    wander.enabled = true;
                }
                index++;
            }
        }
    }

    void Build () {
        Vector3 center = new Vector3(
            transform.position.x,
            transform.position.y,
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

        Vector3 groundLocation = center + offset + new Vector3(0, 0, 0);
        
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(groundLocation, out hit, 0.1f, UnityEngine.AI.NavMesh.AllAreas)) {
            if (RtsManager.current.teams[1].stone >= 100) {
                Instantiate(building, location, Quaternion.identity);
                RtsManager.current.teams[1].stone -= 100;
            }
        }
    }
	
}
