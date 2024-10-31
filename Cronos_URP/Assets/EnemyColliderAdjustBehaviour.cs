using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyColliderAdjustBehaviour : SceneLinkedSMB<ATypeEnemyBehavior>
{
	public bool drawGizmos;
	Vector3 OriginCenter;
	Vector3 OriginSize;

	[Header("Collider")]
	[SerializeField] Vector3 Center = Vector3.zero;
	[SerializeField] Vector3 Size = Vector3.zero;

	[Header("DamageType")]
	public Damageable.DamageType damageType;

	[SerializeField]
	public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
// 		if (Size == Vector3.zero)
// 		{
// 			_monoBehaviour.enemySword.GetComponent<MeleeWeapon>().bAdjuster.Reset();
// 
// 		}
// 		else
// 		{
// 			_monoBehaviour.enemySword.GetComponent<MeleeWeapon>().bAdjuster.newCenter = Center;
// 			_monoBehaviour.enemySword.GetComponent<MeleeWeapon>().bAdjuster.newSize = Size;
// 		}
// 
// 		_monoBehaviour.enemySword.GetComponent<MeleeWeapon>().bAdjuster.Adjust();
// 
// 		_monoBehaviour.enemySword.GetComponent<MeleeWeapon>().simpleDamager.currentDamageType = damageType;
	}

	public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnSLStateExit(animator, stateInfo, layerIndex);
// 		_monoBehaviour.enemySword.GetComponent<MeleeWeapon>().bAdjuster.Reset();
// 		_monoBehaviour.enemySword.GetComponent<MeleeWeapon>().bAdjuster.newCenter = _monoBehaviour.enemySword.GetComponent<MeleeWeapon>().bAdjuster._originalCenter;
// 		_monoBehaviour.enemySword.GetComponent<MeleeWeapon>().bAdjuster.newSize = _monoBehaviour.enemySword.GetComponent<MeleeWeapon>().bAdjuster._originalSize;
	}

}
