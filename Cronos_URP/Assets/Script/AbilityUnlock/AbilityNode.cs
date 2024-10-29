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
    public enum State
    {
        Deactivate = 0,
        Interactible,
        Activate
    }

    public AbilityNode.State CurrentState { get { return _state; } }
    [SerializeField] private AbilityNode.State _state;

    public int PointNeed { get { return _pointNeed; } }
    [SerializeField] private int _pointNeed;

    public List<AbilityNode> childNodes;

    [Header("Effect")]
    public ImageGrayscale background;
    public ImageGrayscale skillIcon;
    public FadeEffector fadeUI;

    [Header("Text")]
    public TMP_Text abilityName;
    public TMP_Text description;
    public TMP_Text subdescription;

    [Header("Event")]
    public UnityEvent OnUpdated;
    [HideInInspector]
    public UnityEvent OnLoaded;

    [HideInInspector]
    public bool isFucus;

    public string id;

    private Button _button;
    private VideoPlayer _videoPlayer;
    private IObserver<AbilityNode> _observer;
    private CinemachineVirtualCamera _virtualCam;

    // BG
    private Image _backgoundImage;
    private RotateUI _backgroundRotate;
    private UnityEngine.Sprite _backGroundActivate;
    private UnityEngine.Sprite _backGroundDeactivate;

    // -----

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

    public void Render()
    {
        switch (_state)
        {
            case AbilityNode.State.Deactivate:
                {
                    _backgoundImage.sprite = _backGroundDeactivate;
                    background.SetGrayscale(1f);
                    skillIcon.SetGrayscale(1f);
                    _backgroundRotate.enabled = false;
                }
                break;
            case AbilityNode.State.Interactible:
                {
                    _backgoundImage.sprite = _backGroundDeactivate;
                    background.SetGrayscale(0f);
                    skillIcon.SetGrayscale(1f);
                    _backgroundRotate.enabled = false;
                }
                break;
            case AbilityNode.State.Activate:
                {
                    _backgoundImage.sprite = _backGroundActivate;
                    background.SetGrayscale(0f);
                    skillIcon.SetGrayscale(0f);
                    _backgroundRotate.enabled = true;

                    // 적용될 스킬 업데이트
                    OnUpdated.Invoke();
                }
                break;
        }
    }

    public void SetChildsInteractible()
    {
        foreach (var child in childNodes)
        {
            child.SetState(AbilityNode.State.Interactible);
        }
    }


    public void SetState(AbilityNode.State state)
    {
        _state = state;

        switch (_state)
        {
            case AbilityNode.State.Deactivate:
                {
                    _backgoundImage.sprite = _backGroundDeactivate;
                    _backgroundRotate.enabled = false;
                    background.SetGrayscale(1f);
                    skillIcon.SetGrayscale(1f);
                }
                break;
            case AbilityNode.State.Interactible:
                {
                    _backgoundImage.sprite = _backGroundDeactivate;
                    _backgroundRotate.enabled = false;
                    background.SetGrayscale(0f);
                    skillIcon.SetGrayscale(1f);
                }
                break;
            case AbilityNode.State.Activate:
                {
                    _backgoundImage.sprite = _backGroundActivate;
                    _backgroundRotate.enabled = true;
                    background.SetGrayscale(0f);
                    skillIcon.Reset();

                    // 자식 노드들을 Interactible로 변경
                    SetChildsInteractible();
                    // 적용될 스킬 업데이트
                    OnUpdated.Invoke();
                }
                break;
        }

        // 기타 업데이트 (진행도 바)
        
    }

    public void Save(string purpose)
    {
        string key = id + purpose;
        PlayerPrefs.SetInt(key, (int)_state);
    }

    public void Load(string purpose)
    {
        string key = id + purpose;
        if (PlayerPrefs.HasKey(key))
        {
            AbilityNode.State loadedstate = (AbilityNode.State)PlayerPrefs.GetInt(key);
            SetState(loadedstate);
            OnLoaded?.Invoke();
        }
    }

    public void Reset()
    {
        PlayerPrefs.DeleteKey(id);
    }

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

        _backgoundImage = background.GetComponent<Image>();
        _backgroundRotate = background.GetComponent<RotateUI>();

        _backGroundActivate = Resources.Load<UnityEngine.Sprite>("UI/Skill/main_gear_nolight");
        _backGroundDeactivate = Resources.Load<UnityEngine.Sprite>("UI/Skill/main_gear_dark");

        _button = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        fadeUI.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void Start()
    {
        Render();
        _button.onClick.AddListener(OnClickButton);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClickButton);
    }

    private void OnDisable()
    {
        isFucus = false;
        _virtualCam.Priority = 0;
    }

    // -----

    private void OnClickButton()
    {
        _observer.OnNext(this);
    }

    private IEnumerator SetFocausAfter(bool val, float time)
    {
        // 지정된 시간(2초) 대기
        yield return new WaitForSecondsRealtime(time);

        isFucus = val;
    }
}
