using UnityEngine;

/// <summary>
/// BTypeEnem�� ���� ���� ��ȯ�� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class BTypeEnemySMBIdle : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("IDLE");
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.FindTarget();

        // Ÿ�� �߰� ��
        GameObject currentTarget = _monoBehaviour.CurrentTarget;
        if (currentTarget != null)
        {
            // ���� ���·� ����
            _monoBehaviour.TriggerPursuit();
        }
        else
        {
            // Idle - ������ ����
            if (_monoBehaviour.IsNearBase() == false)
            {
                _monoBehaviour.TriggerReturn();
            }
        }
    }
}