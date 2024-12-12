using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 경비대장 클래스
/// 
/// 죽었을 때 목표달성 + Enemy Behaviour에서 사운드 찾는데 사용한다
/// </summary>
public class Centurion : MonoBehaviour
{
    private UIManager um;

    // Start is called before the first frame update
    void Start()
    {
        um = UIManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 목표 달성
    private void OnDestroy()
    {
        um.Achieve();
    }
}
