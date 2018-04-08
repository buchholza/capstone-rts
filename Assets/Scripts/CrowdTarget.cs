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

    public void RemoveIndex (int index) {
       totalUnits--;
       if (totalUnits <= 0) 
       managedUnits[index] = null;
    }

    void RemoveTarget () {
        foreach (GameObject unit in managedUnits) {
            if (unit) unit.GetComponent<CrowdMovement>().DestroyCrowdTarget();
        }
        Destroy(gameObject);
    }

    void CheckUnits () {
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
    }
}
