using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class AbilityTree : MonoBehaviour, IObserver<AbilityNode>
{
    [SerializeField] public AbilityNode rootAbilityNode;
    [SerializeField] public AbilityAmountLimit abilityAmounts;

    public Canvas abilityTreeCanvas;
    public PopupController popup;
    public CanvasGroup canvasGroup;
    public CinemachineVirtualCamera mainVirtualCam;

    public bool useParser;

    private bool isFocus;
    private List<AbilityNode> _abilityNodes;

    private List<IObservable<AbilityNode>> _obserables;
    private List<IDisposable> _unsubscribers;

    private AbilityNode _lastPressed;

    private bool _isTransition;

    public UnityEvent OnEnter;

    // IObserver /////////////////////////////////////////////////////////////

    public virtual void Subscribe(IObservable<AbilityNode> provider)
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

    public virtual void OnNext(AbilityNode value)
    {
        if (value.isFucus == false)
        {
            value.FocusIn();

            if (_lastPressed != null &&
                _lastPressed != value)
            {
                _lastPressed.FocusOut();
            }
        }
        else if (value.isFucus == true && value.CurrentState == AbilityNode.State.Interactible)
        {
            if (abilityAmounts.CanSpend(value.PointNeed) != -1)
            {
                // 팝업창을 열고, 확인 버튼을 눌렀을 때 수행할 동작을 정의
                popup.OpenPopup("확실합니까?", () =>
                {
                    // 확인 버튼을 눌렀을 때 실행할 동작
                    value.SetState(AbilityNode.State.Activate);
                    abilityAmounts.UpdateSpent(value.PointNeed);
                });
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

    private void Awake()
    {
        // 구독자 구독
        _obserables = GetComponentsInChildren<IObservable<AbilityNode>>().ToList();
        _abilityNodes = GetComponentsInChildren<AbilityNode>().ToList();

        foreach (var obserable in _obserables)
        {
            obserable.Subscribe(this);
        }

        abilityTreeCanvas.enabled = false;

        if (mainVirtualCam == null)
        {
            mainVirtualCam = PlayerCamControler.Instance.VirtualCamera;
        }
    }

    private void Start()
    {
        rootAbilityNode.SetState(AbilityNode.State.Interactible);

        if (mainVirtualCam == null) mainVirtualCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
    }

    public void SaveData(string purpose)
    {
        foreach (var node in _abilityNodes)
        {
            node.Save(purpose);
        }
    }

    public void LoadData(string purpose)
    {
        foreach (var node in _abilityNodes)
        {
            node.Load(purpose);
        }

        if(rootAbilityNode.CurrentState == AbilityNode.State.Deactivate)
        {
            rootAbilityNode.SetState(AbilityNode.State.Interactible);
        }
    }

    public void ResetData()
    {
        foreach (var node in _abilityNodes)
        {
            node.Reset();
        }
    }

    public void EnterAbility()
    {
        if (_isTransition == false)
        {
            if (isFocus == false)
            {
                StartCoroutine(Enter());
                OnEnter.Invoke();
            }
        }
    }

    public void ExitAbility()
    {
        if (_isTransition == false)
        {
            if (isFocus == true)
            {
                StartCoroutine(Exit());
            }
        }
    }

    public IEnumerator Enter()
    {
        _isTransition = true;

        PauseManager.Instance.PauseGame();

        yield return StartCoroutine(ScreenFader.FadeSceneOut());

        while (ScreenFader.IsFading)
        {
            yield return null;
        }

        abilityTreeCanvas.enabled = true;
        abilityAmounts.UpdatePlayerTimePoint();

        SetEnabledButtons(true);
        SetPlayerCamPriority(0);
        canvasGroup.alpha = 1f;

        yield return StartCoroutine(ScreenFader.FadeSceneIn());

        while (ScreenFader.IsFading)
        {
            yield return null;
        }

        isFocus = true;
        _isTransition = false;
    }

    public IEnumerator Exit()
    {
        _isTransition = true;

        yield return StartCoroutine(ScreenFader.FadeSceneOut());

        while (ScreenFader.IsFading)
        {
            yield return null;
        }

        abilityTreeCanvas.enabled = false;
        SetPlayerCamPriority(10);
        canvasGroup.alpha = 0f;
        SetEnabledButtons(false);

        yield return StartCoroutine(ScreenFader.FadeSceneIn());

        while (ScreenFader.IsFading)
        {
            yield return null;
        }

        isFocus = false;
        PauseManager.Instance.UnPauseGame();

        _isTransition = false;
    }

    void SetPlayerCamPriority(int val)
    {
        if (mainVirtualCam != null)
        {
            mainVirtualCam.Priority = val;
        }
    }

    private void SetEnabledButtons(bool val)
    {
        foreach (var node in _abilityNodes)
        {
            node.gameObject.SetActive(val);
        }
    }
}