using UnityEngine;

/// <summary>
/// ������ �ð� ���� �ڽ� ��带 �������� �ʰ� ��ٿ��� �����ϴ� ����Դϴ�.
/// </summary>
public class Cooldown : DecoratorNode
{
    public float duration = 1.0f;
    float _startTime;

    protected override void OnStart()
    {
        _startTime = Time.time;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float elapsed = Time.time - _startTime;

        if (elapsed < duration)
        {
            return State.Running;
        }

        return child.Update();
    }
}
