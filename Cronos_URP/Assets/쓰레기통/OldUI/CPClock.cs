using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPClock : MonoBehaviour
{
    [SerializeField]
    private GameObject clockHandle;
    public Material mat;

    [SerializeField]
    private float CPAmount;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        clockHandle.transform.localRotation = Quaternion.Euler(0, 0, CPAmount * -360);
    }
}
