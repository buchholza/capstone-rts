// 
using System.Collections;
// 
using System.Collections.Generic;
// 
using UnityEngine;
// 
public class Action_GatherResource : MonoBehaviour
{
    
    public Vector3 positionOfResource;
    
    //     public float resourceTimer = 5.0f;
    public Vector3 positionOfStoreHouse;

    public string resource;

    public bool movingToResource = false;

    public bool collectingResource = false;
    public bool movingToStoreHouse = false;
    public bool storedResource = false;

    public bool loop = true;

    public bool atRes = false;

    public bool atStore = false;

    public bool hasRock;
    public bool hasWood;
    public bool hasGlod;

    public bool hasResource()
    {
        if (hasRock == true || hasWood == true || hasGlod == true)
        {
            return true;
        }

        return false;
    }
    void moveToResource()
    {
        if (movingToResource == false)
        {
            movingToResource = true;
        }
    }
    void moveToStoreHouse()
    {
        if (movingToStoreHouse == false)
        {
            movingToStoreHouse = true;
        }
    }
    void depositResource()
    {
        storedResource = true;
    }
    void collectResource()
    {
        hasWood = true;
    }
    bool atResource()
    {
        Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        Vector3 targetPos = new Vector3(positionOfResource.x, positionOfResource.y, positionOfResource.z);

        if (Vector3.Distance(pos, targetPos) < 2.0f)
        {
            movingToResource = true;
            return true;
        }
        else
            return false;


    }
    bool atStoreHouse()
    {
        Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        Vector3 targetPos = new Vector3(positionOfStoreHouse.x, positionOfStoreHouse.y, positionOfStoreHouse.z);

        if (Vector3.Distance(pos, targetPos) < 2.0f)
        {
            movingToStoreHouse = false;
            return true;
        }
        else
            return false;
    }
    void gatherResource()
    {

    }
    void resetAction()
    {
        movingToResource = false;
        collectingResource = false;
        movingToStoreHouse = false;
        storedResource = false;
    }



    public void doAction()
    {
        atRes = atResource();
        atStore = atStoreHouse();
        if (atResource() == false && movingToResource == false)
            moveToResource();
        if (atResource() == true && collectingResource == false)
            gatherResource();
        if (atStoreHouse() == false && movingToStoreHouse == false)
            moveToStoreHouse();
        if (atStoreHouse() == true && storedResource == false)
            depositResource();


    }
}