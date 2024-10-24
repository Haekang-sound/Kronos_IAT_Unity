using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTransition : MonoBehaviour
{
	public TransitionPoint transitionPoint;
	public Collider Collider;
	public GameObject obj;
	// Update is called once per frame
	private void Update()
	{
		if(obj == null)
		{
			transitionPoint.enabled = true;
			Collider.enabled = true;
		}
	}
	public void EnableTransition()
	{
		
	}
}
