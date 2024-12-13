using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 전투 구역 내 적을 관리하는 클래스입니다. 이 클래스는 타겟을 추적하고, 타겟을 잃었을 때 일정 시간 동안 추적을 계속하거나 추적을 멈춥니다.
/// 또한, 타겟의 위치에 맞춰 이동해야 할 지점을 계산하여 적이 타겟을 추적할 수 있도록 도와줍니다.
/// </summary>
public class CombatZoneEnemy : MonoBehaviour
{
    public bool drawGizmos;

    public float timeToStopPursuit = 0.0f;
    protected float _timerSinceLostTarget = 0.0f;

    // finder의 타깃
    public GameObject CurrentTarget { get; private set; }

    // Scan 영역
    public bool useCombatZone;
    public CombatZone combatZone { private get; set; }

    // 타깃 발견 시 이동해야할 위치
    public TargetDistributor.TargetFollower FollowerData { get; private set; }

    public UnityEvent OnDown;

    private void OnDisable()
    {
        FollowerData.distributor.UnregisterFollower(FollowerData);
    }

    // 타겟 객체의 위치에서 특정 방향으로 90%의 거리에 있는 지점을 FollowerData.requiredPoint로 설정
    public void RequestTargetPosition(float distance)
    {
        if (FollowerData == null)
            return;

        Vector3 fromTarget = transform.position - CurrentTarget.transform.position;
        fromTarget.y = 0;

        FollowerData.requiredPoint = CurrentTarget.transform.position + fromTarget.normalized * distance * 0.9f;
    }

    /// <summary>
    /// 타겟을 찾고 추적 상태를 업데이트합니다. 
    /// 전투 구역을 사용할 경우, CombatZone에서 타겟을 탐지하고, 그렇지 않으면 기본적으로 설정된 타겟을 사용합니다.
    /// </summary>
    public void FindTarget()
    {
        GameObject target = null;
        if (useCombatZone == true)
        {
            // scanner의 타깃
            target = combatZone.Detect(transform, CurrentTarget == null);
        }
        else
        {
            target = GetComponent<EnemyController>().target;
        }

        if (CurrentTarget == null)
        {
            // 현재 타깃이 없고, Scanner가 타깃을 발견한 경우.
            if (target != null)
            {
                CurrentTarget = target;
                TargetDistributor distributor = target.GetComponentInChildren<TargetDistributor>();

                if (distributor != null)
                {
                    // 타깃 발견 시 이동해야할 위치를 얻습니다.
                    FollowerData = distributor.RegisterNewFollower();
                }
            }
        }
        else
        {
            // 현재 타깃이 있지만 Scanner가 더는 타깃을 찾지 못하는 경우.
            if (target == null)
            {
                _timerSinceLostTarget += Time.deltaTime;

                if (_timerSinceLostTarget > timeToStopPursuit)
                {
                    // 타깃의 Distributer 구독 해제
                    if (FollowerData != null)
                    {
                        FollowerData.distributor.UnregisterFollower(FollowerData);
                    }

                    // 현제 타깃이 없도록 재설정한다.
                    CurrentTarget = null;
                }
            }
            else
            {
                // 여전히 현재 타깃이 있지만 Scanner가 타깃을 찾을 수 없는 경우
                if (target != CurrentTarget)
                {
                    // 이동위치 업데이트 1: 타깃의 Distributer 구독 해제.
                    if (FollowerData != null)
                    {
                        FollowerData.distributor.UnregisterFollower(FollowerData);
                    }

                    // 현재 타깃 업데이트
                    CurrentTarget = target;

                    // 이동위치 업데이트 2: 타깃의 Distributer 재구독.
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
