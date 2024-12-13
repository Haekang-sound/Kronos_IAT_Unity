
/// <summary>
///�ൿ Ʈ������ Ʈ���� ���� ��带 ��Ÿ���ϴ�.
/// �� ���� �� �ϳ��� �ڽ� ��带 ������, 
/// �ڽ� ����� ������Ʈ�� ȣ���Ͽ� �ൿ Ʈ���� �����մϴ�.
/// </summary>
public class Start : Node
{
    public Node child;
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    { 
        return child.Update();
    }

    public override Node Clone()
    {
        Start node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
