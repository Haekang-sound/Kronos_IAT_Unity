using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CapsuleColliderUtility 
{
	public CapsuleColliderData CapsuleColliderData {  get; private set; }
	[SerializeField] public DefaultColliderData DefaultColliderData {  get; private set; }
	[SerializeField] public SlopeData slopeData {  get; private set; }

	public void Initialize(GameObject gameObject)
	{
		if(CapsuleColliderData != null)
		{
			return; 
		}
		CapsuleColliderData = new CapsuleColliderData();

		CapsuleColliderData.Initialize(gameObject);
	}

	public void CalculateCapsulcolliderDimentsions()
	{
		SetCapsulColliderRadius(DefaultColliderData.Radius);
		SetCapsulColliderHeight(DefaultColliderData.Hieght*(1f - slopeData.StepHeightPercentage));
		RecalculateCapsuleColliderCenter();
	}

	public void SetCapsulColliderRadius(float radius)
	{
		CapsuleColliderData.Collider.radius = radius;
	}

	public void SetCapsulColliderHeight(float height)
	{
		CapsuleColliderData.Collider.height = height;
	}

	public void RecalculateCapsuleColliderCenter()
	{
		float colliderHeightDifference = DefaultColliderData.Hieght - CapsuleColliderData.Collider.height;
	
		Vector3 newColliderCenter = new Vector3(0f, DefaultColliderData.CenterY + (colliderHeightDifference / 2f), 0f);

		CapsuleColliderData.Collider.center = newColliderCenter;

		float halfColliderHeight = CapsuleColliderData.Collider.height / 2f;

		if(CapsuleColliderData.Collider.height/2f<CapsuleColliderData.Collider.radius)
		{
			SetCapsulColliderRadius(halfColliderHeight);
		}

		CapsuleColliderData.UpdateColliderData();
	}

}
