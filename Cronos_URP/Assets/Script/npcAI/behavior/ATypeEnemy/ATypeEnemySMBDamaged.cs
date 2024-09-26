using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATypeEnemySMBDamaged : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ResetAiming();
        _monoBehaviour.ChangeDebugText("DAMAGED");

        _monoBehaviour.SetUseKnockback(true);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var currentTarget = _monoBehaviour.CurrentTarget;

        if (currentTarget != null)
        {
            // STRAFE - 범위 안에 있을 때
            Vector3 toTarget = currentTarget.transform.position - _monoBehaviour.transform.position;
            float strafeDistance = _monoBehaviour.strafeDistance;
            if (toTarget.sqrMagnitude < strafeDistance * strafeDistance)
            {
                _monoBehaviour.TriggerStrafe();
            }
            // PURSUIT - 범위 보다 멀 때
            else
            {
                _monoBehaviour.StartPursuit();
            }
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.UseBulletTimeScale();
        _monoBehaviour.SetUseKnockback(false);
    }
}