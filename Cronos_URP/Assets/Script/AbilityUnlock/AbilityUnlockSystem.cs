using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class AbilityUnlockSystem : MonoBehaviour, IObserver<AbilityIncreaseButton>
{
    [SerializeField] public Button rootAbilityNode;
    [SerializeField] public AbilityAmountLimit abilityAmounts;

    private List<IObservable<AbilityIncreaseButton>> _obserables;
    private List<IDisposable> _unsubscribers;

    private AbilityIncreaseButton _lastPressed;

    // IObserver /////////////////////////////////////////////////////////////

    public virtual void Subscribe(IObservable<AbilityIncreaseButton> provider)
    {
        if (provider != null)
            _unsubscribers.Add(provider.Subscribe(this));
    }

    public virtual void OnCompleted()
    {
        this.Unsubscribe();
    }

    public virtual void OnError(Exception e)
    {
    }

    public virtual void OnNext(AbilityIncreaseButton value)
    {

        if (value.isFocaus == false)
        {
            value.FocusIn();

            if (_lastPressed != null &&
                _lastPressed != value)
            {
                _lastPressed.FocusOut();
            }
        }
        else if (value.isFocaus == true)
        {

            if (abilityAmounts.CanSpend(value.abilityLevel.pointNeeded) != -1)
            {
                if (value.Increment() == true)
                {
                    abilityAmounts.UpdateSpent(value.abilityLevel.pointNeeded);
                }

            }
        }
        
        _lastPressed = value;
    }

    public virtual void Unsubscribe()
    {
        foreach (var unsubscriber in _unsubscribers)
        {
            unsubscriber.Dispose();
            _unsubscribers.Remove(unsubscriber);
        }
    }

    // MonoBehaviour /////////////////////////////////////////////////////////////

    void Awake()
    {
        // 구독자 구독
        _obserables = GetComponentsInChildren<IObservable<AbilityIncreaseButton>>().ToList();

        foreach (var obserable in _obserables)
        {
            obserable.Subscribe(this);
        }

        rootAbilityNode.interactable = false;
    }

    public void OnEnable()
    {
        rootAbilityNode.interactable = true;
    }
}