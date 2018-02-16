using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBuildingSite : MonoBehaviour {
    public float maxBuildDistance = 30;
    public GameObject buildingPrefab;
    //public PlayerSetupDefinition info;
    public Transform source;

    private Renderer rend;
    private Color red = new Color(1, 0, 0, 0.3f);
    private Color green = new Color(0, 1, 0, 0.3f);

    void Start() {
        //MouseManager.current.enabled = false;

        rend = GetComponent<Renderer>();
    }

	// Update is called once per frame
	void Update () {
        var tempTarget = RtsManager.current.ScreenPointToMapPosition(Input.mousePosition);
        if(tempTarget.HasValue == false) {
            return;
        }

        transform.position = tempTarget.Value;

        if(Vector3.Distance(transform.position, source.position) > maxBuildDistance) {
            rend.material.color = red;
            return;
        }

        if(RtsManager.current.IsGameObjectSafeToPlace(gameObject)) {
            rend.material.color = green;
            if(Input.GetMouseButtonDown(0)) {
                var go = GameObject.Instantiate(buildingPrefab);
                go.transform.position = transform.position;
                //go.AddComponent<Player>().info = info;
                Destroy(this.gameObject);
            }
        } else {
            rend.material.color = red;
        }
    }

    void OnDestroy() {
        //MouseManager.current.enabled = true;
    }
}
