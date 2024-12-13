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
    // UxmlFactory : 'GraphView'�� 'UxmlFactory' �� ��ӹ޴� ���� Ŭ������, UXML�� ���� 'BehaviorTreeView'�� �ν��Ͻ�ȭ �� �� ���ȴ�.
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits> { }

    // ���� �����Ϳ��� �۾����� 'BehaviorTree' ��ü, �� Ʈ���� ����� ���հ� �׵� ������ ���� ���踦 �����Ѵ�.
    private BehaviorTree _tree;
    private BehaviorTreeSettings _treeSettings;

    /// <summary>
    /// �����ڿ��� ������ �ε��ϰ�, 'GraphView'�� �ʿ��� ���۱�(mainiplators)�� UI ��Ҹ� �߰��Ѵ�.
    /// �̴� ����ڰ� Ʈ���� ���� ������ �� �ְ� ���ش�.
    /// ���۱⿡�� ContentZoomer, ContentDragger, SelectionDragger, RectangleSelector ���� ���ԵǾ���.
    /// ���� ��Ÿ�Ͻ�Ʈ�� �߰��Ͽ� ���� �ܰ��� �����Ѵ�.
    /// Undo/Redo ����� ���� Event �����ʸ� �����Ѵ�.
    /// </summary>
    public BehaviorTreeView()
    {
        _treeSettings = BehaviorTreeSettings.GetOrCreateSettings();

        Insert(0, new GridBackground()); // ��׶��� ��ο�

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new DoubleClickSelection());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = _treeSettings.behaviourTreeStyle;
        styleSheets.Add(styleSheet); // ��Ÿ�� ��Ʈ ��������

        Undo.undoRedoPerformed += OnUndoRedo;

        // 'BehaviorTreeView' �� �����ڴ� �ൿ Ʈ�� �������� �ֿ� �並 �ʱ�ȭ�ϰ�, �ʿ��� ���۱�(Mainpulator)�� ��Ÿ�Ͻ�Ʈ�� �߰��Ѵ�.
        // ���⿡�� �������� Ȯ��/����� �� �ִ� 'ContentZoomer', ������ �巡���� �� �ִ� 'ContentDragger', ���� Ŭ������ Ư�� ������ �����ϴ� 'DoubleClickSelection'
        // ���õ� ��Ҹ� �巡���ϴ� 'SelectionDragger', ���� ������ �����ϰ� �ϴ� 'RectangleSelector' ���� ���Եȴ�.
        // ���� 'Undo/Redo' ����� ���� �̺�Ʈ �����ʵ� �����ȴ�.
    }

    // Undo�� Redo �۾��� ����� ��, ȣ��Ǿ� Ʈ�� �並 ���ο� ���·�  ������Ʈ �ϰ�, ���� ������ �����Ѵ�.
    private void OnUndoRedo()
    {
        PopulateView(_tree);
        AssetDatabase.SaveAssets();
    }

    // �־��� 'Node' ��ü�� �ش��ϴ� 'NodeView'�� ã�� ��ȯ�Ѵ�. �̴� ����� GUID�� ����Ͽ� �˻��Ѵ�.
    public NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    internal void PopulateView(BehaviorTree tree)
    {
        _tree = tree;

        // �׷��� ���� ������ ����
        // ���� ��� ����
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements.ToList());  // �� �� �̻� ���� ��� ����
        graphViewChanged += OnGraphViewChanged;
        // 'graphViewChanged' �̺�Ʈ���� 'OnGraphViewChanged' �޼��带 �����ϰ� �ٽ� �߰������ν�, �׷��� �䰡 ����� ������ ������ ó���� �̷�������� �Ѵ�.
        // �̴� �׷��� �䰡 ������Ʈ�� �� �߻��� �� �ִ� �̺�Ʈ�� �����ϱ� ���� �غ� �۾��̴�.
        // 'DeleteElements' �� ȣ���Ͽ� 'graphElements.ToList()' �� ��ȯ�� ���� �׷��� �� ���� ��� ��Ҹ� �����Ѵ�.
        // �̴� ���ο� Ʈ���� ǥ���ϱ� ���� ������ ��� ���� ������ �����ϱ� �����̴�.

        // ��Ʈ ��� ����
        if (tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(Start)) as Start;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }
        // ���� 'tree.rootNode' �� 'null' �� ���, 'tree.CreateNode(typeof(RootNode))' �� ȣ���Ͽ� ���ο� 'RootNode' �� �����ϰ�, Ʈ���� ��Ʈ ���� �����Ѵ�.
        // �̴� �ൿ Ʈ���� �ּ� �ϳ��� ��Ʈ ��带 ������ �ֵ��� �����Ѵ�.

        // ��� �� ����
        _tree.nodes.ForEach(n => CreateNodeView(n));
        // 'tree.nodes.ForEach' �� ����Ͽ� Ʈ���� ���Ե� ��� ��忡 ���� 'CreateNodeView' �޼��带 ȣ���Ѵ�.
        // �� �������� �� ��带 �ð������� ǥ���ϴ� 'NodeView' ��ü�� �����ȴ�.

        // �� ��忡 ���� �ڽ� ��带 ���, �� ��忡 ���� ����(Edge) ���� �� ����-�ڽ� ����� ���Է� ��Ʈ ���� �� �׷��� �信 �߰�.
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

    // ������ �����ϴ� ��Ʈ�� ������ ������ �ٸ� ��Ʈ���� ã�´�. ��Ģ�� �Ʒ��� ����.
    // ������ �����ϴ� ��Ʈ 'startPort' �� �ݴ� ������ ��Ʈ�� ���� ������� ����Ѵ�.
    // ������ ��忡 ���� ��Ʈ������ ������� �ʴ´�.
    // ���������, �� �޼���� ����ڰ� ��� �� ������ ���� �� �ִ� ��ȿ�� ��Ʈ�鸸�� ������ �� �ֵ��� �����ش�.

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter adapter)
    {
        return ports.ToList().Where(
            endPort => endPort.direction != startPort.direction &&
            endPort.node != startPort.node).ToList();
    }
    // ������ �����ϴ� ��Ʈ�� �ݴ� ����(direction)�� ��Ʈ�� ���� ������� ����Ѵ�.
    // �̴� �Է� ��Ʈ�� ��� ��Ʈ�͸� ����ǰ�, ��� ��Ʈ�� �Է� ��Ʈ�͸� ����� �� ������ �ǹ��Ѵ�.
    // ������ ��忡 ���� ��Ʈ������ ������� �ʴ´�.
    // �̴� ��尡 �ڱ� �ڽŰ� ����Ǵ� ���� �����Ѵ�.

    // �� ü���� �̺�Ʈ �Լ�
    // �׷��� �信 ��������� �߻����� �� �̸� ó���Ѵ�. �ַ� ��� �Ǵ� ����(Edge)�� �߰� �� ���ſ� ���õ� �۾��� �����Ѵ�.
    // ������ ��Ұ� ������, �ش� ��Ұ� ����� ��� Ʈ������ ��带 �����ϰ�, Edge�� ��� ����� ��� ���� ���踦 �����Ѵ�.
    // ������ Edge�� ������, ���ο� Edge�� ����� ��� ���� �θ�-�ڽ� ���踦 �߰��Ѵ�.
    // ��� ����� �ڽ��� �����Ѵ�.
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
        // 'elementsToRemove' �� null �� �ƴϸ�, �� ��ҿ� ���� Ÿ���� �˻��Ͽ� 'NodeView' �� ��� Ʈ������ ��带 �����ϰ�,
        // 'Edge' �� ��� ����� ��� ���� ���踦 �����Ѵ�.

        // ������ Edge�� �ִٸ�, �� Edge�� ���� ����� ��� ���� �θ�-�ڽ� ���� �߰�
        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                _tree.AddChild(parentView.node, childView.node);
            });
        }
        // 'edgesToCreate' �� null �� �ƴϸ�, �� 'Edge' �� ���� ����� ��� ���� �θ�-�ڽ� ���踦 �߰��Ѵ�.
        // �̴� ���ο� ������ �׷����� �߰��� ������ �ش� ������ ���� ��尣�� ���谡 ���ǵ��� �ǹ��Ѵ�.

        // ����� �ڽ� ����
        nodes.ForEach((n) =>
        {
            NodeView view = n as NodeView;
            view.SortChildren();
        });
        // ��� ��忡 ���� 'SortChildren' �޼��带 ȣ���Ͽ�, �ڽ� ������ �����Ѵ�.
        // �̴� ������ �׷��� �������̽����� �ϰ��ǰ� �������� ������ ǥ�õǵ��� �Ѵ�.


        return graphViewChange;
    }

    // �޴� ������
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

        // ��� ���� ��ġ ����
        Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
        // ������ �޴��� ȣ���� ��ġ�� ������� ��带 �����ϱ� ����, 'ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition)' �� ����Ͽ� ���콺 Ŭ�� ��ġ�� �׷��� �� ���� ��ǥ�� ��ȯ�Ѵ�.
        // �� ��ǥ�� 'CreateNode' �޼��忡 ���޵Ǿ�, ������ ��尡 ����ڰ� Ŭ���� ��ġ�� ��ġ�ȴ�.

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
