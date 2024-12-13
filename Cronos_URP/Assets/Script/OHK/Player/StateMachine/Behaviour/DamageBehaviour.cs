using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ǰݴ����� ���� �ൿ�� �����ϴ� Ŭ����
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
