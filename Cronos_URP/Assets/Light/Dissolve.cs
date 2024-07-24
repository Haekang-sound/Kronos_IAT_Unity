using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField]
    private float dissolveTime = 0.75f;

    public GameObject powCollider;

    new Renderer renderer;
    Material material;
    int dissolveAmount = Shader.PropertyToID("_DissolveAmount");

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        material = renderer.material;
        if (material == null)
        {
            Debug.Log("no mat");
        }
    }

    public void DoVanish()
    {
        powCollider.SetActive(false);
        StartCoroutine(Vanish());
    }

    IEnumerator Vanish()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpDissolve = Mathf.Lerp(0, 1.1f, (elapsedTime / dissolveTime));
            material.SetFloat(dissolveAmount, lerpDissolve);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
