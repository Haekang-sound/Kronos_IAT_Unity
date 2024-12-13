using System.Collections;
using UnityEngine;

/// <summary>
/// 지정된 목표 위치로 오브젝트를 부드럽게 이동시키는 클래스입니다.
/// 이동은 주어진 시간 동안 선형 보간(Lerp) 방식으로 이루어집니다.
/// </summary>
public class MoveObject : MonoBehaviour
{
    public Vector3 targetPosition; // 목표 위치
    public float duration = 1f; // 이동 시간 (1초)

    private Vector3 _startPosition;
    private float _elapsedTime = 0f;

    void Start()
    {
        _startPosition = transform.position; // 현재 위치 저장
        StartCoroutine(MoveToPosition());
    }

    IEnumerator MoveToPosition()
    {
        while (_elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(_startPosition, targetPosition, _elapsedTime / duration);
            _elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 한 프레임 대기
        }

        // 마지막으로 정확한 목표 위치로 설정
        transform.position = targetPosition;
    }
}
