using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBuilding : ActionBehavior {
    public GameObject ghostPrefab;

    private GameObject active = null;

    public override System.Action GetClickAction() {
        return delegate () {
            var go = GameObject.Instantiate(ghostPrefab);
            go.AddComponent<FindBuildingSite>();
            active = go;
        };
    }

    void Start() {
        var go = GameObject.Instantiate(ghostPrefab);
        go.AddComponent<FindBuildingSite>();
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
