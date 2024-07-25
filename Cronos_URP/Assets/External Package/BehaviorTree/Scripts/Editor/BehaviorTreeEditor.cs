using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class BehaviorTreeEditor : EditorWindow
{
    public static string uxmlPath = "Assets/BehaviorTree/UIBuilder/BehaviorTreeEditor.uxml";
    public static string ussPath = "Assets/BehaviorTree/UIBuilder/BehaviorTreeEditor.uss";

    private BehaviorTreeView _treeView;
    private BehaviorTree _tree;
    private InspectorView _inspectorView;
    private IMGUIContainer _blackboardView;
    private ToolbarMenu _toolbarMenu; // �ൿ Ʈ�� �������� ���� ���� �޴��̴�.

    // ���� ���� �ൿ Ʈ���� �� �����带 ����ȭ�ϴ� ��ü�̴�.
    SerializedObject _treeObject;
    // treeObject : ���� �������� 'BehaviorTree' ��ü�� 'SerializedObject' �� ����, �ν����Ϳ��� ������ �� �ְ� �ϴ� ��ü�̴�.
    // Unity �����Ϳ��� ��ü�� ������Ƽ�� �ð������� �����ϱ� ���� ���ȴ�.
    SerializedProperty _blackboardProperty;

    //[MenuItem("BehaviorTreeEditor/Editor ...")]
    [MenuItem("Window/BehaviorTreeEditor")]
    public static void OpenWindow()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
        wnd.minSize = new Vector2(800, 600);
    }

    // �ൿ Ʈ�� ������ ���� Ŭ���� �� �ش� ������ �����츦 �ڵ����� ���� ����� �����Ѵ�.
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        if (Selection.activeObject is BehaviorTree)
        {
            OpenWindow();
            return true;
        }

        return false;
    }

    // ������ Ÿ���� ��� ������ �ε��ϴ� ��ƿ��Ƽ �޼����̴�.
    List<T> LoadAssets<T>() where T : UnityEngine.Object
        // ���׸� 'T' �� �ش��ϴ� ��� ������ �ε��ϴ� ��ƿ��Ƽ �޼����̴�.
        // ���⼭ 'T' �� 'UnityEngine.Object' �� ��ӹ޴� ��� Ÿ���� �� �� �ִ�.
    {
        string[] assetIds = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        // 'AssetDatabase.FindAssets($"t:{typeof(T).Name}")' �� ����Ͽ� ������ Ÿ���� ��� ������ GDUI�� ã�´�.
        List<T> assets = new List<T>();
        foreach (var assetId in assetIds)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetId);
            // �� GDUI�� ���� 'AssetDatabase.GUIDToAssetPath(assetId)' �� ȣ���Ͽ� ������ ��θ� ���,
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            // 'AssetDatabase.LoadAssetAtPath<T>(path)' �� ���� ������ �ε��Ѵ�.
            assets.Add(asset);
            // �ε�� ������ 'List<T>' �� �߰��Ѵ�.
        }
        return assets;
        // �ε�� ���µ��� ��ȯ�Ѵ�.
    }
    // �� �޼���� �ൿ Ʈ�� �����Ϳ��� ����� ������ �������� �ε��� �� ���� �� �ִ�.

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        // ������ �������� 'rootVisualElement' ��ü�� �����Ͽ� UI�� �⺻ �����̳ʷ� ����Ѵ�.

        // UXML ��������
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
        visualTree.CloneTree(root);

        // ��Ÿ�� ��Ʈ
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
        root.styleSheets.Add(styleSheet);

        // ���� Ʈ�� ��
        _treeView = root.Q<BehaviorTreeView>();
        _treeView.OnNodeSelected = OnNodeSelectionChanged;
        root.styleSheets.Add(styleSheet);

        // �ν����� ��
        _inspectorView = root.Q<InspectorView>();

        // ������ ��
        _blackboardView = root.Q<IMGUIContainer>();
        _blackboardView.onGUIHandler = () =>
        {
            if (_treeObject != null && _treeObject.targetObject != null)
            {
                _treeObject.Update();
                EditorGUILayout.PropertyField(_blackboardProperty);
                _treeObject.ApplyModifiedProperties();
            }
        };

        // ���� ���� �޴�
        _toolbarMenu = root.Q<ToolbarMenu>();
        var behaviourTrees = LoadAssets<BehaviorTree>();
        behaviourTrees.ForEach(tree =>
        {
            _toolbarMenu.menu.AppendAction($"{tree.name}", (a) =>
            {
                Selection.activeObject = tree;
            });
        });
        _toolbarMenu.menu.AppendSeparator();

        if (_tree == null)
        {
            OnSelectionChange();
        }
        else
        {
            SelectTree(_tree);
        }
        // ���� ���� ���õ� Ʈ��(tree)�� ���ٸ�, 'OnSelectionChange()' �޼��带 ȣ���Ͽ� ���õ� Ʈ���� ó���Ѵ�.
        // �̹� ���õ� Ʈ���� �ִٸ�, 'SelectTree(tree)' �� ȣ���Ͽ� �ش� Ʈ���� �����Ϳ� ǥ���Ѵ�.
    }

    // OnEnable, OnDisable : �����찡 Ȱ��ȭ/��Ȳ��ȭ �� ��, ȣ��Ǿ� �÷��� ��� ���� ���� �̺�Ʈ�� ���� ������ �����Ѵ�.
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    // 'EditorApplication.playModeStateChanged' �̺�Ʈ�� ���� ������ ���� ������ �� �ٽ� �����Ѵ�.
    // �̴� �ߺ� ������ �����ϱ� �����̴�. �� �̺�Ʈ�� �÷��� ��� ���°� ����� ������ �߻��Ѵ�.
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    // �÷��� ��� ���°� ����� �� ȣ��Ǵ� �޼����̴�.
    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }

    // �����Ϳ��� �ٸ� ��ü�� ���õ� �� ȣ��Ǿ�, ���õ� �ൿ Ʈ���� �����Ϳ� ǥ���Ѵ�.
    // ���� ���õ� ��ü�� 'BehaviorTedd' ���� Ȥ��, 'BehaviorTreeRunner' ������Ʈ�� ���� ���ӿ�����Ʈ������ Ȯ���ϰ�, �ش� Ʈ�� �����Ϳ� ǥ���Ѵ�.
    private void OnSelectionChange()
    {
        EditorApplication.delayCall += () =>
        {
            BehaviorTree tree = Selection.activeObject as BehaviorTree;
            if (!tree)
            {
                if (Selection.activeGameObject)
                {
                    BehaviorTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviorTreeRunner>();
                    if (runner)
                    {
                        tree = runner.tree;
                    }
                }
            }

            SelectTree(tree);
        };

        // 'EditorApplication.delayCall' �� ����Ͽ�, ���� ���� ���� ��� ó���� �Ϸ�� �Ŀ� �ڵ����� �����ϵ��� �Ѵ�.
        // �̴� �������� ���°� ������ �Ŀ� ���õ� Ʈ���� ó���ϱ� �����̴�.
        // 'Selection.activeObjet' �� Ȯ���Ͽ� ���� ���õ� ��ü�� 'BehaviorTree' Ÿ������ �˻��Ѵ�.
        // �׷��� �ʰ� ���õ� ���ӿ�����Ʈ�� ���� ���, �ش� ������Ʈ���� 'BehaviorTreeRunner' ������Ʈ�� ã�� ���õ� Ʈ���� Ȯ���Ѵ�.
        // Ȯ�ε� �ൿ Ʈ���� 'SelectTree' �޼��忡 �����Ͽ�, �����Ϳ��� �ش� Ʈ���� ǥ���ϰ� �۾��� �� �ְ� �Ѵ�.
    }


    // �־��� �ൿ Ʈ���� �����ϰ� �����Ϳ� ǥ���ϴ� �޼����̴�.
    void SelectTree(BehaviorTree newTree)
    {
        if (_treeView == null)
        {
            return;
        }

        if (!newTree)
        {
            return;
        }

        _tree = newTree;

        //overlay.style.visibility = Visibility.Hidden;

        if (Application.isPlaying)
        {
            _treeView.PopulateView(_tree);
        }
        else
        {
            _treeView.PopulateView(_tree);
        }

        _treeObject = new SerializedObject(_tree);
        _blackboardProperty = _treeObject.FindProperty("blackboard");

        EditorApplication.delayCall += () =>
        {
            _treeView.FrameAll();
        };
    }

    void OnNodeSelectionChanged(NodeView node)
    {
        _inspectorView.UpdateSelection(node);
    }

    private void OnInspectorUpdate()
    {
        _treeView?.UpdateNodeStates();
    }
}
