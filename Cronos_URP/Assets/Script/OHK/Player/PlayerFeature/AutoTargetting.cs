using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;

/// <summary>
/// �ڵ����ذ� lockOn�� 
/// ���� ��Ű�� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class AutoTargetting : MonoBehaviour
{
	// �̱����� ���� ����
	private static AutoTargetting _instance;
	public static AutoTargetting GetInstance() { return _instance; }

	private PlayerStateMachine _stateMachine;

	// �۵������� ���� �浹ü
	public Collider sphere;

	// ī�޶� ������ �޾ƿ��� ����
	public CinemachineVirtualCamera playerCamera;
	public CinemachinePOV cinemachinePOV;
	private Transform _maincamTransform;

	// ī�޶� �̵��ӵ�
	public float horizontalSpeed = 10.0f;
	public float verticalSpeed = 3f;

	// Ÿ���� �����ϱ����� �÷��̾�� Ÿ��
	public GameObject player;
	public Transform target = null;
	public Transform playerObject;

	// ���ػ��¸� Ȯ���ϴ� ����
	public float lockOnAixsDamp = 0.99f;
	public float autoTargettingAixsDamp = 0.99f;
	public float exitValue;
	private Vector3 _direction;
	private float _xDotResult;
	private float _yDotResult;
	private bool _isFacing = false;
	public bool isTargetting = false;

	[SerializeField] private List<GameObject> _monsterList;
	private float? _min = null;

	private void Awake()
	{
		_instance = this;
		_monsterList = new List<GameObject>();
	}

	public void OnEnable()
	{
		_stateMachine = player.GetComponent<PlayerStateMachine>();

		_maincamTransform = Camera.main.transform;

		if (_maincamTransform != null)
		{
			cinemachinePOV = playerCamera.GetCinemachineComponent<CinemachinePOV>();
		}
	}

	private void OnDisable()
	{
		target = null;
	}

	private void FixedUpdate()
	{
		// Player�� ���� �������� ���� ������.
		if ((isTargetting || _stateMachine.Player.IsLockOn || _isFacing)
			&& _stateMachine.GetState().ToString() != "PlayerParryState")
		{
			if (_direction.magnitude != 0f && _stateMachine.InputReader.moveComposite.magnitude == 0f)
			{
				_stateMachine.Rigidbody.MoveRotation(Quaternion.Slerp(_stateMachine.transform.rotation, Quaternion.LookRotation(_direction.normalized), 0.2f));
			}
		}
	}

	/// �ݶ��̴��� ���Ͱ� ������ ����Ʈ�� �߰��Ѵ�.
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Respawn"))
		{
			_monsterList.Add(FindChildRecursive(other.gameObject.transform, "LockOnTarget").gameObject);
		}

	}
	/// �ݶ��̴����� ���Ͱ� ������ ����Ʈ���� �����Ѵ�.
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Respawn"))
		{
			_monsterList.Remove(FindChildRecursive(other.gameObject.transform, "LockOnTarget").gameObject);
		}
	}

	void Update()
	{
		// Player�� �ٶ� ������ ���Ѵ�.
		if (target != null)
		{
			_direction = target.position - playerObject.position;
			_direction.y = 0;
		}

		_xDotResult = Mathf.Abs(Vector3.Dot(_maincamTransform.right, Vector3.Cross(Vector3.up, _direction.normalized).normalized));
		_yDotResult = Mathf.Abs(Vector3.Dot(_maincamTransform.up, Vector3.Cross(Vector3.right, _direction.normalized).normalized));

		if (_stateMachine.Player.IsLockOn)
		{
			LockOn();
		}
	}

	public Transform GetTarget()
	{
		return target;
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
		if (Mathf.Abs(Vector3.Dot(_stateMachine.transform.forward, _direction.normalized)) > 0.9f)
		{
			_isFacing = false;
		}
	}

	private void StopAutoTargetting()
	{
		isTargetting = false;
	}

	public void AutoTargeting()
	{
		if (target == null)
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
		_isFacing = true;

		while (_isFacing)
		{
			FacingTarget();
			yield return null;
		}

		while (isTargetting)
		{
			if (target == null)
			{
				StopAutoTargetting();
			}
			else
			{
				Vector3 targetPos = TransformPosition(_maincamTransform, target.position);

				// �������� 0.99 ���� ������ ���Ѵ�.
				if (_xDotResult < autoTargettingAixsDamp)
				{
					if (_xDotResult > exitValue && _stateMachine.InputReader.moveComposite.magnitude != 0f)
					{
						StopAutoTargetting();
					}
					TurnCamHorizontal(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)) * (1 - _xDotResult));
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
		cinemachinePOV.m_HorizontalAxis.Value += value;
	}

	private void TurnCamVertical(float value)
	{
		cinemachinePOV.m_VerticalAxis.Value -= value;
	}

	// Ư�� �������� Ư�� Ʈ���������� �ٶ󺻴�.
	private Vector3 TransformPosition(Transform transform, Vector3 worldPosition)
	{
		return transform.worldToLocalMatrix.MultiplyPoint3x4(worldPosition);
	}

	public bool FindTarget()
	{
		// ���͸���Ʈ�� ���ų� ����� 0�̶�� false
		if (_monsterList == null || _monsterList.Count == 0)
		{
			target = null;
			return false;
		}
		if (_stateMachine.Player.IsLockOn)
		{
			return true;
		}

		// ���� ���� ����� ��ȸ�Ѵ�.
		for (int i = _monsterList.Count - 1; i >= 0; i--)
		{
			if (_monsterList[i] == null || !_monsterList[i].activeSelf)
			{
				_monsterList.RemoveAt(i);
			}
		}

		// ���͸���Ʈ�� �Ÿ� ������ �����Ѵ�.
		SortMosterList();

		// ���� ���� ����� ��ȸ�Ѵ�.
		for (int i = 0; i < _monsterList.Count; i++)
		{
			{
				// ���Ϳ� �÷��̾� ������ �Ÿ������� ũ�⸦ ���Ѵ�.
				float value = Mathf.Abs((playerObject.position - _monsterList[i].GetComponent<Transform>().position).magnitude);

				// ���� min������ value�� �۴ٸ� Ȥ�� min�� ���� ������� �ʴٸ� 
				if (_min > value || _min == null)
				{
					// min ���� ��ü�ϰ� 
					_min = value;

					// ���� ���� ���� ���� Ʈ�������� Ÿ������ �����Ѵ�.
					//if (!stateMachine.Player.IsLockOn)
					{
						target = _monsterList[i].GetComponent<Transform>();

					}
				}
			}

		}

		_min = null;
		return true;
	}

	public void LockOn()
	{
		// Ÿ���� Player���� ���ʿ� �ִ��� �����ʿ� �ִ��� �˻��Ѵ�.
		if (target != null)
		{

			_stateMachine.Player.IsLockOn = true;

			Vector3 targetPos = TransformPosition(_maincamTransform, target.position);


			// �������� 99 ���� ������ ���Ѵ�.
			if (_xDotResult < lockOnAixsDamp)
			{
				// targetpos�� �¿츦 �����ؼ� ������.
				TurnCamHorizontal(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)) * (1f - _xDotResult));
			}
			if (_yDotResult < lockOnAixsDamp)
			{
				TurnCamVertical(verticalSpeed * Time.deltaTime * (targetPos.y / math.abs(targetPos.y)) * (1f - _yDotResult));
			}
		}
		else if (_monsterList.Count != 0)
		{
			_stateMachine.Player.IsLockOn = false;
			FindTarget();
			_stateMachine.Player.IsLockOn = true;
		}
		else
		{
			LockOff();
		}

	}

	public void LockOff()
	{
		// ������ �����Ѵ�.
		_stateMachine.Player.IsLockOn = false;
		target = null;
	}

	public void SwitchTarget()
	{
		for (int i = _monsterList.Count - 1; i >= 0; i--)
		{
			if (_monsterList[i] == null)
			{
				_monsterList.RemoveAt(i);
			}
		}
		// ���͸���Ʈ�� �Ÿ� ������ �����Ѵ�.
		SortMosterList();

		// ���ĵ� ���͸���Ʈ���� ���� ����� ���͸� Target�� ���Ѵ�
		if (_monsterList.Count > 0)
		{
			// ���ĵ� ���� �������Ϳ� target�� ��ġ�ϸ� �������� ����� ���͸� Ÿ�����Ѵ�.
			if (target != _monsterList[0].transform)
			{
				target = _monsterList[0].transform;
			}
			else
			{
				if (_monsterList.Count <= 1)
					return;
				target = _monsterList[1]?.transform;
			}
		}
	}

	public void SortMosterList()
	{
		// ���͸���Ʈ�� �Ÿ� ������ �����Ѵ�.
		_monsterList.Sort((x, y) =>
			(playerObject.position - x.transform.position).sqrMagnitude
			.CompareTo((playerObject.position - y.transform.position).sqrMagnitude));
	}

}
