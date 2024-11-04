using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSlideDoor : MonoBehaviour
{
	[SerializeField] private Transform left;
	[SerializeField] private Transform right;

	public bool isOpen;


	[SerializeField] private float speed = 1f;
	[SerializeField] private float _distance = 3f;
	[SerializeField] private float _currentDistance = 0f;

	private void Update()
	{
		if (isOpen)
		{
			Debug.Log(_currentDistance);
			if (_currentDistance < _distance)
			{
				_currentDistance += Time.deltaTime * speed;

				Vector3 tempL = left.localPosition;
				Vector3 tempR = right.localPosition;

				tempL.x += _currentDistance;
				tempR.x -= _currentDistance;
				left.localPosition = tempL;
				right.localPosition = tempR;
			}

			
		}
	}

	public void OpenSlideDoor()
	{
		isOpen = true; 
	}

}
