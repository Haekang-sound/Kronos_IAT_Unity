using UnityEngine;

public class BossSMBAttack : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ��Ʈ �ִϸ��̼��� ��ġ ���� ��ġ�� �ݿ�
        //_monoBehaviour.GetComponent<Rigidbody>().velocity = (animator.deltaPosition / Time.deltaTime);

        // ���� �ϴ� ���� �ٶ󺸰� ����
        _monoBehaviour.LookAtTarget();
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
