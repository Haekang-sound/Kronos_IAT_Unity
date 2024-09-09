using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	public GameObject timeline;

    public bool isActived;

    private void Start()
    {
        timeline.SetActive(false);
    }

    public void PlayTimeline()
    {
        if (isActived == false)
        {
            ActiveTimeline(true);
            isActived = true;
        }
    }

    private void ActiveTimeline(bool val)
    {
        timeline.SetActive(val);
    }
}
