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
public class AutoTargetting : MonoBehaviour
{

    //public CinemachineFreeLook freeLookCamera;
    public CinemachineVirtualCamera PlayerCamera;
	public CinemachinePOV CinemachinePOV;
    public float horizontalSpeed = 10.0f; // ���� ȸ�� �ӵ�
    public float verticalSpeed = 5.0f;    // ���� ȸ�� �ӵ�

    public GameObject Player;       // �÷��̾�
    public Transform Target = null;     // Player�� �ٶ� ���
    public Transform PlayerObject; // �÷��̾� ������Ʈ 
    /*public */Transform maincamTransform;

    public float AixsDamp = 0.99f;  // ����������� ���� ���ΰ�!

    private PlayerStateMachine stateMachine;

    private Vector3 direction;
    private float xDotResult;
    private float yDotResult;
    bool istargetting;


	// ���͸���Ʈ
	List<GameObject> MonsterList;
	float? min = null;

	private void Awake()
	{
		MonsterList = new List<GameObject>();
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
			MonsterList.Add(other.gameObject);
			Debug.Log("monster in");
		}


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
            //direction.y = 0;    // y�����δ� ȸ������ �ʴ´�.
        }

        xDotResult = Vector3.Dot(maincamTransform.right, PlayerObject.right);
		yDotResult = Vector3.Dot(maincamTransform.up, Vector3.Cross(direction.normalized, Vector3.right)); //PlayerObject.up);

        /// ������ �Ͼ���� 
		/// ����� ���Ÿ� inputsystem�� ����ϴ� ������� ��ġ��
        if (Input.GetButton("Fire1"))
        {
			FindTarget();

            if ( Target == null)
            {
                 return; 
            }
            else
            {
                istargetting = true;
            }
        }

        if (istargetting)
        {
            AutoTarget();
        }
    }

    private void AutoTarget()
    {
        // Player�� ���� �������� ���� ������.
        stateMachine.Rigidbody.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(direction.normalized), 0.1f);

        // Ÿ���� Player���� ���ʿ� �ִ��� �����ʿ� �ִ��� �˻��Ѵ�.
		if(Target != null)
		{
			Vector3 targetPos = TransformPosition(maincamTransform, Target.position);
			if (yDotResult < AixsDamp)
			{
				TurnCamy((verticalSpeed * Time.deltaTime * (targetPos.y / math.abs(targetPos.y))));
			}
			if (xDotResult < AixsDamp)
			{
				// targetpos�� �¿츦 �����ؼ� ������.
				TurnCam(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)));

// 				if(yDotResult < AixsDamp)
// 				{
// 					TurnCamy(verticalSpeed * Time.deltaTime * (targetPos.y / math.abs(targetPos.y)));
// 				}
			}
			else
			{
				istargetting = false;
			}

		}
     
    }

    // ī�޶� ������
    private void TurnCam(float value)
    {
		CinemachinePOV.m_HorizontalAxis.Value += value;

	}
	private void TurnCamy(float value)
	{
		CinemachinePOV.m_VerticalAxis.Value += value;

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

}
