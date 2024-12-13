using UnityEngine.AI;
using UnityEngine;

/// <summary>
/// BTypeEnem�� ���� ���� ��ȯ�� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class BTypeEnemySMBPursuit : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("PURSUIT");
        
        _monoBehaviour.Controller.SetFollowNavmeshAgent(true);

        _monoBehaviour.SetFollowerDataRequire(true);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.FindTarget();

        // ��θ� ã�� �� ���� ��
        if (_monoBehaviour.Controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathPartial
            || _monoBehaviour.Controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            _monoBehaviour.TriggerIdle();
            return;
        }

        if (_monoBehaviour.CurrentTarget != null)
        {
            _monoBehaviour.RequestTargetPosition();

            // ATTACK - ���� ��Ÿ� �ȿ� ���� ��
            if (_monoBehaviour.IsInAttackRange())
            {
                _monoBehaviour.TriggerAim();
            }
            // PURSUIT - ���� ��ġ�� �Ҵ� �޾��� ��
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
        // IDLE - �� ��(���� �÷��̾� ����� ������)
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