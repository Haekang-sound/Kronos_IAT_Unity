using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCombatZoneEnemySMBIdle : SceneLinkedSMB<TestCombatZoneEnemyBehavior>
{
    public float minimumIdleGruntTime = 2.0f;
    public float maximumIdleGruntTime = 5.0f;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (minimumIdleGruntTime > maximumIdleGruntTime)
            minimumIdleGruntTime = maximumIdleGruntTime;

    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        _monoBehaviour.FindTarget();

        if (_monoBehaviour.target != null)
        {
            Vector3 toTarget = _monoBehaviour.target.transform.position - _monoBehaviour.transform.position;

            if (toTarget.sqrMagnitude < _monoBehaviour.attackDistance * _monoBehaviour.attackDistance)
            {
                _monoBehaviour.TriggerAttack();
            }
            else
            {
                _monoBehaviour.StartPursuit();
            }
        }
    }
}