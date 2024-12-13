using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// UI �̹����� �׷��̽����� ȿ���� �����ϰ�, 
/// ������ �ð� ���� �׷��̽����� ȿ���� �����մϴ�.
/// </summary>
public class ImageGrayscale : MonoBehaviour
{
    public Material grayscaleMaterial;

    [SerializeField]
    private Image _image;
    [SerializeField]
    private float _duration = 1f;

    private void Awake()
    {
        _image = GetComponent<Image>();

        _image.material = new Material(grayscaleMaterial);
    }

    // -----

    public void SetGrayscale(float amount = 1)
    {
        _image.material.SetFloat("_GrayscaleAmount", amount);
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
