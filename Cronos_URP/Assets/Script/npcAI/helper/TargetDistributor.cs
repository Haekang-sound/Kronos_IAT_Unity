using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ش� Ŭ������ ���� ���� ������ ��� ������ ȣ�� �л��Ų��.
// ������ �ٸ� ���⿡�� �÷��̾�(�Ǵ� ��� ���)�� �����ϵ��� �Ѵ�.
[DefaultExecutionOrder(-1)]
public class TargetDistributor : MonoBehaviour
{
    // ���� Ÿ���� ���� ��ü�� �ȷο���� �Ѵ�.
    // �̷��� �ȷο� ���� Ŀ�����̼� �����̴�.
    public class TargetFollower
    {
        // target�� �ý��ۿ��� ��ġ�� �����ؾ� �� �� �̸� true�� �����ؾ� �Ѵ�.
        public bool requireSlot;
        // ���� �Ҵ�� ��ġ�� ������ -1�̴�.
        public int assignedSlot;
        // �ȷο��� Ÿ�ٿ� �����ϰ��� �ϴ� ��ġ�̴�.
        public Vector3 requiredPoint;

        public TargetDistributor distributor;

        public TargetFollower(TargetDistributor owner)
        {
            distributor = owner;
            requiredPoint = Vector3.zero;
            requireSlot = false;
            assignedSlot = -1;
        }
    }

    // 0���ϸ� �ȵȴ�.
    public int arcsCount;

    protected Vector3[] _worldDirection;

    protected bool[] _freeArcs;
    protected float _arcDegree;

    protected List<TargetFollower> _followers;

    public void OnEnable()
    {
        _worldDirection = new Vector3[arcsCount];
        _freeArcs = new bool[arcsCount];

        _followers = new List<TargetFollower>();

        _arcDegree = 360.0f / arcsCount;
        Quaternion rotation = Quaternion.Euler(0, -_arcDegree, 0);
        Vector3 currentDirection = Vector3.forward;
        for (int i = 0; i < arcsCount; ++i)
        {
            _freeArcs[i] = true;
            _worldDirection[i] = currentDirection;
            currentDirection = rotation * currentDirection;
        }
    }

    public TargetFollower RegisterNewFollower()
    {
        TargetFollower follower = new TargetFollower(this);
        _followers.Add(follower);
        return follower;
    }

    public void UnregisterFollower(TargetFollower follower)
    {
        if (follower.assignedSlot != -1)
        {
            _freeArcs[follower.assignedSlot] = true;
        }


        _followers.Remove(follower);
    }

    // �������� ������ Ÿ�� ��ġ�� �ʿ���ϴ� ��� �ȷο����� Ÿ�� ��ġ�� �����Ѵ�.
    private void LateUpdate()
    {
        for (int i = 0; i < _followers.Count; ++i)
        {
            var follower = _followers[i];

            // �ȷο��� �Ҵ�� ��ġ�� ���´ٸ� �����Ѵ�.
            // ������ �ʿ��ϴٸ� ���� �ڵ忡 �ٽ� �Ҵ�ȴ�.
            // ��ġ�� �ٲ�� ���ο� ��ġ�� ���õȴ�.
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

    public Vector3 GetDirection(int index)
    {
        return _worldDirection[index];
    }

    public int GetFreeArcIndex(TargetFollower follower)
    {
        bool found = false;

        Vector3 wanted = follower.requiredPoint - transform.position;
        Vector3 rayCastPosition = transform.position + Vector3.up * 0.4f;

        wanted.y = 0;
        float wantedDistance = wanted.magnitude;

        wanted.Normalize();

        float angle = Vector3.SignedAngle(wanted, Vector3.forward, Vector3.up);
        if (angle < 0)
            angle = 360 + angle;

        int wantedIndex = Mathf.RoundToInt(angle / _arcDegree);
        if (wantedIndex >= _worldDirection.Length)
            wantedIndex -= _worldDirection.Length;

        int choosenIndex = wantedIndex;

        RaycastHit hit;
        if (!Physics.Raycast(rayCastPosition, GetDirection(choosenIndex), out hit, wantedDistance))
            found = _freeArcs[choosenIndex];

        if (!found)
        {//we are going to test left right with increasing offset
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

        if (!found)
        {// ����ִ� ������ ã�� �� �����Ƿ� -1�� ��ȯ�Ͽ� ȣ���ڿ��� ���� ������ ������ �˸�.
            return -1;
        }

        _freeArcs[choosenIndex] = false;
        return choosenIndex;
    }

    public void FreeIndex(int index)
    {
        _freeArcs[index] = true;
    }
}