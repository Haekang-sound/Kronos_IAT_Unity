 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MoveTreeBehaviour : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;

	//private readonly int animSpeedHash = Animator.StringToHash("animSpeed");
	//[SerializeField] float animSpeed = 1f;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine = PlayerStateMachine.GetInstance();
		stateMachine.SwitchState(new PlayerMoveState(stateMachine));
		stateMachine.AutoTargetting.target = null;
		animator.ResetTrigger(PlayerHashSet.Instance.Dodge);
		animator.ResetTrigger(PlayerHashSet.Instance.Respawn);
		animator.ResetTrigger(PlayerHashSet.Instance.Death);
		
// 		if(!Player.Instance.isBuff)
// 		{
// 			Player.Instance.IsEnforced = false;
// 			EffectManager.Instance.SwordAuraOff();
// 		}

	}

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		//animator.SetFloat(animSpeedHash, animSpeed);

	}
	//OnStateExit is called when a transition ends and the state machine finishes evaluating this state
// 	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
// 	{
// 		//animator.speed = 1f;
// 	}

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
