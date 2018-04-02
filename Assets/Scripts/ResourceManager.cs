using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public int food = 0;
    public int glod = 0;
    public int metal = 1000;
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
            case "Glod":
                if(glod>amount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case "metal":
                if(metal>amount)
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
        case "glod":
            glod += amount;
            break;
        case "metal":
             metal += amount;
             break;
        default:
            break;
    }
}
}
