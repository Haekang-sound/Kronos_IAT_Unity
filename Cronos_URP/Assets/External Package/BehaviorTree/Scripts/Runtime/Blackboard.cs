using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blackboard
{
    [HideInInspector]
    public GameObject monobehaviour;

    public GameObject target;
    public Vector3 moveToPosition;
}