using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal.Internal;
using System.Data.SqlTypes;
using UnityEngine.Purchasing.Extension;
using Unity.Mathematics;
public class AutoTargetting : MonoBehaviour
{

	public CinemachineFreeLook freeLookCamera;
	public float horizontalSpeed = 10.0f; // ���� ȸ�� �ӵ�
	public float verticalSpeed = 5.0f;    // ���� ȸ�� �ӵ�

	public GameObject Player;       // �÷��̾�
	public Transform Target;       // Player�� �ٶ� ���
	public Transform PlayerObject; // �÷��̾� ������Ʈ 
	public Transform maincamTransform;

	public float AixsDamp = 0.99f;  // ����������� ���� ���ΰ�!

	private PlayerStateMachine stateMachine;
	private MonsterSelector monsterSelector;

	private Vector3 direction;
	private float xDotResult;
	bool istargetting;

	// Start is called before the first frame update
	void Start()
	{
		stateMachine = Player.GetComponent<PlayerStateMachine>();

	}

	// Update is called once per frame
	void Update()
	{
		if (monsterSelector == null)
		{
			monsterSelector = GetComponent<MonsterSelector>();

		}

		// Player�� �ٶ� ������ ���Ѵ�.
		direction = Target.position - PlayerObject.position;
		direction.y = 0;    // y�����δ� ȸ������ �ʴ´�.

		xDotResult = Vector3.Dot(maincamTransform.right, PlayerObject.right);

		// ������ �Ͼ���� 
		if (Input.GetButton("Fire1"))
		{
			istargetting = true;
			monsterSelector.FindTarget();
		}
		if (istargetting)
		{
			AutoTarget();
		}
	}

	private void AutoTarget()
	{
		// Player�� ���� �������� ���� ������.
		stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(direction.normalized), 1f);

		// Ÿ���� Player���� ���ʿ� �ִ��� �����ʿ� �ִ��� �˻��Ѵ�.
		float targetPos = TransformPosition(maincamTransform, Target.position).x;
		if (xDotResult < AixsDamp)
		{
			// targetpos�� �¿츦 �����ؼ� ������.
			TurnCam(horizontalSpeed * Time.deltaTime * (targetPos/math.abs(targetPos)));
		}
		else
		{
			istargetting = false;
		}
	}

	// ī�޶� ������
	private void TurnCam(float value)
	{
		freeLookCamera.m_XAxis.Value += value;
	}

	// Ư�� �������� Ư�� Ʈ���������� �ٶ󺻴�.
	private Vector3 TransformPosition(Transform transform, Vector3 worldPosition)
	{
		return transform.worldToLocalMatrix.MultiplyPoint3x4(worldPosition);
	}

}
