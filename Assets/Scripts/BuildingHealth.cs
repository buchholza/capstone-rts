using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHealth : MonoBehaviour {

    public float health;
    public bool isCapitol;
    public  GameObject lose;
    public bool defaultPlayer;

	// Use this for initialization
	void Start () {
        lose.SetActive(false);
      
	}
	
	// Update is called once per frame
	void Update () {
        CheckHealth();
	}

    public void TakeDamage(float damage) {
        health -= damage;
        CheckHealth();
    }

    public void CheckHealth() {
        if (health <= 0) {
            if (isCapitol) {
                //the player loses
                
                Time.timeScale = 0;
                if (defaultPlayer) { lose.SetActive(true); }
                RtsManager.current.CapitolStatus();
            }
            gameObject.SetActive(false);
        }
    }
}
