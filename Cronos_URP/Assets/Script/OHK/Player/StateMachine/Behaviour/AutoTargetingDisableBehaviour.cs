using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오토타겟팅 해제를 
/// 애니메이션단위에서 관리하기 위한 클래스
/// 
/// ohk    v1
/// </summary>
public class AutoTargetingDisableBehaviour : StateMachineBehaviour
{
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().autoTargetting.enabled = false;
		PlayerStateMachine.GetInstance().autoTargetting.sphere.enabled = false;
	}
}
