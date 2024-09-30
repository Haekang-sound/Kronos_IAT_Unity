using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHUD : MonoBehaviour
{
	// �̱ۺ�����
	static UI_BossHUD instance;
	public static UI_BossHUD GetInstance() { return instance; }

	[SerializeField]
	[Range(0, 1)] float GroggyProgress = 0f;
	[SerializeField]
	Image circleImageCPL;
	[SerializeField]
	Image circleImageCPR;



	// ui �����̴� �ð�
	public float uiDuration = 0.2f;

	GroggyStack BossGroggyStack;
	float groggyPoint;
	float MaxGroggyPoint;


	Transform parentTrans;
	ParticleSystem ps;
	
	private void Start()
	{
		instance = this;

		BossGroggyStack = GameObject.Find("Boss").GetComponent<GroggyStack>();

		if (BossGroggyStack == null)
		{
			Debug.Log("������ ���ε���");
		}

	}

	// Update is called once per frame
	void Update()
	{
		// �÷��̾��� CP�� �޾ƿ´�.
		MaxGroggyPoint = BossGroggyStack.maxStack;
		groggyPoint = BossGroggyStack._currentStack;
		GroggyProgress = groggyPoint / MaxGroggyPoint;
		circleImageCPL.fillAmount = GroggyProgress;
		circleImageCPR.fillAmount = GroggyProgress;
	}


}
