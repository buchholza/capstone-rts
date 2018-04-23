using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitAttribute : MonoBehaviour {
    
    public enum UnitType { NormalUnit, PitchforkUnit, SwordUnit, SpartanUnit, Capitol, Barracks, Tower};

    [HideInInspector]
    public GameObject selectionCircle;

    // Called when selected
    public void OnSelect() { }

    // Called when deselected
    public void OnDeselect() { }

    public int LOS;
    public float health = 20.0f;
    public float maxHealth = 20.0f;
    private bool isAttacked;
    public Component Aggression;
    public WanderNPC wanderNPC;
    public bool isPlayerControlled = true;
    // public int team = 0;
    public UnitType type = UnitType.NormalUnit;

    public Image healthBarFill;
    public Transform healthBarCanvas;

    public GameObject winText;
    public GameObject lose;

    // Use this for initialization
    void Start () {
        if(isPlayerControlled == false && type == UnitType.NormalUnit) {
            wanderNPC = GetComponent<WanderNPC>();
            wanderNPC.enabled = true;
        }
        lose.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () { 
        if (health <= 0) {
            if (type == UnitType.Capitol && isPlayerControlled) {
                Time.timeScale = 0;
                // if (defaultPlayer) { // ???
                //     GameManager.current.lose.SetActive(true);
                // }
                //RtsManager.current.CapitolStatus(); // ???
            }
            Destroy(this.gameObject);
        }

        if (healthBarFill) healthBarFill.fillAmount = health / maxHealth;
        if (healthBarCanvas)
            healthBarCanvas.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        if (isPlayerControlled && type == UnitType.Capitol && health <= 0) {
            lose.SetActive(true);
        }
        else if (!isPlayerControlled && type == UnitType.Capitol && health <= 0) {
            winText.SetActive(true);
        }
    }

    public void beingAttacked(int damage) {
        health -= damage;
        //isAttacked = false;
    }
}
