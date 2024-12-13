
/// <summary>
/// ������ ��� �ð��� ����ϸ� ���� ���¸� ��ȯ�ϴ� �׼� ����Դϴ�.
/// </summary>
public class Wait : ActionNode
{
    public float duration = 1f;
    float _elapse;

    protected override void OnStart()
    {
        _elapse = 0f;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        _elapse += blackboard.bulletTimeScalable.GetDeltaTime();

        if (_elapse > duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
