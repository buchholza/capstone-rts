using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Temporary script to demo selling buildings
public class SellBuilding : MonoBehaviour {
    private bool isSelecting = false;

    public void OnClick() {
        isSelecting = true;
    }

	// Update is called once per frame
	void Update () {
        if(isSelecting && Input.GetMouseButtonDown(0)) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit) && hit.collider.gameObject.name.Contains("Building")) {
                hit.collider.gameObject.GetComponent<Player>().info.currency += 200;
                Destroy(hit.collider.gameObject);
                isSelecting = false;
            }
        }
    }
}
