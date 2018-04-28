using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    //public static ResourceManager current = null;
    public int wood=200;
    public int stone=300;
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
                if(stone>amount)
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
            wood += amount;
            break;
        case "stone":
            stone += amount;
            break;
        
        default:
            break;
    }
}
}
