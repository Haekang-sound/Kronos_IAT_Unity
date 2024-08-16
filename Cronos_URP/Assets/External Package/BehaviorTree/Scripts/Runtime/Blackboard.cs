using System.Collections;
using UnityEngine;

[System.Serializable]
public class Blackboard
{
    [HideInInspector]
    public GameObject monobehaviour;

    public GameObject target;
    public Vector3 moveToPosition;
    public float timer;

    public void Update()
    {
        timer -= Time.deltaTime;
    }
}