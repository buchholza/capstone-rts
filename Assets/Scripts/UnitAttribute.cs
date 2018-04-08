using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitAttribute : MonoBehaviour {

    public int LOS;
    public float health = 20.0f;
    public float maxHealth = 20.0f;
    private bool isAttacked;
    public Component Aggression;
    public int team;
    public WanderNPC wanderNPC;

    public List<Action> actions = new List<Action>();

    public Image healthBarFill;
    public Transform healthBarCanvas;

	// Use this for initialization
	void Start () {
        if(team != 0) {
            wanderNPC = GetComponent<WanderNPC>();
            wanderNPC.enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update () { 
        if (health <= 0) {
            Destroy(this.gameObject);
        }

        if (healthBarFill) healthBarFill.fillAmount = health / maxHealth;
        if (healthBarCanvas)
            healthBarCanvas.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
	}

    public void beingAttacked(int damage) {
        health -= damage;
        //isAttacked = false;
    }
}
