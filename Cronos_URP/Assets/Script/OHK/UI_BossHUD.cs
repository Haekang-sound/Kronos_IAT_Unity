using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 보스 ui정보를 관리하는 클래스
/// 
/// ohk    v1
/// </summary>
public class UI_BossHUD : MonoBehaviour
{
	private static UI_BossHUD instance;
	public static UI_BossHUD GetInstance() { return instance; }

	public float speed = 1f ;
	public GameObject boss;
	public BossNameShaker shaker;

	[SerializeField]
	[Range(0, 1)] private float _bossTPProgress = 0f;
	[SerializeField]
	private Image _imageTPL;
	[SerializeField]
	private Image _imageTPR;

	[SerializeField]
	[Range(0, 1)] private float _groggyProgress = 0f;
	[SerializeField]
	private Image _imageGPL;
	[SerializeField]
	private Image _iImageGPR;

	// ui 움직이는 시간
	public float uiDuration = 0.2f;

	private Damageable _bossTP;
	private float _bossTp;
	private float _maxTP;

	private GroggyStack _bossGroggyStack;
	private float _groggyPoint;
	private float _maxGroggyPoint;

	private void Start()
	{
		instance = this;

		_bossGroggyStack = boss.GetComponent<GroggyStack>();
		_bossTP = boss.GetComponent<Damageable>();

		if (_bossGroggyStack == null || _bossTP == null)
		{
			Debug.Log("보스가 없습니다");
		}

	}

	void Update()
	{
        // 보스의 TP를 받아온다.
        _maxTP = _bossTP.maxHitPoints;
		_bossTp = _bossTP.currentHitPoints;
		_bossTPProgress = _bossTp / _maxTP;

		float temp = _imageTPL.fillAmount;
		float dValue = 	Mathf.Lerp(temp, _bossTPProgress,Time.deltaTime* speed);
		_imageTPL.fillAmount = dValue;
		_imageTPR.fillAmount = dValue;

		// 보스의 GrogyPoint를 받아온다.
		_maxGroggyPoint = _bossGroggyStack.maxStack;
		_groggyPoint = _bossGroggyStack._currentStack;
		_groggyProgress = _groggyPoint / _maxGroggyPoint;

		float gtemp = _imageGPL.fillAmount;
		float gdValue = Mathf.Lerp(gtemp, _groggyProgress, Time.deltaTime * speed);
		_imageGPL.fillAmount = gdValue;
		_iImageGPR.fillAmount = gdValue;

		if (_groggyProgress >= 1f)
		{
			shaker.isShaking = true;
		}
        else
        {
             shaker.isShaking = false;
        }

    }


}
