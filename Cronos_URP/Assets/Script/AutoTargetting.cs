using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal.Internal;
using System.Data.SqlTypes;
using UnityEngine.Purchasing.Extension;
using Unity.Mathematics;
using System.Security;
using System.Data;
public class AutoTargetting : MonoBehaviour
{
	static AutoTargetting instance;
	public static AutoTargetting GetInstance() { return instance; }

	public CinemachineVirtualCamera PlayerCamera;
	public CinemachinePOV CinemachinePOV;

	public float horizontalSpeed = 10.0f; // ���� ȸ�� �ӵ�
	public float verticalSpeed = 3f;    // ���� ȸ�� �ӵ�

	public GameObject Player;       // �÷��̾�
	public Transform Target = null;     // Player�� �ٶ� ���
	public Transform PlayerObject; // �÷��̾� ������Ʈ 

	Transform maincamTransform;

	public float lockOnAixsDamp = 0.99f;  // ����������� ���� ���ΰ�!
	public float autoTargettingAixsDamp = 0.99f;  // ����������� ���� ���ΰ�!
	public float exitValue;

	private PlayerStateMachine stateMachine;

	private Vector3 direction;
	private float xDotResult;
	private float yDotResult;
	bool isTargetting = false;
	bool isFacing = false;


	// ���͸���Ʈ
	List<GameObject> MonsterList;
	float? min = null;

	private void Awake()
	{
		instance = this;
		MonsterList = new List<GameObject>();
	}

	public Transform GetTarget()
	{
		return Target;
	}

	// Start is called before the first frame update
	public void OnEnable()
	{
		stateMachine = Player.GetComponent<PlayerStateMachine>();
		maincamTransform = Camera.main.transform;
		CinemachinePOV = PlayerCamera.GetCinemachineComponent<CinemachinePOV>();
		var pov = PlayerCamera.GetCinemachineComponent<CinemachinePOV>();
		var test = PlayerCamera.IsValid;
	}

	private void OnTriggerEnter(Collider other)
	{
		// �ݶ��̴��� ���Ͱ� ������ ����Ʈ�� �߰��Ѵ�.
		if (other.CompareTag("Respawn"))
		{
			MonsterList.Add(FindChildRecursive(other.gameObject.transform, "LockOnTarget").gameObject);
		}

	}
	private void OnTriggerExit(Collider other)
	{
		// �ݶ��̴����� ���Ͱ� ������ ����Ʈ���� �����Ѵ�.
		if (other.CompareTag("Respawn"))
		{
			MonsterList.Remove(FindChildRecursive(other.gameObject.transform, "LockOnTarget").gameObject);
		}
	}

	Transform FindChildRecursive(Transform parent, string childName)
	{
		// ���� �θ��� ���� ������Ʈ���� ��ȸ�մϴ�.
		foreach (Transform child in parent)
		{
			if (child.name == childName)
			{
				return child;
			}

			// ��������� ���� ������Ʈ���� Ž���մϴ�.
			Transform found = FindChildRecursive(child, childName);
			if (found != null)
			{
				return found;
			}
		}

		// �̸��� �´� ���� ������Ʈ�� ã�� ���ϸ� null�� ��ȯ�մϴ�.
		return null;
	}



	// Update is called once per frame
	void Update()
	{

		// Player�� �ٶ� ������ ���Ѵ�.
		if (Target != null)
		{
			direction = Target.position - PlayerObject.position;
			direction.y = 0;
		}

		
		xDotResult = Mathf.Abs(Vector3.Dot(maincamTransform.right, Vector3.Cross(Vector3.up, direction.normalized).normalized)); 
		yDotResult = Mathf.Abs(Vector3.Dot(maincamTransform.up, Vector3.Cross(Vector3.right, direction.normalized).normalized));

		/// ������ �Ͼ���� 
		/// ����� ���Ÿ� inputsystem�� ����ϴ� ������� ��ġ��
		if (Input.GetButtonDown("Fire1"))
		{

			FindTarget();
			if (Target == null)
			{
				return;
			}
			else
			{
				isTargetting = true;
				StartAutoTargetting();
			}
		}

		if (stateMachine.Player.IsLockOn)
		{
			LockOn();
		}
	}



	private void FixedUpdate()
	{
		// Player�� ���� �������� ���� ������.
		if (isTargetting || stateMachine.Player.IsLockOn)
		{
			stateMachine.Rigidbody.rotation = Quaternion.LookRotation(direction.normalized);
		}
	}

	// Ÿ���� �ٶ󺸴� �� ���� ������? 
	private void FacingTarget()
	{
		if (Mathf.Abs(Vector3.Dot(stateMachine.transform.forward, direction.normalized)) > 0.7f)
		{
			isFacing = false;
		}
	}

	private void LateUpdate() { }

	private void StartAutoTargetting()
	{
		StartCoroutine(AutoTarget());
	}

	private void StopAutoTargetting()
	{
		isTargetting = false;
	}

	private IEnumerator AutoTarget()
	{
		isTargetting = true;
		isFacing = true;

		while (isFacing)
		{
			FacingTarget();
			yield return null;
		}

		while (isTargetting)
		{
			if (Target == null)
			{
				StopAutoTargetting();
			}
			else
			{
				Vector3 targetPos = TransformPosition(maincamTransform, Target.position);

				// �������� 0.99 ���� ������ ���Ѵ�.
				if (xDotResult < autoTargettingAixsDamp)
				{
					if (xDotResult > exitValue && stateMachine.InputReader.moveComposite.magnitude != 0f)
					{
						StopAutoTargetting();
					}
					TurnCamHorizontal(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)) * (1 - xDotResult));
				}
				else
				{
					StopAutoTargetting();
				}
			}
			yield return null;
		}

	}

	// ī�޶� ������
	private void TurnCamHorizontal(float value)
	{
		CinemachinePOV.m_HorizontalAxis.Value += value;
	}
	private void TurnCamVertical(float value)
	{
		CinemachinePOV.m_VerticalAxis.Value -= value;
	}

	// Ư�� �������� Ư�� Ʈ���������� �ٶ󺻴�.
	private Vector3 TransformPosition(Transform transform, Vector3 worldPosition)
	{
		return transform.worldToLocalMatrix.MultiplyPoint3x4(worldPosition);
	}
	public bool FindTarget()
	{
		// ���͸���Ʈ�� ���ų� ����� 0�̶�� false
		if (MonsterList == null || MonsterList.Count == 0)
		{
			Target = null;
			return false;
		}

		// ���� ���� ����� ��ȸ�Ѵ�.
		for (int i = MonsterList.Count - 1; i >= 0; i--)
		{
			if (MonsterList[i] == null)
			{
				MonsterList.RemoveAt(i);
			}
		}
		// ���͸���Ʈ�� �Ÿ� ������ �����Ѵ�.
		MonsterList.Sort((x, y) =>
		(PlayerObject.position - x.transform.position).sqrMagnitude
		.CompareTo((PlayerObject.position - y.transform.position).sqrMagnitude));

		// ���� ���� ����� ��ȸ�Ѵ�.
		for (int i = 0; i < MonsterList.Count; i++)
		{
			{
				// ���Ϳ� �÷��̾� ������ �Ÿ������� ũ�⸦ ���Ѵ�.
				float value = Mathf.Abs((PlayerObject.position - MonsterList[i].GetComponent<Transform>().position).magnitude);

				// ���� min������ value�� �۴ٸ� Ȥ�� min�� ���� ������� �ʴٸ� 
				if (min > value || min == null)
				{
					// min ���� ��ü�ϰ� 
					min = value;

					// ���� ���� ���� ���� Ʈ�������� Ÿ������ �����Ѵ�.
					//if (!stateMachine.Player.IsLockOn)
					{
						Target = MonsterList[i].GetComponent<Transform>();

					}
				}
			}

		}

		min = null;
		return true;
	}

	public void LockOn()
	{
		// Ÿ���� Player���� ���ʿ� �ִ��� �����ʿ� �ִ��� �˻��Ѵ�.
		if (Target != null)
		{

			stateMachine.Player.IsLockOn = true;

			Vector3 targetPos = TransformPosition(maincamTransform, Target.position);


			// �������� 99 ���� ������ ���Ѵ�.
			if (xDotResult < lockOnAixsDamp)
			{
				// targetpos�� �¿츦 �����ؼ� ������.
				TurnCamHorizontal(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)) * (1f - xDotResult));
			}
			if (yDotResult < lockOnAixsDamp)
			{
				TurnCamVertical(verticalSpeed * Time.deltaTime * (targetPos.y / math.abs(targetPos.y)) * (1f - yDotResult));
			}
		}

	}

	public void LockOff()
	{
		// ������ �����Ѵ�.
		stateMachine.Player.IsLockOn = false;
		Target = null;
	}

	public void SwitchTarget()
	{

		// 		// ���� ���� ����� ��ȸ�Ѵ�.
		// 		for (int i = 0; i < MonsterList.Count; i++)
		// 		{
		// 			if (MonsterList[i] == null)
		// 			{
		// 				MonsterList.Remove(MonsterList[i]);
		// 			}
		// 		}

		for (int i = MonsterList.Count - 1; i >= 0; i--)
		{
			if (MonsterList[i] == null)
			{
				MonsterList.RemoveAt(i);
			}
		}

		// ���͸���Ʈ�� �Ÿ� ������ �����Ѵ�.
		MonsterList.Sort((x, y) =>
			(PlayerObject.position - x.transform.position).sqrMagnitude
			.CompareTo((PlayerObject.position - y.transform.position).sqrMagnitude));

		// ���ĵ� ���͸���Ʈ���� ���� ����� ���͸� Target�� ���Ѵ�
		if (MonsterList.Count > 0)
		{
			// ���ĵ� ���� �������Ϳ� target�� ��ġ�ϸ� �������� ����� ���͸� Ÿ�����Ѵ�.
			if (Target != MonsterList[0].transform)
			{
				Target = MonsterList[0].transform;
			}
			else
			{
				if (MonsterList.Count <= 1)
					return;
				Target = MonsterList[1]?.transform;
			}
		}
	}

}
