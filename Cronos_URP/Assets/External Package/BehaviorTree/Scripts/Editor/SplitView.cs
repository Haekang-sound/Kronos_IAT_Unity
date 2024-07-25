using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 'SplitView' Ŭ������ 'TwoPaneSplitView' �� ����� ��ӹ޾�, �� ���� �ֿ� �����̳� �г��� ���� ���� �並 UI�� �����ϴ� �� ���ȴ�.
/// �� Ŭ���� ��ü�� ��ӹ��� ��� �ܿ� �߰����� �����̳� ��� ������ �������� �ʰ� �ִ�.
/// �׷��� �� Ŭ������ ��������ν�, 'TwoPaneSplitView' �� ���� ����� �߰��ϰų�, ���� ����� �������̵��Ͽ� ����� ���� ������ ������ �� �ִ� ����� ������ �� �ִ�.
/// </summary>
public class SplitView : TwoPaneSplitView
{
    public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }

    /*
     * public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
     * �� ���� Ŭ������ 'UxmlFactory' �� ��ӹ޾�, UI Toolkit�� UXML ���Ͽ��� 'SplitView' ��Ҹ� ����� �� �ְ� ���ش�.
     * �̸� ���� 'SplitView' ��Ҹ� UXML ���� ������ ���� ����� �� �ְ� �Ǹ�,
     * UI Toolkit�� ����� UI ���� �� 'SplitView' Ŀ���� ������Ʈ�� ȿ�������� Ȱ���� �� �ִ�.
     */
}
