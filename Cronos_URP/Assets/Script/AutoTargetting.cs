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

	//public CinemachineFreeLook freeLookCamera;
	public CinemachineVirtualCamera PlayerCamera;
	public CinemachinePOV CinemachinePOV;

	public float horizontalSpeed = 10.0f; // ���� ȸ�� �ӵ�
	public float verticalSpeed = 3f;    // ���� ȸ�� �ӵ�

	public GameObject Player;       // �÷��̾�
	public Transform Target = null;     // Player�� �ٶ� ���
	public Transform PlayerObject; // �÷��̾� ������Ʈ 
	/*public */
	Transform maincamTransform;

	public float AixsDamp = 0.99f;  // ����������� ���� ���ΰ�!

	private PlayerStateMachine stateMachine;

	private Vector3 direction;
	private float xDotResult;
	private float yDotResult;
	bool isTargetting;


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
	}

	private void OnTriggerEnter(Collider other)
	{
		// �ݶ��̴��� ���Ͱ� ������ ����Ʈ�� �߰��Ѵ�.
		if (other.CompareTag("Respawn"))
		{
			MonsterList.Add(FindChildRecursive(other.gameObject.transform, "Spine1").gameObject);
			//MonsterList.Add(other.gameObject);
			Debug.Log("monster in");
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

    private void OnTriggerExit(Collider other)
	{
		// �ݶ��̴����� ���Ͱ� ������ ����Ʈ���� �����Ѵ�.
		if (other.CompareTag("Respawn"))
		{
			MonsterList.Remove(other.gameObject);
			Debug.Log("monster out");
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

		xDotResult = Mathf.Abs(Vector3.Dot(maincamTransform.right, PlayerObject.right));
		yDotResult = Mathf.Abs(Vector3.Dot(maincamTransform.up, Vector3.Cross(Vector3.right, direction.normalized).normalized));

		/// ������ �Ͼ���� 
		/// ����� ���Ÿ� inputsystem�� ����ϴ� ������� ��ġ��
		if (Input.GetButton("Fire1"))
		{
			FindTarget();

			if (Target == null)
			{
				return;
			}
			else
			{
				isTargetting = true;
			}
		}
		else if (!stateMachine.Player.IsLockOn)
		{
			Target = null;
			isTargetting = false;
		}

		if (isTargetting)
		{
			AutoTarget();
		}
	}
	private void FixedUpdate()
	{
		// Player�� ���� �������� ���� ������.
		if (isTargetting || stateMachine.Player.IsLockOn)
		{
			stateMachine.Rigidbody.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(direction.normalized), 0.1f);

		}
	}
	private void LateUpdate() { }

	private void AutoTarget()
	{
		// Ÿ���� Player���� ���ʿ� �ִ��� �����ʿ� �ִ��� �˻��Ѵ�.
		if (Target != null)
		{
			Vector3 targetPos = TransformPosition(maincamTransform, Target.position);

			// �������� 99 ���� ������ ���Ѵ�.
			if (Mathf.Abs(xDotResult) < AixsDamp)
			{
				TurnCamHorizontal(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)) * (1 - xDotResult));
			}
			else
			{
				isTargetting = false;
			}
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
			return false;
		}
		// ���� ���� ����� ��ȸ�Ѵ�.
		for (int i = 0; i < MonsterList.Count; i++)
		{
			if (MonsterList[i] == null)
			{
				MonsterList.Remove(MonsterList[i]);
			}
			else
			{
				// ���Ϳ� �÷��̾� ������ �Ÿ������� ũ�⸦ ���Ѵ�.
				/// ������ Ʈ�������� 
				/// �Ź� ������Ʈ �������� ã�ƾ��ϴ� ���� 
				/// �ʹ� �ƽ��� ���̴�. ����� ã�ƺ���?
				/// 1. findTarget����� ��ġ�� �ٲ۴�.
				float value = Mathf.Abs((PlayerObject.position - MonsterList[i].GetComponent<Transform>().position).magnitude);

				// ���� min������ value�� �۴ٸ� Ȥ�� min�� ���� ������� �ʴٸ� 
				if (min > value || min == null)
				{
					// min ���� ��ü�ϰ� 
					min = value;

					// ���� ���� ���� ���� Ʈ�������� Ÿ������ �����Ѵ�.
					Target = MonsterList[i].GetComponent<Transform>();
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
			Vector3 targetPos = TransformPosition(maincamTransform, Target.position);

			// �������� 99 ���� ������ ���Ѵ�.
			if (xDotResult < AixsDamp)
			{
				// targetpos�� �¿츦 �����ؼ� ������.
				TurnCamHorizontal(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)) * (1f - xDotResult));
			}
			if (yDotResult < AixsDamp)
			{
				TurnCamVertical(verticalSpeed * Time.deltaTime * (targetPos.y / math.abs(targetPos.y)) * (1f - yDotResult));
			}
		}

	}

	public void LockOff()
	{
		// ������ �����Ѵ�.
		Target = null;
	}

	public void SwitchTarget()
	{

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
				Target = MonsterList[1]?.transform;
			}
		}
	}

}
