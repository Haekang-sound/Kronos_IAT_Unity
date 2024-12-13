using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// ���� ������ Ư�� Ÿ���� ������ �л��Ű��, �ȷο����� ���� �ٸ� ���⿡�� Ÿ���� �����ϵ��� �ϴ� �ý����Դϴ�.
/// �� Ŭ������ Ÿ���� ��ġ�� ���� �ȷο����� �й��ϰ�, �� �ȷο��� Ÿ���� ���� �����̵��� �մϴ�.
/// </summary>
[DefaultExecutionOrder(-1)]
public class TargetDistributor : MonoBehaviour
{
    /// <summary>
    /// Ÿ���� �����ϴ� �ȷο��� ��Ÿ���� ���� Ŭ�����Դϴ�.
    /// �ȷο��� �й�� ��ġ�� �̵��ϱ� ���� �ʿ��� ������ �����մϴ�.
    /// </summary>
    public class TargetFollower
    {
        public bool requireSlot;  // �ȷο��� ��ġ�� �䱸�ϴ��� ����
        public int assignedSlot;  // �Ҵ�� ��ġ�� �ε��� (-1�� �Ҵ���� ����)
        public Vector3 requiredPoint;  // �ȷο��� ��ǥ�� �ϴ� ��ġ
        public TargetDistributor distributor;  // �ȷο��� ���� Distributor �ν��Ͻ�

        public TargetFollower(TargetDistributor owner)
        {
            distributor = owner;
            requiredPoint = Vector3.zero;
            requireSlot = false;
            assignedSlot = -1;
        }
    }

    public int arcsCount;  // �й��� ��ũ(����)�� �� (1���� Ŀ�� ��)
    protected Vector3[] _worldDirection;  // �� ��ũ�� ������ ����
    protected bool[] _freeArcs;  // �� ��ũ�� ��� �������� ����
    protected float _arcDegree;  // �� ��ũ�� ����

    protected List<TargetFollower> _followers;  // �ȷο� ����Ʈ

    /// <summary>
    /// ��ü�� Ȱ��ȭ�� �� ȣ��Ǹ�, ��ũ ����� �ȷο����� �ʱ�ȭ�մϴ�.
    /// </summary>
    public void OnEnable()
    {
        _worldDirection = new Vector3[arcsCount];
        _freeArcs = new bool[arcsCount];
        _followers = new List<TargetFollower>();
        _arcDegree = 360.0f / arcsCount;

        // ��ũ ���� �ʱ�ȭ
        Quaternion rotation = Quaternion.Euler(0, -_arcDegree, 0);
        Vector3 currentDirection = Vector3.forward;
        for (int i = 0; i < arcsCount; ++i)
        {
            _freeArcs[i] = true;
            _worldDirection[i] = currentDirection;
            currentDirection = rotation * currentDirection;
        }
    }

    /// <summary>
    /// ���ο� �ȷο��� ����ϰ� �ش� �ȷο��� ��ȯ�մϴ�.
    /// </summary>
    public TargetFollower RegisterNewFollower()
    {
        TargetFollower follower = new TargetFollower(this);
        _followers.Add(follower);
        return follower;
    }

    /// <summary>
    /// ���� �ȷο��� ��� �����մϴ�.
    /// </summary>
    public void UnregisterFollower(TargetFollower follower)
    {
        if (follower.assignedSlot != -1)
        {
            _freeArcs[follower.assignedSlot] = true;
        }
        _followers.Remove(follower);
    }

    /// <summary>
    /// �� ������ ���� �� ��� �ȷο����� Ÿ�� ��ġ�� �Ҵ��մϴ�.
    /// </summary>
    private void LateUpdate()
    {
        for (int i = 0; i < _followers.Count; ++i)
        {
            var follower = _followers[i];

            // �̹� �Ҵ�� ��ġ�� �����ϰ�, �ʿ� �� ���ο� ��ġ�� �Ҵ��մϴ�.
            if (follower.assignedSlot != -1)
            {
                _freeArcs[follower.assignedSlot] = true;
            }

            if (follower.requireSlot)
            {
                follower.assignedSlot = GetFreeArcIndex(follower);
            }
        }
    }

    /// <summary>
    /// �־��� �ε����� ������ ��ȯ�մϴ�.
    /// </summary>
    public Vector3 GetDirection(int index)
    {
        return _worldDirection[index];
    }

    /// <summary>
    /// �ȷο����� ������ ��� �ִ� ��ũ �ε����� ã�� �Ҵ��մϴ�.
    /// </summary>
    public int GetFreeArcIndex(TargetFollower follower)
    {
        bool found = false;

        Vector3 wanted = follower.requiredPoint - transform.position;
        Vector3 rayCastPosition = transform.position + Vector3.up * 0.4f;

        wanted.y = 0;
        float wantedDistance = wanted.magnitude;
        wanted.Normalize();

        // Ÿ�� ��ġ�� ���� ���
        float angle = Vector3.SignedAngle(wanted, Vector3.forward, Vector3.up);
        if (angle < 0)
            angle = 360 + angle;

        int wantedIndex = Mathf.RoundToInt(angle / _arcDegree);
        if (wantedIndex >= _worldDirection.Length)
            wantedIndex -= _worldDirection.Length;

        int choosenIndex = wantedIndex;

        // Ÿ�� ��ġ���� ����ĳ��Ʈ �˻�
        RaycastHit hit;
        if (!Physics.Raycast(rayCastPosition, GetDirection(choosenIndex), out hit, wantedDistance))
            found = _freeArcs[choosenIndex];

        // ����ִ� ��ũ�� ã�� �� ������ �ֺ� ��ũ�� Ȯ��
        if (!found)
        {
            int offset = 1;
            int halfCount = arcsCount / 2;
            while (offset <= halfCount)
            {
                int leftIndex = wantedIndex - offset;
                int rightIndex = wantedIndex + offset;

                if (leftIndex < 0) leftIndex += arcsCount;
                if (rightIndex >= arcsCount) rightIndex -= arcsCount;

                if (!Physics.Raycast(rayCastPosition, GetDirection(leftIndex), wantedDistance) &&
                    _freeArcs[leftIndex])
                {
                    choosenIndex = leftIndex;
                    found = true;
                    break;
                }

                if (!Physics.Raycast(rayCastPosition, GetDirection(rightIndex), wantedDistance) &&
                    _freeArcs[rightIndex])
                {
                    choosenIndex = rightIndex;
                    found = true;
                    break;
                }

                offset += 1;
            }
        }

        // ��� �ִ� ��ũ�� ã�� ���ϸ� -1�� ��ȯ
        if (!found)
        {
            return -1;
        }

        // ������ ��ũ�� ��� ������ ����
        _freeArcs[choosenIndex] = false;
        return choosenIndex;
    }

    /// <summary>
    /// Ư�� �ε����� ��ũ�� �����Ӱ� ����ϴ�.
    /// </summary>
    public void FreeIndex(int index)
    {
        _freeArcs[index] = true;
    }
}
