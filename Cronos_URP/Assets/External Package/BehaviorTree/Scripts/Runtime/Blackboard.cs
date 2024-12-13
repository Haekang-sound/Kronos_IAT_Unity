using UnityEngine;

/// <summary>
/// 행동 트리에서 사용할 데이터를 저장하는 클래스입니다.
/// 주로 게임 오브젝트와 이동 위치, BulletTimeScalable 객체 등을 저장하여 트리에서 사용할 수 있도록 합니다.
/// </summary>
[System.Serializable]
public class Blackboard
{
    public GameObject target;
    public Vector3 moveToPosition;
    public BulletTimeScalable bulletTimeScalable;
}