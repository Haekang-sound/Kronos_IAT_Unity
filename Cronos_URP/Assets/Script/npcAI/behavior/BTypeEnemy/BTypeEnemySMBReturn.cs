﻿using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// BTypeEnem의 추적 이후 원래 자리로 되돌아가는 상태 전환을 관리하는 클래스입니다.
/// </summary>
public class BTypeEnemySMBReturn : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("RETURN");

        _monoBehaviour.Controller.SetFollowNavmeshAgent(true);
        _monoBehaviour.Controller.SetTarget(_monoBehaviour.BasePosition);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 경로를 찾을 수 없을 때
        if (_monoBehaviour.Controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathPartial
            || _monoBehaviour.Controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            _monoBehaviour.TriggerIdle();
            return;
        }

        _monoBehaviour.FindTarget();

        // PURSUIT - 타깃 발견 시
        GameObject currentTarget = _monoBehaviour.CurrentTarget;
        if (currentTarget != null)
        {
            _monoBehaviour.TriggerPursuit();
        }
        // IDLE - 목적지 도착 시
        else
        {
            Vector3 toBase = _monoBehaviour.BasePosition - _monoBehaviour.transform.position;
            toBase.y = 0;
            if (_monoBehaviour.IsNearBase() == true)
            {
                _monoBehaviour.TriggerIdle();
            }
        }
    }
    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.Controller.SetFollowNavmeshAgent(false);
    }
}
