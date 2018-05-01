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

            // check it is close enough to other buildings

            float minDist = Mathf.Infinity;
            float maxDist = Mathf.NegativeInfinity;

            foreach (GameObject buildingGo in RtsManager.current.teams[0].buildings) {
                Vector3 myLocation = transform.position;
                Vector3 itsLocation = buildingGo.transform.position;

                float thisDistance = Vector3.Distance(myLocation, itsLocation);
                if (thisDistance > maxDist) maxDist = thisDistance;
                if (thisDistance < minDist) minDist = thisDistance;
            }

            if (minDist < 9.0f && maxDist > 3.0f) {
                rend.material.color = green;
                if(Input.GetMouseButtonDown(0)) {
                    if (RtsManager.current.teams[0].stone >= 100) {

                        var go = GameObject.Instantiate(buildingPrefab);
                        go.transform.position = transform.position;
                        RtsManager.current.teams[0].buildings.Add(go);
                        RtsManager.current.teams[0].stone -= 100;
                        go.AddComponent<Player>().info = Player.defaultPlayer; // what is this for?
                    }
                    Destroy(this.gameObject);
                }
            } else {
                rend.material.color = red; // bad probably
                if(Input.GetMouseButtonDown(0)) {
                    Destroy(this.gameObject);
                }
            }

        } else {
            rend.material.color = red;
            if(Input.GetMouseButtonDown(0)) {
                Destroy(this.gameObject);
            }
        }
    }

    void OnDestroy() {
        //MouseManager.current.enabled = true;
    }
}
