using UnityEngine;

/// <summary>
/// BTypeEnem의 무력화 전환을 관리하는 클래스입니다.
/// </summary>

public class BTypeEnemySMBDown : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("DOWN");
    }
}