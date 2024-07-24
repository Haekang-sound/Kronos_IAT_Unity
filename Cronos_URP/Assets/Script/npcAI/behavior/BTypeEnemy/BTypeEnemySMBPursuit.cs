using UnityEngine.AI;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BTypeEnemySMBPursuit : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("PURSUIT");
        
        _monoBehaviour.Controller.SetFollowNavmeshAgent(true);

        // Damaged - 상태에서 피격 당했을 때
        _monoBehaviour.ResetTriggerDamaged();

        _monoBehaviour.SetFollowerDataRequire(true);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.FindTarget();

        // 경로를 찾을 수 없을 때
        if (_monoBehaviour.Controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathPartial
            || _monoBehaviour.Controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            _monoBehaviour.TriggerIdle();
            return;
        }

        if (_monoBehaviour.CurrentTarget != null)
        {
            _monoBehaviour.RequestTargetPosition();

            // ATTACK - 공격 사거리 안에 있을 때
            if (_monoBehaviour.IsInAttackRange())
            {
                _monoBehaviour.TriggerAim();
            }
            // PURSUIT - 공격 위치를 할당 받았을 때
            else if (_monoBehaviour.FollowerData.assignedSlot != -1)
            {
                Vector3 targetPoint = _monoBehaviour.FollowerData.requiredPoint;

                _monoBehaviour.Controller.SetTarget(targetPoint);
            }
            else
            {
                //_monoBehaviour.TriggerAim();
            }
        }
        // IDLE - 그 외(아직 플레이어 사망은 미포함)
        else
        {
            _monoBehaviour.TriggerIdle();
        }
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.Controller.SetFollowNavmeshAgent(false);
        _monoBehaviour.SetFollowerDataRequire(false);
    }
}