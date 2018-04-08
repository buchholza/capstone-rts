using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBuilding : MonoBehaviour {
    public GameObject upgradePrefab;

    private bool isSelecting = false;

    public void OnClick() {
        isSelecting = true;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(isSelecting && Input.GetMouseButtonDown(0)) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit) && hit.collider.gameObject.name.Contains("Building")) {
                hit.collider.gameObject.GetComponent<Player>().info.stone -= 100;
                Destroy(hit.collider.gameObject);
                GameObject.Instantiate(upgradePrefab, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);
                isSelecting = false;
            }
        }
    }
}
