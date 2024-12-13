using UnityEngine;

/// <summary>
/// ������ ���� ���� ��ȯ�� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class BossSMBLightSpeedRush : SceneLinkedSMB<BossBehavior>
{
    [SerializeField]
    public float speed = 1f;

    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.thisRigidbody.isKinematic = true;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var rootmotion = animator.deltaPosition;

        var toTarget = _monoBehaviour.target.transform.position - _monoBehaviour.transform.position;

        var scale = toTarget.magnitude * rootmotion.magnitude * speed * BulletTime.Instance.GetCurrentSpeed();

        rootmotion *= scale;

        _monoBehaviour.transform.position += rootmotion;
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.thisRigidbody.isKinematic = false;
    }
}
