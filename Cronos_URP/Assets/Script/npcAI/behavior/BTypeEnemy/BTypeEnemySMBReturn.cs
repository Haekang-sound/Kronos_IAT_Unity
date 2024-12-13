using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// BTypeEnem�� ���� ���� ���� �ڸ��� �ǵ��ư��� ���� ��ȯ�� �����ϴ� Ŭ�����Դϴ�.
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
        // ��θ� ã�� �� ���� ��
        if (_monoBehaviour.Controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathPartial
            || _monoBehaviour.Controller.navmeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            _monoBehaviour.TriggerIdle();
            return;
        }

        _monoBehaviour.FindTarget();

        // PURSUIT - Ÿ�� �߰� ��
        GameObject currentTarget = _monoBehaviour.CurrentTarget;
        if (currentTarget != null)
        {
            _monoBehaviour.TriggerPursuit();
        }
        // IDLE - ������ ���� ��
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
