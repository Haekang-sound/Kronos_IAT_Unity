using UnityEngine;

/// <summary>
/// ATypeEnem의 피격 상태 전환을 관리하는 클래스입니다.
/// </summary>
public class ATypeEnemySMBDamaged : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ResetAiming();
        _monoBehaviour.ChangeDebugText("DAMAGED");
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.UseBulletTimeScale();
    }
}