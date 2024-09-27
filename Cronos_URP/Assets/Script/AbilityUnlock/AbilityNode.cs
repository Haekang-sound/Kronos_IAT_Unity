using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilityNode : MonoBehaviour, IObservable<AbilityNode>
{
    public AbilityLevel abilityLevel = new AbilityLevel();

    public ImageGrayscale background;
    public ImageGrayscale skillIcon;

    [SerializeField] public TMP_Text abilityName;
    [SerializeField] public TMP_Text description;
    [SerializeField] public TMP_Text subdescription;

    //public Button[] childButton;
    public List<AbilityNode> childNodes;

    public bool isFocaus;
    public bool interactable;
    public Button button;
    public FadeEffector fadeUI;

    private CinemachineVirtualCamera _virtualCam;
    private IObserver<AbilityNode> _observer;

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
    }

    public void FocusOut()
    {
        StartCoroutine(SetFocausAfter(false, 2));
        _virtualCam.Priority = 0;
        fadeUI.StartFadeOut(1.0f);
    }

    public void InitRender()
    {
        description.text = abilityLevel.descriptionText;
        subdescription.text = $"CP {abilityLevel.pointNeeded} 필요";

        Render();
    }

    public void UpdateChilds()
    {
        if (abilityLevel.currentPoint == abilityLevel.nextNodeUnlockCondition ||
            abilityLevel.currentPoint >= abilityLevel.maxPoint)
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
        abilityName.text = $"{abilityLevel.abilityName}";

        // TEST
        if (abilityLevel.currentPoint == 1)
        {
            skillIcon.SetGrayscale(0f);
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Skill/main_gear_nolight");
        }
    }

    public bool Increment()
    {
        int addedPoint = abilityLevel.currentPoint + 1;

        bool result = addedPoint <= abilityLevel.maxPoint;

        if (result == true)
        {
            abilityLevel.currentPoint = addedPoint;

            // 자식 노드 활성화
            UpdateChilds();
            Render();
        }

        return result;
    }
}
