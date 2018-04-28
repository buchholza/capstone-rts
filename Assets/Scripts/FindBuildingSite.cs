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

        // TODO: Find a better way to keep buildings above ground
        transform.position = tempTarget.Value + new Vector3(0, 0, 0);

        if(Vector3.Distance(transform.position, source.position) > maxBuildDistance) {
            rend.material.color = red;
            return;
        }

        if(RtsManager.current.IsGameObjectSafeToPlace(gameObject)) {
            rend.material.color = green;
            if(Input.GetMouseButtonDown(0)) {
                var go = GameObject.Instantiate(buildingPrefab);
                go.transform.position = transform.position;
                // Player.defaultPlayer.currency -= 200;
                go.AddComponent<Player>().info = Player.defaultPlayer;
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
