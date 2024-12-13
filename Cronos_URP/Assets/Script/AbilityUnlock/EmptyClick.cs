using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 스킬 트리 UI에서 빈 공간을 클릭했을 때 이벤트를 처리하며, 
/// 연결된 모든 AbilityNode의 포커스를 해제합니다.
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