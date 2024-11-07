using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Video;

public class AbilityTree : MonoBehaviour, IObserver<AbilityNode>
{
	[SerializeField] public AbilityNode rootAbilityNode;
	[SerializeField] public AbilityAmountLimit abilityAmounts;

	public Canvas abilityTreeCanvas;

	[Header("Node Detail")]
	public CanvasGroup abilityNodeDetail;
	public VideoPlayer abilityVideoPlayer;
	public TMP_Text skillNameText;
	public TMP_Text nodeCostText;
	public TMP_Text nodeDetailText;
	public Button buttonUnlock;
	public Button buttonCancel;

	[Header("Other UI")]
    public PopupController popup;
	public CanvasGroup canvasGroup;
	public CanvasGroup esc;
	public CinemachineVirtualCamera mainVirtualCam;

	public bool useParser;

	private bool isFocus;
	private bool _isTransition;

	private AbilityNode _lastPressed;
	private FadeEffector _nodedetailEffector;

	private List<AbilityNode> _abilityNodes;
	private List<IDisposable> _unsubscribers;
	private List<IObservable<AbilityNode>> _obserables;

	public UnityEvent OnEnter;

	SoundManager sm;

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
		//if (value.isFucus == false)
		{
			_nodedetailEffector.StartFadeIn(1.5f);
			abilityNodeDetail.blocksRaycasts = true;
			value.FocusIn();

            skillNameText.text = value.skillName.text;
            nodeCostText.text = "TP " + value.PointNeed + " 필요";
			nodeDetailText.text = value.description;

			abilityVideoPlayer.clip = value.videoClip;
            abilityVideoPlayer.time = 0;
            abilityVideoPlayer?.Play();

            if (_lastPressed != null &&
				_lastPressed != value)
			{
				_lastPressed.FocusOut();
            }
		}
		//else if (value.isFucus == true && value.CurrentState == AbilityNode.State.Interactible)
		//{
		//	if (abilityAmounts.CanSpend(value.PointNeed) != -1)
		//	{
		//		// �˾�â�� ����, Ȯ�� ��ư�� ������ �� ������ ������ ����
		//		popup.OpenPopup("Ȯ���մϱ�?", () =>
		//		{
		//			// Ȯ�� ��ư�� ������ �� ������ ����
		//			value.SetState(AbilityNode.State.Activate);
		//			abilityAmounts.UpdateSpent(value.PointNeed);
		//		});
		//	}
		//}
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
        _nodedetailEffector = abilityNodeDetail.GetComponent<FadeEffector>();

        // ������ ����
        _obserables = GetComponentsInChildren<IObservable<AbilityNode>>().ToList();
		_abilityNodes = GetComponentsInChildren<AbilityNode>().ToList();

		for (int i = 0; i < _abilityNodes.LongCount(); i++)
		{
			var node = _abilityNodes[i];
			node.id = i.ToString();
		}

		foreach (var obserable in _obserables)
		{
			obserable.Subscribe(this);
		}

		abilityTreeCanvas.enabled = false;

		if (mainVirtualCam == null)
		{
			mainVirtualCam = PlayerCamControler.Instance.VirtualCamera;
		}

		rootAbilityNode.SetState(AbilityNode.State.Interactible);
    }

    private void OnEnable()
    {
		abilityNodeDetail.alpha = 0f;
        abilityNodeDetail.blocksRaycasts = false;
    }

    private void Start()
	{
        if (mainVirtualCam == null) mainVirtualCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();

        buttonUnlock.onClick.AddListener(UnlockAbility);
		buttonCancel.onClick.AddListener(FocusOutAll);

		sm = SoundManager.Instance;
    }

    // -----

    public void FocusOutAll()
    {
        foreach (var button in _abilityNodes)
        {
            button.FocusOut();
        }
    }

    public void UnlockAbility()
    {
        if (_lastPressed.isFucus == true && _lastPressed.CurrentState == AbilityNode.State.Interactible)
        {
            if (abilityAmounts.CanSpend(_lastPressed.PointNeed) != -1)
            {
				// �˾�â�� ����, Ȯ�� ��ư�� ������ �� ������ ������ ����
				//popup.OpenPopup("Ȯ���մϱ�?", () =>
				//{
				// Ȯ�� ��ư�� ������ �� ������ ����
				// 사운드?
				AbilityOpenSound();

                _lastPressed.SetState(AbilityNode.State.Activate);
                abilityAmounts.UpdateSpent(_lastPressed.PointNeed);
                //});
                _nodedetailEffector.StartFadeOut(1.5f);
                abilityNodeDetail.blocksRaycasts = false;
                FocusOutAll();
            }
        }
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
		esc.alpha = 1f;

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
		esc.alpha = 0f;
		abilityNodeDetail.alpha = 0f;
		abilityNodeDetail.blocksRaycasts = false;

        SetEnabledButtons(false);

		yield return StartCoroutine(ScreenFader.FadeSceneIn());

		while (ScreenFader.IsFading)
		{
			yield return null;
		}

		isFocus = false;
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

	private void SetEnabledButtons(bool val)
	{
		foreach (var node in _abilityNodes)
		{
			node.gameObject.SetActive(val);
		}
	}

    public void AbilityOpenSound()
    {
        sm.PlaySFX("02 Ability_Open_Sound_SE", transform);
    }
}