using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBuildingSite : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        var tempTarget = RtsManager.current.ScreenPointToMapPosition(Input.mousePosition);
        if(tempTarget.HasValue == true) {
            transform.position = tempTarget.Value;
        }
	}
}
