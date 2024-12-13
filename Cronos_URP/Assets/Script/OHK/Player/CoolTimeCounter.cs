using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 스킬의 쿨타임을 관리하는 클래스
/// 
/// ohk    v1
/// </summary>
public class CoolTimeCounter : MonoBehaviour
{
	// 싱글턴 객체 입니다. 
	public static CoolTimeCounter Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}

			// 인스턴스가 없다면 계층 구조창에서 검색해서 가져옴.
			instance = FindObjectOfType<CoolTimeCounter>();

			return instance;
		}
	}
	protected static CoolTimeCounter instance;

	[SerializeField] private Image dodgeImage;
	[SerializeField] private Image rushAttackImage;
	[SerializeField] private Image rushAttackBackground;
	[SerializeField] private Image slash;
	[SerializeField] private Image timeStop;
	[SerializeField] private Image bomb;


	[SerializeField] private Image rushAttackMask;
	[SerializeField] private Image slashMask;
	[SerializeField] private Image timeStopMask;


	// 사용여부
	public bool IsRushAttackUsed { get; set; }
	public bool IsDodgeUsed { get; set; }

	// 쿨타임
	public float rushAttackCoolTime;
	public float dodgeCoolTime;

	private float _rushATimer;
	private float _dodgeTimer;

	private void Update()
	{
		if(PlayerStateMachine.GetInstance().Animator.GetBool(PlayerHashSet.Instance.IsRushAttack))
		{
			rushAttackMask.enabled = false;
			rushAttackImage.enabled = true;
			rushAttackBackground.enabled = true;
			if (IsRushAttackUsed)
			{
				_rushATimer += Time.unscaledDeltaTime;
				rushAttackImage.fillAmount = _rushATimer / rushAttackCoolTime;
				if (_rushATimer >= rushAttackCoolTime)
				{
					IsRushAttackUsed = false;
					_rushATimer = 0f;
					return;
				}
			}
		}
		else
		{
			rushAttackMask.enabled = true;
			rushAttackImage.enabled = false;
			rushAttackBackground.enabled = false;
		}
		
		if (IsDodgeUsed)
		{
			_dodgeTimer += Time.unscaledDeltaTime;
			dodgeImage.fillAmount = _dodgeTimer / dodgeCoolTime;
			if (_dodgeTimer >= dodgeCoolTime)
			{
				IsDodgeUsed = false;
				_dodgeTimer = 0f;
				return;
			}
		}

		if (PlayerStateMachine.GetInstance().Animator.GetBool(PlayerHashSet.Instance.IsFlashSlash))
		{
			slashMask.enabled = false;
			if (Player.Instance.CP >= 20f)
			{
				slash.enabled = true;
			}
			else
			{
				slash.enabled = false;
			}
		}
		else
		{
			slashMask.enabled = true;
		}

		if(PlayerStateMachine.GetInstance().Animator.GetBool(PlayerHashSet.Instance.IsTimeStop))
		{
			timeStopMask.enabled = false;
			if (Player.Instance.CP >= 100f)
			{
				timeStop.enabled = true;
			}
			else
			{
				timeStop.enabled = false;
			}
		}
		else
		{
			timeStopMask.enabled = true;
		}
		
		if (Player.Instance.IsDecreaseCP
			&& PlayerStateMachine.GetInstance().Animator.GetBool(PlayerHashSet.Instance.IsCPBoomb))
		{
			bomb.enabled = true;
		}
		else
		{
			bomb.enabled = false;
		}

	}



}
