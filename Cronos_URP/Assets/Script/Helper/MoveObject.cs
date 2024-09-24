using System.Collections;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Vector3 targetPosition; // ��ǥ ��ġ
    public float duration = 1f; // �̵� �ð� (1��)

    private Vector3 startPosition;
    private float elapsedTime = 0f;

    void Start()
    {
        startPosition = transform.position; // ���� ��ġ ����
        StartCoroutine(MoveToPosition());
    }

    IEnumerator MoveToPosition()
    {
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // �� ������ ���
        }

        // ���������� ��Ȯ�� ��ǥ ��ġ�� ����
        transform.position = targetPosition;
    }
}
