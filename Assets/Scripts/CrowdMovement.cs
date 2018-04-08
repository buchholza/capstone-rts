using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMovement : MonoBehaviour {

    private int myIndex = -1;
    private CrowdTarget crowdTarget;
    public Vector3 moveTarget;

    void Start () {
        InvokeRepeating("CheckTarget", 0.5f, 0.5f);
    }

    public void SetCrowdTarget(CrowdTarget ct, int index) {
        if (crowdTarget) crowdTarget.RemoveIndex(myIndex);
        crowdTarget = ct;
        myIndex = index;
        moveTarget = ct.transform.position;
    }

    public void DestroyCrowdTarget() {
        crowdTarget = null;
        myIndex = -1;
    }

    void CheckTarget () {
        if (!crowdTarget && moveTarget != null) {
            float distance = Vector3.Distance(transform.position, moveTarget);
            if (distance < 1.0f)
                gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
        }
    }
}
