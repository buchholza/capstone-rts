using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggression : MonoBehaviour {

    private int team;
    public UnitAttribute unitAttribute;
    public LayerMask unitMask;
    public bool attacking;
    private float period=3;

    // Use this for initialization
    void Start () {
        unitAttribute = GetComponent<UnitAttribute>();
        team = unitAttribute.team;
        unitMask = LayerMask.GetMask("Unit");
	}
	
	// Update is called once per frame
	void Update () {
        if (period > 2) {
            Scan();
            period = 0;
        }
        period += UnityEngine.Time.deltaTime;
	}

    public void Scan() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, unitAttribute.LOS, unitMask);
        if (hitColliders.Length != 0) {
            UnitAttribute enemy = hitColliders[0].GetComponentInParent<UnitAttribute>();

            if (enemy.team != this.team) { // on different teams
                Attack(hitColliders[0]);
            }
        }
        else {
            if (unitAttribute.team == 1) unitAttribute.wanderNPC.enabled = true;
        }
    }

    public void Attack(Collider target) {
        UnitAttribute enemy = target.GetComponentInParent<UnitAttribute>();

        if (unitAttribute.team == 1) unitAttribute.wanderNPC.enabled = false;

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 2 * Time.deltaTime);
        enemy.beingAttacked(4);
        // print(enemy.health);
    }

        
}
