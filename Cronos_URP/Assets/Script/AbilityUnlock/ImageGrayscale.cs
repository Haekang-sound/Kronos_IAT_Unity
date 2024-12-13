using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// UI 이미지에 그레이스케일 효과를 적용하고, 
/// 지정된 시간 동안 그레이스케일 효과를 제어합니다.
/// </summary>
public class ImageGrayscale : MonoBehaviour
{
    public Material GrayscaleMaterial;

    [SerializeField]
    private Image image;
    [SerializeField]
    private float _duration = 1f;

    private void Awake()
    {
        image = GetComponent<Image>();

        image.material = new Material(GrayscaleMaterial);
    }

    // -----

    public void SetGrayscale(float amount = 1)
    {
        image.material.SetFloat("_GrayscaleAmount", amount);
    }

    public void StartGrayScaleRoutine()
    {
        StartCoroutine(GrayscaleRoutine(_duration, true));
    }

    public void Reset()
    {
        StartCoroutine(GrayscaleRoutine(_duration, false));
    }

    // -----

    private IEnumerator GrayscaleRoutine(float duration, bool isGrayscale)
    {
        float time = 0f;
        while (duration > time)
        {
            float durationFrame = Time.unscaledDeltaTime;
            float ratio = time / duration;
            float grayAmount = isGrayscale ? ratio : 1 - ratio;
            
            SetGrayscale(grayAmount);

            time += durationFrame;

            yield return null;
        }

        SetGrayscale(isGrayscale ? 1 : 0);
    }
}
