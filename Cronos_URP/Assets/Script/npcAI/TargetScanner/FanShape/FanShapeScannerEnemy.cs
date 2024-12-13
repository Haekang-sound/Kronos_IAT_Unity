using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 적이 FanShapeScanner를 사용하여 플레이어를 추적하고, 
/// 타겟을 발견한 경우 해당 위치로 이동하는 역할을 하는 클래스입니다.
/// </summary>
public class FanShapeScannerEnemy : MonoBehaviour
{
    public float timeToStopPursuit = 100.0f;
    protected float _timerSinceLostTarget = 0.0f;
    // finder의 타깃
    public GameObject CurrentTarget { get; private set; }

    public FanShapeScanner scanner;

    // 타깃 발견 시 이동해야할 위치
    public TargetDistributor.TargetFollower FollowerData { get; private set; }

    public UnityEvent OnDown;

    // 타겟 객체의 위치에서 특정 방향으로 90%의 거리에 있는 지점을 FollowerData.requiredPoint로 설정
    public void RequestTargetPosition(float distance)
    {
        if (FollowerData == null)
            return;

        Vector3 fromTarget = transform.position - CurrentTarget.transform.position;
        fromTarget.y = 0;

        FollowerData.requiredPoint = CurrentTarget.transform.position + fromTarget.normalized * distance * 0.9f;
    }

    public void FindTarget()
    {
        GameObject target = null;

        target = scanner.Detect(transform, CurrentTarget == null);

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

    // -----

    private void Awake()
    {
        scanner = GetComponent<FanShapeScanner>();
    }

    private void OnDisable()
    {
        FollowerData.distributor.UnregisterFollower(FollowerData);
    }
}
