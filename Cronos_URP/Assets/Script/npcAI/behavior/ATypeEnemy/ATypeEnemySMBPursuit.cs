using UnityEngine.AI;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ATypeEnemySMBPursuit : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public bool sAttack;

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("PURSUIT");

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

        if (_monoBehaviour.CurrentTarget != null )
        {
            _monoBehaviour.RequestTargetPosition();

            // STRONG ATTACK - ���� ��Ÿ� �ȿ� �ְ�, SAttack���� �̵��ؾ� �ϴ� ���
            if (_monoBehaviour.IsInStrongAttackRange() && sAttack == true)
            {
                _monoBehaviour.TriggerStrongAttack();
            }
            // ATTACK - ���� ��Ÿ� �ȿ� ���� ��
            else if (_monoBehaviour.IsInAttackRange())
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