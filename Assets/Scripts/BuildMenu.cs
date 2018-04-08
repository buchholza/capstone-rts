using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour {

    public GameObject buildMenu;

    public void BuildMenuToggle () {
        if (buildMenu.activeSelf) {
            buildMenu.SetActive(false);
        } else {
            buildMenu.SetActive(true);
        }
    }
}
