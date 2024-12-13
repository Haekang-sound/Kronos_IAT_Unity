using UnityEngine;


/// <summary>
/// 분수 위의 삼각형 인디케이터
/// 항상 플레이어를 바라보게
/// </summary>
public class PlazaIndicator : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180f);
    }
}
