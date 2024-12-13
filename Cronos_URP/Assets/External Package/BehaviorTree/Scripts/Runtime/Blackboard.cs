using UnityEngine;

/// <summary>
/// �ൿ Ʈ������ ����� �����͸� �����ϴ� Ŭ�����Դϴ�.
/// �ַ� ���� ������Ʈ�� �̵� ��ġ, BulletTimeScalable ��ü ���� �����Ͽ� Ʈ������ ����� �� �ֵ��� �մϴ�.
/// </summary>
[System.Serializable]
public class Blackboard
{
    public GameObject target;
    public Vector3 moveToPosition;
    public BulletTimeScalable bulletTimeScalable;
}