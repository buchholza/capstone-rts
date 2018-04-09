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

    public void Reset () {
        needsToFind = true;
        isIdle = true;
        hasResource = false;
        nearestResource = null;
    }

	// Update is called once per frame
	void Update () {

        if (!hasResource && !isIdle) {
            print("???");
        }
        
        //Tells an idle unit to gather the nearest resource
        if (isIdle) {
            if (needsToFind) {
                var allResources = GameObject.FindGameObjectsWithTag("Resource");
                nearestResource = GetClosestResource(new List<GameObject>(allResources));

                if (nearestResource != null) {
                    needsToFind = false;
                }

                agent.SetDestination(nearestResource.transform.position);
            }

            if (Vector3.Distance(transform.position, nearestResource.transform.position) < .5) {
                isIdle = false;
                hasResource = true;
                print(nearestResource.name+";");
                if (nearestResource.name == "RocksPrefab(Clone)") {
                    RtsManager.current.teams[0].stone++; 
                }
                if (nearestResource.name == "TreePrefab(Clone)") {
                    RtsManager.current.teams[0].wood++; 
                }
                nearestResource.SetActive(false);
            }
        }

        //Once a resource is gathered, unit returns it to the capitol
        if (hasResource) {
            agent.SetDestination(homeBase.position);

            if (Vector3.Distance(transform.position, homeBase.position) < 1) {
                hasResource = false;
                RtsManager.current.woodText.text =
                    "Wood: " + RtsManager.current.teams[0].wood.ToString();
                RtsManager.current.stoneText.text =
                    "Stone: " + RtsManager.current.teams[0].stone.ToString();

                isIdle = true;
                needsToFind = true;
            }
        }
	}


    //Method to find the nearest resource to the unit at the time
    public GameObject GetClosestResource(List<GameObject> resources) {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in resources) {
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
