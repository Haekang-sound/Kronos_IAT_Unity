using UnityEngine;

/// <summary>
/// 보스의 공격 상태 전환을 관리하는 클래스입니다.
/// </summary>
public class BossSMBJumpAttack : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.UseGravity(false);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var toTarget = _monoBehaviour.target.transform.position - _monoBehaviour.transform.position;
        YZero(ref toTarget);

        var rootmotion = animator.deltaPosition;
        var rootmotionY = rootmotion.y;
        YZero(ref rootmotion);

        var scale = toTarget.magnitude * rootmotion.magnitude;

        rootmotion *= scale;

        // 수평 이동 적용
        _monoBehaviour.transform.position += rootmotion;
        // 수직 이동 적용
        _monoBehaviour.transform.position += new Vector3(0f, rootmotionY, 0f);
    }

    private void YZero(ref Vector3 vector)
    {
        vector.y = 0;
    }
}
