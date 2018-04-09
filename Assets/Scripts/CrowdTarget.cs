using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdTarget : MonoBehaviour {
    
    int totalUnits = - 1;
    public List<GameObject> managedUnits;

    void Start () {
        InvokeRepeating("CheckUnits", 1.0f, 1.0f);
    }

    public void SetManagedUnits(List<GameObject> units) {
        managedUnits = units;
        totalUnits = managedUnits.Count;
    }

    // called when this target still has people trying to get to it and one of
    // them gets a new target
    public void RemoveIndex (int index) {
       totalUnits--;
       if (totalUnits <= 0) 
       managedUnits[index] = null;
    }

    // called when all the units that are trying to get to this target either
    // get new targets or make it here
    void RemoveTarget () {
        foreach (GameObject unit in managedUnits) {
            if (unit) {
                CrowdMovement cm = unit.GetComponent<CrowdMovement>();
                if (cm) cm.DestroyCrowdTarget();
            }
        }
        Destroy(gameObject);
    }

    void CheckUnits () {
        // if totalunits is -1 we're still being made or something?
        // this should never happen but just in case we'll just not do anything
        if (totalUnits == -1) return;

        // loop though all units and see how many are close to the target
        int unitsInside = 0;
        foreach (GameObject unit in managedUnits) {
            if (unit) {
                float distance = Vector3.Distance(unit.transform.position, transform.position);
                if (distance < Mathf.Sqrt(totalUnits * 3.0f / Mathf.PI)) unitsInside++;
            }
        }

        // if enough are inside, then destroy the target because we're done
        if (unitsInside > totalUnits - 3) {
            RemoveTarget();
        }

        // the 3.0f and the 3 in that function were totally arbitrary so those
        // be tweaked if nessasary
    }
}
