using System.Collections;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Vector3 targetPosition; // 목표 위치
    public float duration = 1f; // 이동 시간 (1초)

    private Vector3 startPosition;
    private float elapsedTime = 0f;

    void Start()
    {
        startPosition = transform.position; // 현재 위치 저장
        StartCoroutine(MoveToPosition());
    }

    IEnumerator MoveToPosition()
    {
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 한 프레임 대기
        }

        // 마지막으로 정확한 목표 위치로 설정
        transform.position = targetPosition;
    }
}
