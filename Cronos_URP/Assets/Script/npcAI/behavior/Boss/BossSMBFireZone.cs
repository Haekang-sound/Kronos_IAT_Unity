using UnityEngine;

/// <summary>
/// 보스의 불꽃 장판 스킬 상태 전환을 관리하는 클래스입니다.
/// </summary>
public class BossSMBFireZone : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        _monoBehaviour.BeginAiming();
    }
    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.StopAiming();
    }
}
