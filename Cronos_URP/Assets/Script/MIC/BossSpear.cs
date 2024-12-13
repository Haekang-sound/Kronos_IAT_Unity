using System.Collections;
using UnityEngine;


/// <summary>
/// 보스 투창 스킬을 담당하는 클래스
/// 프리팹에서 창을 활성화하고 날려보내는 역할
/// </summary>
public class BossSpear : MonoBehaviour
{
    public GameObject spear1;
    public GameObject spear2;
    public GameObject spear3;
    public GameObject spear4;
    public GameObject spear5;
    private GameObject[] spears;
    public float lookSpeed = 3.0f;
    public float delay = 3.0f;
    public float shotOffset = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        spears = new GameObject[] {spear1, spear2, spear3, spear4, spear5};
        foreach (GameObject s in spears)
        {
            BossSpearScript bss = s.gameObject.GetComponent<BossSpearScript>();
            bss.lookSpeed = lookSpeed;
            bss.elapsedTime = shotOffset;
            s.SetActive(false);
        }

        StartCoroutine(ActivateCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 차례대로 소환하고 차례대로 발사
    public IEnumerator ActivateCoroutine()
    {
        foreach (GameObject s in spears)
        {
            s.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);
        Debug.Log("Spear spawn finished.");
        foreach (GameObject s in spears)
        {
            BossSpearScript bss = s.gameObject.GetComponent<BossSpearScript>(); ;
            bss.act = true;
        }

        foreach (GameObject s in spears)
        {
            BossSpearScript bss = s.gameObject.GetComponent<BossSpearScript>(); ;
            bss.StartCoroutine(bss.Saturate(delay));
            yield return new WaitForSeconds(1.0f);
        }
    }
}
