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
		
	}



}
