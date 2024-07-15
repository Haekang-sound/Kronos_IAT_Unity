using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATypeEnemySMBDamaged : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("DAMAGED");
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var currentTarget = _monoBehaviour.CurrentTarget;

        if (currentTarget != null)
        {
            // STRAFE - ���� �ȿ� ���� ��
            Vector3 toTarget = currentTarget.transform.position - _monoBehaviour.transform.position;
            float strafeDistance = _monoBehaviour.strafeDistance;
            if (toTarget.sqrMagnitude < strafeDistance * strafeDistance)
            {
                _monoBehaviour.TriggerStrafe();
            }
            // PURSUIT - ���� ���� �� ��
            else
            {
                _monoBehaviour.StartPursuit();
            }
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.UseBulletTimeScale();
    }
}