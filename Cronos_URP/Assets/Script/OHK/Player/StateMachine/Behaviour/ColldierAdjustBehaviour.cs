using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 동작 무기충돌범위를 지정하는 클래스
/// 
/// ohk    v1
/// </summary>
public class ColldierAdjustBehaviour : StateMachineBehaviour
{
	public bool drawGizmos;
	[SerializeField] private Vector3 _center = Vector3.zero;
	[SerializeField] private Vector3 _size = Vector3.zero;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (_size == Vector3.zero)
		{
			Player.Instance.meleeWeapon.bAdjuster.Reset();

		}
		else
		{
			Player.Instance.meleeWeapon.bAdjuster.newCenter = _center;
			Player.Instance.meleeWeapon.bAdjuster.newSize = _size;
		}

		Player.Instance.meleeWeapon.bAdjuster.Adjust();

	}
}
