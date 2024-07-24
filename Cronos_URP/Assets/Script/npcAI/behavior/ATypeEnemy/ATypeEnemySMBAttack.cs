using UnityEngine;

public class ATypeEnemySMBAttack : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("ATTACK");
        _monoBehaviour.ResetTriggerDown();
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

        // STRAFE - 애니메이션이 종료 됐을 때
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.EndAttack();
        _monoBehaviour.ResetAiming();
    }
}
