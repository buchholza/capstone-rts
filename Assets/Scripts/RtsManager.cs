using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsManager : MonoBehaviour {
    public static RtsManager current = null;

    public Collider groundCollider;

    public Vector3? ScreenPointToMapPosition(Vector2 point) {
        var ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;
        if(!groundCollider.Raycast(ray, out hit, Mathf.Infinity)) {
            return null;
        }

        return hit.point;
    }

	// Use this for initialization
	void Start () {
        current = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
