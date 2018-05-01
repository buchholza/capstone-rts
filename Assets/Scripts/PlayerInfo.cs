using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2235:MarkAllNonSerializableFields")]
    public Color accentColor;
    public bool isAI;
    public int wood = 60;
    public int stone = 60;
    public bool hasCapitol;
    public List<GameObject> units = new List<GameObject>();
    public List<GameObject> buildings = new List<GameObject>();
    public int researchLevel = 1;
}
