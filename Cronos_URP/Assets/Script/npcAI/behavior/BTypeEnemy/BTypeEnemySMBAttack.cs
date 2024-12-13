using UnityEngine;

/// <summary>
/// BTypeEnem의 공격 상태 전환을 관리하는 클래스입니다.
/// </summary>
public class BTypeEnemySMBAttack : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("ATTACK");
        //_monoBehaviour.aimEnd.SetActive(true);

    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.LookAtTarget();

        // DOWN - 받은 공격이 플레이어의 특정 애니메이션 일 때
        /// TODO = 추후 추가 요망
        if (false)
        {
            //_monoBehaviour.TriggerDown();
        }

        // IDLE - 타겟을 찾을 수 없을 때
        _monoBehaviour.FindTarget();
        if (_monoBehaviour.CurrentTarget == null)
        {
            _monoBehaviour.TriggerIdle();
        }

        // RELOAD - 애니메이션이 종료 됐을 때

    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //_monoBehaviour.aimEnd.SetActive(false);
    }
}
