using Cinemachine;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityIncreaseButton : MonoBehaviour, IObservable<AbilityIncreaseButton>
{
    public AbilityLevel abilityLevel = new AbilityLevel();

    [SerializeField] public TMP_Text abilityName;
    [SerializeField] public TMP_Text description;
    [SerializeField] public TMP_Text subdescription;

    public Button[] childButton;

    public FadeEffector fadeUI;

    public bool isFocaus;

    private Button _button;
    private CinemachineVirtualCamera _virtualCam;
    private IObserver<AbilityIncreaseButton> _observer;


    // IObservable /////////////////////////////////////////////////////////////

    public IDisposable Subscribe(IObserver<AbilityIncreaseButton> observer)
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
        private AbilityIncreaseButton _observable;

        public Unsubscriber(AbilityIncreaseButton observable)
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
        _button = GetComponent<Button>();
        _virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClickButton);
        fadeUI.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void Start()
    {
        description.text = abilityLevel.descriptionText;
        subdescription.text = $"CP {abilityLevel.pointNeeded} 필요";

        Render();
    }


    private void OnDisable()
    {
        isFocaus = false;
        _virtualCam.Priority = 0;
    }

    public void FocusIn()
    {
        StartCoroutine(SetFocausAfter(true, 2));
        _virtualCam.Priority = 10;
        fadeUI.StartFadeIn(1.8f);
    }

    public void FocusOut()
    {
        StartCoroutine(SetFocausAfter(false, 2));
        _virtualCam.Priority = 0;
        fadeUI.StartFadeOut(1.8f);
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
        abilityName.text = $"{abilityLevel.abilityName} ({abilityLevel.currentPoint}/{abilityLevel.maxPoint})";
    }

    public bool Increment()
    {
        int addedPoint = abilityLevel.currentPoint + 1;

        bool result = addedPoint <= abilityLevel.maxPoint;

        if (result == true)
        {
            abilityLevel.currentPoint = addedPoint;
        }

        // 자식 노드 활성화
        UpdateChilds();
        Render();

        return result;
    }

    private void UpdateChilds()
    {
        if (abilityLevel.currentPoint == abilityLevel.nextNodeUnlockCondition ||
            abilityLevel.currentPoint == abilityLevel.maxPoint)
        {
            foreach (var child in childButton)
            {
                child.interactable = true;
            }
        }
    }
}
