using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void Callback();

[System.Serializable]
public class Action {
    public Image buttonPicture;
    public Callback callback;
}
