using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoAim()
    {
        StartCoroutine(Aiming());
    }

    public void OffAim()
    {
        aimer.SetActive(false);
    }

    public IEnumerator Aiming()
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
