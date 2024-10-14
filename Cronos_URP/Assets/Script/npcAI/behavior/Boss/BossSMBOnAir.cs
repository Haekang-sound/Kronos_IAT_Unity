using UnityEngine;
using UnityEngine.AI;

public class BossSMBOnAir : SceneLinkedSMB<BossBehavior>
{
    public float distance = 3f;
    public float speed = 5f;
    public float rotationSpeed = 5f;

    public override void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Chase();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Chase();

        // ATTACK - ���� ��Ÿ� �ȿ� ���� ��

        Vector2 target = new Vector2();
        target.x = _monoBehaviour.target.transform.position.x;
        target.y = _monoBehaviour.target.transform.position.z;

        Vector2 pos = new Vector2();
        pos.x = _monoBehaviour.transform.position.x;
        pos.y = _monoBehaviour.transform.position.z;

        Vector2 toTarget = target - pos;
        var result =  toTarget.sqrMagnitude < distance * distance;

        if (result)
        {
            _monoBehaviour.AnimatorSetTrigger("land");
        }
    }

    void Chase()
    {
        // ���� ������Ʈ�� ��ġ
        Vector3 currentPosition = _monoBehaviour.transform.position;

        // Ÿ���� ��ġ
        Vector3 targetPosition = _monoBehaviour.target.transform.position;

        // Ÿ�ٱ����� ���� ���� ���
        Vector3 direction = (targetPosition - currentPosition).normalized;

        // ������Ʈ�� �� ��ġ ���
        Vector3 newPosition = currentPosition + direction * speed * Time.deltaTime;

        // �� ��ġ�� ������Ʈ �̵�
        var newPos = currentPosition;
        newPos.x = newPosition.x;
        newPos.z = newPosition.z;

        _monoBehaviour.transform.position = newPos;

        _monoBehaviour.rotationSpeed = rotationSpeed;
        _monoBehaviour.LookAtTarget();
    }
}
