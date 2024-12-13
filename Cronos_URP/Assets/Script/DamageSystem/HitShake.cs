using System.Collections;
using UnityEngine;

/// <summary>
/// 오브젝트가 타격을 받았을 때 
/// 흔들리는 효과를 구현하는 클래스입니다.
/// </summary>
public class HitShake : MonoBehaviour
{
    [Header("Settings")]
    [Range(0f, 2f)]
    public float time = 0.2f;
    [Range(0f, 2f)]
    public float distance = 0.1f;
    [Range(0f, 0.1f)]
    public float delayBetweenShakes = 0f;

    private Vector3 _originalPos;
    private float _timer;
    private Vector3 _randomPos;

    public void Begin()
    {
        _originalPos = transform.position;
        StopAllCoroutines();
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        _timer = 0f;

        while (_timer < time)
        {
            _timer += Time.deltaTime;

            _randomPos = _originalPos + (Random.insideUnitSphere * distance);
            _randomPos.y = _originalPos.y;

            transform.position = _randomPos;

            if (delayBetweenShakes > 0f)
            {
                yield return new WaitForSeconds(delayBetweenShakes);
            }
            else
            {
                yield return null;
            }
        }

        transform.position = _originalPos;
    }
}
