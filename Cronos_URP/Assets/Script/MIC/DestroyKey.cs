using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyKey : MonoBehaviour
{
    QuestManager qm;
    UIManager um;

    // Start is called before the first frame update
    void Start()
    {
        qm = QuestManager.Instance;
        um = UIManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        um.Achieve();
    }
}
