using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmptyClick : MonoBehaviour, IPointerClickHandler
{
    public Action onEmptyClick;

    public void OnEnable()
    {
        var abilityButtons = GetComponentsInChildren<AbilityIncreaseButton>();
        foreach (var button in abilityButtons)
        {
            onEmptyClick += button.FocusOut;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var SomthingClicked = EventSystem.current.currentSelectedGameObject;

        if (SomthingClicked == null)
        {
            onEmptyClick?.Invoke();
        }
    }
}