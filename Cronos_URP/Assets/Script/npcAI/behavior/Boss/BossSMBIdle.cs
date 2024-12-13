using UnityEngine;

/// <summary>
/// 보스의 유휴 상태 전환을 관리하는 클래스입니다.
/// </summary>
public class BossSMBIdle : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.thisRigidbody.isKinematic = true;

        _monoBehaviour.UseGravity(true);

        _monoBehaviour.StopAiming();

        _monoBehaviour.EndAttack();
        _monoBehaviour.EndImpactAttack();
        _monoBehaviour.EndSoulderAttack();
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.thisRigidbody.isKinematic = false;
    }
}
