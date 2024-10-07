using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

public class AbilityNode : MonoBehaviour, IObservable<AbilityNode>
{
    public AbilityLevel levelData = new AbilityLevel();

    public ImageGrayscale background;
    public ImageGrayscale skillIcon;

    [SerializeField] public TMP_Text abilityName;
    [SerializeField] public TMP_Text description;
    [SerializeField] public TMP_Text subdescription;

    //public Button[] childButton;
    public List<AbilityNode> childNodes;

    public bool isFocaus;
    public bool  interactable;
    public Button button;
    public FadeEffector fadeUI;

    private CinemachineVirtualCamera _virtualCam;
    private IObserver<AbilityNode> _observer;
    private VideoPlayer _videoPlayer;

    public UnityEvent OnUpdated;

    // IObservable /////////////////////////////////////////////////////////////

    public IDisposable Subscribe(IObserver<AbilityNode> observer)
    {
        if (_observer != null)
        {
            throw new InvalidOperationException("관찰자(observer)는 한 명만 있어야 합니다.");
        }
        _observer = observer;
        return new Unsubscriber(this);
    }

    private class Unsubscriber : IDisposable
    {
        private AbilityNode _observable;

        public Unsubscriber(AbilityNode observable)
        {
            _observable = observable;
        }

        public void Dispose()
        {
            _observable._observer = null;
        }
    }

    // MonoBehaviour /////////////////////////////////////////////////////////////

    private void Awake()
    {
        _virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();
        _videoPlayer = GetComponentInChildren<VideoPlayer>();
        _videoPlayer?.Pause();


        //background.SetGrayscale(1f);
        //skillIcon.SetGrayscale(1f);
    }

    private void OnEnable()
    {
        fadeUI.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void Start()
    {
        InitRender();
        button.onClick.AddListener(OnClickButton);
    }

    //private void Update()
    //{
    //}

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClickButton);
    }

    public void SetInteractable(bool val)
    {
        //button.interactable = val;
        interactable = val;

        if (interactable)
        {
            background.SetGrayscale(0f);
        }
    }

    private void OnDisable()
    {
        isFocaus = false;
        _virtualCam.Priority = 0;
    }

    // =====

    public void FocusIn()
    {
        StartCoroutine(SetFocausAfter(true, 2));
        _virtualCam.Priority = 10;
        fadeUI.StartFadeIn(1.4f);
        _videoPlayer.time = 0;
        _videoPlayer?.Play();
    }

    public void FocusOut()
    {
        StartCoroutine(SetFocausAfter(false, 2));
        _virtualCam.Priority = 0;
        fadeUI.StartFadeOut(1.0f);
        _videoPlayer?.Pause();
    }

    public void InitRender()
    {
        description.text = levelData.descriptionText;
        subdescription.text = $"CP {levelData.pointNeeded} 필요";

        Render();
    }

    public void UpdateChilds()
    {
        if (levelData.currentPoint == levelData.nextNodeUnlockCondition ||
            levelData.currentPoint >= levelData.maxPoint)
        {
            foreach (var child in childNodes)
            {
                child.SetInteractable(true);
            }
        }
    }

    private void OnClickButton()
    {
        _observer.OnNext(this);
    }

    private IEnumerator SetFocausAfter(bool val, float time)
    {
        // 지정된 시간(2초) 대기
        yield return new WaitForSecondsRealtime(time);

        isFocaus = val;
    }

    private void Render()
    {
        //abilityName.text = $"{abilityLevel.abilityName} ({abilityLevel.currentPoint}/{abilityLevel.maxPoint})";
        abilityName.text = $"{levelData.abilityName}";

        if (levelData.currentPoint == 1)
        {
            //skillIcon.SetGrayscale(0f);
            skillIcon.StartGrayScaleRoutine();
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Skill/main_gear_nolight");
            background.GetComponent<RotateUI>().active = true;
        }
    }

    public bool Increment()
    {
        int addedPoint = levelData.currentPoint + 1;

        bool result = addedPoint <= levelData.maxPoint;

        if (result == true)
        {
            levelData.currentPoint = addedPoint;

            // 자식 노드 활성화
            UpdateChilds();
            Render();
        }

        return result;
    }

    public void Save()
    {
        var key = "node_" + levelData.id;
        var interactableInt = interactable ? 1 : 0;
        PlayerPrefs.SetInt(key, interactableInt);
        PlayerPrefs.SetInt("currentPoint_" + levelData.id, levelData.currentPoint);

        Debug.Log("TreeNode( " + key + ": " + interactableInt + ") has Saved");
    }

    internal void Load()
    {
        var key = "node_" + levelData.id;

        if (PlayerPrefs.HasKey("node_" + levelData.id))
        {
            var num = PlayerPrefs.GetInt(key);
            if (num >= 1)
            {
                SetInteractable(true);
                background.SetGrayscale(0f);
            }

            var point = PlayerPrefs.GetInt("currentPoint_" + levelData.id);
            for (int i = 0; i < point; i++)
            {
                if(true == Increment());
                    OnUpdated.Invoke();
            }

            Debug.Log("TreeNode( " + key + ": " + num + "  ) has Loaded");
        }
    }
}
