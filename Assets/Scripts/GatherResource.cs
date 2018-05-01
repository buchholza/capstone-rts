using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GatherResource : MonoBehaviour {

    public NavMeshAgent agent;
    public Transform homeBase;
    private bool needsToFind = true;
    private bool isIdle = true;
    private bool hasResource = false;
    private GameObject nearestResource;
    private int team;

    public enum ResourceType { Tree, Rock, Whatever };
    public ResourceType resourceToGet = ResourceType.Whatever;

    public void Reset () {
        needsToFind = true;
        isIdle = true;
        hasResource = false;
        nearestResource = null;
    }

    void Start () {
        team = gameObject.GetComponent<UnitAttribute>().team;
    }

	// Update is called once per frame
	void Update () {

        if (isIdle && !needsToFind && !nearestResource.activeSelf) {
            needsToFind = true;
        }
        
        //Tells an idle unit to gather the nearest resource
        if (isIdle) {
            if (needsToFind) {
                nearestResource = GetClosestResource();

                if (nearestResource != null && nearestResource.activeSelf) {
                    if (agent.SetDestination(nearestResource.transform.position)) {
                        // goal = nearestResource.transform.position;
                        needsToFind = false;
                        agent.isStopped = false;
                    }
                }
            }

            if (Vector3.Distance(transform.position, nearestResource.transform.position) < .5) {
                isIdle = false;
                hasResource = true;
                if (nearestResource.name == "rock_h(Clone)"|| 
                    nearestResource.name == "rock_g(Clone)"|| 
                    nearestResource.name == "rock_e(Clone)"|| 
                    nearestResource.name == "rock_f(Clone)") {
                    RtsManager.current.teams[team].stone += 20; 
                }
                if (nearestResource.name == "tree2(Clone)"||
                    nearestResource.name == "tree1(Clone)") {
                    RtsManager.current.teams[team].wood += 20; 
                }
                nearestResource.SetActive(false);
            }
        }

        //Once a resource is gathered, unit returns it to the capitol
        if (homeBase) {
            if (hasResource) {
                agent.SetDestination(homeBase.position);

                if (Vector3.Distance(transform.position, homeBase.position) < 1) {
                    hasResource = false;
                    if (team == 0) {
                        RtsManager.current.woodText.text =
                            "Wood: " + RtsManager.current.teams[team].wood.ToString();
                        RtsManager.current.stoneText.text =
                            "Stone: " + RtsManager.current.teams[team].stone.ToString();
                    } 
                    isIdle = true;
                    needsToFind = true;
                }
            }
        }
	}

    //Method to find the nearest resource to the unit at the time
    public GameObject GetClosestResource() {
        List<GameObject> resourcesToScan = new List<GameObject>();

        if (resourceToGet == ResourceType.Rock || resourceToGet == ResourceType.Whatever) {
            var allResources = GameObject.FindGameObjectsWithTag("Rock");
            resourcesToScan.AddRange(allResources);
        }
        if (resourceToGet == ResourceType.Tree || resourceToGet == ResourceType.Whatever) {
            var allResources = GameObject.FindGameObjectsWithTag("Tree");
            resourcesToScan.AddRange(allResources);
        }

        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in resourcesToScan) {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
