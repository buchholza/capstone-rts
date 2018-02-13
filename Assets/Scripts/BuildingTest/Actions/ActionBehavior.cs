using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBehavior : MonoBehaviour {
    public Sprite ButtonPic;

    public abstract Action GetClickAction();
}
