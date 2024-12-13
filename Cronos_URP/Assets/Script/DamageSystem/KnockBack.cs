using UnityEngine;

/// <summary>
/// Rigidbody를 사용해 오브젝트를 밀어내는 클래스입니다. 
/// 주로 타격이나 충격 효과에서 사용됩니다.
/// </summary>
public class KnockBack : MonoBehaviour
{
    public float ForcePower = 100f;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    public void Begin(Vector3 forceSource)
    {
        Vector3 direction = transform.position - forceSource;
        direction.y = 0f;
        _rigidbody.AddForce(direction.normalized * ForcePower, ForceMode.Impulse);
    }
}
