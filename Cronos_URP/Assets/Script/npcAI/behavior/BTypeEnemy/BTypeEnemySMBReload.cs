using UnityEngine;

/// <summary>
/// BTypeEnem의 재장전 전환을 관리하는 클래스입니다.
/// </summary>
public class BTypeEnemySMBReload : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("RELOAD");

    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.FindTarget();

        if (_monoBehaviour.CurrentTarget != null)
        {

            // AIM - 타겟이 공격 사거리 안에 있을 때
            if (_monoBehaviour.IsInAttackRange())
            {
                _monoBehaviour.TriggerAim();
            }
            // PURSUIT - 타겟이 공격 사거리 안에 없을 때
            else
            {
                _monoBehaviour.TriggerPursuit();
            }
        }
        // IDLE - 타겟이 없을 때
        else
        {
            _monoBehaviour.TriggerIdle();
        }
    }
}