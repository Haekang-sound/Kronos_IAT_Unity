using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleColliderData
{
	public CapsuleCollider collider { get; private set; }
	public Vector3 colliderCenterInLocalSpace { get; private set; }

	public void Initialize(GameObject gameObject)
	{
		if (collider != null)
		{
			return;
		}

		collider = gameObject.AddComponent<CapsuleCollider>();
		UpdateColliderData();

	}

	private void UpdateColliderData()
	{
		colliderCenterInLocalSpace = collider.center;
	}
}
