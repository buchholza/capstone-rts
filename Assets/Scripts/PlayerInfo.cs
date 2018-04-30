using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo {
    public Color accentColor;
    public bool isAI;
    public int wood;
    public int stone;
    public bool hasCapitol;
    public List<GameObject> units = new List<GameObject>();
    public int researchLevel = 1;
}
