using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTree : MonoBehaviour, IObserver<AbilityNode>
{
    [SerializeField] public AbilityNode rootAbilityNode;
    [SerializeField] public AbilityAmountLimit abilityAmounts;

    public Canvas abilityTreeCanvas;
    public CanvasGroup canvasGroup;
    //public CinemachineVirtualCamera playerVirtualCam;

    private bool isFocaus;
    private List<AbilityNode> _abilityNodes;

    private List<IObservable<AbilityNode>> _obserables;
    private List<IDisposable> _unsubscribers;

    private AbilityNode _lastPressed;

    private AbilityDataParser parser = new AbilityDataParser();

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

    private void Awake()
    {
        // 구독자 구독
        _obserables = GetComponentsInChildren<IObservable<AbilityNode>>().ToList();
        _abilityNodes = GetComponentsInChildren<AbilityNode>().ToList();

        foreach (var obserable in _obserables)
        {
            obserable.Subscribe(this);
        }

        rootAbilityNode.SetInteractable(false);
        abilityTreeCanvas.enabled = false;
        //playerVirtualCam = PlayerCamControler.Instance.VirtualCamera;
    }

    private void OnEnable()
    {
        rootAbilityNode.SetInteractable(true);
        canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        LoadAbilityLevelData();
        InitChildNodeDatas();
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

    public void LoadAbilityLevelData()
    {
        var LoadedlevelDatas = parser.LoadLevelDataXML();
        var loadedUserDatas = parser.LoadUserDataXML();

        for (int i = 0; i < _abilityNodes.Count; i++)
        {
            _abilityNodes[i].abilityLevel = LoadedlevelDatas[i];
            _abilityNodes[i].abilityLevel.currentPoint = loadedUserDatas[i];
            _abilityNodes[i].InitRender();

        }
    }

    public void InitChildNodeDatas()
    {
        foreach (var node in _abilityNodes)
        {
            foreach (var childNodeId in node.abilityLevel.childIdNodes)
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
            node.abilityLevel.currentPoint = 0;
        }

        rootAbilityNode.abilityLevel.currentPoint = 1;
    }

    public void Enter()
    {
        abilityTreeCanvas.enabled = true;
        abilityAmounts.UpdatePlayerTimePoint();

        SetEnabledButtons(true);
        isFocaus = true;
        canvasGroup.alpha = 1f;
        PlayerCamControler.Instance.VirtualCamera.Priority = 0;
        PauseManager.Instance.PauseGame();
    }

    public void Exit()
    {
        abilityTreeCanvas.enabled = false;
        PauseManager.Instance.UnPauseGame();
        PlayerCamControler.Instance.VirtualCamera.Priority = 10;
        canvasGroup.alpha = 0f;
        isFocaus = false;
        SetEnabledButtons(false);

        SaveUserData();
    }

    private void SaveUserData()
    {
        List<AbilityLevel> abilityData = new List<AbilityLevel>();
        foreach (var node in _abilityNodes)
        {
            abilityData.Add(node.abilityLevel);
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