using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 초기 플레이어 클래스
/// 
/// ohk    v1
/// </summary>
public class PlayerMove : MonoBehaviour
{
	// 필요한 변수
	public float speed;

	private float _hAxis;
	private float _vAxis;
	private bool walking;
	private bool wRun;
	private Transform _PlayerCAMRoot;    
	private Transform _Player;   
	private Animator _animator;

	private Vector3 _moveVec;
	private Animator _anim;

	private float _dTime;

	void Start()
	{
		_anim = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		_dTime = Time.deltaTime;
		_hAxis = Input.GetAxisRaw("Horizontal"); // 이동방향을 가져온다.
		_vAxis = Input.GetAxisRaw("Vertical");

		// 이동버튼이 눌렸다면
		if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
		{
			_anim.SetBool("isWalking", true);
			walking = true;
		}
		else
		{
			_anim.SetBool("isWalking", false);
			walking = false;
		}

		// 구르기
		if (Input.GetButtonDown("Jump") && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
		{
			_anim.SetTrigger("Dodge");
		}

		if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
		{
			transform.position += transform.forward.normalized * speed * 1.5f * _dTime;
		}

		// 공격모션
		if (Input.GetButtonDown("Punch") && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
		{
			_anim.SetTrigger("Punch");
		}

		if (Input.GetButtonDown("Hook") && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Hook"))
		{
			_anim.SetTrigger("Hook");
		}

		// 달리기 버튼이 눌렸다면
		if (Input.GetButton("Run"))
		{
			_anim.SetBool("isRunning", true);
			wRun = true;
		}
		else
		{
			_anim.SetBool("isRunning", false);
			wRun = false;
		}

		// 이동중이라면 
		if (walking)
		{
			Debug.Log("앞으로");
			transform.position += transform.forward.normalized * (wRun == true ? speed * 3f : speed) * _dTime; // 전진
		}
	}
}

