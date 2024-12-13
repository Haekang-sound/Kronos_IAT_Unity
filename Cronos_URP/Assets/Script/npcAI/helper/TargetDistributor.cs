using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// 적의 군집이 특정 타겟을 주위에 분산시키고, 팔로워들이 서로 다른 방향에서 타겟을 공격하도록 하는 시스템입니다.
/// 이 클래스는 타겟의 위치를 여러 팔로워에게 분배하고, 각 팔로워가 타겟을 향해 움직이도록 합니다.
/// </summary>
[DefaultExecutionOrder(-1)]
public class TargetDistributor : MonoBehaviour
{
    /// <summary>
    /// 타겟을 추적하는 팔로워를 나타내는 내부 클래스입니다.
    /// 팔로워는 분배된 위치로 이동하기 위해 필요한 정보를 저장합니다.
    /// </summary>
    public class TargetFollower
    {
        public bool requireSlot;  // 팔로워가 위치를 요구하는지 여부
        public int assignedSlot;  // 할당된 위치의 인덱스 (-1은 할당되지 않음)
        public Vector3 requiredPoint;  // 팔로워가 목표로 하는 위치
        public TargetDistributor distributor;  // 팔로워가 속한 Distributor 인스턴스

        public TargetFollower(TargetDistributor owner)
        {
            distributor = owner;
            requiredPoint = Vector3.zero;
            requireSlot = false;
            assignedSlot = -1;
        }
    }

    public int arcsCount;  // 분배할 아크(각도)의 수 (1보다 커야 함)
    protected Vector3[] _worldDirection;  // 각 아크의 방향을 저장
    protected bool[] _freeArcs;  // 각 아크가 사용 가능한지 여부
    protected float _arcDegree;  // 각 아크의 각도

    protected List<TargetFollower> _followers;  // 팔로워 리스트

    /// <summary>
    /// 객체가 활성화될 때 호출되며, 아크 방향과 팔로워들을 초기화합니다.
    /// </summary>
    public void OnEnable()
    {
        _worldDirection = new Vector3[arcsCount];
        _freeArcs = new bool[arcsCount];
        _followers = new List<TargetFollower>();
        _arcDegree = 360.0f / arcsCount;

        // 아크 방향 초기화
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
    /// 새로운 팔로워를 등록하고 해당 팔로워를 반환합니다.
    /// </summary>
    public TargetFollower RegisterNewFollower()
    {
        TargetFollower follower = new TargetFollower(this);
        _followers.Add(follower);
        return follower;
    }

    /// <summary>
    /// 기존 팔로워를 등록 해제합니다.
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
    /// 매 프레임 끝날 때 모든 팔로워에게 타겟 위치를 할당합니다.
    /// </summary>
    private void LateUpdate()
    {
        for (int i = 0; i < _followers.Count; ++i)
        {
            var follower = _followers[i];

            // 이미 할당된 위치를 해제하고, 필요 시 새로운 위치를 할당합니다.
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
    /// 주어진 인덱스의 방향을 반환합니다.
    /// </summary>
    public Vector3 GetDirection(int index)
    {
        return _worldDirection[index];
    }

    /// <summary>
    /// 팔로워에게 적절한 비어 있는 아크 인덱스를 찾아 할당합니다.
    /// </summary>
    public int GetFreeArcIndex(TargetFollower follower)
    {
        bool found = false;

        Vector3 wanted = follower.requiredPoint - transform.position;
        Vector3 rayCastPosition = transform.position + Vector3.up * 0.4f;

        wanted.y = 0;
        float wantedDistance = wanted.magnitude;
        wanted.Normalize();

        // 타겟 위치의 각도 계산
        float angle = Vector3.SignedAngle(wanted, Vector3.forward, Vector3.up);
        if (angle < 0)
            angle = 360 + angle;

        int wantedIndex = Mathf.RoundToInt(angle / _arcDegree);
        if (wantedIndex >= _worldDirection.Length)
            wantedIndex -= _worldDirection.Length;

        int choosenIndex = wantedIndex;

        // 타겟 위치와의 레이캐스트 검사
        RaycastHit hit;
        if (!Physics.Raycast(rayCastPosition, GetDirection(choosenIndex), out hit, wantedDistance))
            found = _freeArcs[choosenIndex];

        // 비어있는 아크를 찾을 수 없으면 주변 아크를 확인
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

        // 비어 있는 아크를 찾지 못하면 -1을 반환
        if (!found)
        {
            return -1;
        }

        // 선택한 아크를 사용 중으로 설정
        _freeArcs[choosenIndex] = false;
        return choosenIndex;
    }

    /// <summary>
    /// 특정 인덱스의 아크를 자유롭게 만듭니다.
    /// </summary>
    public void FreeIndex(int index)
    {
        _freeArcs[index] = true;
    }
}
