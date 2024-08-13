using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	public GameObject timeline;
    public CombatZone combatZone;

    private bool _actived;

    private void Start()
    {
        timeline.SetActive(false);
    }

    public void ActiveTimeline(bool val)
	{
        timeline.SetActive(val);
	}

    public void Update()
    {
        if (_actived == false && combatZone.isClear == true) 
        {
            ActiveTimeline(true);
            _actived = true;
        }
    }
}
