using UnityEngine;

/// <summary>
/// 지정된 범위 내에서 무작위로 대기 시간을 설정한 후, 
/// 대기 시간이 지나면 성공 상태를 반환하는 액션 노드입니다.
/// </summary>
public class RandomWait : ActionNode
{
    public float minDuration = 0f;
    public float maxDuration = 1f;

    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _elapse;

    protected override void OnStart()
    {
        _duration = Random.Range(minDuration, maxDuration + 1f);
        _elapse = 0f;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        //float speed = 1;

        _elapse += blackboard.bulletTimeScalable.GetDeltaTime();

        if (_elapse > _duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
