using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTypeEnemySMBReload : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("RELOAD");

    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.FindTarget();

        if (_monoBehaviour.CurrentTarget != null)
        {

            // AIM - Ÿ���� ���� ��Ÿ� �ȿ� ���� ��
            if (_monoBehaviour.IsInAttackRange())
            {
                _monoBehaviour.TriggerAim();
            }
            // PURSUIT - Ÿ���� ���� ��Ÿ� �ȿ� ���� ��
            else
            {
                _monoBehaviour.TriggerPursuit();
            }
        }
        // IDLE - Ÿ���� ���� ��
        else
        {
            _monoBehaviour.TriggerIdle();
        }
    }
}