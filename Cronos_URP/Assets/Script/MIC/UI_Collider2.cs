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
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Player.Instance.gameObject)
        {
            um.StartRegion(regionNum);
            Debug.Log("STAY");
            Destroy(gameObject);
        }
    }
}
