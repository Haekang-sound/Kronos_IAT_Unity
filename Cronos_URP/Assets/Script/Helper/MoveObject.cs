using System.Collections;
using UnityEngine;

/// <summary>
/// ������ ��ǥ ��ġ�� ������Ʈ�� �ε巴�� �̵���Ű�� Ŭ�����Դϴ�.
/// �̵��� �־��� �ð� ���� ���� ����(Lerp) ������� �̷�����ϴ�.
/// </summary>
public class MoveObject : MonoBehaviour
{
    public Vector3 targetPosition; // ��ǥ ��ġ
    public float duration = 1f; // �̵� �ð� (1��)

    private Vector3 _startPosition;
    private float _elapsedTime = 0f;

    void Start()
    {
        _startPosition = transform.position; // ���� ��ġ ����
        StartCoroutine(MoveToPosition());
    }

    IEnumerator MoveToPosition()
    {
        while (_elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(_startPosition, targetPosition, _elapsedTime / duration);
            _elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // �� ������ ���
        }

        // ���������� ��Ȯ�� ��ǥ ��ġ�� ����
        transform.position = targetPosition;
    }
}
