using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 피격당했을 때의 행동을 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class DamageBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		PlayerStateMachine.GetInstance().SwitchState(new PlayerDamagedState(PlayerStateMachine.GetInstance()));
		PlayerStateMachine.GetInstance().autoTargetting.target = null;
	}
}
