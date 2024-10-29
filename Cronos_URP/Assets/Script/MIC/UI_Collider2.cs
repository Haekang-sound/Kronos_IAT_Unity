using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Collider2 : MonoBehaviour
{
    public int regionNum;

    UIManager um;
    QuestManager qm;

    // Start is called before the first frame update
    void Start()
    {
        um = UIManager.Instance;
        if (um == null)
            Debug.Log("nm doesn't assigned");
        qm = QuestManager.Instance;
        if (qm == null)
            Debug.Log("qm doesn't assigned");
        StartCoroutine(ShowRegionCoroutine());
    }

    IEnumerator ShowRegionCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        um.StartRegion(regionNum);
        Debug.Log("Call Region Name");
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject == Player.Instance.gameObject)
    //    {
    //        um.StartRegion(regionNum);
    //    }
    //}
}
