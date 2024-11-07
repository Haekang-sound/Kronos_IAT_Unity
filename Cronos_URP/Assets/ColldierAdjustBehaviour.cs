using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColldierAdjustBehaviour : StateMachineBehaviour
{
	public bool drawGizmos;
	Vector3 OriginCenter;
	Vector3 OriginSize;
	[SerializeField] Vector3 Center = Vector3.zero;
	[SerializeField] Vector3 Size = Vector3.zero;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (Size == Vector3.zero)
		{
			Player.Instance.meleeWeapon.bAdjuster.Reset();

		}
		else
		{
			Player.Instance.meleeWeapon.bAdjuster.newCenter = Center;
			Player.Instance.meleeWeapon.bAdjuster.newSize = Size;
		}

		Player.Instance.meleeWeapon.bAdjuster.Adjust();

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		//Player.Instance.meleeWeapon.bAdjuster.Reset();
		//Player.Instance.meleeWeapon.bAdjuster.newCenter = Player.Instance.meleeWeapon.bAdjuster._originalCenter;
		//Player.Instance.meleeWeapon.bAdjuster.newSize = Player.Instance.meleeWeapon.bAdjuster._originalSize;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that processes and affects root motion
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}


}
