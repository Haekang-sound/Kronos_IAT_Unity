using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미닫이 문을 위한 클래스
/// 
/// ohk    v1
/// </summary>
public class NewSlideDoor : MonoBehaviour
{
	[SerializeField] private Transform _left;
	[SerializeField] private Transform _right;

	public bool isOpen;

	[SerializeField] private float _speed = 1f;
	[SerializeField] private float _distance = 3f;
	[SerializeField] private float _currentDistance = 0f;

	private void Update()
	{
		if (isOpen)
		{
			if (_currentDistance < _distance)
			{
				_currentDistance += Time.deltaTime * _speed;

				Vector3 tempL = _left.localPosition;
				Vector3 tempR = _right.localPosition;

				tempL.x += _currentDistance;
				tempR.x -= _currentDistance;
				_left.localPosition = tempL;
				_right.localPosition = tempR;
			}

			
		}
	}

	public void OpenSlideDoor()
	{
		isOpen = true; 
	}

}
