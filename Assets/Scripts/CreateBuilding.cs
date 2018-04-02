using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBuilding : MonoBehaviour {
    public GameObject ghostPrefab;
    public GameObject buildingPrefab;
    public float maxBuildDistance = 30;
    public GameObject player;

    private GameObject active = null;

<<<<<<< HEAD:Assets/Scripts/BuildingTest/CreateBuilding.cs
    public override System.Action GetClickAction() {
        return delegate () {
            var go = GameObject.Instantiate(ghostPrefab);
            var finder = go.AddComponent<FindBuildingSite>();
            
            finder.buildingPrefab = buildingPrefab;
            finder.maxBuildDistance = maxBuildDistance;
            //finder.info = GetComponent<Player>().info;
            finder.source = transform;
            active = go;
        };
    }

=======
>>>>>>> 50456b2c968bf204f70fa5878ff621518126a53f:Assets/Scripts/CreateBuilding.cs
    public void OnClick() {
        
        var go = GameObject.Instantiate(ghostPrefab);
        var finder = go.AddComponent<FindBuildingSite>();
       
        
        
        finder.buildingPrefab = buildingPrefab;
        finder.maxBuildDistance = maxBuildDistance;
        //finder.info = GetComponent<Player>().info;
        finder.source = transform;
        active = go;
       
    }

    void Start() {
        
    }

    void Update() {
        if(active != null) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                GameObject.Destroy(active);
            }
        }
    }

    void OnDestroy() {
        if(active != null) {
            Destroy(active);
        }
    }
}
