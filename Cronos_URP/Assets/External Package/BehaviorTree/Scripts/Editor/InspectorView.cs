using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 'InspectorView' 클래스는 사용자가 행동 트리 에디터 내에서 노드를 선택했을 때, 해당 노드의 속성을 표시하는 인스펙터 뷰의 역할을 한다.
/// 이는 Unity의 기본 인스펙터 기능을 모방하여, 선택된 객체의 세부 사항을 수정할 수 있는 인터페이스를 제공한다.
/// </summary>
public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    // 선택된 노드를 위한 'Editor' 인스턴스이다. 이 'Editor' 객체는 선택된 노드의 속성을 인스펙터 UI에 표시하고 편집할 수 있게 해준다.
    Editor _editor;

    // 'InspectorView' 의 생성자는 기본적으로 비어있다.
    // 이 클래스의 인스턴스가 생성될 때 특별한 초기화 작업은 필요하지 않으며, 주된 기능은 선택된 노드를 인스펙터 뷰에 표시하는 것이다.
    public InspectorView()
    {

    }

    // 사용자가 행동 트리 에디터 내에서 다른 노드를 선택했을 때 호출된다.
    internal void UpdateSelection(NodeView nodeView)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(_editor);

        _editor = Editor.CreateEditor(nodeView.node);
        IMGUIContainer container = new IMGUIContainer(() =>
        {
            if (_editor && _editor.target)
            {
                _editor.OnInspectorGUI();
            }
        });
        Add(container);
    }
}