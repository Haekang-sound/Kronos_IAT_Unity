using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BossNameShaker : MonoBehaviour
{
	public bool isShaking {  get; set; }
	[SerializeField] Image chainUI;
	// Start is called before the first frame update

	Vector3 _currnetPosition;

	// Update is called once per frame
	void Update()
	{
		
		if (isShaking)
		{
			Shake();
			chainUI.enabled = true;
		}
		else
		{
			_currnetPosition = transform.position;
			chainUI.enabled = false;
		}
	}

	private void Shake()
	{
		Vector3 temp = transform.position;
		temp.x = UnityEngine.Random.Range(0.99f, 1.01f) * _currnetPosition.x;
		temp.y = UnityEngine.Random.Range(0.99f, 1.01f) * _currnetPosition.y;
		transform.position = temp;	
	}
}
