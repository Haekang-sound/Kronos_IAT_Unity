using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	// 사용여부
	public bool isRushAttackUsed { get; set; }
	public bool isDodgeUsed{ get; set; }

	// 쿨타임
	public float rushAttackCoolTime;
	public float dodgeCoolTime;

	[SerializeField] private float rushATimer;
	[SerializeField] private float dodgeTimer;

	private void Update()
	{
		if (isRushAttackUsed)
		{
			rushATimer += Time.unscaledDeltaTime;
			rushAttackImage.fillAmount = rushATimer/rushAttackCoolTime;
			if (rushATimer >= rushAttackCoolTime)
			{
				isRushAttackUsed = false;
				rushATimer = 0f ;
				return;
			}
		}
		

		if (isDodgeUsed)
		{
			dodgeTimer += Time.unscaledDeltaTime;
			dodgeImage.fillAmount = dodgeTimer / dodgeCoolTime;
			if (dodgeTimer >= dodgeCoolTime)
			{
				isDodgeUsed = false;
				dodgeTimer = 0f;
				return;
			}
		}
		
	}



}
