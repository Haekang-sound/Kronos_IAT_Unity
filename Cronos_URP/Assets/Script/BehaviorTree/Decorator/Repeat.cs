
/// <summary>
/// �ڽ� ��带 �ݺ������� �����ϴ� ����Դϴ�.
/// �ڽ� ����� ���¿� ������� ����ؼ� �ڽ� ��带 �����մϴ�.
/// </summary>
public class Repeat : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        child.Update();
        return State.Running;
    }
}
