using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 클래스를 가진 적의 군집은 대상 주위에 호를 분산시킨다.
// 적증니 다른 방향에서 플레이어(또는 모든 대상)를 공격하도록 한다.
[DefaultExecutionOrder(-1)]
public class TargetDistributor : MonoBehaviour
{
    // 같은 타깃을 가진 객체를 팔로워라고 한다.
    // 이러한 팔로워 간의 커뮤케이션 수단이다.
    public class TargetFollower
    {
        // target은 시스템에서 위치를 제공해야 할 때 이를 true로 설정해야 한다.
        public bool requireSlot;
        // 현재 할당된 위치가 없으면 -1이다.
        public int assignedSlot;
        // 팔로워가 타겟에 도달하고자 하는 위치이다.
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

    // 프레임이 끝나면 타겟 위치를 필요로하는 모든 팔로워에게 타깃 위치를 공유한다.
    private void LateUpdate()
    {
        for (int i = 0; i < _followers.Count; ++i)
        {
            var follower = _followers[i];

            // 팔로워가 할당된 위치를 갖는다면 해제한다.
            // 여전히 필요하다면 다음 코드에 다시 할당된다.
            // 위치가 바뀌면 새로운 위치가 선택된다.
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
        {// 비어있는 방향을 찾을 수 없으므로 -1을 반환하여 호출자에게 여유 공간이 없음을 알림.
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