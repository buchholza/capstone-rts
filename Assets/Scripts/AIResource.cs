using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIResource : MonoBehaviour {
    public bool isIdle;
    public bool movingToResource;
    public bool atStoreHouse;
    public bool isCollecting;
    public bool atResource;
    public bool movingToStoreHouse;
    public bool hasResource;
    public Transform homeBase;
    private GameObject closestResource;
    private GameObject nearestResource;
    public NavMeshAgent agent;
    private bool needsToFind = true;
    public bool hasTask;
    //Timer timer = 5000;
	// Use this for initialization

	void Reset () {
        isIdle = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (movingToResource)
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
            atResource = true;
            atStoreHouse = false;
            movingToResource = false;
            print(nearestResource.name + ";");
            if (nearestResource.name == "RocksPrefab(Clone)")
            {
                RtsManager.current.teams[0].stone += 10;
            }
            if (nearestResource.name == "TreePrefab(Clone)")
            {
                RtsManager.current.teams[0].wood += 10;
            }
            nearestResource.SetActive(false);
        }
        if(Vector3.Distance(transform.position, homeBase.position)<.5)
        {
            atStoreHouse = true;
        }
    
}
    public bool checkIsIdel()
    {
        if (movingToResource == true || atResource == true || isCollecting == true || movingToResource == true || hasResource == true)
            return false;
        return true;
    }
    public bool movingToTheStoreHouse()
    {

        if(hasResource)
        {
            agent.SetDestination(homeBase.position);

            return true;
        }
        return false;

    }
    public void isItCollecting()
    {
        if(atResource)
        {
            movingToResource = false;
            //timer.Start();
            //if(timer=0)
            //{
              //  hasResource = true;
            //}
        }
    }
    public void depositResource()
    {
        if(movingToTheStoreHouse()==true && atStoreHouse==true)
        {
            RtsManager.current.teams[1].wood += 100;
            hasResource = false;
            movingToResource = true;
        }
    }
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

}
