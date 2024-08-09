using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;

public class CombatZoneEnemy : MonoBehaviour
{
    public bool drawGizmos;

    public float timeToStopPursuit = 0.0f;
    protected float _timerSinceLostTarget = 0.0f;
    // finder�� Ÿ��
    public GameObject CurrentTarget { get; private set; }
    // Scan ����
    public CombatZone combatZone { private get; set; }
    // Ÿ�� �߰� �� �̵��ؾ��� ��ġ
    public TargetDistributor.TargetFollower FollowerData { get; private set; }

    // �ٿ� �̺�Ʈ
    public UnityEvent OnDown;

    private void OnDisable()
    {
        FollowerData.distributor.UnregisterFollower(FollowerData);
    }

    // Ÿ�� ��ü�� ��ġ���� Ư�� �������� 90%�� �Ÿ��� �ִ� ������ FollowerData.requiredPoint�� ����
    public void RequestTargetPosition(float distance)
    {
        Vector3 fromTarget = transform.position - CurrentTarget.transform.position;
        fromTarget.y = 0;

        FollowerData.requiredPoint = CurrentTarget.transform.position + fromTarget.normalized * distance * 0.9f;
    }

    public void FindTarget()
    {
        // scanner�� Ÿ��
        var target = combatZone.Detect(transform, CurrentTarget == null);
        
        if (CurrentTarget == null)
        {
            // ���� Ÿ���� ����, Scanner�� Ÿ���� �߰��� ���.
            if (target != null) 
            {
                CurrentTarget = target;
                TargetDistributor distributor = target.GetComponentInChildren<TargetDistributor>();

                if (distributor != null)
                {
                    // Ÿ�� �߰� �� �̵��ؾ��� ��ġ�� ����ϴ�.
                    FollowerData = distributor.RegisterNewFollower();
                }
            }
        }
        else
        {
            // ���� Ÿ���� ������ Scanner�� ���� Ÿ���� ã�� ���ϴ� ���.
            if (target == null)
            {
                _timerSinceLostTarget += Time.deltaTime;

                if (_timerSinceLostTarget > timeToStopPursuit)
                {
                    // Ÿ���� Distributer ���� ����
                    if (FollowerData != null)
                    {
                        FollowerData.distributor.UnregisterFollower(FollowerData);
                    }

                    // ���� Ÿ���� ������ �缳���Ѵ�.
                    CurrentTarget = null;
                }
            }
            else
            {
                // ������ ���� Ÿ���� ������ Scanner�� Ÿ���� ã�� �� ���� ���
                if (target != CurrentTarget)
                {
                    // �̵���ġ ������Ʈ 1: Ÿ���� Distributer ���� ����.
                    if (FollowerData != null)
                    {
                        FollowerData.distributor.UnregisterFollower(FollowerData);
                    }

                    // ���� Ÿ�� ������Ʈ
                    CurrentTarget = target;

                    // �̵���ġ ������Ʈ 2: Ÿ���� Distributer �籸��.
                    TargetDistributor distributor = target.GetComponentInChildren<TargetDistributor>();
                    
                    if (distributor != null) 
                    {
                        FollowerData = distributor.RegisterNewFollower();
                    }

                    _timerSinceLostTarget = 0f;
                }
            }
        }
    }
}
