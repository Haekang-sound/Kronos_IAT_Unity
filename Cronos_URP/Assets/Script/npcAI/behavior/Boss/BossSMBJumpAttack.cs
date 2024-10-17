using UnityEngine;
using UnityEngine.UIElements;

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

        // ���� �̵� ����
        _monoBehaviour.transform.position += rootmotion;
        // ���� �̵� ����
        _monoBehaviour.transform.position += new Vector3(0f, rootmotionY, 0f);
    }

    private void YZero(ref Vector3 vector)
    {
        vector.y = 0;
    }
}
