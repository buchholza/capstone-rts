using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBuilding : ActionBehavior {
    public GameObject ghostPrefab;
    public GameObject buildingPrefab;
    public float maxBuildDistance = 30;

    private GameObject active = null;

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

    void Start() {
        var go = GameObject.Instantiate(ghostPrefab);
        var finder = go.AddComponent<FindBuildingSite>();
        finder.buildingPrefab = buildingPrefab;
        finder.maxBuildDistance = maxBuildDistance;
        //finder.info = GetComponent<Player>().info;
        finder.source = transform;
        active = go;
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
