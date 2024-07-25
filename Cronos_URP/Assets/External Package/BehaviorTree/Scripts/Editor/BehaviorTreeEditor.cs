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
    private ToolbarMenu _toolbarMenu; // 행동 트리 에디터의 도구 모음 메뉴이다.

    // 편집 중인 행동 트리와 그 블랙보드를 직렬화하는 객체이다.
    SerializedObject _treeObject;
    // treeObject : 현재 편집중인 'BehaviorTree' 객체를 'SerializedObject' 로 감싸, 인스펙터에서 수정할 수 있게 하는 객체이다.
    // Unity 에디터에서 객체의 프로퍼티를 시각적으로 편집하기 위해 사용된다.
    SerializedProperty _blackboardProperty;

    //[MenuItem("BehaviorTreeEditor/Editor ...")]
    [MenuItem("Window/BehaviorTreeEditor")]
    public static void OpenWindow()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
        wnd.minSize = new Vector2(800, 600);
    }

    // 행동 트리 에셋을 더블 클릭할 때 해당 에디터 윈도우를 자동으로 여는 기능을 구현한다.
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

    // 지정된 타입의 모든 에셋을 로드하는 유틸리티 메서드이다.
    List<T> LoadAssets<T>() where T : UnityEngine.Object
        // 제네릭 'T' 에 해당하는 모든 에셋을 로드하는 유틸리티 메서드이다.
        // 여기서 'T' 는 'UnityEngine.Object' 를 상속받는 모든 타입이 될 수 있다.
    {
        string[] assetIds = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        // 'AssetDatabase.FindAssets($"t:{typeof(T).Name}")' 을 사용하여 지정된 타입의 모든 에셋의 GDUI를 찾는다.
        List<T> assets = new List<T>();
        foreach (var assetId in assetIds)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetId);
            // 각 GDUI에 대해 'AssetDatabase.GUIDToAssetPath(assetId)' 를 호출하여 에셋의 경로를 얻고,
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            // 'AssetDatabase.LoadAssetAtPath<T>(path)' 로 실제 에셋을 로드한다.
            assets.Add(asset);
            // 로드된 에셋을 'List<T>' 에 추가한다.
        }
        return assets;
        // 로드된 에셋들을 반환한다.
    }
    // 이 메서드는 행동 트리 에디터에서 사용할 에셋을 동적으로 로드할 때 사용될 수 있다.

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        // 에디터 윈도우의 'rootVisualElement' 객체에 접근하여 UI의 기본 컨테이너로 사용한다.

        // UXML 가져오기
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
        visualTree.CloneTree(root);

        // 스타일 시트
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
        root.styleSheets.Add(styleSheet);

        // 매인 트리 뷰
        _treeView = root.Q<BehaviorTreeView>();
        _treeView.OnNodeSelected = OnNodeSelectionChanged;
        root.styleSheets.Add(styleSheet);

        // 인스펙터 뷰
        _inspectorView = root.Q<InspectorView>();

        // 블랙보드 뷰
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

        // 툴바 에셋 메뉴
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
        // 만약 현재 선택된 트리(tree)가 없다면, 'OnSelectionChange()' 메서드를 호출하여 선택된 트리를 처리한다.
        // 이미 선택된 트리가 있다면, 'SelectTree(tree)' 를 호출하여 해당 트리를 에디터에 표시한다.
    }

    // OnEnable, OnDisable : 윈도우가 활성화/비황성화 될 때, 호출되어 플레이 모드 상태 변경 이벤트에 대한 구독을 관리한다.
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    // 'EditorApplication.playModeStateChanged' 이벤트에 대한 구독을 먼저 해제한 후 다시 구독한다.
    // 이는 중복 구독을 방지하기 위함이다. 이 이벤트는 플레이 모드 상태가 변경될 때마다 발생한다.
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    // 플레이 모드 상태가 변경될 때 호출되는 메서드이다.
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

    // 에디터에서 다른 객체가 선택될 때 호출되어, 선택된 행동 트리를 에디터에 표시한다.
    // 현재 선택된 객체가 'BehaviorTedd' 인지 혹은, 'BehaviorTreeRunner' 컴포넌트를 가진 게임오브젝트인지를 확인하고, 해당 트리 에디터에 표시한다.
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

        // 'EditorApplication.delayCall' 을 사용하여, 현재 실행 중인 모든 처리가 완료된 후에 코드블록을 실행하도록 한다.
        // 이는 데이터의 상태가 안정된 후에 선택된 트리를 처리하기 위함이다.
        // 'Selection.activeObjet' 를 확인하여 현재 선택된 객체가 'BehaviorTree' 타입인지 검사한다.
        // 그렇지 않고 선택된 게임오브젝트가 있을 경우, 해당 오브젝트에서 'BehaviorTreeRunner' 컴포넌트를 찾아 관련된 트리를 확인한다.
        // 확인된 행동 트리를 'SelectTree' 메서드에 전달하여, 에디터에서 해당 트리를 표시하고 작업할 수 있게 한다.
    }


    // 주어진 행동 트리를 선택하고 에디터에 표시하는 메서드이다.
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
