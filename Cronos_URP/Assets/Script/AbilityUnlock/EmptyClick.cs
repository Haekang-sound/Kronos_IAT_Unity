using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ��ų Ʈ�� UI���� �� ������ Ŭ������ �� �̺�Ʈ�� ó���ϸ�, 
/// ����� ��� AbilityNode�� ��Ŀ���� �����մϴ�.
/// </summary>

public class EmptyClick : MonoBehaviour, IPointerClickHandler
{
    public Action OnEmptyClick;

    public void OnEnable()
    {
        var abilityButtons = GetComponentsInChildren<AbilityNode>();
        foreach (var button in abilityButtons)
        {
            OnEmptyClick += button.FocusOut;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var SomthingClicked = EventSystem.current.currentSelectedGameObject;

        if (SomthingClicked == null)
        {
            OnEmptyClick?.Invoke();
        }
    }
}