/*
 * 이 코드는 Unity의 UI Toolkit(구 Unity Editor GUI)를 사용하여 그래프 뷰 내에서 노드 포트를 정의하는 'NodePort' 클래스이다.
 * 'NodePort' 는 'Port' 클래스는 상속받아 구현되며, 노드 간 연결을 생성하는 인터페이스를 제공한다.
 * 또한 연결(Edge)을 관리하는 사용자 지정 리스너(DefaultEdgeConnectorListener)를 포함한다.
 *
 * 'NodePort' 클래스와 'DefaultEdgeConnectorListener' 내부 클래스는 Unity 에디터 내에서 사용자 정의 그래프 뷰를 구현할 때,
 * 노드 간의 연결을 쉽게 만들고 관리할 수 있는 기능을 제공한다.
 */
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

/// <summary>
/// 노드 간의 연결점을 나타매여, 연결의 방향과 용량을 지정할 수 있다.
/// 이 클래스는 그래프 뷰 내에서 노드 간의 연결을 생성하고 관리하기 위한 기능을 제공한다.
/// </summary>
public class NodePort : Port
{

    // 연결 생성 및 삭제 로직을 처리하는 리스너이다. 그래그 앤 드롭을 통해 포트 간 연결을 생성하거나 기존 연결을 삭제할 때 사용된다.
    // GITHUB:UnityCsReference-master\UnityCsReference-master\Modules\GraphViewEditor\Elements\Port.cs
    private class DefaultEdgeConnectorListener : IEdgeConnectorListener
    {
        private GraphViewChange m_GraphViewChange; // 연결 변경 사항을 저장하는 GraphViewChange' 객체
        private List<Edge> m_EdgesToCreate; // 생성할 연결을 저장하는 리스트
        private List<GraphElement> m_EdgesToDelete; // 삭제할 연결을 저장하는 리스트


        // DefaultEdgeConnectorListener의 생성자는 이 리스너 객체가 인스턴스화될 때 초기화 작업을 수행한다.
        // 구체적으로, 연결(Edge) 생성과 삭제를 관리하기 위한 리스트를 초기화한다.
        public DefaultEdgeConnectorListener()
        {
            // 이 리스트는 사용자가 포트 간에 새로운 연결을 드래그 앤 드롭하여 생성하려고 할 때, 해당 연결 객체들을 임시로 저장하는 데 사용된다.
            m_EdgesToCreate = new List<Edge>();
            // 사용자가 새 연결을 만들 때 기존의 연결을 대체해야 하는 경우(예 : 포트 용량이 'Single' 인 경우), 삭제될 기존 연결들을 저장하는 리스트이다.
            m_EdgesToDelete = new List<GraphElement>();

            m_GraphViewChange.edgesToCreate = m_EdgesToCreate;
        }

        // 외부 포트에 연결이 떨어졌을 때 호출되지만, 여기서는 구현되지 않는다.
        public void OnDropOutsidePort(Edge edge, Vector2 position) { }
        // 주어진 포인트가 포트의 영역 내에 있는지 확인한다. 이 메서드는 사용자 인터페이스 내에서 포트를 클릭하거나 선택할 때 사용된다.
        public void OnDrop(GraphView graphView, Edge edge)
        {
            m_EdgesToCreate.Clear();
            m_EdgesToCreate.Add(edge);

            // We can't just add these edges to delete to the m_GraphViewChange
            // because we want the proper deletion code in GraphView to also
            // be called. Of course, that code (in DeleteElements) also
            // sends a GraphViewChange.
            m_EdgesToDelete.Clear();
            if (edge.input.capacity == Capacity.Single)
                foreach (Edge edgeToDelete in edge.input.connections)
                    if (edgeToDelete != edge)
                        m_EdgesToDelete.Add(edgeToDelete);
            if (edge.output.capacity == Capacity.Single)
                foreach (Edge edgeToDelete in edge.output.connections)
                    if (edgeToDelete != edge)
                        m_EdgesToDelete.Add(edgeToDelete);
            if (m_EdgesToDelete.Count > 0)
                graphView.DeleteElements(m_EdgesToDelete);

            var edgesToCreate = m_EdgesToCreate;
            if (graphView.graphViewChanged != null)
            {
                edgesToCreate = graphView.graphViewChanged(m_GraphViewChange).edgesToCreate;
            }

            foreach (Edge e in edgesToCreate)
            {
                graphView.AddElement(e);
                edge.input.Connect(e);
                edge.output.Connect(e);
            }
        }
        /*
         * 리스너 동작 설명
         * 연결 드래그 앤 드롭 동작을 감지하고 처리하기 위해 DefaultEdgeConnectorListener를 사용한다.
         * 사용자가 연결을 드래그하여 포트에 떨어뜨릴 때, OnDrop 메서드가 호출되어 새 연결을 생성한다.
         * 포트의 용량에 따라, 기존 연결을 삭제할 수 있다. 예를 들어, 포트의 용량이 Single인 경우 이미 연결이 존재하면 그 연결을 삭제하고 새 연결을 추가한다.
         * 연결의 생성 및 삭제 후, 그래프 뷰에 변경 사항을 적용하여 연결이 시각적으로 업데이트된다.
         */
    }

    // 'NodePort' 의 생성자는 연결 방향(Direction)과 용량(Capacity)을 인자로 받아 초기화한다.
    // 연결 리스너를 설정하고, 연결 조작기(EdgeConnector)를 포트에 추가한다. 또한, 포트의 너비를 지정한다.
    public NodePort(Direction direction, Capacity capacity) : base(Orientation.Vertical, direction, capacity, typeof(bool))
    {
        var connectorListener = new DefaultEdgeConnectorListener();
        m_EdgeConnector = new EdgeConnector<Edge>(connectorListener);
        this.AddManipulator(m_EdgeConnector);
        style.width = 100;
    }

    // 주어진 포인트가 포트의 영역 내에 있는지 확인한다.
    // 이 메서드는 사용자 인터페이스 내에서 포트를 클릭하거나 선택할 때 사용된다.
    public override bool ContainsPoint(Vector2 localPoint)
    {
        Rect rect = new Rect(0, 0, layout.width, layout.height);
        return rect.Contains(localPoint);
    }
}
#endif