using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeCounter : MonoBehaviour
{
	// �̱��� ��ü �Դϴ�. 
	public static CoolTimeCounter Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}

			// �ν��Ͻ��� ���ٸ� ���� ����â���� �˻��ؼ� ������.
			instance = FindObjectOfType<CoolTimeCounter>();

			return instance;
		}
	}
	protected static CoolTimeCounter instance;

	[SerializeField] private Image dodgeImage;
	[SerializeField] private Image rushAttackImage;
	[SerializeField] private Image slash;
	[SerializeField] private Image timeStop;
	[SerializeField] private Image bomb;

	// ��뿩��
	public bool isRushAttackUsed { get; set; }
	public bool isDodgeUsed{ get; set; }

	// ��Ÿ��
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

		if(Player.Instance.CP >= 20f)
		{
			slash.enabled = true;
		}
		else
		{
			slash.enabled = false;
		}

		if (Player.Instance.CP >= 100f)
		{
			timeStop.enabled = true;
		}
		else
		{
			timeStop.enabled = false;
		}

		if (Player.Instance.IsDecreaseCP) 
		{ 
			bomb.enabled = true;
		}
		else 
		{
			bomb.enabled = false;
		}

	}



}
