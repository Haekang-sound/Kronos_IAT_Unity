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

    private bool isFocaus;
    private List<AbilityNode> _abilityNodes;

    private List<IObservable<AbilityNode>> _obserables;
    private List<IDisposable> _unsubscribers;

    private AbilityNode _lastPressed;

    private AbilityDataParser parser = new AbilityDataParser();

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
        if (value.isFocaus == false)
        {
            value.FocusIn();

            if (_lastPressed != null &&
                _lastPressed != value)
            {
                _lastPressed.FocusOut();
            }
        }
        else if (value.isFocaus == true && value.levelData.IscCmpleted() == false)
        {
            if (value.interactable == false) return;

            if (abilityAmounts.CanSpend(value.levelData.pointNeeded) != -1)
            {
                // 팝업창을 열고, 확인 버튼을 눌렀을 때 수행할 동작을 정의
                popup.OpenPopup("확실합니까?", () =>
                {
                    if (value.Increase() == true)
                    {
                        // 확인 버튼을 눌렀을 때 실행할 동작
                        abilityAmounts.UpdateSpent(value.levelData.pointNeeded);
                        value.OnUpdated.Invoke();
                    }
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

        if (mainVirtualCam == null)
        {
            mainVirtualCam = PlayerCamControler.Instance.VirtualCamera;
        }
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 0f;

        rootAbilityNode.SetInteractable(true);

        // 인덱스 부여
        for (int i = 0; i < _abilityNodes.Count; i++)
        {
            _abilityNodes[i].levelData.id = i;
        }

        // 로드 및 초기화
        LoadData();
    }

    public void SaveData()
    {
        foreach (var node in _abilityNodes)
        {
            node.Save();
        }
    }

    public void LoadData()
    {
        var edges = GetComponentsInChildren<SkillProgressLine>();
        foreach(var edge in  edges)
        {
            edge.Reset();
        }

        foreach (var node in _abilityNodes)
        {
            node.Reset();
        }

        rootAbilityNode.interactable = true;

        foreach (var node in _abilityNodes)
        {
            node.Load();
        }
    }

    public void EnterAbility()
    {
        if (_isTransition == false)
        {
            if (isFocaus == false)
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
            if (isFocaus == true)
            {
                StartCoroutine(Exit());
            }
        }
    }

    //public void LoadAbilityLevelData()
    //{
    //    var LoadedlevelDatas = parser.LoadLevelDataXML();
    //    var loadedUserDatas = parser.LoadUserDataXML();

    //    for (int i = 0; i < _abilityNodes.Count; i++)
    //    {
    //        _abilityNodes[i].levelData = LoadedlevelDatas[i];
    //        _abilityNodes[i].levelData.currentPoint = loadedUserDatas[i];
    //        _abilityNodes[i].Render();
    //    }
    //}

    public void InitChildNodeDatas()
    {
        foreach (var node in _abilityNodes)
        {
            foreach (var childNodeId in node.levelData.childIdNodes)
            {
                if (childNodeId != -1)
                {
                    var childNode = _abilityNodes[childNodeId];
                    node.childNodes.Add(childNode);
                }
            }
        }

        foreach (var node in _abilityNodes)
        {
            node.UpdateChilds();
        }
    }

    public void ResetUserData()
    {
        foreach (var node in _abilityNodes)
        {
            node.levelData.currentPoint = 0;
        }

        rootAbilityNode.levelData.currentPoint = 1;
    }

    public IEnumerator Enter()
    {
        _isTransition = true;

        PauseManager.Instance.PauseGame();
        PauseManager.Instance.abilityPause = true;

        yield return StartCoroutine(ScreenFader.FadeSceneOut());

        while (ScreenFader.IsFading)
        {
            yield return null;
        }

        abilityAmounts.UpdatePlayerTimePoint();

        SetEnabledButtons(true);
        SetPlayerCamPriority(0);
        canvasGroup.alpha = 1f;

        yield return StartCoroutine(ScreenFader.FadeSceneIn());

        while (ScreenFader.IsFading)
        {
            yield return null;
        }

        isFocaus = true;
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

        //abilityTreeCanvas.enabled = false;
        SetPlayerCamPriority(10);
        canvasGroup.alpha = 0f;
        SetEnabledButtons(false);

        if (useParser)
        {
            SaveUserData();
        }

        yield return StartCoroutine(ScreenFader.FadeSceneIn());

        while (ScreenFader.IsFading)
        {
            yield return null;
        }

        isFocaus = false;
        PauseManager.Instance.abilityPause = false;
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

    private void SaveUserData()
    {
        List<AbilityLevel> abilityData = new List<AbilityLevel>();
        foreach (var node in _abilityNodes)
        {
            abilityData.Add(node.levelData);
        }

        parser.SaveUserDataXML(abilityData);
    }

    private void SetEnabledButtons(bool val)
    {
        foreach (var node in _abilityNodes)
        {
            node.gameObject.SetActive(val);
        }
    }
}