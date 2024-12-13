using UnityEngine;

/// <summary>
/// ������ ���� ������ �������� ��� �ð��� ������ ��, 
/// ��� �ð��� ������ ���� ���¸� ��ȯ�ϴ� �׼� ����Դϴ�.
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
