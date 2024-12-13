using UnityEngine;

/// <summary>
/// BTypeEnem의 유휴 상태 전환을 관리하는 클래스입니다.
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

        // 타깃 발견 시
        GameObject currentTarget = _monoBehaviour.CurrentTarget;
        if (currentTarget != null)
        {
            // 추적 상태로 전이
            _monoBehaviour.TriggerPursuit();
        }
        else
        {
            // Idle - 목적지 도착
            if (_monoBehaviour.IsNearBase() == false)
            {
                _monoBehaviour.TriggerReturn();
            }
        }
    }
}