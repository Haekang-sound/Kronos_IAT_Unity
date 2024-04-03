using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	// �ʿ��� ����

	public float speed;
	float hAxis;
	float vAxis;
	bool wDown;

	Vector3 moveVec;
	Animator anim;

	void Start()
	{
		// anim = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		hAxis = Input.GetAxisRaw("Horizontal");	// GetAxisRaw�� -,0,+ ���� �����´�(���⸸ ������)
		vAxis = Input.GetAxisRaw("Vertical");
		// wDown = Input.GetButton("Walk"); // ���ۿ��θ� �޴´�.

		// ������ ���� �̵��� ������ 0�� �ִ´�.
		moveVec = new Vector3(hAxis, 0, vAxis).normalized;


		transform.position += moveVec * speed */* (wDown ? 1f : 0.5f) **/ Time.deltaTime;

		//anim.SetBool("isWalk", moveVec != Vector3.zero);
		//anim.SetBool("isRun", wDown);

		transform.LookAt(transform.position + moveVec); // �츮�� ���ư��� �������� �ٶ󺻴�.

	}
}

