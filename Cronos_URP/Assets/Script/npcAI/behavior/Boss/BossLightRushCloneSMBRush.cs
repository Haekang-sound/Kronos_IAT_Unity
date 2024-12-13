using UnityEngine;

/// <summary>
/// BossLightRushCloneBehavior�� �ִϸ��̼� ���¿��� ��Ʈ ����� �����ϴ� Ŭ����.
/// �ִϸ��̼��� ��Ʈ ����� ����Ͽ� ������ Ŭ���� Ÿ���� ���� �̵��ϵ��� �����մϴ�.
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
