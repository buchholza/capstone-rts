using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttribute : MonoBehaviour {

    public int LOS;
    public Vector3 safeArea;
    public float safeRadius;
    public float health;
    private bool isAttacked;
    public Component Aggression;
    public int team;
    public WanderNPC wanderNPC;

    public List<Action> actions = new List<Action>();

	// Use this for initialization
	void Start () {
        if(team != 0) {
            wanderNPC = GetComponent<WanderNPC>();
            wanderNPC.enabled = true;
        }
        safeArea = new Vector3(safeRadius,0,safeRadius);
	}
	
	// Update is called once per frame
	void Update () { 
        if (health <= 0) {
            Destroy(this.gameObject);
        }
	}

    public void beingAttacked(int damage) {
        health -= damage;
        //isAttacked = false;
    }
}
