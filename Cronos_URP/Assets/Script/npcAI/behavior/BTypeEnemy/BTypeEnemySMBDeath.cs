using UnityEngine;

/// <summary>
/// BTypeEnem의 사망 상태 전환을 관리하는 클래스입니다.
/// </summary>
public class BTypeEnemySMBDeath : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("DEATH");
    }
}