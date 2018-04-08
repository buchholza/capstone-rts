using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggression : MonoBehaviour {

    private bool isPlayerControlled;
    public UnitAttribute unitAttribute;
    public LayerMask unitMask;
    public bool attacking;
    private float period=3;

    // Use this for initialization
    void Start () {
        unitAttribute = GetComponent<UnitAttribute>();
        isPlayerControlled = unitAttribute.isPlayerControlled;
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

            // it should check if it's on its own team
            Attack(hitColliders[0]);
        }
        else {
            unitAttribute.wanderNPC.enabled = true;
        }
    }

    public void Attack(Collider target) {
        UnitAttribute enemy = target.GetComponentInParent<UnitAttribute>();
        unitAttribute.wanderNPC.enabled = false;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 2 * Time.deltaTime);
        enemy.beingAttacked(4);
        // print(enemy.health);
    }

        
}
