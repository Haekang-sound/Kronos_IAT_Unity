using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUnlockSystem : MonoBehaviour, IObserver<AbilityIncreaseButton>
{
    [SerializeField] public Button rootAbilityNode;
    [SerializeField] public AbilityAmountLimit abilityAmounts;

    public CanvasGroup canvasGroup;
    public CinemachineVirtualCamera playerVirtualCam;

    private bool isFocaus;
    private List<Button> _abilityNodes;

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
        _abilityNodes = GetComponentsInChildren<Button>().ToList();

        foreach (var obserable in _obserables)
        {
            obserable.Subscribe(this);
        }

        rootAbilityNode.interactable = false;
    }

    public void OnEnable()
    {
        rootAbilityNode.interactable = true;
        canvasGroup.alpha = 0f;
    }

    public void Update()
    {
        // Test
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isFocaus == false)
            {
                Enter();
            }
            else if (isFocaus == true)
            {
                Exit();
            }
        }
    }

    public void Enter()
    {
        abilityAmounts.UpdatePlayerTimePoint();

        SetEnabledButtons(true);
        isFocaus = true;
        canvasGroup.alpha = 1f;
        playerVirtualCam.Priority = 0;
        PauseManager.Instance.PauseGame();
    }

    public void Exit()
    {
        PauseManager.Instance.UnPauseGame();
        playerVirtualCam.Priority = 10;
        canvasGroup.alpha = 0f;
        isFocaus = false;
        SetEnabledButtons(false);
    }

    private void SetEnabledButtons(bool val)
    {
        foreach (var node in _abilityNodes)
        {
            node.gameObject.SetActive(val);
        }
    }
}