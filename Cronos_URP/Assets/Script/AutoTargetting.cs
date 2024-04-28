using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal.Internal;
using System.Data.SqlTypes;
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

    private float xDotResult;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = Player.GetComponent<PlayerStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        // ĳ���Ͱ� �ٶ� ������ ���Ѵ�.
        Vector3 direction = Target.position - PlayerObject.position;
        direction.y = 0;    // y�����δ� ȸ������ �ʴ´�.

        xDotResult = Vector3.Dot(maincamTransform.right, PlayerObject.right);

        // ������ �Ͼ���� ĳ���Ͱ� ���� �������� ���� ������.
        if (Input.GetButton("Fire1"))
        {
            stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(direction.normalized), 1f);

            float targetPos = TransformPosition(maincamTransform, Target.position).x;

            if (targetPos > 0)
            {
                // ī�޶� �÷��̾� �ڿ��� ���͸� �ٶ󺻴�
                if (xDotResult < AixsDamp)
                {
                    TurnCam(horizontalSpeed * Time.deltaTime);
                }
            }
            else // �����ʿ� �ִٸ�
            {
                if (xDotResult < AixsDamp)
                {
                    TurnCam(horizontalSpeed * Time.deltaTime * -1f);
                }
            }
        }
    }

    // ī�޶� ������
    private void TurnCam(float value)
    {
        freeLookCamera.m_XAxis.Value += value;
    }

    // ī�޶���ġ���� Ÿ���� �ٶ󺻴�
    private Vector3 TransformPosition(Transform transform, Vector3 worldPosition)
    {
        return transform.worldToLocalMatrix.MultiplyPoint3x4(worldPosition);
    }

}
