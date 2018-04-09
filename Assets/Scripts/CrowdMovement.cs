using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMovement : MonoBehaviour {

    private int myIndex = -1;
    public Vector3 moveTarget;

    // crowd target is to manage multiple units all going to the same movetarget
    private CrowdTarget crowdTarget;

    void Start () {
        InvokeRepeating("CheckTarget", 0.5f, 0.5f);
    }

    // called when the unit that has this is sent to a new place
    public void SetCrowdTarget(CrowdTarget ct, int index) {
        if (crowdTarget) crowdTarget.RemoveIndex(myIndex);
        crowdTarget = ct;
        myIndex = index;
        moveTarget = ct.transform.position;
    }

    // called when the target this unit is trying to go to decides it has
    // enough units around it and destroys itself
    public void DestroyCrowdTarget() {
        crowdTarget = null;
        myIndex = -1;
    }

    void CheckTarget () {
        // if we have a crowd target and a move target
        if (!crowdTarget && moveTarget != null) {

            // if we're gathering then we shouldn't bother with this stuff at all
            var gather = gameObject.GetComponent<GatherResource>();
            if (gather && gather.enabled) {
                crowdTarget.RemoveIndex(myIndex);
                DestroyCrowdTarget();
                return;
            }

            // if the crowd target is satisfied and we're vaugley in the area we
            // should stop moving
            float distance = Vector3.Distance(transform.position, moveTarget);

            if (distance < 3.0f) {
                gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
            }
        }
    }
}
