using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오토타겟팅 사용을 
/// 애니메이션단위에서 관리하기 위한 클래스
/// 
/// ohk    v1
/// </summary>
public class AutoTargetingEnableBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		PlayerStateMachine.GetInstance().autoTargetting.enabled = true;
		PlayerStateMachine.GetInstance().autoTargetting.sphere.enabled = true;
	}
}
