using UnityEngine;

/// <summary>
/// BossLightRushCloneBehavior의 애니메이션 상태에서 루트 모션을 제어하는 클래스.
/// 애니메이션의 루트 모션을 사용하여 보스의 클론이 타겟을 향해 이동하도록 조정합니다.
/// </summary>
public class BossLightRushCloneSMBRush : SceneLinkedSMB<BossLightRushCloneBehavior>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_monoBehaviour == null) return;


        var rootmotion = animator.deltaPosition;

        var toTarget = _monoBehaviour.target.transform.position - _monoBehaviour.transform.position;
        
        var scale = toTarget.magnitude * rootmotion.magnitude;

        rootmotion *= scale;

        _monoBehaviour.transform.position += rootmotion;
    }

}
