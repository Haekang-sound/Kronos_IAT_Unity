using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���� FanShapeScanner�� ����Ͽ� �÷��̾ �����ϰ�, 
/// Ÿ���� �߰��� ��� �ش� ��ġ�� �̵��ϴ� ������ �ϴ� Ŭ�����Դϴ�.
/// </summary>
public class FanShapeScannerEnemy : MonoBehaviour
{
    public float timeToStopPursuit = 100.0f;
    protected float _timerSinceLostTarget = 0.0f;
    // finder�� Ÿ��
    public GameObject CurrentTarget { get; private set; }

    public FanShapeScanner scanner;

    // Ÿ�� �߰� �� �̵��ؾ��� ��ġ
    public TargetDistributor.TargetFollower FollowerData { get; private set; }

    public UnityEvent OnDown;

    // Ÿ�� ��ü�� ��ġ���� Ư�� �������� 90%�� �Ÿ��� �ִ� ������ FollowerData.requiredPoint�� ����
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
