using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public int food = 0;
    void Awake()
    {
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
            case "food":
                if (food > amount)
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
        case "food":
            food += amount;
            break;
        default:
            break;
    }
}
}
