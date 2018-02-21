using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour {
    public GameObject villager;
    public Transform target;
    public float speed;
	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {
        moveToLocation(target.position);
	}
    public void moveToLocation(Vector3 destination)
    {
        if(transform.position!=target.position)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
    }
    public void doNothing()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.position, step);
    }

    
}
