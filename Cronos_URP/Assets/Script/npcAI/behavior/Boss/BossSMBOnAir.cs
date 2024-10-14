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

        // ATTACK - 공격 사거리 안에 있을 때

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
        // 현재 오브젝트의 위치
        Vector3 currentPosition = _monoBehaviour.transform.position;

        // 타겟의 위치
        Vector3 targetPosition = _monoBehaviour.target.transform.position;

        // 타겟까지의 방향 벡터 계산
        Vector3 direction = (targetPosition - currentPosition).normalized;

        // 오브젝트의 새 위치 계산
        Vector3 newPosition = currentPosition + direction * speed * Time.deltaTime;

        // 새 위치로 오브젝트 이동
        var newPos = currentPosition;
        newPos.x = newPosition.x;
        newPos.z = newPosition.z;

        _monoBehaviour.transform.position = newPos;

        _monoBehaviour.rotationSpeed = rotationSpeed;
        _monoBehaviour.LookAtTarget();
    }
}
