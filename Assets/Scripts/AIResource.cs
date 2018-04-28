using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIResource : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform homeBase;
    private bool needsToFind = true;
    private bool isIdle = true;
    private bool hasResource = false;
    private GameObject nearestResource;
    private GameObject nearestStone;
    public int stone = 110;
    public int wood = 100;
    public string currentResource;
    public void Reset()
    {
        needsToFind = true;
        isIdle = true;
        hasResource = false;
        nearestResource = null;
        nearestStone = null;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (wood <= stone)
        {
            currentResource = "wood";
        }
        else
        {
            currentResource = "stone";
        }
        if (!hasResource && !isIdle)
        {
            print("???");
        }
        if (currentResource == "wood")
        {
            //Tells an idle unit to gather the nearest resource
            if (isIdle)
            {
                if (needsToFind)
                {
                    var allResources = GameObject.FindGameObjectsWithTag("Resource");
                    nearestResource = GetClosestResource(new List<GameObject>(allResources));

                    if (nearestResource != null)
                    {
                        needsToFind = false;
                    }

                    agent.SetDestination(nearestResource.transform.position);
                }

                if (Vector3.Distance(transform.position, nearestResource.transform.position) < .5)
                {
                    isIdle = false;
                    hasResource = true;
                    
                    nearestResource.SetActive(false);
                }
            }

            //Once a resource is gathered, unit returns it to the capitol
            if (hasResource)
            {
                agent.SetDestination(homeBase.position);

                if (Vector3.Distance(transform.position, homeBase.position) < 1)
                {
                    hasResource = false;
                    IncreaseResources("wood", 10);        
                    isIdle = true;
                    needsToFind = true;
                }
            }
        }
        if (currentResource == "stone")
        {
            //Tells an idle unit to gather the nearest resource
            if (isIdle)
            {
                if (needsToFind)
                {
                    var allStone = GameObject.FindGameObjectsWithTag("Stone");
                    nearestStone = GetClosestResource(new List<GameObject>(allStone));

                    if (nearestResource != null)
                    {
                        needsToFind = false;
                    }

                    agent.SetDestination(nearestStone.transform.position);
                }

                if (Vector3.Distance(transform.position, nearestStone.transform.position) < .5)
                {
                    isIdle = false;
                    hasResource = true;
                  
                    nearestStone.SetActive(false);
                }
            }

            //Once a resource is gathered, unit returns it to the capitol
            if (hasResource)
            {
                agent.SetDestination(homeBase.position);

                if (Vector3.Distance(transform.position, homeBase.position) < 1)
                {
                    hasResource = false;
                    
                    IncreaseResources("stone", 10);

                    isIdle = true;
                    needsToFind = true;
                }
            }
        }
    }

    //Method to find the nearest resource to the unit at the time
    public GameObject GetClosestResource(List<GameObject> resources)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in resources)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
    public GameObject GetClosestResourceStone(List<GameObject> stone)
    {
        GameObject bestTargetStone= null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in stone)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTargetStone = potentialTarget;
            }
        }

        return bestTargetStone;
    }
    public void ReducedResources(string resource, int amount)
    {
        alterResourceValue(resource, amount * -1);
    }
    public void IncreaseResources(string resource, int amount)
    {
        alterResourceValue(resource, amount);
    }
    public bool resourceAmountCheck(string resource, int amount)
    {
        switch (resource)
        {
            case "wood":
                if (wood > amount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case "stone":
                if (stone > amount)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            default:
                break;
        }
        return false;
    }


    void alterResourceValue(string resource, int amount)
    {
        switch (resource)
        {
            case "wood":
                wood =wood+ amount;
                break;
            case "stone":
                stone += amount;
                break;

            default:
                break;
        }
    }
}
