using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;


/// <summary>
/// 'NodeView' Ŭ������ �ൿ Ʈ�� ���� �� ��带 �׷��� ����� �������̽�(GUI) �󿡼� ǥ���ϴ� ������ �Ѵ�.
/// �� Ŭ������ ����� ����, Ÿ��, �̸�, ��ġ ���� ������ �ð������� ��Ÿ����, ����� �Է¿� ���� ����(��:����, �巡�� �� ���)�� ó���Ѵ�.
/// </summary>
public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    // ��尡 ���õ� �� ȣ��� �ݹ� �Լ�. 'Action<NodeView>' Ÿ������, 'NodeView' �ν��Ͻ��� �Ű������� �޴´�.
    public Action<NodeView> OnNodeSelected;
    // �� �䰡 ǥ���ϴ� 'Node' ��ü. �ൿ Ʈ���� ���� ��� �� �ϳ��̴�.
    public Node node;
    // 'input', 'output'
    // ����� �Է� ��Ʈ�� ��� ��Ʈ�� ��Ÿ����. �� ��Ʈ���� ��� ���� ������ �����ϴµ� ���ȴ�.
    public Port input;
    public Port output;

    // �����ڴ� 'Node' ��ü�� �Ű������� �޾�, ����� �ð��� ǥ���� �ʱ�ȭ�Ѵ�.
    // �� �������� ����� �̸�, ��ġ, �Է�/��� ��Ʈ ����, CSS Ŭ���� ����, ������ ���ε� ���� ���� �����Ѵ�.
    public NodeView(Node node): base(AssetDatabase.GetAssetPath(BehaviorTreeSettings.GetOrCreateSettings().nodeXml))
    {
        if (node == null)
        {
            Debug.LogError("NodeView �������� Node �� null �Դϴ�. ��θ� Ȯ���ϼ���");
            return;
        }

        this.node = node;
        this.node.name = node.GetType().Name;
        this.viewDataKey = node.guid;
        this.title = node.name;//node.name.Replace("(Clone)", "").Replace("Node", "");

        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();
        SetupClasses();
        SetupDataBinding();
    }

    // ����� �Ӽ�(��:����)�� UI ��ҿ� ���ε��ϴ� �޼����̴�. �̸� ���� ����� �����Ͱ� UI�� �������� �ݿ��ȴ�.
    private void SetupDataBinding()
    {
        Label descriptionLabel = this.Q<Label>("description");
        descriptionLabel.bindingPath = "description";
        descriptionLabel.Bind(new SerializedObject(node));
    }

    // ��� Ÿ��(Action, Composite, Decorate)�� ���� CSS Ŭ������ �������� �߰��ϴ� �޼����̴�. ����� �ð��� ��Ÿ���� �����Ѵ�.
    private void SetupClasses()
    {
        if (node is ActionNode)
        {
            AddToClassList("action");
        }
        else if (node is CompositeNode)
        {
            AddToClassList("composite");
        }
        else if (node is DecoratorNode)
        {
            AddToClassList("decorator");
        }
        else if (node is Start)
        {
            AddToClassList("root");
        }
    }

    // CreateInputPorts, CreateOutputPorts : ����� �Է� ��Ʈ�� ��� ��Ʈ�� �����Ѵ�.
    // ��Ʈ Ÿ�԰� �뷮�� ����� ������ ���� �ٸ���, �� �޼������ �ش� ��Ʈ���� ��� �信 �߰��Ѵ�.
    private void CreateInputPorts()
    {
        if (node is ActionNode)
        {
            input = new NodePort(Direction.Input, Port.Capacity.Single);
        }
        else if (node is CompositeNode)
        {
            input = new NodePort(Direction.Input, Port.Capacity.Single);
        }
        else if (node is DecoratorNode)
        {
            input = new NodePort(Direction.Input, Port.Capacity.Single);
        }
        else if (node is Start)
        {

        }

        if (input != null)
        {
            input.portName = "";
            input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        if (node is ActionNode)
        {

        }
        else if (node is CompositeNode)
        {
            output = new NodePort(Direction.Output, Port.Capacity.Multi);
        }
        else if (node is DecoratorNode)
        {
            output = new NodePort(Direction.Output, Port.Capacity.Single);
        }
        else if (node is Start)
        {
            output = new NodePort(Direction.Output, Port.Capacity.Single);
        }

        if (output != null)
        {
            output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(output);
        }
    }

    // ����ڰ� ��带 �巡���� ��, ����� �� ��ġ�� �����Ѵ�.
    // �� �޼���� Undo �ý��۰� ���յǾ�, ����ڰ� ��� ��ġ ������ �ǵ��� �� �ְ� �Ѵ�.
    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);

        Undo.RecordObject(node, "Behavior Tree (Set Position)");

        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;

        EditorUtility.SetDirty(node);
    }

    // ��� �䰡 ���õ� �� ȣ��ȴ�. �� �޼���� 'OnNodeSelected' �̺�Ʈ�� �߻�����, ��尡 ���õǾ����� �˸���.
    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }

    // 'CompositeNode' Ÿ���� ��尡 �ڽ� ��带 ������ �ִ� ���, �� �޼���� �� �ڽ� ������ ���� ��ġ�� ���� �����Ѵ�.
    public void SortChildren()
    {
        if (node is CompositeNode composite)
        {
            composite.children.Sort(SortByHorizontalPosition);
        }
    }

    // �ڽ� ������ ���� ��ġ(position.x)�� ���� �����ϱ� ���� �� �Լ��̴�. 'CompositeNode'�� ���� ���� �ڽ� ��带 ���� �� �ִ� ��� Ÿ�Կ��� ���ȴ�.
    private int SortByHorizontalPosition(Node left, Node right)
    {
        return left.position.x < right.position.x ? -1 : 1;
    }

    // ����� ���� ����(Success, Running, Failure)�� ���� CSS Ŭ������ �������� �߰��ϰų� �����Ͽ�, ����� ���¸� �ð������� ǥ���Ѵ�.
    public void UpdateState()
    {

        RemoveFromClassList("running");
        RemoveFromClassList("failure");
        RemoveFromClassList("success");

        if (Application.isPlaying)
        {
            switch (node.state)
            {
                case Node.State.Running:
                    if (node.started)
                    {
                        AddToClassList("running");
                    }
                    break;
                case Node.State.Failure:
                    AddToClassList("failure");
                    break;
                case Node.State.Success:
                    AddToClassList("success");
                    break;
            }
        }
    }
}
