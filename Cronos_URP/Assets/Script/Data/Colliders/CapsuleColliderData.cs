 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleColliderData
{
	public CapsuleCollider Collider {  get; private set; }
	public Vector3 ColliderCenterInLocalSpace { get; private set; }

	public void Initialize(GameObject gameobject)
	{
		if(Collider != null)
		{
			return;
		}	

		Collider = gameobject.GetComponent<CapsuleCollider>();
		UpdateColliderData();
	}

	public void UpdateColliderData()
	{
		ColliderCenterInLocalSpace = Collider.center;
	}
}
