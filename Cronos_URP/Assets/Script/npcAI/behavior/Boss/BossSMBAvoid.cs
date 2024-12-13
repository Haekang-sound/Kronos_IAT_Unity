using UnityEngine;

/// <summary>
/// 보스의 회피 애니메이션 상태 전환을 관리하는 클래스입니다.
/// </summary>
public class BossSMBAvoid : SceneLinkedSMB<BossBehavior>
{

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.StopAiming();
    }

}
