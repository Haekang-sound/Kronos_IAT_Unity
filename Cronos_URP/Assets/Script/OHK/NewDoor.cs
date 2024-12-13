using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 여닫이 문을 위한 클래스
/// 
/// ohk    v1
/// </summary>
public class NewDoor : MonoBehaviour
{
	[SerializeField] private Transform _left;
	[SerializeField] private Transform _right;

	public bool isOpen;

	private float _degree;
	
	[SerializeField] private float _speed = 1f;
	[SerializeField] private float _time = 1f;

	private void Update()
	{
		if (isOpen)
		{
			if (_degree < _time)
			{
				_degree += Time.deltaTime * _speed;
				_left.Rotate(new Vector3(0, 1, 0), -_degree);
				_right.Rotate(new Vector3(0, 1, 0), _degree);
			}

			
		}
	}

	public void OpenDoor()
	{
		isOpen = true;
		SoundManager.Instance.PlaySFX("Effect_StoneGate_Sound_SE", transform);
	}

}
