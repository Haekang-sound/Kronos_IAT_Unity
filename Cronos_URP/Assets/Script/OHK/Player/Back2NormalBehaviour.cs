using UnityEngine;

/// <summary>
/// 강화상태를 해제하는 행동을 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class Back2NormalBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Player.Instance.isBuff = false;
		Player.Instance.IsEnforced = false;
		EffectManager.Instance.SwordAuraOff();
	}
}
