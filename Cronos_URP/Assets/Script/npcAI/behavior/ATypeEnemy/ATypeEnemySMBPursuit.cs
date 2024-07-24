using UnityEngine.AI;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ATypeEnemySMBPursuit : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public enum PursuitFor
    {
        Normal,
        SAttack
    }
    public PursuitFor purpose;


    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("PURSUIT");

        _monoBehaviour.StartPursuit();
        _monoBehaviour.Controller.SetFollowNavmeshAgent(true);

        // Damaged - 상태에서 피격 당했을 때
        _monoBehaviour.ResetTriggerDamaged();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.FindTarget();

        // 경로를 찾을 수 없을 때
        if (_monoBehaviour.Controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathPartial
            || _monoBehaviour.Controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            _monoBehaviour.StopPursuit();
            return;
        }

        if (_monoBehaviour.CurrentTarget != null)
        {
            _monoBehaviour.RequestTargetPosition();

            if (purpose == PursuitFor.Normal)
            {
                // ATTACK - 공격 사거리 안에 있을 때
                if (_monoBehaviour.IsInAttackRange())
                {
                    _monoBehaviour.TriggerAttack();
                }
                // PURSUIT - 공격 위치를 할당 받았을 때
                else if (_monoBehaviour.FollowerData.assignedSlot != -1)
                {
                    Vector3 targetPoint = _monoBehaviour.FollowerData.requiredPoint;

                    _monoBehaviour.Controller.SetTarget(targetPoint);
                }
                else // Strafe - 타깃을 찾을 수 있을 때
                {
                    _monoBehaviour.TriggerStrafe();
                }
            }
            // STRONG ATTACK - 공격 사거리 안에 있고, SAttack으로 이동해야 하는 경우
            else if (purpose == PursuitFor.SAttack)
            {
                if (_monoBehaviour.IsInStrongAttackRange())
                {
                    _monoBehaviour.TriggerStrongAttack();
                }
                else if (_monoBehaviour.FollowerData.assignedSlot != -1)
                {
                    Vector3 targetPoint = _monoBehaviour.FollowerData.requiredPoint;

                    _monoBehaviour.Controller.SetTarget(targetPoint);
                }
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
    }
}