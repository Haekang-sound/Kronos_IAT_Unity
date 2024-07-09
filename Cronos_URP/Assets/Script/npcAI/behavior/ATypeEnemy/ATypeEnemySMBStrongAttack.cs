using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATypeEnemySMBStrongAttack : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("STRONG ATTACK");
        _monoBehaviour.ResetTriggerDown();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Down - �÷��̾ ���� �丵�� ���� ���� ��
        /// TODO: ���� ���� ���
        if(false)
        {
            //_monoBehaviour.TriggerDown();
        }

        // Strafe - �÷��̾ ���� �и��� ���� ���� ��(�ִϸ��̼��� ��� ���� ��)
    }
}