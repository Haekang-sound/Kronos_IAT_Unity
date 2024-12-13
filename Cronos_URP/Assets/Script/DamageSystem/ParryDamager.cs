using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 패리(반격) 기능을 처리하는 클래스입니다.
/// 공격이 들어올 때 패리 가능한 상태를 확인하고, 패리 성공 시 이벤트를 발생시킵니다.
/// </summary>
public class ParryDamager : SimpleDamager
{
    [SerializeField]
    private bool _isGuard;
    [SerializeField]
    private bool _isParrying;
    private bool _canBeParryied;

	public UnityEvent beParryied, parrying;

    private void OnTriggerEnter(Collider other)
    {
        bool isParryied = ParryiedCheck(other);

        if (isParryied == false)
        {
            DamageCheck(other);
        }
    }

    public void BeginGuard()
    {
        _isGuard = true;
    }

    public void EndGuard()
    {
        _isGuard = false;
        EndPerfectGuard();
        EndBeParried();
    }

    public void BeginBeParried()
    {
        _canBeParryied = true;
    }
    public void EndBeParried()
    {
        _canBeParryied = false;
    }

    public void BeginPerfectGuard()
    {
        _isParrying = true;
    }

    public void EndPerfectGuard()
    {
        _isParrying = false;
    }

    /// <summary>
    /// 패리 상태를 확인합니다. 상대가 패리 가능한 상태인지, 패리 중인지 체크합니다.
    /// </summary>
    /// <param name="other">충돌한 오브젝트</param>
    /// <returns>패리 성공 여부</returns>
    public bool ParryiedCheck(Collider other)
    {
        var otherParryDamager = other.GetComponent<ParryDamager>();

        // 패리할 수 없는 조건들
        if (otherParryDamager == null ||
            otherParryDamager?._isParrying == false ||
            _canBeParryied == false)
        {
            return false;
        }

        // 패리 성공 시 이벤트 발생
        beParryied.Invoke();
        otherParryDamager.parrying?.Invoke();

        return true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

        if (_isGuard == false)
        {
            // yellow - 패리 당할 수 있는 공격 타이밍
            if (_canBeParryied == true)
            {
                Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
            }
            // Red - 공격
            else if (inAttack == true)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            }
        }
        else if (_isGuard == true)
        {
            // Green - 패리 가능한 가드
            if (_isParrying == true)
            {
                Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            }
            // Blue - 가드
            else
            {
                Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
            }
        }

        DrawGizmos();
    }
}
