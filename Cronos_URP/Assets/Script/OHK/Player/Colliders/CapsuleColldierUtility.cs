using System;
using UnityEngine;

[Serializable]
public class CapsuleColldierUtility
{
	public CapsuleColliderData CapsuleColliderData { get; private set; }
	[field: SerializeField] public DefaultColliderData DefaultColliderData { get; private set; }
	[field: SerializeField] public SlopeData SlopeData { get; private set; }

	public void Initialize(GameObject gameObject)
	{
		if(CapsuleColliderData != null)
		{
			return;
		}

		CapsuleColliderData = new CapsuleColliderData();
		CapsuleColliderData.Initialize(gameObject);
	}

	public void CalculateCapsuleColliderDimensions()
	{
		SetCapsuleColliderRadius(DefaultColliderData.Radius);
		SetCapsulecolliderHeight(DefaultColliderData.Height);// * (1f - SlopeData.StepHeightPercentage));
		RecalculateCapsuleColliderCenter();
		float halfColiderHeight = CapsuleColliderData.Collider.height / 2f;
		if(halfColiderHeight < CapsuleColliderData.Collider.radius)
		{
			SetCapsuleColliderRadius(halfColiderHeight);
		}
		CapsuleColliderData.UpdateColliderData();
	}

	public void SetCapsuleColliderRadius(float radius)
	{
		CapsuleColliderData.Collider.radius = radius;
	}

	private void SetCapsulecolliderHeight(float height)
	{
		CapsuleColliderData.Collider.height = height;
	}

	public void RecalculateCapsuleColliderCenter()
	{
		float colliderHeightDifference = DefaultColliderData.Height - CapsuleColliderData.Collider.height;
		Vector3 newColliderCenter
			= new Vector3(0f, DefaultColliderData.CeneterY + (colliderHeightDifference / 2f), 0f);
		CapsuleColliderData.Collider.center = newColliderCenter;
	}
}
