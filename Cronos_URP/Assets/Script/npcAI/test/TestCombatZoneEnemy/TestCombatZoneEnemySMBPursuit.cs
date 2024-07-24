using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestCombatZoneEnemySMBPursuit : SceneLinkedSMB<TestCombatZoneEnemyBehavior>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        _monoBehaviour.FindTarget();

        if (_monoBehaviour.controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathPartial
            || _monoBehaviour.controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            _monoBehaviour.StopPursuit();
            return;
        }

        if (_monoBehaviour.target == null)
        {//if the target was lost or is respawning, we stop the pursit
            _monoBehaviour.StopPursuit();
        }
        else
        {
            _monoBehaviour.RequestTargetPosition();

            Vector3 toTarget = _monoBehaviour.target.transform.position - _monoBehaviour.transform.position;

            if (toTarget.sqrMagnitude < _monoBehaviour.attackDistance * _monoBehaviour.attackDistance)
            {
                _monoBehaviour.TriggerAttack();
            }
            else if (_monoBehaviour.followerData.assignedSlot != -1)
            {
                Vector3 targetPoint = _monoBehaviour.target.transform.position +
                    _monoBehaviour.followerData.distributor.GetDirection(_monoBehaviour.followerData
                        .assignedSlot) * _monoBehaviour.attackDistance * 0.9f;

                _monoBehaviour.controller.SetTarget(targetPoint);
                _monoBehaviour.StopLookAtTarget();
            }
            else
            {
                _monoBehaviour.StopPursuit();
            }
        }
    }
}
