using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ATypeEnemySMBReturn : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("RETURN");

        _monoBehaviour.Controller.SetFollowNavmeshAgent(true);
        _monoBehaviour.Controller.SetTarget(_monoBehaviour.BasePosition);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        _monoBehaviour.FindTarget();

        // 타깃 발견 시
        GameObject currentTarget = _monoBehaviour.CurrentTarget;
        if (currentTarget != null)
        {
            // 추적 상태로 전이
            _monoBehaviour.StartPursuit();
        }
        else
        {
            // 목적지 도착 시 Idle 상태 진입
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
