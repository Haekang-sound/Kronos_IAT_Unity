using System;
using System.Globalization;
using System.Resources;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public abstract class PlayerBaseState : State
{
    // ������ �б��������� ����
    protected readonly PlayerStateMachine stateMachine;
    Vector3 moveDirection;
    private SlopeData slopeData;

    protected PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        slopeData = stateMachine.Player.CapsuleColldierUtility.SlopeData;
    }



    /// <summary>
    /// ī�޶� ������ �������
    /// Player�� �̵������� ���Ѵ�.
    /// </summary>
    protected void CalculateMoveDirection()
    {
        // ������Ʈ �ӽſ� ����ִ� ī�޶� ������ �������
        // ī�޶��� ����, �¿� ���͸� �����Ѵ�.
        Vector3 cameraForward = new(stateMachine.MainCamera.forward.x, 0, stateMachine.MainCamera.forward.z);
        Vector3 cameraRight = new(stateMachine.MainCamera.right.x, 0, stateMachine.MainCamera.right.z);

        // �̵����ͻ���,
        // ī�޶��� ���溤�Ϳ� ��ǲ�� move.y ��ġ�� ���Ѵ�,
        // ī�޶��� �¿캤�Ϳ� ��ǲ�� movecomposite.x�� ���Ѵ�.
        moveDirection = cameraForward.normalized * stateMachine.InputReader.moveComposite.y // ����
                                + cameraRight.normalized * stateMachine.InputReader.moveComposite.x;    // �Ĺ�

        // ���¸ӽ��� �ӵ��� �̵����Ϳ� �ӷ��� ���̴�.
        stateMachine.Velocity.x = moveDirection.x * stateMachine.Player.moveSpeed;
        stateMachine.Velocity.y = 0f;
        stateMachine.Velocity.z = moveDirection.z * stateMachine.Player.moveSpeed;
    }

    /// <summary>
    /// �÷��̾ �̵��������� ȸ����Ų��.
    /// </summary>
    protected void FaceMoveDirection()
    {
        Vector3 faceDirection = new(stateMachine.Velocity.x, 0f, stateMachine.Velocity.z);

        // �̵��ӵ��� ���ٸ�
        if (faceDirection == Vector3.zero)
        {
            // �ƹ��͵� ���� �ʰڴ�.
            return;
        }
        // �÷��̾��� ȸ���� ���� ���������� ���·� �̷������. 
        stateMachine.Rigidbody.MoveRotation(Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), stateMachine.Player.lookRotationDampFactor * Time.fixedDeltaTime));



    }

    /// <summary>
    /// Player�� ���� �ִ� �����͸� �����ؼ�
    /// Player�� �̵���Ų��
    /// </summary>
    protected void Move()
    {

        bool isOnSlope = IsOnSlope();
        if (isOnSlope)
        {
            stateMachine.Rigidbody.useGravity = false;
        }
        else
        {
            stateMachine.Rigidbody.useGravity = true;
        }
        Vector3 velocity = isOnSlope ? AdjustDirectionToSlope(stateMachine.Velocity) : stateMachine.Velocity;//.normalized;
        Vector3 gravity = isOnSlope ? Vector3.zero : Vector3.down * Mathf.Abs(stateMachine.Rigidbody.velocity.y);

        stateMachine.Rigidbody.velocity = velocity * Time.fixedDeltaTime * stateMachine.Player.moveSpeed + gravity;

        Float();

    }

    protected Vector3 GetPlayerHorizentalVelocity()
    {
        Vector3 playerHorizentalvelocity = stateMachine.Rigidbody.velocity;
        playerHorizentalvelocity.y = 0f;
        return playerHorizentalvelocity;
    }


    protected void MoveOnAnimation()
    {
        float animationSpeed = stateMachine.Animator.deltaPosition.magnitude;

        // test
        Vector3 direction = stateMachine.transform.forward.normalized;

        // ���ο� ���� ���
        Vector3 newDeltaPosition = direction * animationSpeed;

        stateMachine.transform.Translate(newDeltaPosition);
    }


    private const float ray_distance = 2f;
    private RaycastHit sloperHit;
    private int groundLayer = 1 << LayerMask.NameToLayer("Ground");
    private float maxSlopeAngle = 90f;


    protected Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, sloperHit.normal);
    }

    public bool IsOnSlope()
    {
        Ray ray = new Ray(stateMachine.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out sloperHit, ray_distance, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, sloperHit.normal);
            return angle != 0f && angle < maxSlopeAngle;
        }
        return false;
    }

    public void Float()
    {
        // ĸ���ݶ��̴� ��ġ�� �����´�.
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.CapsuleColldierUtility.CapsuleColliderData.Collider.bounds.center;

        // ������ġ�� ĸ���ݶ��̴� ���ͷ� ��´�.
        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);
        
        // ���̸� ���� ĳ�����Ѵ�.
        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, slopeData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

            SetSlopeSpeedModifierOnAngle(groundAngle);

            float distanceToFloatingPoint = stateMachine.Player.CapsuleColldierUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y
                * stateMachine.Player.transform.localScale.y - hit.distance;

            if (distanceToFloatingPoint == 0f)
            {
                return;
            }
            float amountToLift = distanceToFloatingPoint * slopeData.StepReachForce - GetPlayerVerticalVelocity().y;
            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);
            stateMachine.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    private void SetSlopeSpeedModifierOnAngle(float groundAngle)
    {
        throw new NotImplementedException();
    }
    protected Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0f, stateMachine.Rigidbody.velocity.y, 0f);
    }


}