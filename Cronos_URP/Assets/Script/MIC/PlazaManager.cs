using System.Collections;
using UnityEngine;

public class PlazaManager : MonoBehaviour
{
    public GameObject statue;
    public GameObject lock1;
    public GameObject lock2;
    public GameObject lock3;
    // 한번에 회전할 정도
    public float rotateAmount = 0.1f;
    // 회전할 각도
    public float rotateDegree = 45.0f;
    // 총 회전한 각도
    public float totalDegree = 0.0f;
    public float elapsedDegree = 0.0f;

    public GameObject soundTrigger;

    void Start()
    {
        if (soundTrigger != null)
            soundTrigger.SetActive(false);
        RegisterDestroyEvents();
    }

    void RegisterDestroyEvents()
    {
        lock1.GetComponent<DestroyablePlazeObject>().OnDestroyed += ObjectDestroyed;
        lock2.GetComponent<DestroyablePlazeObject>().OnDestroyed += ObjectDestroyed;
        lock3.GetComponent<DestroyablePlazeObject>().OnDestroyed += ObjectDestroyed;
    }

    void ObjectDestroyed()
    {
        StartCoroutine(Spin45Degree());
    }

    IEnumerator Spin45Degree()
    {
        elapsedDegree += rotateDegree;
        soundTrigger.SetActive(true);
        while (totalDegree < elapsedDegree)
        {
            statue.transform.Rotate(0, rotateAmount, 0);
            totalDegree += rotateAmount;
            yield return null;
        }
        soundTrigger.SetActive(false);

    }
}
