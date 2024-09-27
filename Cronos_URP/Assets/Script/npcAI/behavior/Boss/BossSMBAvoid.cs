using UnityEngine;

public class BossSMBAvoid : SceneLinkedSMB<BossBehavior>
{

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.StopAiming();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 루트 애니메이션의 위치 실제 위치에 반영
        //_monoBehaviour.transform.position += animator.deltaPosition;
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ResetAiming();
    }
}
