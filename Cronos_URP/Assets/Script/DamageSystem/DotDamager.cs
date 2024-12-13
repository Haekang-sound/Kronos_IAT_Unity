using UnityEngine;

/// <summary>
/// 일정 주기마다 지속적으로 대미지를 입히는 
/// '도트(Damage over Time)' 대미지 처리 클래스입니다.
/// </summary>
public class DotDamager : SimpleDamager
{
    [SerializeField]
    private float _elapse;
    public float period = 1f;

    private void Start()
    {
        _elapse = period;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_elapse > period)
        {
            bool damaged = DamageCheck(other);
            if (damaged)
            {
                _elapse = 0f;
                //Debug.Log("도트댐");
            }
        }
    }

    private void Update()
    {
        _elapse += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        DrawGizmos();
    }
}
