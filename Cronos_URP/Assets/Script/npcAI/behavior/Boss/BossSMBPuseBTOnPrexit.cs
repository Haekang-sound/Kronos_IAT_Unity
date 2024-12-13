using UnityEngine;

/// <summary>
/// 보스의 BehaviourTree Runner를 진행을 비활성화 하는 클래스 입니다.
/// </summary>
public class BossSMBPuseBTOnPrexit : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.PlayBT(false);
    }
}