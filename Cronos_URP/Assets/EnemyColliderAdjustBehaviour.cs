using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyColliderAdjustBehaviour : SceneLinkedSMB<ATypeEnemyBehavior>
{
	public bool drawGizmos;
	Vector3 OriginCenter;
	Vector3 OriginSize;
	[SerializeField] Vector3 Center = Vector3.zero;
	[SerializeField] Vector3 Size = Vector3.zero;
	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (Size == Vector3.zero)
		{
			_monoBehaviour.enemySword.GetComponentInChildren<BoxColliderAdjuster>().Reset();

		}
		else
		{
			_monoBehaviour.enemySword.GetComponentInChildren<BoxColliderAdjuster>().newCenter = Center;
			_monoBehaviour.enemySword.GetComponentInChildren<BoxColliderAdjuster>().newSize = Size;
		}

		_monoBehaviour.enemySword.GetComponentInChildren<BoxColliderAdjuster>().Adjust();

	}

	public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateExit(animator, stateInfo, layerIndex);
		_monoBehaviour.enemySword.GetComponentInChildren<BoxColliderAdjuster>().Reset();
		_monoBehaviour.enemySword.GetComponentInChildren<BoxColliderAdjuster>().newCenter = _monoBehaviour.enemySword.GetComponentInChildren<BoxColliderAdjuster>()._originalCenter;
		_monoBehaviour.enemySword.GetComponentInChildren<BoxColliderAdjuster>().newSize = _monoBehaviour.enemySword.GetComponentInChildren<BoxColliderAdjuster>()._originalSize;
	}

}
