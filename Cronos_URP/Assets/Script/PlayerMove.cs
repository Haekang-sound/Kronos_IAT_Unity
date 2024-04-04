using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	// �ʿ��� ����
	public float speed;
	float hAxis;
	float vAxis;
	bool wRun;
	Transform PlayerCAMRoot;	// ī�޶��Ʈ�� ���� ? 
	Animator m_animator;		// �ִϸ����������� �����ͼ� �ݿ�����

	Vector3 moveVec;
	Animator anim;

	private void Awake()
	{
		// �÷��̾� ī�޶��� Ʈ�������� �����´�.
		PlayerCAMRoot = GetComponentsInChildren<Transform>()[0];
	}

	void Start()
	{
		anim = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		// GetAxisRaw�� -,0,+ ���� �����´�(���⸸ ������)
		hAxis = Input.GetAxisRaw("Horizontal");	// x�࿡ �������� �����δ�.
		vAxis = Input.GetAxisRaw("Vertical");	// x�࿡ �������� �����δ�.

		wRun = Input.GetButton("Run"); // ���ۿ��θ� �޴´�.

		// ������ ���� �̵��� ������ 0�� �ִ´�.
		moveVec = new Vector3(hAxis, 0, vAxis).normalized;


		transform.position += moveVec * speed */* (wDown ? 1f : 0.5f) **/ Time.deltaTime;

		//anim.SetBool("isWalk", moveVec != Vector3.zero);
		//anim.SetBool("isRun", wDown);

		transform.LookAt(transform.position + moveVec); // �츮�� ���ư��� �������� �ٶ󺻴�.

	}
}

