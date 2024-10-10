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

        // Damaged - ���¿��� �ǰ� ������ ��
        _monoBehaviour.ResetTriggerDamaged();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.FindTarget();

        // ��θ� ã�� �� ���� ��
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
                // ATTACK - ���� ��Ÿ� �ȿ� ���� ��
                if (_monoBehaviour.IsInMeleeAttackRange())
                {
                    _monoBehaviour.TriggerAttack();
                }
                // PURSUIT - ���� ��ġ�� �Ҵ� �޾��� ��
                else if (_monoBehaviour.FollowerData.assignedSlot != -1)
                {
                    Vector3 targetPoint = _monoBehaviour.FollowerData.requiredPoint;

                    _monoBehaviour.Controller.SetTarget(targetPoint);
                }
                else // Strafe - Ÿ���� ã�� �� ���� ��
                {
                    _monoBehaviour.TriggerStrafe();
                }
            }
            // STRONG ATTACK - ���� ��Ÿ� �ȿ� �ְ�, SAttack���� �̵��ؾ� �ϴ� ���
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
        // IDLE - �� ��(���� �÷��̾� ����� ������)
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