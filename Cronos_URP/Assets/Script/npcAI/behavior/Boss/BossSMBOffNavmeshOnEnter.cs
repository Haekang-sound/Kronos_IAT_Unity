using UnityEngine;

/// <summary>
/// 보스의 네비메시를 비활성화하는 클래스입니다.
/// </summary>
public class BossSMBOffNavmeshOnEnter : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.controller.SetFollowNavmeshAgent(false);
    }
}
