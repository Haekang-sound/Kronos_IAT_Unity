using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 'InspectorView' Ŭ������ ����ڰ� �ൿ Ʈ�� ������ ������ ��带 �������� ��, �ش� ����� �Ӽ��� ǥ���ϴ� �ν����� ���� ������ �Ѵ�.
/// �̴� Unity�� �⺻ �ν����� ����� ����Ͽ�, ���õ� ��ü�� ���� ������ ������ �� �ִ� �������̽��� �����Ѵ�.
/// </summary>
public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    // ���õ� ��带 ���� 'Editor' �ν��Ͻ��̴�. �� 'Editor' ��ü�� ���õ� ����� �Ӽ��� �ν����� UI�� ǥ���ϰ� ������ �� �ְ� ���ش�.
    Editor _editor;

    // 'InspectorView' �� �����ڴ� �⺻������ ����ִ�.
    // �� Ŭ������ �ν��Ͻ��� ������ �� Ư���� �ʱ�ȭ �۾��� �ʿ����� ������, �ֵ� ����� ���õ� ��带 �ν����� �信 ǥ���ϴ� ���̴�.
    public InspectorView()
    {

    }

    // ����ڰ� �ൿ Ʈ�� ������ ������ �ٸ� ��带 �������� �� ȣ��ȴ�.
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