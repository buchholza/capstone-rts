using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PeriodicSpawn : MonoBehaviour {
    public GameObject unit;
    public Text woodText;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Spawn", 2.0f, 10f);
    }
	
	void Spawn() {
        Vector3 location = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z + 5);
        var go = GameObject.Instantiate(unit, location, transform.rotation);
        go.GetComponent<TapToMove1>().woodText = woodText;
    }
}
