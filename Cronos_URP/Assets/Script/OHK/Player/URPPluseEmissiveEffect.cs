using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URPPluseEmissiveEffect : MonoBehaviour
{
    public float duration = 2f;

    private string shaderValuename = "_EmissionScale";
    private Material[] _materials;
    private float[] _originalVal;

    // -----

    public void Emissive()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            StartCoroutine(EmissiveCoroutine(_materials[i], _originalVal[i], duration));
        }
    }

    // -----

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();

        // _materials 초기화
        Material[] originalMaterials = renderer.materials;
        _materials = new Material[originalMaterials.Length];
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            _materials[i] = new Material(originalMaterials[i]); // 기존 머터리얼을 인스턴스화
        }
        renderer.materials = _materials; // 기존 머터리얼 인스턴싱

    }

    private void Start()
    {
        _originalVal = new float[_materials.Length];

        for (int i = 0; i < _originalVal.Length; i++)
        {
            _originalVal[i] = _materials[i].GetFloat(shaderValuename);
            _materials[i].SetFloat(shaderValuename, 1.0f);
        }
    }

    // -----

    private IEnumerator EmissiveCoroutine(Material material, float targetValue, float duration)
    {
        float elapsedTime = 0f;

        float startValue = material.GetFloat(shaderValuename);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            float currentValue = Mathf.Lerp(startValue, targetValue, t);
            material.SetFloat(shaderValuename, currentValue);

            yield return null;
        }

        material.SetFloat(shaderValuename, targetValue);
    }

}
