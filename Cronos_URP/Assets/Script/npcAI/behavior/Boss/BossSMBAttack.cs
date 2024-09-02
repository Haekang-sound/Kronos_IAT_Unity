using UnityEngine;

public class BossSMBAttack : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 루트 애니메이션의 위치 실제 위치에 반영
        //_monoBehaviour.transform.position += animator.deltaPosition;

        // 공격 하는 동안 바라보고 있음
        _monoBehaviour.LookAtTarget();
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
