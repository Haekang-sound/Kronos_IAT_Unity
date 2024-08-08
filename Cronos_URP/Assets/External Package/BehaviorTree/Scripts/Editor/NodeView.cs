using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;


/// <summary>
/// 'NodeView' 클래스는 행동 트리 내의 각 노드를 그래픽 사용자 인터페이스(GUI) 상에서 표현하는 역할을 한다.
/// 이 클래스는 노드의 상태, 타입, 이름, 위치 등의 정보를 시각적으로 나타내고, 사용자 입력에 대한 반응(예:선택, 드래그 앤 드롭)을 처리한다.
/// </summary>
public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public static string uxmlPath = "Assets/BehaviorTree/UIBuilder/NodeView.uxml";

    // 노드가 선택될 때 호출될 콜백 함수. 'Action<NodeView>' 타입으로, 'NodeView' 인스턴스를 매개변수로 받는다.
    public Action<NodeView> OnNodeSelected;
    // 이 뷰가 표현하는 'Node' 객체. 행동 트리의 구성 요소 중 하나이다.
    public Node node;
    // 'input', 'output'
    // 노드의 입력 포트와 출력 포트를 나타낸다. 이 포트들은 노드 간의 연결을 생성하는데 사용된다.
    public Port input;
    public Port output;

    // 생성자는 'Node' 객체를 매개변수로 받아, 노드의 시각적 표현을 초기화한다.
    // 이 과정에서 노드의 이름, 위치, 입력/출력 포트 생성, CSS 클래스 설정, 데이터 바인딩 설정 등을 수행한다.
    public NodeView(Node node): base(uxmlPath)
    {
        if (node == null)
        {
            Debug.LogError("NodeView 생성자의 Node 가 null 입니다. 경로를 확인하세요");
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

    // 노드의 속성(예:설명)을 UI 요소에 바인딩하는 메서드이다. 이를 통해 노드의 데이터가 UI에 동적으로 반영된다.
    private void SetupDataBinding()
    {
        Label descriptionLabel = this.Q<Label>("description");
        descriptionLabel.bindingPath = "description";
        descriptionLabel.Bind(new SerializedObject(node));
    }

    // 노드 타입(Action, Composite, Decorate)에 따라 CSS 클래스를 동적으로 추가하는 메서드이다. 노드의 시각적 스타일을 결정한다.
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

    // CreateInputPorts, CreateOutputPorts : 노드의 입력 포트와 출력 포트를 생성한다.
    // 포트 타입과 용량은 노드의 종류에 따라 다르며, 이 메서드들은 해당 포트들을 노드 뷰에 추가한다.
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

    // 사용자가 노드를 드래그할 때, 노드의 새 위치를 설정한다.
    // 이 메서드는 Undo 시스템과 통합되어, 사용자가 노드 위치 변경을 되돌릴 수 있게 한다.
    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);

        Undo.RecordObject(node, "Behavior Tree (Set Position)");

        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;

        EditorUtility.SetDirty(node);
    }

    // 노드 뷰가 선택될 때 호출된다. 이 메서드는 'OnNodeSelected' 이벤트를 발생시켜, 노드가 선택되었음을 알린다.
    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }

    // 'CompositeNode' 타입의 노드가 자식 노드를 가지고 있는 경우, 이 메서드는 그 자식 노드들을 가로 위치에 따라 정렬한다.
    public void SortChildren()
    {
        if (node is CompositeNode composite)
        {
            composite.children.Sort(SortByHorizontalPosition);
        }
    }

    // 자식 노드들을 가로 위치(position.x)에 따라 정렬하기 위한 비교 함수이다. 'CompositeNode'와 같이 여러 자식 노드를 가질 수 있는 노드 타입에서 사용된다.
    private int SortByHorizontalPosition(Node left, Node right)
    {
        return left.position.x < right.position.x ? -1 : 1;
    }

    // 노드의 실행 상태(Success, Running, Failure)에 따라 CSS 클래스를 동적으로 추가하거나 제거하여, 노드의 상태를 시각적으로 표현한다.
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
