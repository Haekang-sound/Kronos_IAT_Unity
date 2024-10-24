using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Collider2 : MonoBehaviour
{
    public int regionNum;

    UIManager um;

    // Start is called before the first frame update
    void Start()
    {
        um = UIManager.Instance;
        um.StartRegion(regionNum);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject == Player.Instance.gameObject)
    //    {
    //        um.StartRegion(regionNum);
    //    }
    //}
}
