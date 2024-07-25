/*
 * 이 클래스는 'MouseManipulator' 를 상속받아, 마우스 이벤트를 기반으로 특정 조작을 수행하는 사용자 지정 마우스 조작기를 구현한다.
 * 특히, 이 클래스는 사용자가 노드를 더블 클릭할 때 해당 노드와 그 자식 노드들을 선택하는 기능을 제공한다.
 *
 * 이 클래스는 사용자가 'BehaviorTreeView' 내에서 노드를 더블 클릭하는 사용자 경험을 향상시키는 데 중요한 역할을 한다.
 * 이를 통해 사용자는 노드와 그 자식 노드들을 빠르게 선택하고, 이들 간의 관계를 시각적으로 확인할 수 있다.
 */

using UnityEditor;
using UnityEngine.UIElements;

/// <summary>
/// 'DoubleClickSelection' 클래스는 Unity의 GraphView API를 확장하며, 사용자가 'BehaviorTreeView' 내의 노드를 더블 클릭된 노드와 그 자식 노드들이 선택된다.
/// 이 기능은 트리 구조 내에서 부모 노드와 관련된 자식 노드들을 함께 선택하고자 할 때 유용하다.
/// </summary>
public class DoubleClickSelection : MouseManipulator
{
    double time; // 마지막 마우스 다운 이벤트가 발생한 시간을 기록한다.
    double doubleClickDuration = 0.3;  // 더블 클릭으로 간주되는 최대 시간 간격을 정의한다. 기본값은 0.3

    // 생성자에서 객체가 생성될 때의 시간(EditorApplication.timeSinceStartup)을 'time' 필드에 초기화한다.
    // 이 시간은 첫 번째 마우스 다운 이벤트의 타임스탬프로 사용된다.
    public DoubleClickSelection()
    {
        time = EditorApplication.timeSinceStartup;
    }

    // 조작기가 대상(BehaviorTreeView)에 연결될 때 호출된다.
    // 'MouseDownEvent' 에 대한 콜백(OnMouseDown)을 등록하여, 마우스 다운 이벤트를 처리할 수 있게 한다.
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
    }

    // 조작기가 대상에서 분리될 때 호출된다. 'MouseDownEvent' 에 대한 콜백을 해제한다.
    protected override void UnregisterCallbacksFromTarget()
    {

        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
    }

    // 마우스 다운 이벤트가 발생했을 때 호출된다. 이 메서드는 마지막 마우스 다운 이벤트 이후 시간 간격을 계산하고,
    // 이 간격이 'doubleClickDuration' 보다 작을 경우 'SelectChildren' 메서드를 호출하여 자식 노드들을 선택한다.
    // 그리고 현재 시간을 'time' 필드에 다시 기록한다.
    private void OnMouseDown(MouseDownEvent evt)
    {
        var graphView = target as BehaviorTreeView;
        if (graphView == null)
            return;

        double duration = EditorApplication.timeSinceStartup - time;
        if (duration < doubleClickDuration)
        {
            SelectChildren(evt);
        }

        time = EditorApplication.timeSinceStartup;
    }

    // 더블 클릭된 노드와 그 자식 노드들을 선택한다.
    // 이 메서드는 더블 클릭된 노드를 식별하고, 'BehaviorTree.Traverse' 메서드를 사용하여 해당 노드와 그 자식 노드들을 순회하며,
    // 각 노드에 대응하는 'NodeView'를 'BehaviorTreeView' 의 선택 목록에 추가한다.
    void SelectChildren(MouseDownEvent evt)
    {

        var graphView = target as BehaviorTreeView;
        if (graphView == null)
            return;

        if (!CanStopManipulation(evt))
            return;

        NodeView clickedElement = evt.target as NodeView;
        if (clickedElement == null)
        {
            var ve = evt.target as VisualElement;
            clickedElement = ve.GetFirstAncestorOfType<NodeView>();
            if (clickedElement == null)
                return;
        }

        // Add children to selection so the root element can be moved
        BehaviorTree.Traverse(clickedElement.node, node => {
            var view = graphView.FindNodeView(node);
            graphView.AddToSelection(view);
        });
    }
}