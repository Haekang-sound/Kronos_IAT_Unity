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

    public GameObject Cam;
    public GameObject Target;       // Player�� �ٶ� ���
    public GameObject Player;       // �÷��̾�
    public GameObject PlayerObject; // �÷��̾� ������Ʈ 

    public float AixsDamp = 0.99f;  // ����������� ���� ���ΰ�!

    Camera mainCam;

    Transform targetTransfrom;
    Transform PlayerObjectTransfrom;
    Transform maincamTransform;

    PlayerStateMachine stateMachine;

    float xDotResult;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        stateMachine = Player.GetComponent<PlayerStateMachine>();

        targetTransfrom = Target.GetComponent<Transform>();
        PlayerObjectTransfrom = PlayerObject.GetComponent<Transform>();
        maincamTransform = mainCam.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // ĳ���Ͱ� �ٶ� ������ ���Ѵ�.
        Vector3 direction = targetTransfrom.position - PlayerObjectTransfrom.position;
        direction.y = 0;    // y�����δ� ȸ������ �ʴ´�.

        xDotResult = Vector3.Dot(maincamTransform.right, PlayerObjectTransfrom.right);

        // ������ �Ͼ���� ĳ���Ͱ� ���� �������� ���� ������.
        if (Input.GetButton("Fire1"))
        {
            stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(direction.normalized), 1f);

            float targetPos = TransformPosition(maincamTransform, targetTransfrom.position).x;

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
