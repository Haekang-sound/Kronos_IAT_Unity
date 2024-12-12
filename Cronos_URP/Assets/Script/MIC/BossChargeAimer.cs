using System.Collections;
using UnityEngine;


/// <summary>
/// 보스가 차지 어택을 할 때 인디케이터를 보여주기 위한 클래스
/// </summary>
public class BossChargeAimer : MonoBehaviour
{
    [SerializeField]
    GameObject aimer;
    public float shrinkTime = 0.5f;
    private Vector3 originScale = new Vector3(2, 2, 2);
    private Vector3 ringShrinkScale = new Vector3(0.5f, 0.5f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        aimer.SetActive(false);
    }

    //// Update is called once per frame
    void Update()
    {

    }

    public void DoAim()
    {
        StartCoroutine(AimingCoroutine());
    }

    public void OffAim()
    {
        aimer.SetActive(false);
    }

    // 단순히 에임 오브젝트 스케일을 줄일 뿐
    public IEnumerator AimingCoroutine()
    {
        aimer.transform.localScale = originScale;
        aimer.SetActive(true);
        float elapsedTime = 0.0f;
        while (elapsedTime < shrinkTime)
        {
            if (aimer != null)
            aimer.transform.localScale = Vector3.Lerp(originScale, ringShrinkScale, elapsedTime / shrinkTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
