using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHUD : MonoBehaviour
{
	// 싱글벙글턴
	static UI_BossHUD instance;
	public static UI_BossHUD GetInstance() { return instance; }

	public float speed = 1f ;

	[SerializeField]
	[Range(0, 1)] float BossTPProgress = 0f;
	[SerializeField]
	Image ImageTPL;
	[SerializeField]
	Image ImageTPR;

	[SerializeField]
	[Range(0, 1)] float GroggyProgress = 0f;
	[SerializeField]
	Image ImageGPL;
	[SerializeField]
	Image ImageGPR;



	// ui 움직이는 시간
	public float uiDuration = 0.2f;

	Damageable BossTP;
	float BossTp;
	float MaxTP;

	GroggyStack BossGroggyStack;
	float groggyPoint;
	float MaxGroggyPoint;


	Transform parentTrans;
	ParticleSystem ps;
	
	private void Start()
	{
		instance = this;

		BossGroggyStack = GameObject.Find("Boss").GetComponent<GroggyStack>();
		BossTP = GameObject.Find("Boss").GetComponent<Damageable>();

		if (BossGroggyStack == null || BossTP == null)
		{
			Debug.Log("보스가 나인데수");
		}

	}

	// Update is called once per frame
	void Update()
	{
		// 보스의 TP를 받아온다.
		MaxTP = BossTP.maxHitPoints;
		BossTp = BossTP.currentHitPoints;
		BossTPProgress = BossTp / MaxTP;

		float temp = ImageTPL.fillAmount;
		float dValue = 	Mathf.Lerp(temp, BossTPProgress,Time.deltaTime* speed);
		ImageTPL.fillAmount = dValue;
		ImageTPR.fillAmount = dValue;

		// 보스의 GrogyPoint를 받아온다.
		MaxGroggyPoint = BossGroggyStack.maxStack;
		groggyPoint = BossGroggyStack._currentStack;
		GroggyProgress = groggyPoint / MaxGroggyPoint;

		float gtemp = ImageGPL.fillAmount;
		float gdValue = Mathf.Lerp(gtemp, GroggyProgress, Time.deltaTime * speed);
		ImageGPL.fillAmount = gdValue;
		ImageGPR.fillAmount = gdValue;

	}


}
