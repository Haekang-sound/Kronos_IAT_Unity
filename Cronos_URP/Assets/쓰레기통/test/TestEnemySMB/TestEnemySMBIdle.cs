using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySMBIdle : SceneLinkedSMB<TestEnemyBehavior>
{
    public float minimumIdleGruntTime = 2.0f;
    public float maximumIdleGruntTime = 5.0f;

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (minimumIdleGruntTime > maximumIdleGruntTime)
            minimumIdleGruntTime = maximumIdleGruntTime;

        if (_monoBehaviour.Target != null)
        {
            Vector3 toTarget = _monoBehaviour.Target.transform.position - _monoBehaviour.transform.position;

            if (toTarget.sqrMagnitude < _monoBehaviour.attackDistance * _monoBehaviour.attackDistance)
            {
                _monoBehaviour.TriggerAttack();
                _monoBehaviour.StopPursuit();
            }
            else
            {
                _monoBehaviour.StartPursuit();
            }
        }

    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        _monoBehaviour.FindTarget();

        if (_monoBehaviour.Target != null)
        {
            Vector3 toTarget = _monoBehaviour.Target.transform.position - _monoBehaviour.transform.position;

            if (toTarget.sqrMagnitude < _monoBehaviour.attackDistance * _monoBehaviour.attackDistance)
            {
                _monoBehaviour.TriggerAttack();
                _monoBehaviour.StopPursuit();
            }
            else
            {
                _monoBehaviour.StartPursuit();
            }
        }

    }
}