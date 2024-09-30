using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Collider : MonoBehaviour
{
    public GameObject LoaderUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            UIManager.Instance.StartCoroutine(UIManager.Instance.FadeRegionAlpha());
            Destroy(gameObject);
        }

    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.name == "Player")
    //    {
    //        UIManager.Instance.StartCoroutine(UIManager.Instance.AchieveSubObjective());
    //    }
    //}
}
