using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
public class AutoTargetting : MonoBehaviour
{
	static AutoTargetting instance;
	public static AutoTargetting GetInstance() { return instance; }

	public CinemachineVirtualCamera PlayerCamera;
	public CinemachinePOV CinemachinePOV;

	public float horizontalSpeed = 10.0f; // ���� ȸ�� �ӵ�
	public float verticalSpeed = 3f;    // ���� ȸ�� �ӵ�

	public GameObject Player;       // �÷��̾�
	public Transform Target = null; // Player�� �ٶ� ���
	public Transform PlayerObject;  // �÷��̾� ������Ʈ 

	Transform maincamTransform;

	public float lockOnAixsDamp = 0.99f;            // ����������� ���� ���ΰ�!
	public float autoTargettingAixsDamp = 0.99f;    // ����������� ���� ���ΰ�!
	public float exitValue;

	private PlayerStateMachine stateMachine;

	private Vector3 direction;
	private float xDotResult;
	private float yDotResult;
	private bool isTargetting = false;
	private bool isFacing = false;


	// ���͸���Ʈ
	List<GameObject> MonsterList;
	float? min = null;

	private void Awake()
	{
		instance = this;    // �̱������� ���ڴ�.
		MonsterList = new List<GameObject>(); // ���͸� ����
											  //PlayerCamera = PlayerCamControler.Instance.gameObject.GetComponent<CinemachineVirtualCamera>();

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

		if (maincamTransform != null)
		{
			CinemachinePOV = PlayerCamera.GetCinemachineComponent<CinemachinePOV>();
			//var pov = PlayerCamera.GetCinemachineComponent<CinemachinePOV>();
			//var test = PlayerCamera.IsValid;
		}
	}

	/// �ݶ��̴��� ���Ͱ� ������ ����Ʈ�� �߰��Ѵ�.
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Respawn"))
		{
			MonsterList.Add(FindChildRecursive(other.gameObject.transform, "LockOnTarget").gameObject);
		}

	}
	/// �ݶ��̴����� ���Ͱ� ������ ����Ʈ���� �����Ѵ�.
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Respawn"))
		{
			MonsterList.Remove(FindChildRecursive(other.gameObject.transform, "LockOnTarget").gameObject);
		}
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

		if (stateMachine.Player.IsLockOn)
		{
			LockOn();
		}
	}

	private void FixedUpdate()
	{
		// Player�� ���� �������� ���� ������.
		if ((isTargetting || stateMachine.Player.IsLockOn || isFacing)
			&& stateMachine.GetState().ToString() != "PlayerParryState")
		{
			if (direction.magnitude != 0f)
			{
				stateMachine.Rigidbody.MoveRotation(Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(direction.normalized), 0.1f));

			}
		}
	}

	/// <summary>
	/// ������Ʈ�� ��ȸ�ϸ� ã�´�.
	/// </summary>
	/// <param name="parent">�θ� ������Ʈ</param>
	/// <param name="childName">�ڽ� ������Ʈ �̸�</param>
	/// <returns></returns>
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

	// Ÿ���� �ٶ󺸴� �� ���� ������? 
	private void FacingTarget()
	{
		if (Mathf.Abs(Vector3.Dot(stateMachine.transform.forward, direction.normalized)) > 0.9f)
		{
			isFacing = false;
		}
	}

	private void StopAutoTargetting()
	{
		isTargetting = false;
	}
	public void AutoTargeting()
	{
        if (Target == null)
        {
            if (FindTarget())
            {
                isTargetting = true;
                StartCoroutine(AutoTarget());
            }
            else
            {
                return;
            }
        }
        else
        {
            isTargetting = true;
            StartCoroutine(AutoTarget());
        }
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
		if (stateMachine.Player.IsLockOn)
		{
			return true;
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
		SortMosterList();

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
		else if (MonsterList.Count != 0)
		{
			stateMachine.Player.IsLockOn = false;
			FindTarget();
			stateMachine.Player.IsLockOn = true;
		}
		else
		{
			LockOff();
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
		for (int i = MonsterList.Count - 1; i >= 0; i--)
		{
			if (MonsterList[i] == null)
			{
				MonsterList.RemoveAt(i);
			}
		}
		// ���͸���Ʈ�� �Ÿ� ������ �����Ѵ�.
		SortMosterList();

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

	public void SortMosterList()
	{
		// ���͸���Ʈ�� �Ÿ� ������ �����Ѵ�.
		MonsterList.Sort((x, y) =>
			(PlayerObject.position - x.transform.position).sqrMagnitude
			.CompareTo((PlayerObject.position - y.transform.position).sqrMagnitude));
	}

}
