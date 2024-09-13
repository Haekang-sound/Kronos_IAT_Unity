using UnityEngine;

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
        _elapse += Time.deltaTime;

        if (_elapse > period)
        {
            _elapse = 0f;
            DamageCheck(other);
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        DrawGizmos();
    }
}
