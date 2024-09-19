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
        if (_elapse > period)
        {
            bool damaged = DamageCheck(other);
            if (damaged)
            {
                _elapse = 0f;
                Debug.Log("µµÆ®´ï");
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
