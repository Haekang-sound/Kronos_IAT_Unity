using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ʱ� �÷��̾� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class PlayerMove : MonoBehaviour
{
	// �ʿ��� ����
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
		_hAxis = Input.GetAxisRaw("Horizontal"); // �̵������� �����´�.
		_vAxis = Input.GetAxisRaw("Vertical");

		// �̵���ư�� ���ȴٸ�
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

		// ������
		if (Input.GetButtonDown("Jump") && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
		{
			_anim.SetTrigger("Dodge");
		}

		if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
		{
			transform.position += transform.forward.normalized * speed * 1.5f * _dTime;
		}

		// ���ݸ��
		if (Input.GetButtonDown("Punch") && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
		{
			_anim.SetTrigger("Punch");
		}

		if (Input.GetButtonDown("Hook") && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Hook"))
		{
			_anim.SetTrigger("Hook");
		}

		// �޸��� ��ư�� ���ȴٸ�
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

		// �̵����̶�� 
		if (walking)
		{
			Debug.Log("������");
			transform.position += transform.forward.normalized * (wRun == true ? speed * 3f : speed) * _dTime; // ����
		}
	}
}

