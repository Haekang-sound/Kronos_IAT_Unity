using System.Collections;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 광장 오브젝트를 관리하는 클래스
/// 오브젝트들이 파괴되었을 때의 이벤트와 회전 등을 관리한다
/// </summary>
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

	public int destroyedCount = 0;
	public UnityEvent OnOpen;
    public GameObject soundTrigger;

    private UIManager um;

    void Start()
    {
        if (soundTrigger != null)
            soundTrigger.SetActive(false);
        RegisterDestroyEvents();
        um = UIManager.Instance;
    }

	void RegisterDestroyEvents()
    {
        lock1.GetComponent<Fracture>().OnDeath += ObjectDestroyed;
        lock2.GetComponent<Fracture>().OnDeath += ObjectDestroyed;
        lock3.GetComponent<Fracture>().OnDeath += ObjectDestroyed;
	}

    void ObjectDestroyed()
    {
		destroyedCount++;
        StartCoroutine(Spin45Degree());

		if(destroyedCount == 3)
		{
			// 여기서 문여는 함수를 호출? 
			OnOpen.Invoke();
            // 우승
            um.Achieve();
		}
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
