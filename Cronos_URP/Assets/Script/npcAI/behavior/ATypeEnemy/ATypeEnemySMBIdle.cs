using UnityEngine;

public class ATypeEnemySMBIdle : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("IDLE");
        _monoBehaviour.StopPursuit();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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
            // Idle - 목적지 도착
            if (_monoBehaviour.IsNearBase() == false)
            {
                _monoBehaviour.TriggerReturn();
            }
        }
    }
}