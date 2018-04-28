using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCradle : MonoBehaviour {
    public float speed = 20;
    public GameObject startingSpot;
	// Use this for initialization
	void Start () {
        transform.position = (startingSpot.transform.position + new Vector3(1,6,-5));
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(
            Input.GetAxis("Horizontal") * speed * Time.deltaTime,
            0,
            Input.GetAxis("Vertical") * speed * Time.deltaTime,
            Space.World
        );
	}
}
