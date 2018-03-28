using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBuilding1 : MonoBehaviour {
    public int version;
    public GameObject up0;
    public GameObject up1;
    public GameObject up2;

    // Use this for initialization
    void Start () {
        version = 0;

	}
	
	// Update is called once per frame
	void Update () {
        if (version == 0) {
            up0.SetActive(true);
            up1.SetActive(false);
            up2.SetActive(false);
        }
        else if (version == 1) {
            up0.SetActive(false);
            up1.SetActive(true);
            up2.SetActive(false);
        }
        else if (version == 2) {
            up0.SetActive(false);
            up1.SetActive(false);
            up2.SetActive(true);
        }
    }
}
