using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo {
    public string name;
    public Transform location;
    public Color accentColor;
    public List<GameObject> startingUnits = new List<GameObject>();
    public bool isAI;
    public float currency;
}
