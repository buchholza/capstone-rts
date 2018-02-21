using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStore : MonoBehaviour {
    

    // Use this for initialization
    
	
	// Update is called once per frame
	void Update () {
		
	}
    public void getNearestBuildingOfType(string buildingType, Vector3 position)
    {
        if(buildingType=="StoreHouse")
        {
            UnitMovement.me.moveToLocation(position);
        }
        else
        {
            UnitMovement.me.doNothing();
        }

    }
}
