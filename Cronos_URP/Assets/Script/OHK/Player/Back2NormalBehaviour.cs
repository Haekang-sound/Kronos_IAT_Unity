using UnityEngine;

/// <summary>
/// ��ȭ���¸� �����ϴ� �ൿ�� �����ϴ� Ŭ����
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
