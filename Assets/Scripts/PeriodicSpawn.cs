using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PeriodicSpawn : MonoBehaviour {
    public GameObject unit;
    public GameObject unit2;
    public GameObject unit3;

    public float spawnTimer = 10f;
    public Vector3 spawnOffset = new Vector3(5.0f, 0.0f, 5.0f);

	// Use this for initialization
	void Start () {
        UnitAttribute unit;
        InvokeRepeating("Spawn", 2.0f, spawnTimer);
    }
	
	void Spawn() {
        Vector3 center = transform.position;
        Vector3 location = center + spawnOffset;

        GameObject unitToMake = unit;

        UnitAttribute newUnitAttribute;
        if (newUnitAttribute = unit.GetComponent<UnitAttribute>()) { 
            int team = newUnitAttribute.team;
            int research = RtsManager.current.teams[team].researchLevel;

            if (research == 2 && unit2 != null) unitToMake = unit2;
            else if (research == 3 && unit3 != null) unitToMake = unit3;
        }

        var newUnit = GameObject.Instantiate(unitToMake, location, transform.rotation);

        var gather = newUnit.GetComponent<GatherResource>();
        if (gather) {
            gather.homeBase = gameObject.transform;
        }

        if (newUnitAttribute = newUnit.GetComponent<UnitAttribute>()) { 
            int team = newUnitAttribute.team;
            RtsManager.current.teams[team].units.Add(newUnit);
        }

    }
}
