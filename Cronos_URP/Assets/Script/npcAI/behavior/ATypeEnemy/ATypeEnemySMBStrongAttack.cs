using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATypeEnemySMBStrongAttack : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("STRONG ATTACK");
        _monoBehaviour.inAttack = true;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.LookAtTarget();

        // Down - 플레이어가 공격 페링에 성공 했을 때
        /// TODO: 추후 구현 요망
        if (false)
        {
            //_monoBehaviour.TriggerDown();
        }

        // Strafe - 플레이어가 공격 패링에 실패 했을 때(애니메이션이 모두 끝난 뒤)
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.EndAttack();
        _monoBehaviour.ResetAiming();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.inAttack = false;
    }
}