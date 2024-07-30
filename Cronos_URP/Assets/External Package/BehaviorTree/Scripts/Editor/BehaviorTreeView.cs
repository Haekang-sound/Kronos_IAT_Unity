using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;

public class BehaviorTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    // 현재 에디터에서 작업중인 'BehaviorTree' 객체, 이 트리는 노드의 집합과 그들 사이의 연결 관계를 포함한다.
    private BehaviorTree _tree;
    // UxmlFactory : 'GraphView'의 'UxmlFactory' 를 상속받는 내부 클래스로, UXML을 통해 'BehaviorTreeView'를 인스턴스화 할 때 사용된다.
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits> { }

    /// <summary>
    /// 생성자에서 설정을 로드하고, 'GraphView'에 필요한 조작기(mainiplators)와 UI 요소를 추가한다.
    /// 이는 사용자가 트리를 쉽게 조작할 수 있게 해준다.
    /// 조작기에는 ContentZoomer, ContentDragger, SelectionDragger, RectangleSelector 등이 포함되었다.
    /// 또한 스타일시트를 추가하여 뷰의 외관을 정의한다.
    /// Undo/Redo 기능을 위한 Event 리스너를 설정한다.
    /// </summary>
    public BehaviorTreeView()
    {
        Insert(0, new GridBackground()); // 백그라운드 드로우

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new DoubleClickSelection());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(BehaviorTreeEditor.ussPath);
        //var styleSheet = _settings.behaviourTreeStyle;
        styleSheets.Add(styleSheet); // 스타일 시트 직접참조

        Undo.undoRedoPerformed += OnUndoRedo;

        // 'BehaviorTreeView' 의 생성자는 행동 트리 에디터의 주요 뷰를 초기화하고, 필요한 조작기(Mainpulator)와 스타일시트를 추가한다.
        // 여기에는 콘텐츠를 확대/축소할 수 있는 'ContentZoomer', 내용을 드래그할 수 있는 'ContentDragger', 더블 클릭으로 특정 동작을 수행하는 'DoubleClickSelection'
        // 선택된 요소를 드래그하는 'SelectionDragger', 영역 선택을 가능하게 하는 'RectangleSelector' 등이 포함된다.
        // 또한 'Undo/Redo' 기능을 위한 이벤트 리스너도 설정된다.
    }

    // Undo나 Redo 작업이 수행될 때, 호출되어 트리 뷰를 새로운 상태로  업데이트 하고, 변경 사항을 저장한다.
    private void OnUndoRedo()
    {
        PopulateView(_tree);
        AssetDatabase.SaveAssets();
    }

    // 주어진 'Node' 객체에 해당하는 'NodeView'를 찾아 반환한다. 이는 노드의 GUID를 사용하여 검색한다.
    public NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    internal void PopulateView(BehaviorTree tree)
    {
        _tree = tree;

        // 그래프 변경 리스너 관리
        // 기존 요소 삭제
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements.ToList());  // 두 개 이상 생성 대비 삭제
        graphViewChanged += OnGraphViewChanged;
        // 'graphViewChanged' 이벤트에서 'OnGraphViewChanged' 메서드를 해제하고 다시 추가함으로써, 그래프 뷰가 변경될 때마다 적절한 처리가 이루어지도록 한다.
        // 이는 그래프 뷰가 업데이트될 때 발생할 수 있는 이벤트를 관리하기 위한 준비 작업이다.
        // 'DeleteElements' 를 호출하여 'graphElements.ToList()' 로 변환된 현재 그래프 뷰 내의 모든 요소를 삭제한다.
        // 이는 새로운 트리를 표시하기 전에 기존의 모든 노드와 연결을 제거하기 위함이다.

        // 루트 노드 생성
        if (tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(Start)) as Start;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }
        // 만약 'tree.rootNode' 가 'null' 인 경우, 'tree.CreateNode(typeof(RootNode))' 를 호출하여 새로운 'RootNode' 를 생성하고, 트리의 루트 노드로 설정한다.
        // 이는 행동 트리가 최소 하나의 루트 노드를 가지고 있도록 보장한다.

        // 노드 뷰 생성
        _tree.nodes.ForEach(n => CreateNodeView(n));
        // 'tree.nodes.ForEach' 를 사용하여 트리에 포함된 모든 노드에 대해 'CreateNodeView' 메서드를 호출한다.
        // 이 과정에서 각 노드를 시각적으로 표현하는 'NodeView' 객체가 생성된다.

        // 각 노드에 대해 자식 노드를 얻고, 각 노드에 대한 엣지(Edge) 생성 후 보모-자식 노드의 출입력 포트 연결 후 그래프 뷰에 추가.
        _tree.nodes.ForEach(n =>
        {
            var children = BehaviorTree.GetChildren(n);
            children.ForEach(c =>
            {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);

                Edge edge = parentView.output.ConnectTo(childView.input);
                AddElement(edge);
            });
        });
    }

    // 연결을 시작하는 포트와 연결이 가능한 다른 포트들을 찾는다. 규칙은 아래와 같다.
    // 연결을 시작하는 포트 'startPort' 와 반대 방향의 포트만 연결 대산으로 고려한다.
    // 동일한 노드에 속한 포트끼리는 연결되지 않는다.
    // 결과적으로, 이 메서드는 사용자가 노드 간 연결을 만들 수 있는 유효한 포트들만을 선택할 수 있도록 도와준다.

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter adapter)
    {
        return ports.ToList().Where(
            endPort => endPort.direction != startPort.direction &&
            endPort.node != startPort.node).ToList();
    }
    // 연결을 시작하는 포트와 반대 방향(direction)의 포트만 연결 대상으로 고려한다.
    // 이는 입력 포트는 출력 포트와만 연결되고, 출력 포트는 입력 포트와만 연결될 수 있음을 의미한다.
    // 동일한 노드에 속한 포트끼리는 연결되지 않는다.
    // 이는 노드가 자기 자신과 연결되는 것을 방지한다.

    // 뷰 체인지 이벤트 함수
    // 그래프 뷰에 변경사항이 발생했을 때 이를 처리한다. 주로 노드 또는 엣지(Edge)의 추가 및 제거와 관련된 작업을 수행한다.
    // 제거할 요소가 있으면, 해당 요소가 노드인 경우 트리에서 노드를 삭제하고, Edge인 경우 연결된 노드 간의 관계를 제거한다.
    // 생성할 Edge가 있으면, 새로운 Edge로 연결된 노드 간의 부모-자식 관계를 추가한다.
    // 모든 노드의 자식을 정렬한다.
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    _tree.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    _tree.RemoveChild(parentView.node, childView.node);
                }
            });
        }
        // 'elementsToRemove' 가 null 이 아니면, 각 요소에 대해 타입을 검사하여 'NodeView' 인 경우 트리에서 노드를 삭제하고,
        // 'Edge' 인 경우 연결된 노드 간의 관계를 제거한다.

        // 생성된 Edge가 있다면, 각 Edge에 대한 연결된 노드 간의 부모-자식 관계 추가
        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                _tree.AddChild(parentView.node, childView.node);
            });
        }
        // 'edgesToCreate' 가 null 이 아니면, 각 'Edge' 에 대해 연결된 노드 간의 부모-자식 관계를 추가한다.
        // 이는 새로운 연결이 그래프에 추가될 때마다 해당 연결을 통해 노드간의 관계가 정의됨을 의미한다.

        // 노드의 자식 정렬
        nodes.ForEach((n) =>
        {
            NodeView view = n as NodeView;
            view.SortChildren();
        });
        // 모든 노드에 대해 'SortChildren' 메서드를 호출하여, 자식 노드들을 정렬한다.
        // 이는 노드들이 그래픽 인터페이스에서 일관되고 직관적인 순서로 표시되도록 한다.


        return graphViewChange;
    }

    // 메뉴 재정의
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);

        {
            evt.menu.AppendAction("Delete", delegate
            {
                DeleteSelectionCallback(AskUser.DontAskUser);
            }, (DropdownMenuAction a) => canDeleteSelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
        }

        evt.menu.AppendSeparator();

        // 노드 생성 위치 결정
        Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
        // 생성자 메뉴를 호출한 위치를 기반으로 노드를 생성하기 위해, 'ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition)' 를 사용하여 마우스 클릭 위치를 그래프 뷰 내의 좌표로 변환한다.
        // 이 좌표는 'CreateNode' 메서드에 전달되어, 생성된 노드가 사용자가 클릭한 위치에 배치된다.

        {

            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Action]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Composite]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Decorator]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }
    }

    void SelectFolder(string path)
    {
        // https://forum.unity.com/threads/selecting-a-folder-in-the-project-via-button-in-editor-window.355357/
        // Check the path has no '/' at the end, if it does remove it,
        // Obviously in this example it doesn't but it might
        // if your getting the path some other way.

        if (path[path.Length - 1] == '/')
            path = path.Substring(0, path.Length - 1);

        // Load object
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));

        // Select the object in the project folder
        Selection.activeObject = obj;

        // Also flash the folder yellow to highlight it
        EditorGUIUtility.PingObject(obj);
    }

    void CreateNode(System.Type type, Vector2 position)
    {
        Node node = _tree.CreateNode(type);
        node.position = position;
        CreateNodeView(node);
    }

    private void CreateNode(System.Type type)
    {
        Node node = _tree.CreateNode(type);
        CreateNodeView(node);
    }

    private void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    public void UpdateNodeStates()
    {
        nodes.ForEach(n =>
        {
            NodeView view = n as NodeView;
            view.UpdateState();
        });
    }
}
