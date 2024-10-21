using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TimeSlashReadyBehaviour : StateMachineBehaviour
{
	float currentTime = 0f;
	bool timeSlash;
	PlayerStateMachine stateMachine;
	[SerializeField] public AnimationCurve TimeSlashCurve;
	// OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		
		stateMachine = PlayerStateMachine.GetInstance();
		stateMachine.SwitchState(new PlayerTimeSlashState(stateMachine));




	}

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
// 
// 		if (timeSlash)
// 		{
// 			currentTime += Time.unscaledDeltaTime;
// 			Player.Instance.currentTime = currentTime;
// 			PlayerStateMachine.GetInstance().Rigidbody.velocity 
// 				= Player.Instance.transform.forward * TimeSlashCurve.Evaluate(currentTime);
// 
// 			Debug.Log(Player.Instance.TimeSlashCurve.Evaluate(currentTime));
// 			Debug.Log(Player.Instance.transform.forward);
// 			if (currentTime > 1f)
// 			{
// 				timeSlash = false;
// 				currentTime = 0f;
// 			}
// 		}
// 		else
// 		{
// 			animator.SetTrigger("TimeSlash");
// 		}
	}

	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}

	// OnStateMove is called before OnStateMove is called on any state inside this state machine
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateIK is called before OnStateIK is called on any state inside this state machine
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateMachineEnter is called when entering a state machine via its Entry Node
	//override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	//{
	//    
	//}

	// OnStateMachineExit is called when exiting a state machine via its Exit Node
	//override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	//{
	//    
	//}
}
